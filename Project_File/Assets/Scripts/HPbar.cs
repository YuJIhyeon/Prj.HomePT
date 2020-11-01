using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;

    private float maxHp = 3000;  // 최대 hp 상태
    public static float curHp = 3000; //현재 hp 상태
    public static int MinusHP = 5;

    float hpRatio = curHp;

    void Start()
    {
        curHp = maxHp;
        hpBar.value = (float)curHp / (float)maxHp;
    }

    private int period = 0;
    void Update()
    {
        period += 1;
        //Debug.Log(period);
        if (Input.GetKeyDown(KeyCode.Space))     // 스페이스바 누르면 INT 측정 시작
        {
            ThalmicMyo.change_CollectFlag();
            curHp = maxHp;
        }

        if (period > 100 && ThalmicMyo.collection_flag)    //1차: spacebar keydown이 있을경우 hpBar가 10씩 떨어짐 >> 팔의 움직임을 조건으로 걸어 떨어지는 수치 변경
        {
            period = 0;
            gradient.HP_FLAG = true;
            if (curHp > 0)
            {
                curHp -= ThalmicMyo.getRMS();
                //Debug.Log("HP : " + HPbar.curHp);
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
        //hpBar.value = hpRatio;
        hpBar.value = Mathf.Lerp(hpBar.value, hpRatio, Time.deltaTime * 10);
    }

}