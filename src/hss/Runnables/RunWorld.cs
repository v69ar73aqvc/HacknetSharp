﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using HacknetSharp;
using HacknetSharp.Server;
using HacknetSharp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace hss.Runnables
{
    [Verb("world", HelpText = "Manage worlds.")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    internal class RunWorld : Executor.IRunnable
    {
        [Verb("create", HelpText = "Create world.")]
        private class Create : Executor.ISelfRunnable
        {
            [Value(0, MetaName = "name", HelpText = "World name.", Required = true)]
            public string Name { get; set; } = null!;

            [Value(1, MetaName = "template", HelpText = "World template name.", Required = true)]
            public string Template { get; set; } = null!;

            public async Task<int> Run(Executor executor)
            {
                var factory = executor.ServerDatabaseContextFactory;
                await using var ctx = factory.CreateDbContext(Array.Empty<string>());

                var xizt = ctx.Set<WorldModel>().FirstOrDefault(w => w.Name == Name);
                if (xizt != null)
                {
                    Console.WriteLine("A world with specified name already exists.");
                    return 0;
                }

                var templates = new TemplateGroup();
                HssUtil.LoadTemplates(templates, HssConstants.ContentFolder);
                if (!templates.WorldTemplates.TryGetValue(Template, out var template))
                {
                    Console.WriteLine("Could not find a template with the specified name.");
                    return 89;
                }

                var db = new ServerDatabase(ctx);
                new Spawn(db).World(Name, templates, template);
                await db.SyncAsync().Caf();
                return 0;
            }
        }

        [Verb("remove", HelpText = "Remove worlds.")]
        private class Remove : Executor.ISelfRunnable
        {
            [Value(0, MetaName = "names", HelpText = "World names.")]
            public IEnumerable<string> Names { get; set; } = null!;

            [Option('a', "all", HelpText = "Remove all worlds.")]
            public bool All { get; set; }

            public async Task<int> Run(Executor executor)
            {
                var factory = executor.ServerDatabaseContextFactory;
                await using var ctx = factory.CreateDbContext(Array.Empty<string>());
                var names = new HashSet<string>(Names);

                var worlds = await (All
                    ? ctx.Set<WorldModel>()
                    : ctx.Set<WorldModel>().Where(u => names.Contains(u.Name))).ToListAsync().Caf();

                foreach (var world in worlds)
                    Console.WriteLine($"{world.Name}:{world.Key} ({world.Name})");

                if (!Util.Confirm("Are you sure you want to proceed with deletion?")) return 0;

                var db = new ServerDatabase(ctx);
                var spawn = new Spawn(db);
                foreach (var world in worlds) spawn.RemoveWorld(world);
                await db.SyncAsync().Caf();
                return 0;
            }
        }

        [Verb("list", HelpText = "List worlds.")]
        private class List : Executor.ISelfRunnable
        {
            [Value(0, MetaName = "names", HelpText = "World names.")]
            public IEnumerable<string> Names { get; set; } = null!;

            [Option('a', "all", HelpText = "List all worlds.")]
            public bool All { get; set; }

            public async Task<int> Run(Executor executor)
            {
                var factory = executor.ServerDatabaseContextFactory;
                await using var ctx = factory.CreateDbContext(Array.Empty<string>());
                var names = new HashSet<string>(Names);

                var worlds = await (All
                    ? ctx.Set<WorldModel>()
                    : ctx.Set<WorldModel>().Where(u => names.Contains(u.Name))).ToListAsync().Caf();

                foreach (var world in worlds)
                    Console.WriteLine($"{world.Name}:{world.Key} ({world.Label})");
                return 0;
            }
        }

        public async Task<int> Run(Executor executor, IEnumerable<string> args) =>
            await Parser.Default.ParseArguments<Create, Remove, List>(args)
                .MapResult<Executor.ISelfRunnable, Task<int>>(x => x.Run(executor),
                    x => Task.FromResult(1)).Caf();
    }
}
