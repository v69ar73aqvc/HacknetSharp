using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HacknetSharp.Server.Models
{
    /// <summary>
    /// Represents a person who owns systems.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PersonModel : WorldMember<Guid>
    {
        /// <summary>
        /// Proper name.
        /// </summary>
        public virtual string Name { get; set; } = null!;

        /// <summary>
        /// Username.
        /// </summary>
        public virtual string UserName { get; set; } = null!;

        /// <summary>
        /// Associated player user, if applicable.
        /// </summary>
        public virtual UserModel? User { get; set; }

        /// <summary>
        /// Primary system owned by the user.
        /// </summary>
        public virtual Guid DefaultSystem { get; set; }

        /// <summary>
        /// Sequence of active shells for the user, from oldest to newest.
        /// </summary>
        public List<ShellProcess> ShellChain { get; set; } = new();

        /// <summary>
        /// If true, this user has completed registration.
        /// </summary>
        public virtual bool StartedUp { get; set; }

        /// <summary>
        /// Set of all systems owned by this user.
        /// </summary>
        public virtual HashSet<SystemModel> Systems { get; set; } = null!;

        /// <summary>
        /// Set of all active missions of this user.
        /// </summary>
        public virtual HashSet<MissionModel> Missions { get; set; } = null!;

        /// <summary>
        /// Reboot duration in seconds.
        /// </summary>
        public virtual double RebootDuration { get; set; }

        /// <summary>
        /// System disk capacity.
        /// </summary>
        public virtual int DiskCapacity { get; set; }

        /// <summary>
        /// CPU cycles required to crack proxy.
        /// </summary>
        public virtual double ProxyClocks { get; set; }

        /// <summary>
        /// Proxy cracking speed.
        /// </summary>
        public virtual double ClockSpeed { get; set; }

        /// <summary>
        /// System memory (bytes).
        /// </summary>
        public virtual long SystemMemory { get; set; }

        /// <summary>
        /// Tag for lookup.
        /// </summary>
        public virtual string? Tag { get; set; }

        /// <summary>
        /// Logical group this entity was spawned in.
        /// </summary>
        public virtual Guid SpawnGroup { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Person {Key} SG {SpawnGroup} {UserName} ({Name})";
        }
    }
}
