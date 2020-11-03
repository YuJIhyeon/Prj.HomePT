using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

enum User_state
{
    Idle,
    exercising
}

public class UserPanel : MonoBehaviour
{
    private User_state user_state;

    public Text setText;
    public Text repText;

    private int reps;
    private int sets;

    #region StopWatch
    float timer;
    float milseconds, seconds, minutes;

    [SerializeField] Text TimeText;
    #endregion StopWatch
    void Start()
    {
        initialUI();
    }

    void initialUI()
    {
        timer = 0;
        reps = 0;
        sets = 0;
        setText.text = "Sets: 0 / 5";
        repText.text = "Reps: 0 / 10";
        TimeText.text = "Time: 00:00:00";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sets >= 5)
            {
                UI_Panel_Manager.srGroup(UI_Panel_Manager.End_Panel, UI_Panel_Manager.User_Panel);
                UI_Panel_Manager.endState = EndState.End_main;
                initialUI();
            }

            if (user_state == User_state.Idle)
            {
                sets += 1;
                timer = 0;
                user_state = User_state.exercising;
                TimeText.text = "Time: 00:00:00";
            }
            else
            {
                setText.text = "Sets: " + sets + " / 5";
                user_state = User_state.Idle;
            }
        }

        if(user_state == User_state.exercising)
        {
            StopWatch();
        }
    }

    void StopWatch()
    {
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
        milseconds = Mathf.Ceil((timer - seconds)*100);
        minutes = (int)((timer / 60) % 60);

        TimeText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milseconds.ToString("00");
    }
}
