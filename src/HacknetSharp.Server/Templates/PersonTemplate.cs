using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HacknetSharp.Server.Models;

namespace HacknetSharp.Server.Templates
{
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PersonTemplate
    {
        public List<string> Usernames { get; set; } = new List<string>();
        public List<string> Passwords { get; set; } = new List<string>();
        public string? AddressRange { get; set; }
        public List<string> EmailProviders { get; set; } = new List<string>();
        public List<string> SystemTemplates { get; set; } = new List<string>();
        public int FleetMin { get; set; }
        public int FleetMax { get; set; }
        public List<string>? FleetSystemTemplates { get; set; } = new List<string>();

        [ThreadStatic] private static Random? _random;

        private static Random Random => _random ??= new Random();

        public void Generate(IServerDatabase database, ISpawn spawn, TemplateGroup templates, WorldModel world,
            string? addressRange)
        {
            if (Usernames.Count == 0) throw new InvalidOperationException($"{nameof(Usernames)} is empty.");
            if (Passwords.Count == 0) throw new InvalidOperationException($"{nameof(Passwords)} is empty.");
            if (SystemTemplates.Count == 0) throw new InvalidOperationException($"{nameof(SystemTemplates)} is empty.");
            if (FleetMin != 0 && (FleetSystemTemplates?.Count ?? 0) == 0)
                throw new InvalidOperationException($"{nameof(FleetSystemTemplates)} is empty.");
            string systemTemplateName = SystemTemplates[Random.Next() % SystemTemplates.Count];
            if (!templates.SystemTemplates.TryGetValue(systemTemplateName, out var systemTemplate))
                throw new KeyNotFoundException($"Unknown template {systemTemplateName}");
            string username = Usernames[Random.Next() % Usernames.Count];
            string password = Passwords[Random.Next() % Passwords.Count];

            var range = new IPAddressRange(addressRange ??
                                           AddressRange ?? systemTemplate.AddressRange ??
                                           Constants.DefaultAddressRange);
            var person = spawn.Person(database, world, username, username);
            var (hash, salt) = ServerUtil.HashPassword(password);
            var system = spawn.System(database, world, systemTemplate, person, hash, salt, range);
            var systems = new List<SystemModel> {system};
            person.DefaultSystem = system.Key;
            if (FleetSystemTemplates == null) return;
            int count = Random.Next(FleetMin, FleetMax + 1);
            bool fixedRange = addressRange != null || AddressRange != null;
            for (int i = 0; i < count; i++)
            {
                string fleetSystemTemplateName =
                    FleetSystemTemplates[Random.Next() % FleetSystemTemplates.Count];
                if (!templates.SystemTemplates.TryGetValue(fleetSystemTemplateName, out var fleetSystemTemplate))
                    throw new KeyNotFoundException($"Unknown template {fleetSystemTemplateName}");
                systems.Add(spawn.System(database, world, fleetSystemTemplate, person, hash, salt,
                    fixedRange
                        ? range
                        : new IPAddressRange(fleetSystemTemplate.AddressRange ?? Constants.DefaultAddressRange)));
            }

            for (int i = 0; i < systems.Count; i++)
            for (int j = i + 1; j < systems.Count; j++)
            {
                spawn.Connection(database, systems[i], systems[j]);
                spawn.Connection(database, systems[j], systems[i]);
            }
        }
    }
}
