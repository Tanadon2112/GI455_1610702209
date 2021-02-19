using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocket
{
    public class WebsocketConnection1 : MonoBehaviour
    {
        
        public struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }
        
        private WebSocket ws;

        private string tempMessageString;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;

        public GameObject Page1;
        public GameObject Page2;
        public GameObject Page3_Create;
        public GameObject Page3_Join;
        public GameObject Page4;
        public Text RoomName;
        public Text JoinRoomName;
        public Text NameInRoom;
        public GameObject Error;

        private void Update()
        {
            UpdateNotifyMessage();
         
        }

        public void Connect()
        {
            string url = "ws://127.0.0.1:65222/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            Page1.SetActive(false);
            Page2.SetActive(true);
        }

        public void CreateRoom(string roomName)
        {
            roomName = RoomName.text;
            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
            Page2.SetActive(false);
            Page3_Create.SetActive(true);
        }
        public void CreateRoomButton()
        {
            Page2.SetActive(false);
            Page3_Create.SetActive(true);
        }
        public void JoinRoomButton()
        {
            Page2.SetActive(false);
            Page3_Create.SetActive(true);
        }
        public void CreateButton()
        {
            Page3_Create.SetActive(false);
            Page4.SetActive(true);
        }
        public void JoinButton()
        {
            Page2.SetActive(false);
            Page3_Join.SetActive(true);
        }
        public void Backtobasic()
        {
            Error.SetActive(false);
        }

        public void JoinRoom(string roomName)
        {
            roomName = JoinRoomName.text;
            SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            Page4.SetActive(false);
            Page2.SetActive(true);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }
        
        private void OnDestroy()
        {
            Disconnect();
        }

        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);
                    if(receiveMessageData.data == "fail")
                    {
                        Error.SetActive(true);
                        print("Error");
                    }
                    else if(receiveMessageData.data != "fail")
                    {
                        NameInRoom.text = RoomName.text;
                        Page4.SetActive(true);
                        Page3_Create.SetActive(false);
                    }
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);
                    if(receiveMessageData.data == "fail")
                    {
                        Error.SetActive(true);
                    }
                    else if(receiveMessageData.data != "Fail")
                    {
                        NameInRoom.text = JoinRoomName.text;
                        Page4.SetActive(true);
                        Page3_Join.SetActive(false);
                    }
                }
                else if(receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;
        }
    }
}


