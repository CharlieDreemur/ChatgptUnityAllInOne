using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChatgptAllInOne;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace ChatgptAllInOne
{
    public class FunctionCallController : MonoBehaviour
    {

        [SerializeField]
        private ChatGPTConversation chatGPTConversation;
        [SerializeField]
        private List<string> registeredFunctions;
        public static void InvokeStatic(FunctionCall functionCall)
        {

            functionCall.parsedArguments = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(functionCall.arguments);
            // Get the method to invoke using reflection, only get the public/static methods
            MethodInfo method = typeof(FunctionCallController).GetMethod(functionCall.name, BindingFlags.Public | BindingFlags.Static);
            InvokeHelper(functionCall, method);
        }

        public void InvokeInstance(FunctionCall functionCall)
        {
            functionCall.parsedArguments = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(functionCall.arguments);
            MethodInfo method = typeof(FunctionCallController).GetMethod(functionCall.name, BindingFlags.Public | BindingFlags.Instance);
            InvokeHelper(functionCall, method, this);
        }

        private static void InvokeHelper(FunctionCall functionCall, MethodInfo method, object instance = null)
        {
            if (method == null)
            {
                Debug.LogError("Method not found: " + functionCall.name + " Please Check the name of the method and the class it belongs");
                return;
            }
            // Get the parameters of the method
            ParameterInfo[] parameters = method.GetParameters();

            // Map the arguments dictionary to an arguments array
            object[] args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                string paramName = parameters[i].Name;
                if (functionCall.parsedArguments.ContainsKey(paramName))
                {
                    // Convert the JToken to the appropriate type
                    args[i] = functionCall.parsedArguments[paramName].ToObject(parameters[i].ParameterType);
                }
                else
                {
                    // Handle case where required argument is missing
                }
            }
            // Invoke the method
            method.Invoke(instance, args);
        }

        private void Awake()
        {
            MethodRegistry.Methods["GetCurrentWeather"] = new MethodMetadata
            {
                Description = "Get the current weather",
                Parameters = new Dictionary<string, ParameterMetadata>
                {
                    ["location"] = new ParameterMetadata
                    {
                        Description = "The city and state, e.g. San Francisco, CA",
                        IsRequired = true,
                        // No enum values for this parameter
                    },
                    ["format"] = new ParameterMetadata
                    {
                        Description = "The temperature unit to use. Infer this from the users location.",
                        IsRequired = true,
                        EnumValues = new List<string> { "celsius", "fahrenheit" },
                    },
                },
            };
            
        }

        //Test Method
        public void GetCurrentWeather(string location, string format)
        {
            string str = $@"The current weather at {location} is 20 degrees {format}";
            Debug.Log("GetCurrentWeather: " + str);
            //Resend to the chatgpt to tell the function's result
            chatGPTConversation.SendAsFunction(str);
        }

       

    }
}
