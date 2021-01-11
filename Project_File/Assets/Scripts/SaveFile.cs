using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SaveFile : MonoBehaviour
{
    string filePath;
    bool writeFlag = false;
    float period = 0;
    public float period_time = 0.1f;
    int sec = 0;
    int cnt_rps = 0;
    FileStream fs = null;
    StreamWriter sw = null;
    public static bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        period = 0;
        writeFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        period += Time.deltaTime;
        if (UserPanel.do_raps == false)
        {
            //Debug.Log("RMS : " + ThalmicMyo.getRMS());
            //Debug.Log("Angle : " + AngleArc.angle);
            if (start && !writeFlag)
            {
                cnt_rps += 1;
                filePath = "./HumbleData/Humble" + DateTime.Now.ToString("MMdd_HHmm_ss")+ "_" + cnt_rps + ".txt";
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
                UserPanel.do_raps = false;
                Debug.Log("Save");

                ProcessStartInfo cmd = new ProcessStartInfo();
                Process pred = new Process();

                cmd.FileName = "cmd.exe";
                cmd.CreateNoWindow = true;
                cmd.UseShellExecute = false;

                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = true;
                cmd.RedirectStandardError = true;

                pred.StartInfo = cmd;
                pred.Start();

                pred.StandardInput.Write("python C:/Users/Neurorobotics/Desktop/Prj.HomePT/pyunicorn/dtw_algorithm.py" + Environment.NewLine);
                pred.StandardInput.Close();
                pred.WaitForExit();
                pred.Close();

                System.IO.StreamReader predict = new System.IO.StreamReader("C:/Users/Neurorobotics/Desktop/Prj.HomePT/pyunicorn/predict.txt");
                String line = predict.ReadLine();

                Debug.Log(line);
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
