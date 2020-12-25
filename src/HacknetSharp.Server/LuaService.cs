using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace HacknetSharp.Server
{
    /// <summary>
    /// Represents a lua-function-backed service.
    /// </summary>
    [IgnoreRegistration]
    public class LuaService : Service
    {
        private readonly Coroutine _coroutine;

        /// <summary>
        /// Creates a lua service from the specified coroutine.
        /// </summary>
        /// <param name="coroutine">Coroutine to use.</param>
        public LuaService(Coroutine coroutine)
        {
            _coroutine = coroutine;
        }

        /// <inheritdoc />
        public override IEnumerator<YieldToken?> Run()
        {
            World.ScriptManager.SetGlobal("system", System);
            World.ScriptManager.SetGlobal("self", this);
            World.ScriptManager.SetGlobal("login", Login);
            World.ScriptManager.SetGlobal("argv", Argv);
            World.ScriptManager.SetGlobal("argc", Argv.Length);
            try
            {
                DynValue? result;
                result = _coroutine.Resume();
                if (_coroutine.State == CoroutineState.Suspended)
                    yield return result?.ToObject() as YieldToken;
            }
            finally
            {
                World.ScriptManager.ClearGlobal("system");
                World.ScriptManager.ClearGlobal("self");
                World.ScriptManager.ClearGlobal("login");
                World.ScriptManager.ClearGlobal("argv");
                World.ScriptManager.ClearGlobal("argc");
            }
        }
    }
}
