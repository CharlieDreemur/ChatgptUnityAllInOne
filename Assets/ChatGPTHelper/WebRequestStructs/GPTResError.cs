using System;
using System.Collections.Generic;

namespace UGC.UGCTinderAI.API {
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