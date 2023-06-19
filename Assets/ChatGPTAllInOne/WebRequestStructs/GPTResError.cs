using System;
using System.Collections.Generic;

namespace ChatgptAllInOne {
    [Serializable]
    public struct ChatGPTResError
    {
        public Error error;

    }

    [Serializable]
    public struct Error {
      public string message;
    }
}