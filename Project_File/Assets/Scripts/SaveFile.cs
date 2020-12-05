using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    string filePath;
    bool writeFlag = false;
    float period = 0;
    public float period_time = 0.1f;
    int sec = 0;
    FileStream fs = null;
    StreamWriter sw = null;

    // Start is called before the first frame update
    void Start()
    {
        period = 0;
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (ThalmicMyo.collection_flag)
        {
            //Debug.Log("RMS : " + ThalmicMyo.getRMS());
            //Debug.Log("Angle : " + AngleArc.angle);
            if (!writeFlag)
            {
                filePath = "./HumbleData/Humble" + DateTime.Now.ToString("MMdd_HHmm_ss") + ".txt";
                fs = new FileStream(filePath, FileMode.Create);
                sw = new StreamWriter(fs);
                writeFlag = true;
            }
        }
        else
        {
            if (writeFlag)
            {
                sw.Close();
                fs.Close();
                sec = 0;
                writeFlag = false;
                Debug.Log("Save");
            }
        }
        
        if (writeFlag && period >= period_time)
        {
            sec += 1;
            period = 0;
            sw.Write(sec*0.1 + "\t" + ThalmicMyo.getRMS() + "\t" + Math.Round(AngleArc.angle, 2) + "\n");
        }
    }
}
