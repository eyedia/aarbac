using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacBase
    {
        public RbacBase()
        {

        }

        public delegate void CallbackEventHandler(string message, LogMessageTypes messageType = LogMessageTypes.Info);
        public event CallbackEventHandler Callback;

        /// <summary>
        /// As this will be called many times, lets keep it short, N stands for Notify
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logMessageType"></param>
        protected void N(string message, LogMessageTypes messageType = LogMessageTypes.Info)
        {
            if (Callback != null)
                Callback(message, messageType);
        }

        /// <summary>
        /// As this will be called many times, lets keep it short, NL stands for Notify with new line
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logMessageType"></param>
        protected void NL(string message, LogMessageTypes messageType = LogMessageTypes.Info)
        {
            message += Environment.NewLine;

            if (Callback != null)
                Callback(message, messageType);
        }

    }
}
