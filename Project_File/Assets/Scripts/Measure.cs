using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measure : MonoBehaviour
{
    public Text RMS;
    public Text maxRMS;
    bool isChange = false;

    float period = 0;
    float rms = 0;
    public static float maxRms = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (period > 0.2f)
        {
            period = 0;
            if (UI_Panel_Manager.curState == DisplayState.After_Measure)
            {
                rms = ThalmicMyo.getRMS();
                if (maxRms < rms)
                {
                    maxRms = rms;
                    isChange = true;
                }
                RMS.text = System.Math.Round(rms, 2) + " RMS";

                if (isChange)
                {
                    isChange = false;
                    maxRMS.text = "당신의 최대 RMS : " + System.Math.Round(maxRms, 2);
                }
            }
        }  
    }
}
