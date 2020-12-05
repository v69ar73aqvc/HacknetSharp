using System;
using System.Collections.Generic;
using HacknetSharp.Server.Models;
using HacknetSharp.Server.Templates;

namespace HacknetSharp.Server
{
    public interface IWorld
    {
        WorldModel Model { get; }
        ISpawn Spawn { get; }
        IServerDatabase Database { get; }
        SystemTemplate PlayerSystemTemplate { get; }
        double Time { get; }
        double PreviousTime { get; }
        void Kill(Process process);
        void ForceReboot(DateTime rebootTime);
        void StartShell(PersonModel personModel, SystemModel systemModel, LoginModel loginModel, string line);
        void StartAICommand(PersonModel personModel, SystemModel systemModel, LoginModel loginModel, string line);
        void StartDaemon(SystemModel systemModel, string line);
        void ExecuteCommand(ProgramContext programContext);
        void RegisterModel<T>(Model<T> model) where T : IEquatable<T>;
        void RegisterModels<T>(IEnumerable<Model<T>> models) where T : IEquatable<T>;
        void DirtyModel<T>(Model<T> model) where T : IEquatable<T>;
        void DirtyModels<T>(IEnumerable<Model<T>> models) where T : IEquatable<T>;
        void DeregisterModel<T>(Model<T> model) where T : IEquatable<T>;
        void DeregisterModels<T>(IEnumerable<Model<T>> models) where T : IEquatable<T>;
    }
}
