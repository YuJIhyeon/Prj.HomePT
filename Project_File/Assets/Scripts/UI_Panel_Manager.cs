using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UI_Panel_Manager : MonoBehaviour
{
    public CanvasGroup u_p;
    public CanvasGroup e_p;
    public static CanvasGroup User_Panel;
    public static CanvasGroup End_Panel;

    public void Start()
    {
        User_Panel = u_p;
        End_Panel = e_p;
        srGroup(User_Panel, End_Panel);
    }

    public static void srGroup(CanvasGroup c1, CanvasGroup c2)         // 순서대로 1, 0 대입
    {
        c1.alpha = 1;
        c1.interactable = true;
        c1.blocksRaycasts = true;

        c2.alpha = 0;
        c2.interactable = false;
        c2.blocksRaycasts = false;
    }
}
