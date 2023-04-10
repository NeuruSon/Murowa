using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextScene : MonoBehaviour
{
    GameObject gameCon, hpCon;

    // Start is called before the first frame update
    void Start()
    {
        gameCon = GameObject.Find("GameController");
        hpCon = GameObject.Find("HP");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButton()
    {
        gameCon.GetComponent<GameController>().gameState = GameController.GameState.nonFirstPlay;
        SceneManager.LoadScene("RunawayBaby_Game");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("RunawayBaby_Story");
    }

    public void ReturnButton()
    {
        SceneManager.LoadScene("RunawayBaby_Title");
    }

    public void HiddenStartButton()
    {
        gameCon.GetComponent<GameController>().gameState = GameController.GameState.hiddenPlay;
        //히든 입성 알림 문구 뭐라도 
        SceneManager.LoadScene("RunawayBaby_Game");
    }
}
