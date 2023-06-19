using System;
using System.Collections;
using System.Dynamic;
using UnityEngine;
using System.Collections.Generic;
using Reqs;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
namespace ChatgptAllInOne
{

    public class ChatGPTConversation : MonoBehaviour
    {
        [SerializeField]
        private bool _useProxy = false;
        [SerializeField]
        private string _proxyUri = null;

        [SerializeField]
        private string _apiKey = null;

        public enum Model
        {
            ChatGPT,
            Davinci,
            Curie
        }
        [SerializeField]
        public Model _model = Model.ChatGPT;
        private string _selectedModel = null;
        [SerializeField]
        private int _maxTokens = 500;
        [SerializeField]
        private float _temperature = 0.5f;
        [SerializeField]
        private bool isLog = true;

        private string _uri;
        private List<(string, string)> _reqHeaders;


        private Requests requests = new Requests();
        private Prompt _prompt;
        private string _lastUserMsg;
        private string _lastChatGPTMsg;

        [SerializeField]
        private string _chatbotName = "ChatGPT";
        [SerializeField]
        private string _initialPrompt;

        public UnityStringEvent chatGPTResponse = new UnityStringEvent();
        [SerializeField]
        private Chat _chat;
        [SerializeField]
        private string functionCallConfig = "auto";
        private List<FunctionInfo> _functions = new List<FunctionInfo>();
        [SerializeField]
        private FunctionCallController functionCallController;

        private void Start()
        {

            FunctionInfo functionInfo = FunctionInfo.ConvertFunctionToJsonFormat(typeof(FunctionCallController).GetMethod("GetCurrentWeather")); 
            _functions.Add(functionInfo);

        }
        private void OnEnable()
        {

            TextAsset textAsset = Resources.Load<TextAsset>("APIKEY");
            if (textAsset != null)
            {
                _apiKey = textAsset.text;
            }


            _reqHeaders = new List<(string, string)>
            {
                ("Authorization", $"Bearer {_apiKey}"),
                ("Content-Type", "application/json")
            };
            //_initialPrompt = 
            switch (_model)
            {
                case Model.ChatGPT:
                    _chat = new Chat(_initialPrompt);
                    _uri = "https://api.openai.com/v1/chat/completions";
                    _selectedModel = "gpt-3.5-turbo-0613";
                    break;
                case Model.Davinci:
                    _prompt = new Prompt(_chatbotName, _initialPrompt);
                    _uri = "https://api.openai.com/v1/completions";
                    _selectedModel = "text-davinci-003";
                    break;
                case Model.Curie:
                    _prompt = new Prompt(_chatbotName, _initialPrompt);
                    _uri = "https://api.openai.com/v1/completions";
                    _selectedModel = "text-curie-001";
                    break;
            }
        }

        public void ResetChat(string initialPrompt)
        {
            switch (_model)
            {
                case Model.ChatGPT:
                    _chat = new Chat(initialPrompt);
                    break;
                default:
                    _prompt = new Prompt(_chatbotName, initialPrompt);
                    break;
            }
        }

        public void SendAsUser(string message)
        {
            SendToChatGPT(Speaker.user, message);
        }
        public void SendAsSystem(string message)
        {
            SendToChatGPT(Speaker.system, message);
        }
        //No method name is needed since it can automatically get the caller's name
        public void SendAsFunction(string message, [CallerMemberName] string methodName = "")
        {
            SendToChatGPT(Speaker.function, message, methodName);
        }
        
        public void SendAsFunctionWithMethodName(string message,  string methodName){
            SendToChatGPT(Speaker.function, message, methodName);
        }
        
        [ContextMenu("Send Current Chat")]
        private void SendCurrentChat()
        {
            if (_model == Model.ChatGPT)
            {
                if (_useProxy)
                {
                    ProxyReq proxyReq = new ProxyReq();
                    proxyReq.max_tokens = _maxTokens;
                    proxyReq.temperature = _temperature;
                    proxyReq.messages = new List<Message>(_chat.CurrentChat);
                    string proxyJson = JsonConvert.SerializeObject(proxyReq);
                    if (isLog) Debug.Log("ProxySend: " + proxyJson);
                    StartCoroutine(requests.PostReq<ChatGPTRes>(_proxyUri, proxyJson, ResolveChatGPT, _reqHeaders));
                }
                else
                {
                    ChatGPTReq chatGPTReq = GetChatGPTReq();
                    string chatGPTJson = JsonConvert.SerializeObject(chatGPTReq);
                    if (isLog) Debug.Log("Send: " + chatGPTJson);
                    StartCoroutine(requests.PostReq<ChatGPTRes>(_uri, chatGPTJson, ResolveChatGPT, _reqHeaders));
                }

            }
            else
            {
                Debug.LogWarning("Only ChatGPT model supports this method");
            }
        }
        public void SendToChatGPT(Speaker messageType, string message, string name = null)
        {

            _lastUserMsg = message;
            if (isLog) Debug.Log(_lastUserMsg);
            if (_model == Model.ChatGPT)
            {
                if (_useProxy)
                {
                    ProxyReq proxyReq = new ProxyReq();
                    proxyReq.max_tokens = _maxTokens;
                    proxyReq.temperature = _temperature;
                    //Deep copy
                    proxyReq.messages = new List<Message>(_chat.CurrentChat);
                    proxyReq.messages.Add(new Message(messageType, message, name));


                    string proxyJson = JsonUtility.ToJson(proxyReq);
                    if (isLog) Debug.Log("ProxySend: " + proxyJson);
                    StartCoroutine(requests.PostReq<ChatGPTRes>(_proxyUri, proxyJson, ResolveChatGPT, _reqHeaders));
                }
                else
                {
                    Message msg = new Message(messageType, message, name);
                    _chat.AppendMessage(msg);
                    ChatGPTReq chatGPTReq = GetChatGPTReq();
                    string chatGPTJson = JsonConvert.SerializeObject(chatGPTReq);
                    if (isLog) Debug.Log("Send: " + chatGPTJson);

                    StartCoroutine(requests.PostReq<ChatGPTRes>(_uri, chatGPTJson, ResolveChatGPT, _reqHeaders));
                }

            }
            else
            {

                _prompt.AppendText(Prompt.Speaker.User, message);

                GPTReq reqObj = new GPTReq();
                reqObj.model = _selectedModel;
                reqObj.prompt = _prompt.CurrentPrompt;
                reqObj.max_tokens = _maxTokens;
                reqObj.temperature = _temperature;
                string json = JsonUtility.ToJson(reqObj);

                StartCoroutine(requests.PostReq<GPTRes>(_uri, json, ResolveGPT, _reqHeaders));
            }
        }
        private ChatGPTReq GetChatGPTReq()
        {
            ChatGPTReq chatGPTReq = new ChatGPTReq();
            chatGPTReq.model = _selectedModel;
            chatGPTReq.max_tokens = _maxTokens;
            chatGPTReq.temperature = _temperature;
            chatGPTReq.functions = new List<FunctionInfo>(_functions);
            chatGPTReq.function_call = functionCallConfig;//"auto" by default
            //Deep Copy
            chatGPTReq.messages = new List<Message>(_chat.CurrentChat);
            return chatGPTReq;
        }
        private void ResolveChatGPT(ChatGPTRes res)
        {
            if (isLog) Debug.Log("Receive: " + JsonConvert.SerializeObject(res));
            Message message = res.choices[0].message;
            FunctionCall functionCall = message.function_call;
            if (functionCall != null)
            {
                functionCallController.InvokeInstance(functionCall);
                _chat.AppendMessage(message);
                return;
            }
            _lastChatGPTMsg = message.content;
            _chat.AppendMessage(message);
            chatGPTResponse.Invoke(message.content);
        }

        private void ResolveGPT(GPTRes res)
        {
            _lastChatGPTMsg = res.choices[0].text
                .TrimStart('\n')
                .Replace("<|im_end|>", "");
            _prompt.AppendText(Prompt.Speaker.Bot, _lastChatGPTMsg);
            if (isLog) Debug.Log(_lastChatGPTMsg);
            chatGPTResponse.Invoke(_lastChatGPTMsg);
        }
    }
}
