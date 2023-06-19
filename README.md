# *ChatGPT Unity All In One* is All Your Need
This Package aims to provide a easy-for-use ChatGPT Unity Wrapper that supports all Chatgpt features, support latest **function_call**, **Chatgpt 3.5 turbo 16k** and  **Chatgpt 4 32k**
## Features 💡
- [x] change between different models: Chatgpt 3.5 Turbo, Chatgpt 3.5 Turbo 16k, Chatgpt 4, Chatgpt 4 32k, davinci, curie
- [x] support basic chatgpt api usage: "role", "content", "name", "function", "tempeature", "max_token", etc
- [x] support function call
- [ ] support stream 


## Installation
### Direclty Download 
Download the unitypackage from Release, or
Direct download whole repo as a zip, unzip it and copy the whole Assets/ChatgptAllInOne Folder to your Assets folder.
You can also clone the whole repo by typing followings in terminal
```
git clone https://github.com/CharlieDreemur/ChatgptUnityAllInOne.git
```
### Install via Unity Package Manager
Go to package manager and select **add package from Git Url**, type this: https://github.com/CharlieDreemur/ChatgptUnityAllInOne.git

Or add this to **manifest.json**: "com.charliedreemur.chatgptall-in-one": "https://github.com/CharlieDreemur/ChatgptUnityAllInOne.git"


## Guide 📖
### Basic
For simple chat with Chatgpt, select a model, enter your api (apply here: https://openai.com/blog/introducing-chatgpt-and-whisper-apis), and add a listener to the Chatgpt Response Event to get the reponse message, you can also view my example in the example folder.

Note: For using Chatgpt 4 model, you have to first have the access for its API.

<img width="304" alt="Type your API here" src="https://github.com/CharlieDreemur/ChatgptUnityAllInOne/assets/91376582/7a5eac34-f747-4521-a4ac-3591c52f2c4c">

<img width="374" alt="DemoJPG" src="https://github.com/CharlieDreemur/ChatgptUnityAllInOne/assets/91376582/a8e3f77c-9487-4559-85ac-9bfbaf6b7e4d">

### Message
![image](https://github.com/CharlieDreemur/ChatgptUnityAllInOne/assets/91376582/64d3adec-44c2-4c52-97ba-808e5658c8fd)
According to the OpenAI's api reference (https://platform.openai.com/docs/api-reference/chat/create#chat/create-functions), in one message you have four property: "role", "content", "name", "function_call".
These are all defined in Message, and if it's null it will not be send to Chatgpt by using JsonIgnore.

### Function Call 🤗
![image](https://github.com/CharlieDreemur/ChatgptUnityAllInOne/assets/91376582/a74a9a85-ce02-4691-9826-e6e9fa265816)
To let Chatgpt invoke your defined functions, you have to give all funciton/arguments' description to the Chatgpt, you have to register your function's info in the *FunctionCallController* class in order for the correct use of function call.

For example:
You want to let Chatgpt invoke *GetCurrentWeather* to get the weather of a specific location:
You first defined your function:
```
//Test Method
        public void GetCurrentWeather(string location, string format)
        {
            string str = $@"The current weather at {location} is 20 degrees {format}";
            Debug.Log("GetCurrentWeather: " + str);
            //Resend to the chatgpt to tell the function's result
            chatGPTConversation.SendAsFunction(str);
        }
```
Then you need to register your method info in Class FunctionCallController Awake()
```
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
```
Noted that your function should only be public, if it's a static method it can be directly call by using InvokeStatic(). If it's a method in an instance, you have to first get the instance and then invoke instance.InvokeInstance() to invoke the corrsponding method according to the Chatgpt's response
```
FunctionCall functionCall = message.function_call;
        if (functionCall != null)
            {
                functionCallController.InvokeInstance(functionCall);
                _chat.AppendMessage(message);
                return;
            }
```
## Credits
This package largely expand base on repo https://github.com/GraesonB/ChatGPT-Wrapper-For-Unity



