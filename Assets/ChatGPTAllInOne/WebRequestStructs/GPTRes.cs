using System;
using System.Collections.Generic;

namespace ChatgptAllInOne {
    [Serializable]
    public struct GPTRes
    {
        public string id;
        public List<GPTChoices> choices;

    }
}
