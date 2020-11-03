﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEvent : MonoBehaviour
{
    public void Btn_Restart()
    {
        if (UI_Panel_Manager.curState == DisplayState.End_main)
            UI_Panel_Manager.srGroup(UI_Panel_Manager.User_Panel, UI_Panel_Manager.End_Panel);
        
        else if (UI_Panel_Manager.curState == DisplayState.After_Recommend)
            UI_Panel_Manager.srGroup(UI_Panel_Manager.User_Panel, UI_Panel_Manager.Recommend_Panel);
        UI_Panel_Manager.curState = DisplayState.Exercise;
    }

    public void Btn_Recommend()
    {
        UI_Panel_Manager.srGroup(UI_Panel_Manager.Recommend_Panel, UI_Panel_Manager.End_Panel);
        UI_Panel_Manager.curState = DisplayState.Before_Recommend;
    }

    public void Btn_Back()
    {
        UI_Panel_Manager.srGroup(UI_Panel_Manager.End_Panel, UI_Panel_Manager.Recommend_Panel);
        UI_Panel_Manager.curState = DisplayState.End_main;
    }

    // Measure 창으로 가는 버튼 함수
    public void Btn_Go_Measure()
    {
        UI_Panel_Manager.srGroup(UI_Panel_Manager.Measure_Panel, UI_Panel_Manager.Start_Panel);
        UI_Panel_Manager.curState = DisplayState.Before_Measure;
    }

    // 누르면 측정 시작
    public void Btn_Measure()
    {
        UI_Panel_Manager.curState = DisplayState.After_Measure;
    }

    public void Btn_Go_Exercise()
    {

        // 측정을 했으면
        if (Measure.maxRms != 0)
        {
            UI_Panel_Manager.srGroup(UI_Panel_Manager.User_Panel, UI_Panel_Manager.Measure_Panel);
            UI_Panel_Manager.curState = DisplayState.Exercise;
        }
        
        // 측정 안했으면
        else
        {
            Debug.Log("측정이 안됐습니다.");
        }
    }

}
