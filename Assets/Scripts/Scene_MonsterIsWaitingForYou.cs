using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_MonsterIsWaitingForYou : MonoBehaviour
{
    public UnityEngine.UI.Image fadePanel, storyPanel;
    float fadeValue = 1.0f;
    bool enough = false;
    bool check = false; //스토리 봤냐? 
    GameObject gameCon;
    public UnityEngine.UI.Text log, alram;

    void Log1()
    {
        log.text = "드디어 탈출이야!";
    }
    void Log2()
    {
        log.text = "아니, 저건...!";
    }
    void Log3()
    {
        log.text = "슬라임을 던져 사람들을 괴롭히기로 유명한\n괴물 달팽이?!";
    }
    void Log4()
    {
        log.text = "...좋아, 누나가 못 던진다면 내가 받으면 되지!";
    }
    void Log5()
    {
        log.text = "너를 연습 대상으로 삼아주지!! 덤벼라!!";
    }
    void Log6()
    {
        log.text = "DF - 왼쪽\nJK - 오른쪽\n슬라임이 빛나는 가로선에\n가까이 왔을 때 쳐내자!\n(DGJK 중 아무 키나 눌러서 시작)";
    }
    void Log7()
    {
        log.text = "";
    }
    void Alram()
    {
        alram.text = "DF : LEFT\nJK : RIGHT";
        //TMP로 구현하면 잘 보이겠다 
    }

    void Start()
    {
        gameCon = GameObject.Find("GameController");
    }

    void UpdateGameState()
    {
        gameCon.GetComponent<GameController>().gameState = GameController.GameState.nonFirstPlay;
    }

    void Update()
    {
        //if (gameCon.GetComponent<GameController>().gameState == GameController.GameState.firstPlay)
        {
            storyPanel.color = new Color(255, 255, 255, 1); //보여줌 
        }
        //페이드인
        if (fadeValue > 0.0f && enough == false) //할 때가 되었다 
        {
            fadeValue -= 0.005f; //씬이 보인다~  
            fadePanel.color = new Color(255, 255, 255, fadeValue);
        }
        else if (fadeValue <= 0.0f && enough == false) //다 했으면 
        {
            enough = true;
            //if (gameCon.GetComponent<GameController>().gameState == GameController.GameState.firstPlay)
            {
                //스토리 진행
                Invoke("Log1", 0);
                Invoke("Alram", 0);
                Invoke("Log2", 2);
                Invoke("Log3", 4);
                Invoke("Log4", 7);
                Invoke("Log5", 10);
                Invoke("Log6", 12);
                check = true;
            }
        }
        if (check == true && gameCon.GetComponent<GameController>().gameState == GameController.GameState.firstPlay)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
            {
                CancelInvoke("Log1"); CancelInvoke("Alram"); CancelInvoke("Log2"); CancelInvoke("Log3");
                CancelInvoke("Log4"); CancelInvoke("Log5"); CancelInvoke("Log6");
                Invoke("Log7", 0);
                Invoke("UpdateGameState", 0);
                storyPanel.color = new Color(255, 255, 255, 0); //없애주기 
                check = false;
            }
        }
    }
}