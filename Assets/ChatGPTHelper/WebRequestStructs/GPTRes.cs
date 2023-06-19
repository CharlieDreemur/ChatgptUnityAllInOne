using System;
using System.Collections.Generic;

namespace UGC.UGCTinderAI.API {
    [Serializable]
    public struct GPTRes
    {
        public string id;
        public List<GPTChoices> choices;

    }
}
