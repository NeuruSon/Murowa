using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMLineController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(float bpm, float speed)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -bpm * speed));
    }

    void OnBecameInvisible() //화면 밖으로 나갔어? 
    {
        Destroy(this.gameObject); //그럼 꺼져; 
    }
}
