﻿using System;
using System.IO;
using Ns;

namespace HacknetSharp.Events.Client
{
    [EventCommand(Command.CS_Command)]
    public class CommandEvent : ClientEvent, IOperation
    {
        public Guid Operation { get; set; }
        public int ConWidth { get; set; } = -1;

        public string Text { get; set; } = null!;

        public override void Serialize(Stream stream)
        {
            stream.WriteGuid(Operation);
            stream.WriteS32(ConWidth);
            stream.WriteUtf8String(Text);
        }

        public override void Deserialize(Stream stream)
        {
            Operation = stream.ReadGuid();
            ConWidth = stream.ReadS32();
            Text = stream.ReadUtf8String();
        }
    }
}
