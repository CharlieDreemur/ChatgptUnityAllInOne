using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace ChatgptAllInOne {
    public struct ProxyReq
    {
        public string model;
        public List<Message> messages;
        public int max_tokens;
        public float temperature;
    }
}
