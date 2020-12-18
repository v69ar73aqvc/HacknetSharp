﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HacknetSharp.Server.Models;

namespace HacknetSharp.Server.CorePrograms
{
    /// <inheritdoc />
    [ProgramInfo("core:mv", "mv", "move (rename) files",
        "move source files to specified destination",
        "<source>... <dest>", false)]
    public class MvProgram : Program
    {
        /// <inheritdoc />
        public override IEnumerator<YieldToken?> Run(ProgramContext context) => InvokeStatic(context);

        private static IEnumerator<YieldToken?> InvokeStatic(ProgramContext context)
        {
            var user = context.User;
            if (!user.Connected) yield break;
            var system = context.System;
            var argv = context.Argv;
            if (argv.Length < 3)
            {
                user.WriteEventSafe(
                    Output("At least 2 operands are required by this command:\n\t<source>... <dest>\n"));
                user.FlushSafeAsync();
                yield break;
            }

            try
            {
                string target;
                string workDir = context.Shell.WorkingDirectory;
                try
                {
                    target = GetNormalized(Combine(workDir, argv[^1]));
                }
                catch
                {
                    yield break;
                }

                var spawn = context.World.Spawn;
                var login = context.Login;
                foreach (var input in argv[1..^1])
                {
                    string inputFmt = GetNormalized(Combine(workDir, input));
                    if (system.TryGetFile(inputFmt, login, out var result, out var closestStr, out var closest))
                    {
                        // Prevent moving common root to subdirectory
                        if (GetPathInCommon(inputFmt, target) == inputFmt)
                        {
                            user.WriteEventSafe(Output($"{inputFmt}: Cannot move to {target}\n"));
                            user.FlushSafeAsync();
                            yield break;
                        }

                        try
                        {
                            var targetExisting =
                                system.Files.FirstOrDefault(f => f.Hidden == false && f.FullPath == target);
                            string lclTarget;
                            string lclName;
                            if (target == "/" || argv.Length != 3 || targetExisting != null
                                && targetExisting.Kind == FileModel.FileKind.Folder)
                            {
                                lclTarget = target;
                                lclName = closest.Name;
                            }
                            else
                            {
                                lclTarget = GetDirectoryName(target) ?? "/";
                                lclName = GetFileName(target);
                            }

                            spawn.MoveFile(closest, lclName, lclTarget, login);
                        }
                        catch (IOException e)
                        {
                            user.WriteEventSafe(Output($"{e.Message}\n"));
                            user.FlushSafeAsync();
                            yield break;
                        }
                    }
                    else
                        switch (result)
                        {
                            case ReadAccessResult.NotReadable:
                                user.WriteEventSafe(Output($"{closestStr}: Permission denied\n"));
                                user.FlushSafeAsync();
                                yield break;
                            case ReadAccessResult.NoExist:
                                user.WriteEventSafe(Output($"{inputFmt}: No such file or directory\n"));
                                user.FlushSafeAsync();
                                yield break;
                        }
                }
            }
            catch (Exception e)
            {
                user.WriteEventSafe(Output($"{e.Message}\n"));
                user.FlushSafeAsync();
            }
        }
    }
}
