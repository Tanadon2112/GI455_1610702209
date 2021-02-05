using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Programchat
{
public class WebsocketConnection : MonoBehaviour
{
        private WebSocket webSocket;
        public InputField message;
        public InputField R;
        public InputField L;
        private string input;
        public Text Output;
        public GameObject Keep;
        // Start is called before the first frame update
        void Start()
    {

    }
            
    // Update is called once per frame
    void Update()
    {

    }
        private void OnDestroy()
        {
            if(webSocket != null)
            {
                webSocket.Close();
            }
        }
        private void OnMessage(object sender,MessageEventArgs messageEventArgs)
        {
                Output.text += messageEventArgs.Data + "\n";     
        }
        public void ReadInput()
        {
            webSocket.Send(message.text);
            Debug.Log(input);
        }
        public void Check()
        {
            if(L.text == "127.0.0.1" && R.text == "65222")
            {
                webSocket = new WebSocket("ws://127.0.0.1:65222/");
                webSocket.OnMessage += OnMessage;

                webSocket.Connect();
                Keep.SetActive(false);
            }
        }
    }
}