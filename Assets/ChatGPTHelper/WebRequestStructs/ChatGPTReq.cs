using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace UGC.UGCTinderAI.API {
    public struct ChatGPTReq
    {
        public string model;
        public List<Message> messages;
        public List<FunctionInfo> functions;
        public string function_call;
        public int max_tokens;
        public float temperature;
    }
}
