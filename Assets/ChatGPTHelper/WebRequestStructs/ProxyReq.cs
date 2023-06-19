using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace UGC.UGCTinderAI.API {
    public struct ProxyReq
    {
        public string model;
        public List<Message> messages;
        public int max_tokens;
        public float temperature;
    }
}
