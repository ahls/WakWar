using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using TwitchLib.Client.Events;
using System;
using System.Linq;
using UnityEngine.UI;

public class TwitchClient : MonoBehaviour
{
    public Client client;
    private string Channel_Name = "wakpanzeebot";
    string[] UnitClasses = new string[3] {"전사","사수","지원가"};
    public GameObject UnitBase;

    // Start is called before the first frame update
    void Start()
    {
        
        Application.runInBackground = true;
        ConnectionCredentials credentials = new ConnectionCredentials("wakpanzeebot", "be3yyp8bjqln1izy9mxa0gf5r4t6au");
        client = new Client();
        client.Initialize(credentials, Channel_Name);
        client.Connect();
        client.OnMessageReceived += MyMessageInputFunction;
    }

    private void MyMessageInputFunction(object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message.StartsWith("!참가 "))
        {
            string UnitClass = e.ChatMessage.Message.Substring(4);
            if (UnitClasses.Contains(UnitClass))
            {
                string UserName = e.ChatMessage.DisplayName;
                client.SendMessage(client.JoinedChannels[0], $"{e.ChatMessage.DisplayName} 님께서 {UnitClass}로 게임에 참가하셨습니다.");
                GameObject instance = Instantiate(UnitBase, Vector3.zero, Quaternion.identity);
                instance.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = UserName;
            }
            else
            {
                client.SendMessage(client.JoinedChannels[0], $"{UnitClass}는 존재하지 않는 직업입니다.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            client.SendMessage(client.JoinedChannels[0], "Testing!");
        }
    }
}
