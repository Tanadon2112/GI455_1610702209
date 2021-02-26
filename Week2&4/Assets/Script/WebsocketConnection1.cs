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

        public Text SendBox;
        public Text ReceiveBox;
        public string UserName;
        public Text MesBox;
        public GameObject Page1;
        public GameObject Page2;
        public GameObject Page2_2;
        public GameObject Page3_Register;
        public GameObject Page3_Create;
        public GameObject Page3_Join;
        public GameObject Page4;
        public Text NameRegis;
        public Text UserRegis;
        public Text PassRegis;
        public Text RepassRegis;
        public Text RoomName;
        public Text JoinRoomName;
        public Text NameInRoom;
        public Text UserID;
        public Text Password;
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
            Page2_2.SetActive(true);
        }
        public void Login()
        {
            string KeepUserData = UserID.text + "#" + Password.text;
            SocketEvent socketEvent = new SocketEvent("Login", KeepUserData);
            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
            //Page2_2.SetActive(false);
            //Page2.SetActive(true);
        }
        public void register()
        {

            if(PassRegis.text == RepassRegis.text)
            {
                string KeepUserDataRegis = UserRegis.text + "#" + PassRegis.text + "#" + NameRegis.text;
                SocketEvent socketEvent = new SocketEvent("Register", KeepUserDataRegis);
                string toJsonStr = JsonUtility.ToJson(socketEvent);
                ws.Send(toJsonStr);
            }
            else
            {
                Error.SetActive(true);
            }
            
        }
        public void Send()
        {
            string MesUser = UserName + "#" + MesBox.text;
            SocketEvent socketEvent = new SocketEvent("SendMessage", MesUser);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }
        public void Openregister()
        {
            Page2_2.SetActive(false);
            Page3_Register.SetActive(true);
        }

        public void CreateRoom(string roomName)
        {
            roomName = RoomName.text;
            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
            Page3_Create.SetActive(false);
            Page4.SetActive(true);
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
                    else if(receiveMessageData.data != "fail")
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
                else if (receiveMessageData.eventName == "Register")
                {
                    if (receiveMessageData.data == "fail")
                    {
                        Error.SetActive(true);
                    }
                    else
                    {
                        Page3_Register.SetActive(false);
                        Page2_2.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "Login")
                {
                    if (receiveMessageData.data == "fail")
                    {
                        Error.SetActive(true);
                    }
                    else
                    {
                        UserName = receiveMessageData.data;
                        Page2_2.SetActive(false);
                        Page2.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "SendMessage")
                {
                    if(tempMessageString != "")
                    {
                        if(receiveMessageData.data == UserName + " : " + MesBox.text)
                        {
                            SendBox.text += "\n" + receiveMessageData.data;
                            ReceiveBox.text += "\n";
                        }
                        else if(receiveMessageData.data != UserName + " : " + MesBox.text)
                        {
                            ReceiveBox.text += "\n" + receiveMessageData.data;
                            SendBox.text += "\n";
                        }
                    }
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


