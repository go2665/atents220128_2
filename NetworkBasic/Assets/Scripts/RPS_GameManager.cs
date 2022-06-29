using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class RPS_GameManager : MonoBehaviour
{
    Button rock;
    Button paper;
    Button scissors;
    TextMeshProUGUI resultText;
    TextMeshProUGUI mySelection;
    TextMeshProUGUI oppenentSelection;

    RPS_Player player = null;
    public RPS_Player Player
    {
        get => player;
        set
        {
            if(player == null)
            {
                player = value;
            }
        }
    }

    RPS_Player enemy = null;
    public RPS_Player Enemy
    {
        get => enemy;
        set
        {
            if (enemy == null)
            {
                enemy = value;
            }
        }
    }


    private static RPS_GameManager instance = null;
    public static RPS_GameManager Inst
    {
        get => instance;
    }

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(480, 10, 150, 150));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host"))
                NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client"))
                NetworkManager.Singleton.StartClient();
        }
        GUILayout.EndArea();
    }

    /// <summary>
    /// 각 종 초기화 실행
    /// </summary>
    void Initialize()
    {
        // UI 찾고
        rock = GameObject.Find("Button_Rock").GetComponent<Button>();
        paper = GameObject.Find("Button_Paper").GetComponent<Button>();
        scissors = GameObject.Find("Button_Scissors").GetComponent<Button>();
        resultText = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();
        mySelection = GameObject.Find("MySelection").GetComponent<TextMeshProUGUI>();
        oppenentSelection = GameObject.Find("OppenentSelection").GetComponent<TextMeshProUGUI>();

        // 이벤트 함수 등록하기
        rock.onClick.AddListener(() => OnHandClick(HandSelection.Rock));
        paper.onClick.AddListener(() => OnHandClick(HandSelection.Paper));
        scissors.onClick.AddListener(() => OnHandClick(HandSelection.Scissors));
    }

    /// <summary>
    /// 가위, 바위, 보 버튼을 클릭했을 때 실행될 함수
    /// </summary>
    /// <param name="hand">클릭한 버튼의 종류</param>
    private void OnHandClick(HandSelection hand)
    {
        // 플레이어가 찾아져 있고 플레이어가 아무런 선택을 하지 않았을 때만 선택 가능
        if (player && player.Selection == HandSelection.None)
        {
            player.Selection = hand;            // 플레이어의 선택 표시
            switch (hand)                       // 텍스트에도 표시
            {                
                case HandSelection.Rock:
                    mySelection.text = "바위";
                    break;
                case HandSelection.Paper:
                    mySelection.text = "보";
                    break;
                case HandSelection.Scissors:
                    mySelection.text = "가위";
                    break;
                case HandSelection.None:
                default:
                    break;
            }            
        }
    }

    /// <summary>
    /// 적이 선택을 완료했음을 표시
    /// </summary>
    public void OpponentSelectComplete()
    {        
        oppenentSelection.text = "선택완료";
    }

    /// <summary>
    /// 둘 다 선택을 했는지 확인
    /// </summary>
    /// <returns>둘 다 선택을 했으면 true. 아니면 false</returns>
    public bool IsBothComplete()
    {
        return (player.Selection != HandSelection.None) && (enemy.Selection != HandSelection.None);
    }

    /// <summary>
    /// 가위, 바위, 보 결과를 출력하는 함수
    /// </summary>
    public void SetBattleResultText()
    {
        BattleResult result = BattleResult.Draw;
        if( Player.Selection == HandSelection.Rock )        // 가위 바위 보 결과 확인
        {
            if( Enemy.Selection == HandSelection.Rock )
            {
                result = BattleResult.Draw;
            }
            else if(Enemy.Selection == HandSelection.Scissors)
            {
                result = BattleResult.PlayerWin;
            }
            else if(Enemy.Selection == HandSelection.Paper)
            {
                result = BattleResult.EnemyWin;
            }
        }
        else if (Player.Selection == HandSelection.Scissors)
        {
            if (Enemy.Selection == HandSelection.Rock)
            {
                result = BattleResult.EnemyWin;
            }
            else if (Enemy.Selection == HandSelection.Scissors)
            {
                result = BattleResult.Draw;
            }
            else if (Enemy.Selection == HandSelection.Paper)
            {
                result = BattleResult.PlayerWin;
            }
        }
        else if (Player.Selection == HandSelection.Paper)
        {
            if (Enemy.Selection == HandSelection.Rock)
            {
                result = BattleResult.PlayerWin;
            }
            else if (Enemy.Selection == HandSelection.Scissors)
            {
                result = BattleResult.EnemyWin;
            }
            else if (Enemy.Selection == HandSelection.Paper)
            {
                result = BattleResult.Draw;
            }
        }

        string select = "";
        switch (Enemy.Selection)
        {
            case HandSelection.Rock:
                select = "바위";
                break;
            case HandSelection.Paper:
                select = "보";
                break;
            case HandSelection.Scissors:
                select = "가위";
                break;
            case HandSelection.None:
            default:
                break;
        }
        oppenentSelection.text = select;

        switch (result) // 결과에 따라서 글자 출력
        {
            case BattleResult.Draw:
                resultText.text = "무승부";
                break;
            case BattleResult.PlayerWin:
                resultText.text = "승리";
                break;
            case BattleResult.EnemyWin:
                resultText.text = "패배";
                break;
            default:
                break;
        }
    }
}
