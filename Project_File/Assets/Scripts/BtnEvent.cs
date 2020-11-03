using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEvent : MonoBehaviour
{
    public void Restart()
    {
        if (UI_Panel_Manager.endState == EndState.End_main)
            UI_Panel_Manager.srGroup(UI_Panel_Manager.User_Panel, UI_Panel_Manager.End_Panel);
        
        else if (UI_Panel_Manager.endState == EndState.After_Recommend)
            UI_Panel_Manager.srGroup(UI_Panel_Manager.User_Panel, UI_Panel_Manager.Recommend_Panel);
        UI_Panel_Manager.endState = EndState.None;
    }

    public void Go_Recommend()
    {
        UI_Panel_Manager.srGroup(UI_Panel_Manager.Recommend_Panel, UI_Panel_Manager.End_Panel);
        UI_Panel_Manager.endState = EndState.Before_Recommend;
    }

    public void Back()
    {
        UI_Panel_Manager.srGroup(UI_Panel_Manager.End_Panel, UI_Panel_Manager.Recommend_Panel);
        UI_Panel_Manager.endState = EndState.End_main;
    }

}
