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
    #region 변수

    //####### 서버 관련
    public Client client;
    private string Channel_Name = "wakpanzeebot";


    //####### 유닛 생성 관련
    public GameObject UnitBase;
    string[] UnitClasses = new string[3] {"전사","사수","지원가"};
    Dictionary<string, GameObject> twitchPlayerList = new Dictionary<string, GameObject>();
    private int openSlots = 12; //초기 시작 유닛 수 여기서 설정



    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //백그라운드에서도 실행하게 해줌
        Application.runInBackground = true;
        //밑에 토큰은 그냥 귀찮아서 냅뒀어요 ㅋㅋㅋ....
        ConnectionCredentials credentials = new ConnectionCredentials("wakpanzeebot", "be3yyp8bjqln1izy9mxa0gf5r4t6au");

        client = new Client();
        client.Initialize(credentials, Channel_Name);
        client.Connect();
        Debug.Log("클라이언트 연결!");
        //이벤트 셋업
        client.OnMessageReceived += MyMessageInputFunction;

    }

    private void MyMessageInputFunction(object sender, OnMessageReceivedArgs e)
    {

        if (openSlots > 0 && e.ChatMessage.Message.StartsWith("!참가 "))
        {//게임 참가 메커니즘
            if(e.ChatMessage.Message.Length < 4)
            {//커맨드 입력이 이상하게 됨. 
                return;
            }
            string userName = e.ChatMessage.DisplayName;
            string UnitClass = e.ChatMessage.Message.Substring(4);

            if (!twitchPlayerList.ContainsKey(userName))
            {//딕셔너리에 이름이 없을경우에만 새로 추가됨
                if (UnitClasses.Contains(UnitClass))
                {//고른 직업이 존재할경우
                    client.SendMessage(client.JoinedChannels[0], $"{userName} 님께서 {UnitClass}로 게임에 참가하셨습니다.");
                    unitCreation(userName, UnitClass);
                    openSlots--;
                }
                /*어차피 그냥 무시해도 될테니까 도배 방지용으로 주석처리 해놨습니다. 뽈롱뽈랑님 보시고 동의 하시면 아래 else 문 지워주세요!
                else
                {
                    client.SendMessage(client.JoinedChannels[0], $"{UnitClass}는 존재하지 않는 직업입니다.");
                }*/
            }
            else
            {//딕셔너리에 이름이 있는경우 
                client.SendMessage(client.JoinedChannels[0], $"{userName}님은 이미 게임에 참가 하셨습니다.");
            }
        }
    }
    public void flushPlayerList()
    {
        twitchPlayerList.Clear();
    }

    private void unitCreation(string userName, string unitClass)
    {
        GameObject instance = Instantiate(UnitBase, Vector3.zero, Quaternion.identity);
        instance.GetComponent<UnitStats>().playerUnitInit(userName);
        instance.GetComponent<UnitCombat>().playerSetup();

        WeaponType inputClass;
        if(unitClass == UnitClasses[0])
        {
            inputClass = WeaponType.Warrior;
        }
        else if(unitClass == UnitClasses[1])
        {
            inputClass = WeaponType.Shooter;
        }
        else
        {
            inputClass = WeaponType.Supporter;
        }

        PanzeeWindow.instance.addToList(userName, instance, inputClass);
        WakWindow.instance.updateStat(inputClass, 1);
        WakgoodBehaviour.instance.addPanzeeStat(inputClass, 1);
        twitchPlayerList.Add(userName, instance);

    }

    public void openEnrolling(int numSlots)
    {
        openSlots = numSlots;
    }
}
