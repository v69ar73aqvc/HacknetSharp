﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using HacknetSharp;
using HacknetSharp.Events.Client;
using HacknetSharp.Events.Server;
using Microsoft.Extensions.Logging;

namespace hsh
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    internal static class Program
    {
        private static async Task Main(string[] args)
            => await Parser.Default
                .ParseArguments<Options>(args)
                .MapResult(Run, _ => Task.FromResult(1)).Caf();

        private class Options
        {
            [Option('f', "forgetoken", HelpText = "Make registration token on server.", SetName = "forgetoken")]
            public bool ForgeToken { get; set; }

            [Option('r', "register", HelpText = "Register on server (requires registration token).",
                SetName = "register")]
            public bool Register { get; set; }

            [Option('v', "verbose", HelpText = "Enable verbose logging.")]
            public bool Verbose { get; set; }

            [Value(0, MetaName = "conString", HelpText = "Connection string (user@server[:port])", Required = true)]
            public string ConString { get; set; } = null!;
        }

        private static async Task<int> Run(Options options)
        {
            var config = new AlertLogger.Config(new[]
            {
                LogLevel.Critical, LogLevel.Debug, LogLevel.Error, LogLevel.Information, LogLevel.Trace,
                LogLevel.Warning
            });
            ILogger? logger = options.Verbose ? new AlertLogger(config) : null;
            var connection = GetConnection(options.ConString, options.Register, logger, out (int, string)? failReason);
            if (connection != null)
                return options.ForgeToken ? await ForgeToken(connection).Caf() : await ExecuteClient(connection).Caf();

            if (!failReason.HasValue) return 0;
            Console.WriteLine(failReason.Value.Item2);
            return failReason.Value.Item1;
        }

        private static async Task<int> ForgeToken(Client connection)
        {
            (UserInfoEvent? user, int resCode) = await Connect(connection).Caf();
            if (user == null) return resCode;

            Console.WriteLine($"Logged in as {connection.User} ({(user.Admin ? "admin" : "normal user")})");
            if (!user.Admin)
            {
                Console.WriteLine("Non-admin users cannot forge tokens.");
                Console.WriteLine(0x20202);
            }

            var operation = Guid.NewGuid();
            connection.WriteEvent(new RegistrationTokenForgeRequestEvent {Operation = operation});
            await connection.FlushAsync().Caf();
            var response = await connection.WaitForAsync(
                e => e is IOperation op && op.Operation == operation,
                10).Caf();
            if (response == null)
            {
                Console.WriteLine("No response received from server.");
                return 0x10102;
            }

            switch (response)
            {
                case FailBaseServerEvent failResponse:
                    Console.WriteLine($"Server returned an error: {failResponse.Message}");
                    return 0x1;
                case RegistrationTokenForgeResponseEvent regResponse:
                    Console.WriteLine($"TOKEN: {regResponse.RegistrationToken}");
                    return 0;
                default:
                    Console.WriteLine($"Unexpected event {response.GetType().FullName}");
                    return 0x10101;
            }
        }

        private static void PrintAlert(string kind, string header, string body)
        {
            List<string> lines = new(body.Split('\n'));
            lines.Insert(0, $"{kind} : {header.ToUpperInvariant()} ");
            int longest = Math.Min(lines.Select(l => l.Length).Max(), Math.Max(10, Console.BufferWidth - 6));
            lines[0] = lines[0] + new string('-', longest - lines[0].Length);
            var sb = new StringBuilder();
            for (int i = 0; i < lines.Count; i++)
                sb.AppendLine(i == 0
                    ? string.Format(CultureInfo.InvariantCulture,
                        $"»» {{0,{-longest}}} ««", lines[i])
                    : lines[i]);
            sb.Append("»» ").Append('-', longest).AppendLine(" ««");
            Console.Write(sb.ToString());
        }

        private static async Task<int> ExecuteClient(Client connection)
        {
            connection.OnReceivedEvent += e =>
            {
                switch (e)
                {
                    case AlertEvent alert:
                        string alertKind = alert.AlertKind switch
                        {
                            AlertEvent.Kind.System => "SYSTEM ALERT",
                            AlertEvent.Kind.Intrusion => "INTRUSION DETECTED",
                            AlertEvent.Kind.UserMessage => "PRIVATE MESSAGE",
                            AlertEvent.Kind.AdminMessage => "ADMIN MESSAGE",
                            _ => "UNKNOWN ALERT TYPE"
                        };

                        PrintAlert(alertKind, alert.Header, alert.Body);
                        break;
                    case ShellPromptEvent shellPrompt:
                        Console.Write($"{Util.UintToAddress(shellPrompt.Address)}:{shellPrompt.WorkingDirectory}> ");
                        break;
                    case OutputEvent output:
                        Console.Write(output.Text);
                        break;
                    case InputRequestEvent inputRequest:
                        connection.WriteEvent(new InputResponseEvent
                        {
                            Operation = inputRequest.Operation, Input = Util.ReadPassword() ?? ""
                        });
                        connection.FlushAsync();
                        break;
                    case EditRequestEvent editRequest:
                    {
                        var result = HsEditor.Open(editRequest.Content, editRequest.ReadOnly);
                        connection.WriteEvent(new EditResponseEvent
                        {
                            Operation = editRequest.Operation,
                            Content = result.Write
                                ? new StringBuilder().AppendJoin('\n', result.Lines).ToString()
                                : editRequest.Content
                        });
                        connection.FlushAsync();
                        break;
                    }
                }
            };
            connection.OnDisconnect += e =>
            {
                Console.WriteLine($"Disconnected by server. Reason: {e.Reason}");
                connection.DisposeAsync().Wait();
                Environment.Exit(0);
            };
            (UserInfoEvent? user, int resCode) = await Connect(connection).Caf();
            if (user == null) return resCode;
            var operation = Guid.NewGuid();
            connection.WriteEvent(new InitialCommandEvent {Operation = operation, ConWidth = Console.WindowWidth});
            await connection.FlushAsync().Caf();
            do
            {
                var operationLcl = operation;
                ServerEvent? endEvt = await connection.WaitForAsync(
                    e => e is IOperation op && op.Operation == operationLcl, 10).Caf();
                // Other important events are already handled by registered delegates
                // Operations are sent / waited on one at a time
                connection.DiscardEvents();
                if (endEvt == null) break;
                operation = Guid.NewGuid();
                connection.WriteEvent(new CommandEvent
                {
                    Operation = operation,
                    ConWidth = Console.WindowWidth,
                    Text = Console.ReadLine() ?? throw new ApplicationException()
                });

                await connection.FlushAsync().Caf();
            } while (true);

            await connection.DisposeAsync();
            return 0;
        }

        private static async Task<(UserInfoEvent?, int)> Connect(Client connection)
        {
            UserInfoEvent userInfoEvent;
            try
            {
                userInfoEvent = await connection.ConnectAsync().Caf();
            }
            catch (Client.LoginException)
            {
                Console.WriteLine("Login failed.");
                return (null, 0x20201);
            }
            catch (ProtocolException e)
            {
                Console.WriteLine($"A protocol error occurred: {e.Message}");
                return (null, 0x10101);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unknown error occurred: {e}.");
                return (null, 0x1);
            }

            //Console.WriteLine($"Logged in as {connection.User} ({(userInfoEvent.Admin ? "admin" : "normal user")})");
            return (userInfoEvent, 0);
        }

        private static Client? GetConnection(string conString, bool askRegistrationToken, ILogger? logger,
            out (int, string)? failReason)
        {
            failReason = null;
            if (!Util.TryParseConString(conString, Constants.DefaultPort, out string? user, out string? server,
                out ushort port, out string? error))
            {
                failReason = (101, error ?? "");
                return null;
            }

            Console.Write("Pass:");
            string? pass = Util.ReadPassword();
            string? registrationToken;
            if (askRegistrationToken)
            {
                Console.Write("Registration Token:");
                registrationToken = Util.ReadPassword();
            }
            else
                registrationToken = null;

            return pass != null ? new Client(server!, port, user!, pass, registrationToken, logger) : null;
        }

        private class AlertLogger : ILogger
        {
            public class Config
            {
                public HashSet<LogLevel> EnabledLevels { get; set; }

                public Config(IEnumerable<LogLevel> enabledLevels)
                {
                    EnabledLevels = new HashSet<LogLevel>(enabledLevels);
                }
            }


            private readonly Config _config;

            public AlertLogger(Config config)
            {
                _config = config;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel)) return;
                PrintAlert(logLevel.ToString(), $"[{eventId.Id,2}: {logLevel,-12}]", formatter(state, exception));
            }

            public bool IsEnabled(LogLevel logLevel) => _config.EnabledLevels.Contains(logLevel);

            public IDisposable BeginScope<TState>(TState state) => default!;
        }
    }
}
