﻿using System;

namespace MySoft.IoC.Communication.Scs.Server
{
    /// <summary>
    /// Stores client information to be used by an event.
    /// </summary>
    public class ServerClientEventArgs : EventArgs
    {
        /// <summary>
        /// Client that is associated with this event.
        /// </summary>
        public IScsServerClient Channel { get; private set; }

        /// <summary>
        /// Get or set server client count.
        /// </summary>
        public int ConnectCount { get; set; }

        /// <summary>
        /// Creates a new ServerClientEventArgs object.
        /// </summary>
        /// <param name="channel">Client that is associated with this event</param>
        public ServerClientEventArgs(IScsServerClient channel)
        {
            Channel = channel;
        }
    }
}