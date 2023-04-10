using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BPMLineGeneretor : MonoBehaviour
{
    ////궁극적인 목표; 비폠 바뀐다고 해도 바뀐 비폠 맞게 라인 보낼 것. 
    //public GameObject bpmLine, musicTime;
    //public AudioClip readySfx, tutoMusic, queueMusic; //queue는 나중에 다른 음악 필요하면 넣을 temp 공간 
    //AudioSource currentMusic;
    //public float bpm; //바깥에서 받긔 
    //public float speed = 1;
    //public float delay = 0;
    //public static float musicPlayTime = 0;
    //public bool musicPlaying = false;

    //void BPMLineGenerator()
    //{
    //    GameObject line = Instantiate(bpmLine) as GameObject;
    //    line.GetComponent<BPMLineController>().Shoot(bpm, speed);
    //}

    //void MusicPlay()
    //{
    //    musicPlaying = true;
    //    currentMusic.clip = tutoMusic;
    //    Debug.Log("Music length : " + currentMusic.clip.length); //일단 음원 길이는 구했는데 메소드라 어디 저장은 안 됨. 저장하고 싶으면 새로 배리어블 만들어라 
    //    currentMusic.Play();
    //    musicTime.GetComponent<Slider>().maxValue = currentMusic.clip.length * 1000; //음악 진행도 바 최댓값을 음악 길이(매끄러운 증가를 위해 소수점 제거: * 1000)로 설정.
    //}

    void Start()
    {
        //currentMusic = GetComponent<AudioSource>();
        //currentMusic.clip = readySfx;
        //currentMusic.Play();

        //InvokeRepeating("BPMLineGenerator", 7.0f, 60 / bpm * speed); //60초를 비폠으로 나누고 1비트당 할당될 초 계산. 
        //Invoke("MusicPlay", 7.0f + 60 / bpm + delay); //이 시간 후에 실행하세요~ 인데 의도한 것: 비폠라인 하나는 보내고 다음 비폠라인(+못갖춘 고려한 딜레이) 맞춰 노래 시작. 하지만 바꿨음.. 의도: 라인 하나 보내고(60/bpm) 
    }

    void Update()
    {
        //if (musicPlaying == true) //정상적으로 재생중이면 
        //{
        //    musicPlayTime += Time.deltaTime; //카운트 시작 
        //    musicTime.GetComponent<Slider>().value = musicPlayTime * 1000;
        //    if (musicPlayTime == currentMusic.clip.length) //끝까지 재생이 됐다면 
        //    {
        //        CancelInvoke("BPMLineGenerator");
        //        musicPlaying = false;
        //        musicPlayTime = 0; //초기화. 
        //    }
        //}
    }
}
