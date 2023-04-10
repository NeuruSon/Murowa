using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    Slider playerHpBar;
    public float playerDefaultHp = 200; //기본 최대 체력
    public float playerCurrentHp; //슬라이더 오류를 감안해 중간 다리 하나 생성 
    bool enough = false;
    bool hurtable = false;
    public GameObject damagePrefab;

    //연동
    GameObject gameCon;

    void Start()
    {
        gameCon = GameObject.Find("GameController");
        playerHpBar = GetComponent<Slider>();
        playerHpBar.maxValue = playerDefaultHp; //최대 HP를 캐릭터 기본 체력치로 설정해줌. 
    }

    void Update()
    {
        if (hurtable == false) 
        {
            if (enough == false) //아직 체력을 안 채웠어? 
            {
                playerHpBar.value++; //채워 그럼 
                if (playerHpBar.value == playerHpBar.maxValue) //다 찼냐? 
                {
                    playerCurrentHp = playerHpBar.value; //다 찬 그 값이 이제부터 현재 HP임
                    enough = true; //다 찼습니다!! 
                    hurtable = true; //다 찼으면 이제부터 맞는 거야 
                }
            }
            else //(enough == true)
            {
                if (gameCon.GetComponent<GameController>().gameState != GameController.GameState.gameOver)
                {
                    Debug.Log("으앙 쥬금"); //잘 들어옴
                    gameCon.GetComponent<GameController>().YouDied(); //gameCon에서 게임 오버 함수를 호출한다
                }
            }
        }
    }

    public void PlayerDamage(float mobStrength) //mainCon에서 접근할 수 있게 public 
    {
        if (hurtable == true && playerCurrentHp - mobStrength > 0)
        {
            playerCurrentHp -= mobStrength; //몬스터 공격력만큼 체력 깎아줌
            playerHpBar.value = playerCurrentHp;
        }
        else if (hurtable == true && playerCurrentHp - mobStrength <= 0) //만약 공격을 받으면 HP가 0 이하로 떨어진다? 
        {
            hurtable = false; //바 밸류 음수로 못 내려가게 막아
            playerCurrentHp = 0; //킥더바께스 
            playerHpBar.value = playerCurrentHp; //여기까지 멀쩡
            //gameCon.GetComponent<GameController>().YouDied(); 
        }
        GameObject damageText = Instantiate(damagePrefab, GameObject.Find("Canvas").transform) as GameObject;
        damageText.GetComponent<DamageTextController>().DamageValue(mobStrength);
    }

    public void PlayerRestarted()
    {
        enough = false;
    }
}
