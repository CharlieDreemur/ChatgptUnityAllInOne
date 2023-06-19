using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ChatgptAllInOne.Example
{
    public class InputFieldUI : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private ChatgptAllInOne.UnityStringEvent onSendToChatGPT;
        [SerializeField]
        private bool isSelected = false;
        [SerializeField]
        private TextSetter textSetter;
        private void Start()
        {
            if (button == null) return;
            button.onClick.AddListener(() =>
            {
                Submit(inputField.text);
            });
        }

        private void Update()
        {
            isSelected = inputField.isFocused;
            if (isSelected && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                Submit(inputField.text);
            }
        }

        private void Submit(string input)
        {
           
            onSendToChatGPT.Invoke(input);
            textSetter.AddUserText(input);
            inputField.text = "";

        }
    }
}