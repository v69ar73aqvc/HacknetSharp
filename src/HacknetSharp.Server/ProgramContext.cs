﻿using System;
using HacknetSharp.Server.Models;

namespace HacknetSharp.Server
{
    public class ProgramContext : ExecutableContext
    {
        public IPersonContext User { get; set; } = null!;
        public PersonModel Person { get; set; } = null!;
        public LoginModel Login { get; set; } = null!;
        public Guid OperationId { get; set; }
        public bool Disconnect { get; set; }
        public InvocationType Type { get; set; }
        public int ConWidth { get; set; } = -1;
        public bool IsAI { get; set; }

        public enum InvocationType
        {
            Standard,
            Connect,
            StartUp
        }
    }
}