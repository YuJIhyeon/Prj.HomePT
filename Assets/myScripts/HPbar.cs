using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;

    private float maxHp = 100;  // 최대 hp 상태
    public static float curHp = 100; //현재 hp 상태

    public static int MinusHP = 5;

    float hpRatio = curHp;

    void Start()
    {
        hpBar.value = (float)curHp / (float)maxHp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))    //1차: spacebar keydown이 있을경우 hpBar가 10씩 떨어짐 >> 팔의 움직임을 조건으로 걸어 떨어지는 수치 변경
        {
            gradient.HP_FLAG = true;
            if (curHp > 0)
            {
                curHp -= MinusHP;
            }
            else
            {
                curHp = 0;
            }
            hpRatio = (float)curHp / (float)maxHp;
        }
        handleHp();

    }

    private void handleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, hpRatio, Time.deltaTime * 10);
    }

}