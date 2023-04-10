using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float RotateSpeed = 1000f;
    public float WalkSpeed = 0.1f;

    [SerializeField] private Animator m_animator; //asset에서 제공하는 스크립트에서 떼어옴
    [SerializeField] private Rigidbody m_rigidBody; //asset에서 제공하는 스크립트에서 떼어옴 

    public void Initialize(GameObject character) //asset에서 제공하는 스크립트에서 떼어옴 
    {
        m_animator = character.GetComponent<Animator>(); //asset에서 제공하는 스크립트에서 떼어옴 
        m_rigidBody = character.GetComponent<Rigidbody>(); //asset에서 제공하는 스크립트에서 떼어옴 
    }

    void Awake() //asset에서 제공하는 스크립트에서 떼어옴 
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); } //asset에서 제공하는 스크립트에서 떼어옴 
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); } //asset에서 제공하는 스크립트에서 떼어옴 
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(-Vector3.up * RotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.forward * WalkSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(-Vector3.forward * WalkSpeed * Time.deltaTime);
    }
}
