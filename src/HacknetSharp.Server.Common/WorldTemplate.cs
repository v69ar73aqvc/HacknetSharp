﻿using HacknetSharp.Server.Common.Models;

namespace HacknetSharp.Server.Common
{
    public class WorldTemplate
    {
        public string? Name { get; set; }
        public string? SystemTemplate { get; set; }
        public string? StartupProgram { get; set; }
        public string? StartupCommandLine { get; set; }

        public void ApplyTemplate(WorldModel model)
        {
            // TODO apply template
        }
    }
}
