using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour //게임 전체적인 진행과 담당 
{
    //비폠: 궁극적인 목표; 비폠 바뀐다고 해도 바뀐 비폠 맞게 라인 보낼 것. 
    public GameObject bpmLine, musicTime;
    public AudioClip readySfx, tutoMusic, loseSfx, winSfx, endSfx;
    AudioClip queueMusic; //queue는 나중에 다른 음악 필요하면 넣을 temp 공간 
    AudioSource currentMusic;
    public float bpm; //앞으로는 파일 읽어서 받으라구~ 
    public float speed = 1;
    public float fixRate = 0;
    public static float musicPlayTime = 0;
    public bool musicPlaying = false; //음악 진행 바 관련
    public Image gameoverFadePanel, gameclearFadePanel, failRestartButton, clearRestartButton, failSprite;
    float fadeValue = 0.0f;
    bool enough = false;
    public TextAsset tempChartText;
    public Text criticalT, criticalNumT, hitT, hitNumT, missT, missNumT;
    public bool startSwitch = true;
    bool hiddenSwitch = false;

    //콤보 
    public int combo = 0;
    int p, g, m;
    TextMeshProUGUI comboCounter;

    //다른 스크립트와 연동
    GameObject mainConL, mainConR;
    public Animator enemy;

    public enum GameState //상태. 스크립트 및 재시작 고려 가능. 
    {
        tempState,
        firstPlay,
        nonFirstPlay,
        gameStart,
        gameOver,
        gameClear,
        hiddenPlay
    }


    public GameState gameState = GameState.firstPlay; //초견 
    //public GameState gameState = GameState.tempState; 

    void Awake() //한 번 겜콘은 영원한 겜콘 
    {
        //GameObject gameCon = GameObject.Find("GameController");
        if (gameState == GameState.tempState) //겜스탯 비어있으면 퍼스트 넣고 아니면 그대로 유지하려는데 잘 안 되네 
        {
            gameState = GameState.firstPlay;
        }
        //if (gameCon != null)
        //{
        //    Destroy(this);
        //}
        //else
        //{
        //    DontDestroyOnLoad(this);
        //}
    }

    public void StageClear() //클리어 했으면 결산 
    {
        GameStop();
        enough = true;
        gameState = GameState.gameClear;
        currentMusic.clip = winSfx;
        currentMusic.Play();
        enemy.GetComponent<Animator>().SetTrigger("Die");
        p = mainConL.GetComponent<MainController>().perfect + mainConR.GetComponent<MainController>().perfect;
        g = mainConL.GetComponent<MainController>().good + mainConR.GetComponent<MainController>().good;
        m = mainConL.GetComponent<MainController>().miss + mainConR.GetComponent<MainController>().miss;
        criticalNumT.text = "" + p;
        hitNumT.text = "" + g;
        missNumT.text = "" + m;
    }

    public void YouDied()
    {
        GameStop();
        enough = true;
        gameState = GameState.gameOver;
        currentMusic.clip = loseSfx;
        currentMusic.Play();
        enemy.GetComponent<Animator>().SetTrigger("Victory");
    }

    void GameOverFadePanel()
    {
        if (fadeValue < 1.0f && enough == true)
        {
            fadeValue += 0.003f; //판넬이 나타난다... 
            gameoverFadePanel.color = new Color(0, 0, 0, fadeValue);
            failSprite.color = new Color(255, 255, 255, fadeValue);
            failRestartButton.color = new Color(255, 255, 255, fadeValue);
        }
        else if (fadeValue >= 1.0f && enough == true) //다 했으면 
        {
            enough = false;
        }
    }

    void GameClearFadePanel()
    {
        if (fadeValue < 1.0f && enough == true)
        {
            fadeValue += 0.003f; //판넬이 나타난다... 
            gameclearFadePanel.color = new Color(255, 255, 255, fadeValue);
            clearRestartButton.color = new Color(255, 255, 255, fadeValue);
            criticalT.color = new Color(255, 255, 255, fadeValue);
            criticalNumT.color = new Color(255, 198, 198, fadeValue);
            hitT.color = new Color(255, 255, 255, fadeValue);
            hitNumT.color = new Color(255, 198, 198, fadeValue);
            missT.color = new Color(255, 255, 255, fadeValue);
            missNumT.color = new Color(255, 198, 198, fadeValue);
        }
        else if (fadeValue >= 1.0f && enough == true) //다 했으면 
        {
            enough = false;
        }
    }

    public void GameStart()
    {
        gameState = GameState.gameStart;
        ChartReader(); //읽어줍니다
        Cursor.visible = false;

        currentMusic.clip = readySfx;
        currentMusic.Play();
        InvokeRepeating("BPMLineGenerator", 7.0f, 60 / bpm);
        //InvokeRepeating("BPMLineGenerator", 7.0f, 60 / bpm * speed); //60초를 비폠으로 나누고 1비트당 할당될 초 계산. 
        Invoke("MusicPlay", 7.0f + 60 / bpm + fixRate); //이 시간 후에 실행하세요~ 인데 의도한 것: 비폠라인 하나는 보내고 다음 비폠라인(+못갖춘 고려한 픽) 맞춰 노래 시작. 하지만 바꿨음.. 의도: 라인 하나 보내고(60/bpm) 
    }

    public void GameStop()
    {
        Debug.Log("GameStop");
        Cursor.visible = true;
        if (musicPlaying == true) //게임 오버면 노래 멈춰줌. 
        {
            currentMusic.Stop();
        }
        currentMusic.clip = endSfx;
        currentMusic.Play();
        mainConL.GetComponent<MainController>().CancelInvoke("NoteGenerator"); //노트 생산 멈춰줌. 
        mainConR.GetComponent<MainController>().CancelInvoke("NoteGenerator"); //노트 생산 멈춰줌. 
        mainConL.GetComponent<MainController>().GameEndNoteDestroyer(); //생성됐지만 치지 않은 노트 전부 삭제
        mainConR.GetComponent<MainController>().GameEndNoteDestroyer(); //생성됐지만 치지 않은 노트 전부 삭제
        CancelInvoke("BPMLineGenerator"); //비폠라인 생산 중단. 
        musicPlaying = false; //노래 안 나옴. 
        musicPlayTime = 0; //초기화.
    }

    void Start()
    {
        mainConL = GameObject.Find("JudgeLine_Left");
        mainConR = GameObject.Find("JudgeLine_Right");
        comboCounter = GameObject.Find("NUM").GetComponent<TextMeshProUGUI>();
        currentMusic = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameState == GameState.nonFirstPlay || gameState == GameState.hiddenPlay)
        {
            if (gameState == GameState.hiddenPlay)
            {
                hiddenSwitch = true;
            }
            GameStart();
        }
        comboCounter.text = "" + combo;
        IsPlaying();
        if(gameState == GameState.gameOver)
        {
            GameOverFadePanel();
        }
        else if (gameState == GameState.gameClear)
        {
            GameClearFadePanel();
        }
    }

    //비폠제너
    void IsPlaying()
    {
        if (musicPlaying == true) //정상적으로 재생중이면 
        {
            musicPlayTime += Time.deltaTime; //카운트 시작 
            musicTime.GetComponent<Slider>().value = musicPlayTime * 1000;
            if (musicPlayTime >= currentMusic.clip.length) //끝까지 재생이 됐다면 
            {
                GameStop();
                StageClear();
            }
        }
    }

    void BPMLineGenerator()
    {
        GameObject line = Instantiate(bpmLine) as GameObject;
        line.GetComponent<BPMLineController>().Shoot(bpm, speed);
    }

    void MusicPlay()
    {
        musicPlaying = true;
        currentMusic.clip = queueMusic;
        Debug.Log("Music length : " + currentMusic.clip.length); //일단 음원 길이는 구했는데 메소드라 어디 저장은 안 됨. 저장하고 싶으면 새로 배리어블 만들어라 
        currentMusic.Play();
        musicTime.GetComponent<Slider>().maxValue = currentMusic.clip.length * 1000; //음악 진행도 바 최댓값을 음악 길이(매끄러운 증가를 위해 소수점 제거: * 1000)로 설정.
    }

    void ChartReader()
    {
        if (hiddenSwitch == true)
        {
            tempChartText = Resources.Load("PSYQUI - Education", typeof(TextAsset)) as TextAsset;
        }
        string[] chartRows = tempChartText.text.Split('\n'); //행 생성하면서 바로 값 넣어줌. 기준은 엔터(\n)
        string[][] chartCols = new string[chartRows.Length][]; //열 생성. 크기 조정해준 것. 값 넣어줘야 함.
        int lineNum = 0; //실제로 rows와 관련있는 것이 아니라 체크를 위한 임의 variable임 
        foreach (string tempText in chartRows)
        {
            chartCols[lineNum] = tempText.Split(':'); //기호를 기준으로 구분해서 넣어줌.
            if (lineNum == 0) //첫 줄은 곡 정보라 예외로 처리. 
            {
                //title 
                queueMusic = Resources.Load(chartCols[0][0], typeof(AudioClip)) as AudioClip;

                //bpm
                bpm = float.Parse(chartCols[0][1]); //게임콘 내의 bpm 설정
                //Debug.Log("BPM: " + bpm);
                mainConL.GetComponent<MainController>().bpm = float.Parse(chartCols[0][1]);
                mainConR.GetComponent<MainController>().bpm = float.Parse(chartCols[0][1]);

                //speed
                speed = float.Parse(chartCols[0][2]); //게임콘 내의 speed 설정
                //Debug.Log("\nSPEED: " + speed);
                mainConL.GetComponent<MainController>().speed = float.Parse(chartCols[0][2]);
                mainConR.GetComponent<MainController>().speed = float.Parse(chartCols[0][2]);

                //fixRate
                fixRate = float.Parse(chartCols[0][3]); //게임콘 내의 fixRate 설정
                //Debug.Log("\nfixRate: " + fixRate);
                //mainConL.GetComponent<MainController>().fixRate = float.Parse(chartCols[0][3]);
                //mainConR.GetComponent<MainController>().fixRate = float.Parse(chartCols[0][3]);

                lineNum++;
            }
            else //이후부터는 row(col:col:col:col) row 기준으로 한 번만 돌아감. 한 번에 네 개의 col 처리 필요 
            {
                //[lineNum][0] noteTerm  
                mainConL.GetComponent<MainController>().noteTerm += float.Parse(chartCols[lineNum][0]); //양쪽 모두 무조건 텀 더해줌 
                mainConR.GetComponent<MainController>().noteTerm += float.Parse(chartCols[lineNum][0]);

                //[lineNum][1] note appear line  
                if (chartCols[lineNum][1] == "-1" || chartCols[lineNum][1] == "0")
                {
                    mainConL.GetComponent<MainController>().InvokeNoteGenerator();
                }
                if (chartCols[lineNum][1] == "1" || chartCols[lineNum][1] == "0")
                {
                    mainConR.GetComponent<MainController>().InvokeNoteGenerator();
                }

                //[lineNum][2] noteDamage
                //노트 대미지를 달리 하기 위해서 이제부터 대미지에 따라 다른 노트를 뱉어내야 함. 

                //[lineNum][3] undeveloped

                lineNum++;
            }
        }
    }
}
