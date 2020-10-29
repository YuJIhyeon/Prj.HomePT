using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{

    public GameObject PracticeText;
    public Text setText;
    public Text repText;

    private int reps;
    private int sets;

    #region StopWatch
    float timer;
    float seconds;
    float minutes;
    float hours;

    bool start;

    [SerializeField] Text TimeText;
    #endregion StopWatch



    void Start()
    {

        reps = 0;
        sets = 0;

        start = false;
        timer = 0;

    }

    
    void Update()
    {
        StopWatch();
    }


    void StopWatch()
    {
        if (start)
        {
            timer += Time.deltaTime;
            seconds = (int)(timer % 60);
            minutes = (int)((timer / 60) % 60);
            hours = (int)(timer / 3600);

            TimeText.text = "Time: " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }   
    }

    public void StartTimer()
    {
        start = true;
    }
    public void StopTimer()
    {
        start = false;
    }

    public void ResetTimer()
    {
        start = false;
        timer = 0;
    }

}
