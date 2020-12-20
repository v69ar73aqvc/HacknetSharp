﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using HacknetSharp.Server.Models;

namespace HacknetSharp.Server.CorePrograms
{
    /// <inheritdoc />
    [ProgramInfo("core:map", "map", "map known systems",
        "list all known systems",
        "[filter]", false)]
    public class MapProgram : Program
    {
        /// <inheritdoc />
        public override IEnumerator<YieldToken?> Run(ProgramContext context) => InvokeStatic(context);

        private static IEnumerator<YieldToken?> InvokeStatic(ProgramContext context)
        {
            var user = context.User;
            if (!user.Connected) yield break;
            if (!context.Login.Admin)
            {
                user.WriteEventSafe(Output("Permission denied."));
                user.FlushSafeAsync();
                yield break;
            }

            string[] argv = context.Argv;
            IEnumerable<SystemModel> systems;
            if (argv.Length != 1)
            {
                var filter = PathFilter.GenerateFilter(argv.Skip(1));
                systems = context.System.KnownSystems.Select(s => s.To)
                    .Where(s => filter.Test(Util.UintToAddress(s.Address)) || filter.Test(s.Name));
            }
            else
                systems = context.System.KnownSystems.Select(s => s.To);

            var sb = new StringBuilder();
            foreach (var s in systems)
            {
                sb.Append($"{Util.UintToAddress(s.Address),16}")
                    .Append(' ')
                    .Append(s.Name)
                    .Append('\n');
            }

            user.WriteEventSafe(Output(sb.ToString()));
            user.FlushSafeAsync();
        }
    }
}
