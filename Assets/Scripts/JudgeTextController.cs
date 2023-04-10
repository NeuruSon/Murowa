using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.Presets;
#endif

public class JudgeTextController : MonoBehaviour
{
    private float moveSpeed, destroyTime, scaleSpeed;
    TextMeshPro judge_default;
    TextMeshProUGUI comboCombo, comboCounter;
    public int info;
    bool enough;

#if UNITY_EDITOR
    public Preset JC, JH, JM;
#endif

    private void GetRidOfText()
    {
        Destroy(gameObject);
    }

    void SizeUpAndDown(bool enough) //콤보 둥둥 
    {
        if (enough == false)
        {
            comboCombo.fontSize = 70;
            comboCounter.fontSize = 80;
            this.enough = true;
        }
        else //true 
        {
            comboCombo.fontSize = 40 * scaleSpeed / Time.time + 50;
            comboCounter.fontSize = 40 * scaleSpeed / Time.time + 60;
        }

    }

    void Start()
    {
        moveSpeed = 1.0f;
        destroyTime = 1.0f;
        scaleSpeed = 0.98f;
        judge_default = GetComponent<TextMeshPro>();
        comboCombo = GameObject.Find("COMBO").GetComponent<TextMeshProUGUI>();
        comboCounter = GameObject.Find("NUM").GetComponent<TextMeshProUGUI>();
        enough = false;

        switch (info) //판정 정보를 받아서 한 프리팹 돌려쓰기 
        {
            case 0: //크리티컬
                judge_default.text = "CRITICAL";
#if UNITY_EDITOR
                JC.ApplyTo(judge_default.colorGradientPreset);
#endif
                break;
            case 1: //힛
                judge_default.text = "HIT";
#if UNITY_EDITOR
                JH.ApplyTo(judge_default.colorGradientPreset);
#endif
                break;
            case 2:  //미스
                judge_default.text = "MISS";
#if UNITY_EDITOR
                JM.ApplyTo(judge_default.colorGradientPreset);
#endif
                break;
        }
        Invoke("GetRidOfText", destroyTime);

    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); //텍스트 위치
        SizeUpAndDown(enough);
    }
}
