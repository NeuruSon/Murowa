using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    static float speedChild = 100;
    static float bpmChild = 100;
    public float strength = 10;

    bool badack = false; //충돌 검사를 한 번만 실행하게 하는 스위치 

    void Start()
    {

    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 30)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(int lineSide, float speed, float bpm)
    {
        speedChild = speed; //하하 이것을 쓰기 위해 static으로 설정했지
        bpmChild = bpm;

        GetComponent<Rigidbody>().AddForce(new Vector3(60 * lineSide, 300, -150));
    }

    public void Hit(GameObject note, int lineSide)
    {
        note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //속도를 초기화하고
        note.GetComponent<Rigidbody>().AddForce(new Vector3(1000 * lineSide, 300, 0));
    }

    void OnCollisionEnter(Collision other)
    {
        if (badack == false) //처음 바닥에 닿으면(=판에 닿았으면) 
        {
            badack = true; //너는 더 이상 충돌 메소드의 바디를 만날 수 없을 것이다. 
            Vector3 shoot = new Vector3(0, 0, -bpmChild * speedChild);
            this.GetComponent<Rigidbody>().useGravity = false; //중력 변수 제거. 일정한 속도로 구르도록.
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //속도를 초기화하고
            GetComponent<Rigidbody>().AddForce(shoot); //다시 force 가함
        }
    }
}
