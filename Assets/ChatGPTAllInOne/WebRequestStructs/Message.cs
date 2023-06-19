using System.Net.Mime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
namespace ChatgptAllInOne
{

    [Serializable]
    public class Message
    {
        public string role;
        [TextArea(10, 10)]
        public string content;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FunctionCall function_call;
        public Message(){
            role = "system";
            content = "";
        }
        public Message(Message message)
        {
            role = message.role;
            content = message.content;
            name = message.name;
            function_call = message.function_call;
        }
        public Message(string r, string c, string n = null)
        {
            role = r;
            content = c;
            name = n;
        }
        public Message(Speaker r, string c, string n = null)
        {
            role = r.ToString();
            content = c;
            name = n;
        }

        public Speaker GetSpeaker()
        {
            return (Speaker)Enum.Parse(typeof(Speaker), role);
        }
    }

    public enum Speaker
    {
        system,
        user,
        assistant,
        function,
    }

}