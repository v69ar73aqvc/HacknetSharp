using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace HacknetSharp.Server.Models
{
    /// <summary>
    /// Represents a networked device in a world.
    /// </summary>
    public class SystemModel : WorldMember<Guid>
    {
        /// <summary>
        /// Template name.
        /// </summary>
        public virtual string Template { get; set; } = null!;

        /// <summary>
        /// Device name.
        /// </summary>
        public virtual string Name { get; set; } = null!;

        /// <summary>
        /// Operating system name.
        /// </summary>
        public virtual string OsName { get; set; } = null!;

        /// <summary>
        /// Device network address.
        /// </summary>
        public virtual uint Address { get; set; }

        /// <summary>
        /// Command line to execute on user login.
        /// </summary>
        public virtual string? ConnectCommandLine { get; set; }

        /// <summary>
        /// Time this system was last booted at.
        /// </summary>
        /// <remarks>
        /// Can be a time in the future, which would signify a reboot that will complete at the specified time.
        /// </remarks>
        public virtual double BootTime { get; set; }

        /// <summary>
        /// Number of required exploits for a successful hack.
        /// </summary>
        public virtual int RequiredExploits { get; set; }

        /// <summary>
        /// Owner of this system.
        /// </summary>
        public virtual PersonModel Owner { get; set; } = null!;

        /// <summary>
        /// Valid logins on this system.
        /// </summary>
        public virtual HashSet<LoginModel> Logins { get; set; } = null!;

        /// <summary>
        /// Filesystem.
        /// </summary>
        public virtual HashSet<FileModel> Files { get; set; } = null!;

        /// <summary>
        /// Time-based tasks.
        /// </summary>
        public virtual HashSet<CronModel> Tasks { get; set; } = null!;

        /// <summary>
        /// Vulnerabilities.
        /// </summary>
        public virtual HashSet<VulnerabilityModel> Vulnerabilities { get; set; } = null!;

        /// <summary>
        /// Systems this system knows.
        /// </summary>
        public virtual HashSet<KnownSystemModel> KnownSystems { get; set; } = null!;

        /// <summary>
        /// Systems this system is known by.
        /// </summary>
        public virtual HashSet<KnownSystemModel> KnowingSystems { get; set; } = null!;

        /// <summary>
        /// Reboot duration in seconds.
        /// </summary>
        public virtual double RebootDuration { get; set; }

        /// <summary>
        /// System disk capacity.
        /// </summary>
        public virtual int DiskCapacity { get; set; }

        /// <summary>
        /// System memory (bytes).
        /// </summary>
        public virtual long SystemMemory { get; set; }

        /// <summary>
        /// CPU cycles required to crack proxy.
        /// </summary>
        public virtual double ProxyClocks { get; set; }

        /// <summary>
        /// Proxy cracking speed.
        /// </summary>
        public virtual double ClockSpeed { get; set; }

        /// <summary>
        /// Number of firewall iterations required for full decode.
        /// </summary>
        public virtual int FirewallIterations { get; set; }

        /// <summary>
        /// Length of firewall analysis string.
        /// </summary>
        public virtual int FirewallLength { get; set; }

        /// <summary>
        /// Additional delay per firewall step.
        /// </summary>
        public virtual double FirewallDelay { get; set; }

        /// <summary>
        /// Fixed firewall string.
        /// </summary>
        public virtual string? FixedFirewall { get; set; }

        /// <summary>
        /// Tag for lookup.
        /// </summary>
        public virtual string? Tag { get; set; }

        /// <summary>
        /// Logical group this entity was spawned in.
        /// </summary>
        public virtual Guid SpawnGroup { get; set; }

        /// <summary>
        /// Processes currently running on this system.
        /// </summary>
        public Dictionary<uint, Process> Processes { get; set; } = new();

        /// <summary>
        /// System event delegate, used for trap signals etc.
        /// </summary>
        public Action<object>? Pulse { get; set; }

        /// <summary>
        /// Shells currently targeting this system.
        /// </summary>
        public HashSet<ShellProcess> TargetingShells { get; set; } = new();

        /// <summary>
        /// Public services.
        /// </summary>
        private Dictionary<Type, Service> PublicServices { get; set; } = new();

        private Dictionary<VulnerabilityModel, int> VulnerabilityVersions
        {
            get
            {
                if (_vulnerabilityVersions == null)
                {
                    _vulnerabilityVersions = new Dictionary<VulnerabilityModel, int>();
                    foreach (var k in Vulnerabilities)
                        _vulnerabilityVersions[k] = 0;
                }

                return _vulnerabilityVersions;
            }
            set => _vulnerabilityVersions = value;
        }

        private Dictionary<VulnerabilityModel, int>? _vulnerabilityVersions;

        private int FirewallVersion { get; set; }

        private int ProxyVersion { get; set; }

        /// <summary>
        /// Attempts to get public service.
        /// </summary>
        /// <param name="service">Service retrieved.</param>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <returns>True if service was retrieved.</returns>
        public bool TryGetService<TService>([NotNullWhen(true)] out TService? service) where TService : Service
        {
            if (!PublicServices.TryGetValue(typeof(TService), out var svc) || svc is not TService ts)
            {
                service = null;
                return false;
            }

            service = ts;
            return true;
        }

        /// <summary>
        /// Registers a service, failing if one already exists.
        /// </summary>
        /// <param name="service">Service to register.</param>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <returns>False if service of same type is already running.</returns>
        public bool TryAddService<TService>(TService service) where TService : Service
        {
            if (PublicServices.ContainsKey(typeof(TService))) return false;
            PublicServices[typeof(TService)] = service;
            return true;
        }

        /// <summary>
        /// Removes a specified service if it is registered.
        /// </summary>
        /// <param name="service">Service to deregister.</param>
        /// <typeparam name="TService">Service type.</typeparam>
        public void RemoveService<TService>(TService service) where TService : Service
        {
            if (PublicServices.TryGetValue(typeof(TService), out var svc) && svc == service)
                PublicServices.Remove(typeof(TService));
        }

        /// <summary>
        /// Represents a trap signal sent to <see cref="SystemModel.Pulse"/>.
        /// </summary>
        public class TrapSignal
        {
            /// <summary>
            /// Singleton object for this type.
            /// </summary>
            public static readonly TrapSignal Singleton = new();
        }

        /// <summary>
        /// Resets a vulnerability.
        /// </summary>
        /// <param name="vulnerability">Vulnerability to reset.</param>
        public void ResetVulnerability(VulnerabilityModel vulnerability)
        {
            if (VulnerabilityVersions.TryGetValue(vulnerability, out int version))
                VulnerabilityVersions[vulnerability] = version + 1;
        }

        /// <summary>
        /// Resets firewall.
        /// </summary>
        public void ResetFirewall() => FirewallVersion++;

        /// <summary>
        /// Resets proxy.
        /// </summary>
        public void ResetProxy() => ProxyVersion++;

        /// <summary>
        /// Updates an existing crack state based on local reset versions.
        /// </summary>
        /// <param name="crackState">Crack state to update.</param>
        public void UpdateCrackState(ShellProcess.CrackState crackState)
        {
            foreach ((var key, int value) in VulnerabilityVersions)
                if (!crackState.OpenVulnerabilities.TryGetValue(key, out int ver) || ver != value)
                    crackState.OpenVulnerabilities.Remove(key);

            if (crackState.FirewallVersion != FirewallVersion)
            {
                crackState.FirewallSolution = FixedFirewall ?? ServerUtil.GeneratePassword(FirewallIterations);
                crackState.FirewallIterations = 0;
                crackState.FirewallSolved = false;
                crackState.FirewallVersion = FirewallVersion;
            }

            if (crackState.ProxyVersion != ProxyVersion)
            {
                crackState.ProxyClocks = 0;
                crackState.ProxyVersion = ProxyVersion;
            }
        }

        /// <summary>
        /// Opens the specified vulnerability.
        /// </summary>
        /// <param name="crackState">Crack state to update.</param>
        /// <param name="vulnerability">Vulnerability to open.</param>
        public void OpenVulnerability(ShellProcess.CrackState crackState, VulnerabilityModel vulnerability)
        {
            if (VulnerabilityVersions.TryGetValue(vulnerability, out int ver))
                crackState.OpenVulnerabilities[vulnerability] = ver;
        }

        /// <summary>
        /// Enumerates processes visible by login or all processes if none is specified.
        /// </summary>
        /// <param name="loginModel">Login to restrict view to or null.</param>
        /// <param name="pid">PID to limit search by.</param>
        /// <param name="parentPid">Parent PID to limit search by.</param>
        /// <returns>Enumeration over matching processes.</returns>
        public IEnumerable<Process> Ps(LoginModel? loginModel, uint? pid, uint? parentPid)
        {
            var src = loginModel != null
                ? Processes.Values.Where(p => p.ProcessContext is ProgramContext pc && pc.Login == loginModel)
                : Processes.Values;
            src = pid.HasValue ? src.Where(p => p.ProcessContext.Pid == pid.Value) : src;
            src = parentPid.HasValue ? src.Where(p => p.ProcessContext.ParentPid == parentPid.Value) : src;
            return src.OrderBy(p => p.ProcessContext.Pid);
        }

        /// <summary>
        /// Attempts to get a file with a specified login.
        /// </summary>
        /// <param name="path">Desired file path.</param>
        /// <param name="login">Login to check with.</param>
        /// <param name="result">Access result.</param>
        /// <param name="closest">Closest filepath with a readable parent.</param>
        /// <param name="readable">Closest readable file, if any.</param>
        /// <param name="caseInsensitive">If true, use case-insensitive filename matching.</param>
        /// <param name="hidden">If true, checks for hidden files.</param>
        /// <returns>True if file and all parents are readable.</returns>
        public bool TryGetFile(string path, LoginModel login, out ReadAccessResult result, out string closest,
            [NotNullWhen(true)] out FileModel? readable, bool caseInsensitive = false, bool? hidden = false)
        {
            path = Executable.GetNormalized(path);
            closest = GetClosestWithReadableParent(path, login, out readable, caseInsensitive, hidden);
            var comparison = caseInsensitive
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;
            result = readable != null && readable.FullPath != closest ? ReadAccessResult.NotReadable :
                readable == null || !readable.FullPath.Equals(path, comparison) ? ReadAccessResult.NoExist :
                ReadAccessResult.Readable;
#pragma warning disable 8762
            return result == ReadAccessResult.Readable;
#pragma warning restore 8762
        }

        /// <summary>
        /// Gets the closest file that has a readable parent.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <param name="login">Login to test against.</param>
        /// <param name="readable">Closest readable file, if any.</param>
        /// <param name="caseInsensitive">If true, use case-insensitive filename matching.</param>
        /// <param name="hidden">If true, checks for hidden files.</param>
        /// <returns>Closest file with a readable parent (may be the file itself) or null.</returns>
        public string GetClosestWithReadableParent(string path, LoginModel login, out FileModel? readable,
            bool caseInsensitive = false, bool? hidden = false)
        {
            var (nPath, nName) = Executable.GetDirectoryAndName(path);
            return GetClosestWithReadableParentInternal(nPath, nName, login, out readable, caseInsensitive, hidden);
        }

        private string GetClosestWithReadableParentInternal(string nPath, string nName, LoginModel login,
            out FileModel? readable, bool caseInsensitive, bool? hidden)
        {
            readable = null;
            if (nPath == "/" && nName == "") return "/";
            string topPath;
            FileModel? topReadable;
            if (nPath != "/")
            {
                var (pPath, pName) = Executable.GetDirectoryAndName(nPath, false);
                topPath = GetClosestWithReadableParentInternal(pPath, pName, login, out topReadable, false, hidden);
            }
            else
            {
                topReadable = null;
                topPath = "/";
            }

            var comparison = caseInsensitive
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;
            var self = Files
                .FirstOrDefault(f =>
                    (hidden == null || f.Hidden == hidden) && f.Path == nPath && f.Name.Equals(nName, comparison));
            if (self != null && (topReadable == null || topReadable.FullPath == nPath))
            {
                readable = self.CanRead(login) ? self : topReadable;
                return self.FullPath;
            }

            readable = topReadable;
            return topPath;
        }

        /// <summary>
        /// Attempts to get file/folder with specified path.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <param name="hidden">If true, checks for hidden files.</param>
        /// <returns>File or null if not found.</returns>
        public FileModel? GetFileSystemEntry(string path, bool hidden = false)
        {
            path = Executable.GetNormalized(path);
            return Files.FirstOrDefault(f => f.Hidden == hidden && f.FullPath == path);
        }

        /// <summary>
        /// Enumerates all files in a specified directory.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <param name="hidden">If true, checks for hidden files.</param>
        /// <returns>Enumeration over files in directory.</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown if specified directory does not exist.</exception>
        public IEnumerable<FileModel> EnumerateDirectory(string path, bool hidden = false)
        {
            path = Executable.GetNormalized(path);
            var (nPath, nName) = Executable.GetDirectoryAndName(path);
            if (nPath == "/" && nName == "")
                return Files.Where(f => f.Hidden == hidden && f.Path == nPath);
            string bPath = Executable.Combine(nPath, nName);
            return Files.Where(f => f.Hidden == hidden && f.Path == nPath)
                .Any(f => f.Name == nName && f.Kind == FileModel.FileKind.Folder)
                ? Files.Where(f => f.Hidden == hidden && f.Path == bPath)
                : throw new DirectoryNotFoundException($"Directory {path} not found.");
        }

        /// <summary>
        /// Gets first available PID in range from 1 to <see cref="int.MaxValue"/>. Should realistically never be null.
        /// </summary>
        /// <returns>Process ID or null if all are exhausted.</returns>
        public uint? GetAvailablePid() =>
            (uint?)Enumerable.Range(1, int.MaxValue).FirstOrDefault(v => Processes.Keys.All(k => k != v));

        /// <summary>
        /// Gets currently used memory.
        /// </summary>
        /// <returns>Used memory.</returns>
        public long GetUsedMemory() => Processes.Select(p => p.Value.ProcessContext.Memory).Sum();

        /// <summary>
        /// Get currently used disk space of system.
        /// </summary>
        /// <returns>Used disk space.</returns>
        public int GetUsedDiskSpace()
        {
            return Files.Count;
            //return Files.Count(f => f.Kind != FileModel.FileKind.Folder);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"System {Key} SG {SpawnGroup} {Name}@{Util.UintToAddress(Address)} (owned by {Owner})";
        }
    }
}
