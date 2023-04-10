using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour //게임 내 판정과 라인별 노트 생성 담당 
{
    //노트 
    public GameObject enemyNote;
    public List<GameObject> notes = new List<GameObject>();
    GameObject note;
    public Animator enemy;

    //사운드효과 
    public AudioClip hitSfx, missSfx;
    AudioSource tempAudio;

    //음악 파일 
    public float bpm = 0; //이거 float으로 안 하시면 invokerepeating 작동 안 해요 님..
    public float speed = 1; //나중에는 채보마다 받아서 적용하는 쪽으로 수정
    public float fixRate = 0.009f; //범용성을 위해 가능하면 모두 수정 가능한 쪽으로 작성
    public float noteTerm = 0;

    //판정
    public KeyCode key1, key2, key3, key4; //키 지정해주기. WA!
    public int lineSide; //좌측 라인은 -1, 우측 라인은 1
    public float goodCutlineP = 1.7f; //힛 판정 커트라인 양수 
    public float goodCutlineN = -1.5f; //음수 

    //콤보 
    public int perfect, good, miss = 0; //한 판 끝나고 초기화 해주는 게 정석이긴 한데 한 판 짜리에 초기화는 사치지 역시ㅎㅎㅎ 
    public GameObject judgeText, hitEff1, hitEff2; //라인 당 두 종류의 파티클 시스템.

    //대미지
    GameObject damaged;

    //게임콘과 연동
    GameObject gameCon;

    void NoteGenerator()
    {
        note = Instantiate(enemyNote) as GameObject;
        note.GetComponent<NoteController>().Shoot(lineSide, speed, bpm); //방금 만든 note를 note안에 들어있는 노트컨트롤러 스크립트의 shoot 메소드를 이용해 조작.
        enemy.GetComponent<Animator>().SetTrigger("Attack");
        notes.Add(note);
    }

    public void InvokeNoteGenerator()
    {
        Invoke("NoteGenerator", 7 + noteTerm + fixRate);
    }

    void Start()
    {
        tempAudio = GetComponent<AudioSource>();
        damaged = GameObject.Find("HP");
        gameCon = GameObject.Find("GameController");
        //InvokeRepeating("NoteGenerator", 7 + fixRate, 60 / bpm * speed);
    }

    void Distance(int ganda) //리스트 내의 모든 배열 거리 계산
    {
        if (notes != null) //하나만 있을 때는 중괄호 필요 없다며.. 몰랐다 난.. 
            for (int i = 0; i < notes.Count;)
            {
                float distance = notes[i].transform.position.z - transform.position.z;
                if (distance < goodCutlineN - 0.5f) //딜리트를 나중에 해주자
                {
                    miss++;
                    gameCon.GetComponent<GameController>().combo = 0; //좌우 관계 없이 초기화 가능 
                    tempAudio.clip = missSfx;
                    tempAudio.Play();
                    damaged.GetComponent<HPController>().PlayerDamage(notes[i].GetComponent<NoteController>().strength);
                    JudgeTextGenerator(2);
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -200), ForceMode.Acceleration);
                    Destroy(notes[i]);
                    notes.RemoveAt(i);
                }
                else if (distance >= goodCutlineP) //아직 판정 구역 바깥이면 인식하지 말기 
                {
                    break;
                }
                else
                {
                    if (ganda == 1)
                    {
                        if (distance > -0.6 && distance < 0.5) //크리티컬 
                        {
                            perfect++;
                            Hit(notes[i]);
                            JudgeTextGenerator(0);
                            Debug.Log("perfect!! : " + distance);
                        }
                        else if (distance > goodCutlineN && distance < goodCutlineP) //힛
                        {
                            good++;
                            Hit(notes[i]);
                            JudgeTextGenerator(1);
                            Debug.Log("good : " + distance);
                        }
                        else //미스, 판정선 지난 이후 삭제되기 전이면 발동됨. 
                        {
                            miss++;
                            Hit(notes[i]);
                            damaged.GetComponent<HPController>().PlayerDamage(notes[i].GetComponent<NoteController>().strength);
                            JudgeTextGenerator(2);
                            gameCon.GetComponent<GameController>().combo = 0; //좌우 관계 없이 초기화 가능 
                            Debug.Log("miss : " + distance);
                        }
                        notes.RemoveAt(i);
                        break; //하나 쳤으면 꺼져 
                    }
                    i++;
                }
            }
    }

    void Hit(GameObject note)
    {
        Transform tempTransform = note.transform;
        GameObject tempEff;
        if (Random.Range(0f, 1f) >= 0.5) //랜덤쓰~ 
        {
            tempEff = hitEff1;
        }
        else
        {
            tempEff = hitEff2;
        }
        GameObject hitEffect = Instantiate(tempEff, tempTransform) as GameObject;
        hitEffect.GetComponent<ParticleSystem>().Play();
        note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //속도를 초기화하고
        note.GetComponent<Rigidbody>().AddForce(new Vector3(1000 * lineSide, 300, 0));
        gameCon.GetComponent<GameController>().combo++; //좌우 관계 없이 초기화 가능
        tempAudio.clip = hitSfx;
        tempAudio.Play();
    }

    public void GameEndNoteDestroyer()
    {
        if (notes != null) //하나만 있을 때는 중괄호 필요 없다며.. 몰랐다 난.. 
            for (int i = 0; i < notes.Count;)
            {
                Destroy(notes[i]); //삭제~
                notes.RemoveAt(i);
                //notes[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //정지하세욧!
                //notes[i].GetComponent<Rigidbody>().isKinematic = true; //안녕히 가세욧! 
                //notes[i].GetComponent<Rigidbody>().useGravity = true; //다음에 또 뵙겠습니닷!
                //if (notes[i].transform.position.y <= -5) //일정 이하로 떨어지면 
                //{
                //    Destroy(notes[i]); //삭제~
                //    notes.RemoveAt(i);
                //}
            }
    }

    void JudgeTextGenerator(int info)
    {
        GameObject judgeTextPrefab = Instantiate(judgeText) as GameObject;
        judgeTextPrefab.GetComponent<JudgeTextController>().info = info;
    }

    void Update()
    {
        Distance(0);
        if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2) || Input.GetKeyDown(key3) || Input.GetKeyDown(key4)) //이게 먹힐지.. 먹힌다구~~~~~~
        {
            Distance(1);
            if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2) || Input.GetKeyDown(key3) || Input.GetKeyDown(key4)) //키에서 손 떼면 
            {
                //this.GetComponent<LineRenderer>().material.color = Color.white;
            }
        }
    }
}