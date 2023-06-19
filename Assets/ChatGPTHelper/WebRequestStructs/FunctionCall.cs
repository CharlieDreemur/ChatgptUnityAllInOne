using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using UGC.UGCTinderAI.Action;
namespace UGC.UGCTinderAI.API
{
    public class FunctionCall
    {
        public string name;
        public string arguments;
        public Dictionary<string, JToken> parsedArguments { get; set; }

    }
}