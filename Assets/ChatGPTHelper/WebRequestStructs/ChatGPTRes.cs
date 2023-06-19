using System;
using System.Collections.Generic;

namespace UGC.UGCTinderAI.API {
    [Serializable]
    public struct ChatGPTRes
    {
        public List<ChatGPTChoices> choices;
    }
}
