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
    public Client Client;
    private string _channelName = "wakpanzeebot";

    //####### 유닛 생성 관련
    public GameObject UnitBase;
    private string[] _unitClasses = new string[3] {"전사","사수","지원가"};
    private Dictionary<string, GameObject> _twitchPlayerDic = new Dictionary<string, GameObject>();
    private int _openSlots = 12; //초기 시작 유닛 수 여기서 설정

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //백그라운드에서도 실행하게 해줌
        Application.runInBackground = true;
        //밑에 토큰은 그냥 귀찮아서 냅뒀어요 ㅋㅋㅋ....
        ConnectionCredentials credentials = new ConnectionCredentials("wakpanzeebot", "be3yyp8bjqln1izy9mxa0gf5r4t6au");

        Client = new Client();
        Client.Initialize(credentials, _channelName);
        Client.Connect();
        Debug.Log("클라이언트 연결!");
        //이벤트 셋업
        Client.OnMessageReceived += MyMessageInputFunction;

    }

    private void MyMessageInputFunction(object sender, OnMessageReceivedArgs e)
    {
        string userName = e.ChatMessage.DisplayName;

        if (_openSlots > 0 && e.ChatMessage.Message.StartsWith("!참가 "))
        {//게임 참가 메커니즘
            if (e.ChatMessage.Message.Length < 4)
            {//커맨드 입력이 이상하게 됨. 
                return;
            }
            string UnitClass = e.ChatMessage.Message.Substring(4);

            if (!_twitchPlayerDic.ContainsKey(userName))
            {//딕셔너리에 이름이 없을경우에만 새로 추가됨
                if (_unitClasses.Contains(UnitClass))
                {//고른 직업이 존재할경우
                    Client.SendMessage(Client.JoinedChannels[0], $"{userName} 님께서 {UnitClass}로 게임에 참가하셨습니다.");
                    UnitCreation(userName, UnitClass);
                    _openSlots--;
                }
            }
            else
            {//딕셔너리에 이름이 있는경우 
                Client.SendMessage(Client.JoinedChannels[0], $"{userName}님은 이미 게임에 참가 하셨습니다.");
            }
        }
        else if (e.ChatMessage.Message.StartsWith("!힘"))
        {
            if (_twitchPlayerDic.ContainsKey(userName) && )
            {//힘찍으면 최대체력 및 체젠
                _twitchPlayerDic[userName].GetComponent<UnitCombat>().
            }
        }
        else if (e.ChatMessage.Message.StartsWith("!민"))
        {

        }
        else if (e.ChatMessage.Message.StartsWith("!지"))
        {

        }
    }

    public void FlushPlayerDic()
    {
        _twitchPlayerDic.Clear();
    }

    private void UnitCreation(string userName, string unitClass)
    {
        ClassType inputClass;
        if(unitClass == _unitClasses[0])
        {
            inputClass = ClassType.Warrior;
        }
        else if(unitClass == _unitClasses[1])
        {
            inputClass = ClassType.Shooter;
        }
        else
        {
            inputClass = ClassType.Supporter;
        }

        GameObject instance = Instantiate(UnitBase, Vector3.zero, Quaternion.identity);
        instance.GetComponent<UnitStats>().PlayerUnitInit(userName);
        instance.GetComponent<UnitCombat>().playerSetup(inputClass);


        PanzeeWindow.instance.addToList(userName, instance, inputClass);
        IngameManager.WakgoodBehaviour.AddPanzeeStat(inputClass, 1);
        _twitchPlayerDic.Add(userName, instance);
        instance.GetComponent<UnitCombat>().UnEquipWeapon();
        instance.GetComponent<Rigidbody2D>().MovePosition(Vector2.left * 0.01f);
    }

    public void OpenEnrolling(int numSlots)
    {
        _openSlots = numSlots;
    }
}
