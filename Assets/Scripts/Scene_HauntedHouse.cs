using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_HauntedHouse : MonoBehaviour
{
    public Image fadePanel, storyPanel;
    float fadeValue = 0.0f;
    bool enough = false;
    public Text log, alram;

    void Log1() => log.text = "우리 누나는\n공을 정말로 못 던진다.";
    void Log2() => log.text = "그럼에도 불구하고\n매번 나와 공놀이를 한다...";
    void Log3() => log.text = "오늘은 누나가 던진 공을 못 잡는 바람에\n이곳까지 공을 쫓아왔다.";
    void Log4() => log.text = "이 근처에서는 괴물 달팽이가 산다는\n소문이 돌기도 하고,\n무엇보다 누나가 기다릴 거야.";
    void Log5() => log.text = "빛이 나오는 곳을 찾아서 얼른 나가자!";
    void Log6()
    { 
        log.text = "";
        storyPanel.color = new Color(255, 255, 255, 0);
    }
    void Alram() => alram.text = "좌우 방향키로 방향을 맞추고 상하 방향키로 이동해서 탈출구를 찾자.\n빛이 들어오는 출구로 나가자!";

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Invoke("Log1", 0);
        Invoke("Log2", 2);
        Invoke("Log3", 4);
        Invoke("Log4", 6);
        Invoke("Log5", 10);
        Invoke("Alram", 10);
        Invoke("Log6", 12);
    }

    // Update is called once per frame
    void Update()
    {
        //페이드아웃
        if (fadeValue < 1.0f && enough == true) //할 때가 되었다 
        {
            fadeValue += 0.005f; //밝아진다~ 
            fadePanel.color = new Color(255, 255, 255, fadeValue);
        }
        else if (fadeValue >= 1.0f && enough == true) //다 했으면 
        {
            enough = false;
            //씬 넘기기
            SceneManager.LoadScene("RunawayBaby_Game");
        }
    }

    private void OnTriggerEnter(Collider col) //(보이지는 않지만) 플레이어가 출구 발판에 발을 디디면 
    {
        if(col.tag == "Player")
        enough = true;
    }
}
