using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEvent : MonoBehaviour
{
    public void Restart()
    {
        UI_Panel_Manager.srGroup(UI_Panel_Manager.User_Panel, UI_Panel_Manager.End_Panel);
    }

}
