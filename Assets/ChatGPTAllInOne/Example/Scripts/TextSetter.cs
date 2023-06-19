using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ChatgptAllInOne.Example
{
    public class TextSetter : MonoBehaviour
    {
        private Text text;
        private void Start()
        {
            text = GetComponent<Text>();
        }
        public void SetText(string newText)
        {
            StopAllCoroutines();
            if (text == null)
            {
                Debug.LogWarning("TextSetter.SetText: text is null");
                return;
            }
            text.text = newText;
        }
        public void AddAssistantText(string newText)
        {
            StopAllCoroutines();
            if (text == null)
            {
                Debug.LogWarning("TextSetter.SetText: text is null");
                return;
            }
            text.text += "Assistant: " + newText + "\n";
        }

        public void AddUserText(string newText)
        {
            StopAllCoroutines();
            if (text == null)
            {
                Debug.LogWarning("TextSetter.SetText: text is null");
                return;
            }
            text.text += "User: " + newText + "\n";
        }
        public void WaitResponse()
        {
            StartCoroutine(WaitAnimation());
        }

        //animation when recording
        private IEnumerator WaitAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                text.text = ".";
                yield return new WaitForSeconds(0.5f);
                text.text = "..";
                yield return new WaitForSeconds(0.5f);
                text.text = "...";
            }
        }
    }

}