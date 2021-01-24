using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Windows.Kinect;
using LightBuzz.Vitruvius;

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
    float period=0;
    public static bool do_raps = false;

    public static double RMS = 0;
    public static double ANG = 0;
    double Y_hat = 1;
    

    #region StopWatch
    float timer;
    float milseconds, seconds, minutes;

    [SerializeField] Text TimeText;
    #endregion StopWatch
    void Start()
    {
        initialUI();
        period = 0;
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
        period += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            //recomand
            if (sets >= 5)
            {
                UI_Panel_Manager.srGroup(UI_Panel_Manager.End_Panel, UI_Panel_Manager.User_Panel);
                UI_Panel_Manager.curState = DisplayState.End_main;
                initialUI();
            }
            

            if (user_state == User_state.Idle)
            {
                //sets += 1;
                timer = 0;
                reps = 0;
                user_state = User_state.exercising;
                TimeText.text = "Time: 00:00:00";
                do_raps = false;
                SaveFile.start = false;
            }
            else
            {
                setText.text = "Sets: " + sets + " / 5";
                repText.text = "Reps: 0 / 10";
                do_raps = true;
                user_state = User_state.Idle;
            }
        }

        if(user_state == User_state.exercising)
        {
            StopWatch();         
        }

        if (user_state==User_state.exercising && period >= 2f)
        {
            period = 0;

            RMS = (double)(Math.Round(ThalmicMyo.getRMS(),2));
            ANG = (double)(Math.Round(AngleArc.angle, 2));
            
            
            //Debug.Log("RMS: " + RMS.ToString());
            
            Debug.Log("ANG: " + ANG.ToString());
            
            Y_hat = formal(RMS, ANG);
            //Debug.Log(RMS + "," + ANG);
            //Debug.Log("Y_hat:" + Y_hat.ToString());
            if (Y_hat >= 0.6254)    //6248 , 6254
            {
                reps += 1;
                do_raps = true;
                repText.text = "Reps: " + (reps-1).ToString() + " /10";
                //Y_hat = formal(RMS, ANG);
            }

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


    double sigmoid(double z)
    {
        return 1.0 / (1 + Math.Exp(-1 * z));
    }

    double formal(double RMS, double ANG)
    {
        //Normalize
        if (RMS >= 5.47)
        {
            RMS = (RMS - 5.47) / (90.48);
        }
        else
            RMS = 0;

        if (ANG >= 36.23)
        {
            ANG = (ANG - 36.23) / (141.12);
        }
        else
            ANG = 0;

        //After first layer
        double X1 = 6.01971921 * RMS - 15.7309641 * ANG - 4.82527526;
        double X2 = -7.96912376 * RMS - 11.27525099 * ANG + 0.96991828;
        

        double H1 = sigmoid(X1);
        double H2 = sigmoid(X2);

        double A = 5.18146232 * H1 - 5.8404422 * H2 + 0.5126402;

        double Y_hat = sigmoid(A);

        return Y_hat; 
    }
   
}
