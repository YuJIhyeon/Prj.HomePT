using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestEmg
{
    class DataManager
    {
        INT int_data = null;
        RMS rms_data = null;
        Gyro gyro_data = null;
        Angle angle_data = null;

        public DataManager()
        {
            int_data = new INT();
            rms_data = new RMS();
            gyro_data = new Gyro();
            angle_data = new Angle();
        }

        public INT getINT()
        {
            return int_data;
        }

        public RMS getRMS()
        {
            return rms_data;
        }

        public Gyro getGyro()
        {
            return gyro_data;
        }
        public Angle getAngle()
        {
            return angle_data;
        }

        public string ToString(int i)
        {
            string result = "";
            try
            {
                result = angle_data.getAngleX()[i] + "\t" + angle_data.getAngleY()[i] + "\t" + angle_data.getAngleZ()[i] + "\t" +
                    gyro_data.getGyroX()[i] + "\t" + gyro_data.getGyroY()[i] + "\t" + gyro_data.getGyroZ()[i] + "\t" +
                    rms_data.getRMS()[i] + "\t" + int_data.getINT()[i];
            }catch (IndexOutOfRangeException e)
            {
                MessageBox.Show("Myo 연결이 잘못 되었습니다. 제대로 연결 후 프로그램을 껏다 켜주세요.");
            }
            return result;
        }

        public void ResetData()
        {
            angle_data.ResetAngle();
            gyro_data.ResetGyro();
            rms_data.ResetRMS();
            int_data.ResetINT();
        }

        public int getSize()
        {
            return angle_data.getAngleX().Count;
        }
    }
}
