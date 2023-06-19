using System;
using System.Collections.Generic;
using UnityEngine;
namespace ChatgptAllInOne {
    // Due to OpenAI's new chat completions api, this replaces the old "Prompt" class, but the prompt class is still used for the older models.
    [Serializable]
    public class Chat
    {
        [SerializeField]

        private string _initialPrompt;
        [SerializeField]

        private  List<Message> _currentChat = new List<Message>();

        public Chat(string initialPrompt) {
            _initialPrompt = initialPrompt;
            Message systemMessage = new Message(Speaker.system, initialPrompt);
            _currentChat.Add(systemMessage);
        }
        public List<Message> CurrentChat { get { return _currentChat; } }
        public void Clone(Chat chat)
        {
            _initialPrompt = chat._initialPrompt;
            _currentChat = chat._currentChat;
        }
        public void AppendMessage(Speaker messageType, string text)
        {
            Message message = new Message(messageType, text);
            _currentChat.Add(message);
        }

        public void AppendMessage(Message message)
        {
            _currentChat.Add(message);
        }

        public void Print(){
            string toPrint = "";
            foreach (Message message in _currentChat)
            {
                toPrint += message.role + ": " + message.content + "\n";
            }
            Debug.Log(toPrint);
        }
    }
}
