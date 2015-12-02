using UnityEngine;
using System.Collections;


namespace TwoTogether.LevelExceptions {

    public class UnsupportedLevelVersion : System.Exception {

        public UnsupportedLevelVersion() : base() { }
        public UnsupportedLevelVersion(string message) : base(message) { }
        public UnsupportedLevelVersion(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected UnsupportedLevelVersion(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }

    }

}
