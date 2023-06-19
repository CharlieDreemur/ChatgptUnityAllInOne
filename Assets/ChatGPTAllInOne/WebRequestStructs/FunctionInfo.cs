using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace ChatgptAllInOne
{
    public struct Parameters
    {
        public string type;
        public Dictionary<string, FunctionProperty> properties;
        public List<string> required;
    }

    public struct FunctionProperty
    {
        public string type;

        //If u don't have a description, don't include it in the json
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description;
        //If u don't have a enum, don't include it in the json
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> @enum;
    }

    public class FunctionInfo
    {
        public string name;
        public string description;
        public Parameters parameters;
        public static FunctionInfo ConvertFunctionToJsonFormat(MethodInfo methodInfo)
        {
            var methodMetadata = MethodRegistry.Methods[methodInfo.Name];

            var functionInfo = new FunctionInfo
            {
                name = methodInfo.Name,
                description = methodMetadata.Description,
                parameters = new Parameters
                {
                    type = "object",
                    properties = new Dictionary<string, FunctionProperty>(),
                    required = new List<string>()
                }
            };

            foreach (var parameter in methodInfo.GetParameters())
            {
                var paramMetadata = methodMetadata.Parameters[parameter.Name];

                functionInfo.parameters.properties.Add(parameter.Name, new FunctionProperty { type = parameter.ParameterType.Name.ToLower(), description = paramMetadata.Description, @enum = paramMetadata.EnumValues });
                if (paramMetadata.IsRequired)
                {
                    functionInfo.parameters.required.Add(parameter.Name);
                }
            }

            return functionInfo;
        }


    }
}