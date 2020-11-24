using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using HacknetSharp.Events.Client;
using HacknetSharp.Events.Server;
using HacknetSharp.Server.Common;
using HacknetSharp.Server.Common.Models;

namespace HacknetSharp.Server
{
    public class HostConnection : IPlayerContext
    {
        public Guid Id { get; }

        public Dictionary<Guid, PlayerModel> PlayerModels { get; }
        private readonly Server _server;
        private readonly TcpClient _client;
        private readonly AutoResetEvent _lockOutOp;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private SslStream? _sslStream;
        private BufferedStream? _bufferedStream;
        private bool _closed;
        private bool _connected;

        public HostConnection(Server server, TcpClient client)
        {
            Id = Guid.NewGuid();
            _server = server;
            _client = client;
            _lockOutOp = new AutoResetEvent(true);
            _cancellationTokenSource = new CancellationTokenSource();
            PlayerModels = new Dictionary<Guid, PlayerModel>();
            _ = Execute(_cancellationTokenSource.Token);
        }

        private async Task Execute(CancellationToken cancellationToken)
        {
            if (!_server.TryIncrementCountdown(LifecycleState.Active, LifecycleState.Active)) return;
            try
            {
                _sslStream = new SslStream(_client.GetStream(), false, default, default,
                    EncryptionPolicy.RequireEncryption);
                // Try and kill the stream, accept the consequences
                cancellationToken.Register(Dispose);
                // Authenticate the server but don't require the client to authenticate
                SslServerAuthenticationOptions opts =
                    new SslServerAuthenticationOptions
                    {
                        ServerCertificate = _server.Cert,
                        ClientCertificateRequired = false,
                        CertificateRevocationCheckMode = X509RevocationMode.Online
                    };
                await _sslStream.AuthenticateAsServerAsync(opts, cancellationToken).Caf();
                _connected = true;
                _sslStream.ReadTimeout = 10 * 1000;
                _sslStream.WriteTimeout = 10 * 1000;
                _bufferedStream = new BufferedStream(_sslStream);
                ClientEvent? evt;
                UserModel? user = null;
                while (!((evt = await _bufferedStream.ReadEventAsync<ClientEvent>(cancellationToken).Caf()) is
                    ClientDisconnectEvent))
                {
                    if (evt == null) return;
                    switch (evt)
                    {
                        case LoginEvent login:
                        {
                            var op = login.Operation;
                            if (user != null)
                            {
                                WriteEvent(new LoginFailEvent {Operation = op});
                                break;
                            }

                            user = await _server.AccessController.AuthenticateAsync(login.User, login.Pass).Caf();
                            if (user == null)
                            {
                                WriteEvent(new LoginFailEvent {Operation = op});
                                WriteEvent(ServerDisconnectEvent.Singleton);
                                await FlushAsync(cancellationToken).Caf();
                                return;
                            }

                            _sslStream.ReadTimeout = 100 * 1000;
                            _sslStream.WriteTimeout = 100 * 1000;

                            // TODO check or generate / register player model

                            WriteEvent(new UserInfoEvent {Operation = op, Admin = user.Admin});
                            WriteEvent(new OutputEvent {Text = "Welcome to ossu. Type \"exit\" to exit."});
                            break;
                        }
                        case RegistrationTokenForgeRequestEvent forgeRequest:
                        {
                            var op = forgeRequest.Operation;
                            var random = new Random();
                            var arr = new byte[32];
                            if (user == null) continue;
                            if (!user.Admin)
                            {
                                WriteEvent(new AccessFailEvent {Operation = op});
                                break;
                            }

                            string token;
                            do
                            {
                                random.NextBytes(arr);
                                token = Convert.ToBase64String(arr);
                            } while (await _server.Database.GetAsync<string, RegistrationToken>(token).Caf() != null);

                            var tokenModel = new RegistrationToken {Key = token};
                            _server.Database.Add(tokenModel);
                            await _server.Database.SyncAsync().Caf();
                            WriteEvent(new RegistrationTokenForgeResponseEvent(op, token));
                            break;
                        }
                        case CommandEvent command:
                        {
                            var op = command.Operation;
                            if (user == null) continue;
                            var line = Arguments.SplitCommandLine(command.Text);
                            // TODO operate on command based on context, this is temporary
                            if (line.Length > 0 && line[0].Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                                WriteEvent(ServerDisconnectEvent.Singleton);
                            else
                                WriteEvent(new OutputEvent {Text = "Output is not yet implemented."});
                            WriteEvent(new OperationCompleteEvent {Operation = op});
                            break;
                        }
                    }

                    await FlushAsync(cancellationToken).Caf();
                }
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                    _server.SelfRemoveConnection(Id);
                _server.DecrementCountdown();
                Dispose();
            }
        }

        public void Dispose()
        {
            if (_closed) return;

            _connected = false;
            _closed = true;
            _cancellationTokenSource.Cancel();

            try
            {
                _sslStream?.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                _client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void WriteEvent(ServerEvent evt)
        {
            if (_closed || _bufferedStream == null) throw new InvalidOperationException();
            _lockOutOp.WaitOne();
            try
            {
                _bufferedStream.WriteEvent(evt);
            }
            finally
            {
                _lockOutOp.Set();
            }
        }

        public void WriteEvents(IEnumerable<ServerEvent> events)
        {
            if (_closed || _bufferedStream == null) throw new InvalidOperationException();
            _lockOutOp.WaitOne();
            try
            {
                foreach (var evt in events) _bufferedStream.WriteEvent(evt);
            }
            finally
            {
                _lockOutOp.Set();
            }
        }

        public Task FlushAsync() => FlushAsync(CancellationToken.None);

        public async Task FlushAsync(CancellationToken cancellationToken)
        {
            if (_closed || _bufferedStream == null) throw new InvalidOperationException();
            _lockOutOp.WaitOne();
            await _bufferedStream.FlushAsync(cancellationToken).Caf();
            _lockOutOp.Set();
        }

        public PlayerModel GetPlayerModel(Guid world) => throw new NotImplementedException();

        public bool Connected => _connected;
    }
}
