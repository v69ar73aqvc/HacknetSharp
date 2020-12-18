﻿using System.Collections.Generic;

namespace HacknetSharp.Server
{
    /// <summary>
    /// Represents an executable that runs in the background.
    /// </summary>
    public abstract class Service : Executable
    {
        /// <summary>
        /// Runs this executable with the given context.
        /// </summary>
        /// <param name="context">Context to use with this execution.</param>
        /// <returns>Enumerator that divides execution steps.</returns>
        public abstract IEnumerator<YieldToken?> Run(ServiceContext context);

        /// <summary>
        /// Tells service on given context to stop execution.
        /// </summary>
        /// <param name="context">Service context to operate with.</param>
        /// <returns>False if service refuses to shutdown.</returns>
        public abstract bool OnShutdown(ServiceContext context);
    }
}
