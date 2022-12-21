using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreBLL
{
    public class GameStoreException : Exception
    {
        public GameStoreException() {}

        public GameStoreException(string message) : base(message) {}

        public GameStoreException(string message, Exception innerException) : base(message, innerException) {}

        protected GameStoreException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}

        public override string Message => "Game Store Exception";
    }
}
