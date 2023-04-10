using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextController : MonoBehaviour
{
    private float moveSpeed, destroyTime, damageValue;
    TextMeshProUGUI damageText;

    private void GetRidOfText()
    {
        Destroy(gameObject);
    }

    public void DamageValue(float mobStrength)
    {
        damageValue = mobStrength;
    }

    void Start()
    {
        moveSpeed = 7.0f;
        destroyTime = 1.0f;
        damageText = GetComponent<TextMeshProUGUI>();
        damageText.text = "-" + damageValue;
        Invoke("GetRidOfText", destroyTime);
    }

    void Update()
    {
        damageText.rectTransform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); //텍스트 위치
    }
}
