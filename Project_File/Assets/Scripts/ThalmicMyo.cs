using UnityEngine;
using System.Collections;

using Arm = Thalmic.Myo.Arm;
using XDirection = Thalmic.Myo.XDirection;
using VibrationType = Thalmic.Myo.VibrationType;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using StreamEmg = Thalmic.Myo.StreamEmg;
using WMPLib;
using System.Data.SqlTypes;
using System.Configuration;
using Windows.Kinect;
using LightBuzz.Vitruvius;

// Represents a Myo armband. Myo's orientation is made available through transform.localRotation, and other properties
// like the current pose are provided explicitly below. All spatial data about Myo is provided following Unity
// coordinate system conventions (the y axis is up, the z axis is forward, and the coordinate system is left-handed).
public class ThalmicMyo : MonoBehaviour {

    // True if and only if Myo has detected that it is on an arm.
    public bool armSynced;

    // Returns true if and only if Myo is unlocked.
    public bool unlocked;

    // The current arm that Myo is being worn on. An arm of Unknown means that Myo is unable to detect the arm
    // (e.g. because it's not currently being worn).
    public Arm arm;

    // The current direction of Myo's +x axis relative to the user's arm. A xDirection of Unknown means that Myo is
    // unable to detect the direction (e.g. because it's not currently being worn).
    public XDirection xDirection;

    // The current pose detected by Myo. A pose of Unknown means that Myo is unable to detect the pose (e.g. because
    // it's not currently being worn).
    public Pose pose = Pose.Unknown;

    // Myo's current accelerometer reading, representing the acceleration due to force on the Myo armband in units of
    // g (roughly 9.8 m/s^2) and following Unity coordinate system conventions.
    public Vector3 accelerometer;

    // Myo's current gyroscope reading, representing the angular velocity about each of Myo's axes in degrees/second
    // following Unity coordinate system conventions.
    public Vector3 gyroscope;

    public Thalmic.Myo.Result streamEmg;
    
    public int[] emg;

    // True if and only if this Myo armband has paired successfully, at which point it will provide data and a
    // connection with it will be maintained when possible.
    public bool isPaired {
        get { return _myo != null; }
    }

    // Vibrate the Myo with the provided type of vibration, e.g. VibrationType.Short or VibrationType.Medium.
    public void Vibrate (VibrationType type) {
        _myo.Vibrate (type);
    }

    // Cause the Myo to unlock with the provided type of unlock. e.g. UnlockType.Timed or UnlockType.Hold.
    public void Unlock (UnlockType type) {
        _myo.Unlock (type);
    }

    // Cause the Myo to re-lock immediately.
    public void Lock () {
        _myo.Lock ();
    }

    /// Notify the Myo that a user action was recognized.
    public void NotifyUserAction () {
        _myo.NotifyUserAction ();
    }

    void Start() {
        if (isPaired) {
            streamEmg = _myo.SetStreamEmg (_myoStreamEmg);
        }
    }
    private int period = 0;
    void Update() {
        lock (_lock) {
            armSynced = _myoArmSynced;
            arm = _myoArm;
            xDirection = _myoXDirection;
            if (_myoQuaternion != null) {
                transform.localRotation = new Quaternion(_myoQuaternion.Y, _myoQuaternion.Z, -_myoQuaternion.X, -_myoQuaternion.W);
            }
            if (_myoAccelerometer != null) {
                accelerometer = new Vector3(_myoAccelerometer.Y, _myoAccelerometer.Z, -_myoAccelerometer.X);
            }
            if (_myoGyroscope != null) {
                gyroscope = new Vector3(_myoGyroscope.Y, _myoGyroscope.Z, -_myoGyroscope.X);
            }
            if (isPaired && streamEmg == Thalmic.Myo.Result.Success) {
                emg = _myo.emgData;
            }

            period += 1;
            if (period == 100)
            {
                period = 0;
                pose = _myoPose;
                unlocked = _myoUnlocked;
            }
            
        }


        double rms = calRMS();
        calINT(rms, collection_flag);
        //Debug.Log(getRMS());
    }

    public double calRMS()
    {
        for (int i = 0; i < _myo.emgData.Length; i++)
        {
            RMS += Mathf.Pow(_myo.emgData[i], 2);
            ED[i] = _myo.emgData[i];
        } 

        RMS = Mathf.Sqrt(RMS / 8);
        RMS = (float)System.Math.Round((double)RMS, 2);
        
        return RMS;
    }

    public static int[] ED = new int[8];
    private static float INT = 0;
    private static float RMS = 0;
    public static bool collection_flag = false;

    public static void change_CollectFlag()
    {
        ThalmicMyo.collection_flag = !ThalmicMyo.collection_flag;
    }

    private double calINT(double rms, bool collection_flag)
    {
        if (collection_flag) INT += (float)rms;
        else INT = 0;
        return INT;
    }
    public static float getRMS()
    {
        return RMS;
    }
    public static float getINT()
    {
        return INT;
    }

    void myo_OnArmSync(object sender, Thalmic.Myo.ArmSyncedEventArgs e) {
        lock (_lock) {
            _myoArmSynced = true;
            _myoArm = e.Arm;
            _myoXDirection = e.XDirection;
        }
    }

    void myo_OnArmUnsync(object sender, Thalmic.Myo.MyoEventArgs e) {
        lock (_lock) {
            _myoArmSynced = false;
            _myoArm = Arm.Unknown;
            _myoXDirection = XDirection.Unknown;
        }
    }

    void myo_OnOrientationData(object sender, Thalmic.Myo.OrientationDataEventArgs e) {
        lock (_lock) {
            _myoQuaternion = e.Orientation;
        }
    }

    void myo_OnAccelerometerData(object sender, Thalmic.Myo.AccelerometerDataEventArgs e) {
        lock (_lock) {
            _myoAccelerometer = e.Accelerometer;
        }
    }

    void myo_OnGyroscopeData(object sender, Thalmic.Myo.GyroscopeDataEventArgs e) {
        lock (_lock) {
            _myoGyroscope = e.Gyroscope;
        }
    }

    void myo_OnEmgData(object sender, Thalmic.Myo.EmgDataEventArgs e) {
        lock (_lock) {
            _myoEmg = e.Emg;
        }
    }

    void myo_OnPoseChange(object sender, Thalmic.Myo.PoseEventArgs e) {
        lock (_lock) {
            _myoPose = e.Pose;
        }
    }

    void myo_OnUnlock(object sender, Thalmic.Myo.MyoEventArgs e) {
        lock (_lock) {
            _myoUnlocked = true;
        }
    }

    void myo_OnLock(object sender, Thalmic.Myo.MyoEventArgs e) {
        lock (_lock) {
            _myoUnlocked = false;
        }
    }

    public Thalmic.Myo.Myo internalMyo {
        get { return _myo; }
        set {
            if (_myo != null) {
                _myo.ArmSynced -= myo_OnArmSync;
                _myo.ArmUnsynced -= myo_OnArmUnsync;
                _myo.OrientationData -= myo_OnOrientationData;
                _myo.AccelerometerData -= myo_OnAccelerometerData;
                _myo.GyroscopeData -= myo_OnGyroscopeData;
                _myo.PoseChange -= myo_OnPoseChange;
                _myo.Unlocked -= myo_OnUnlock;
                _myo.Locked -= myo_OnLock;
            }
            _myo = value;
            if (value != null) {
                value.ArmSynced += myo_OnArmSync;
                value.ArmUnsynced += myo_OnArmUnsync;
                value.OrientationData += myo_OnOrientationData;
                value.AccelerometerData += myo_OnAccelerometerData;
                value.GyroscopeData += myo_OnGyroscopeData;
                value.PoseChange += myo_OnPoseChange;
                value.Unlocked += myo_OnUnlock;
                value.Locked += myo_OnLock;
            }
        }
    }

    private Object _lock = new Object();

    private bool _myoArmSynced = false;
    private Arm _myoArm = Arm.Unknown;
    private XDirection _myoXDirection = XDirection.Unknown;
    private Thalmic.Myo.Quaternion _myoQuaternion = null;
    private Thalmic.Myo.Vector3 _myoAccelerometer = null;
    private Thalmic.Myo.Vector3 _myoGyroscope = null;
    private int[] _myoEmg = new int[7];
    private Pose _myoPose = Pose.Unknown;
    private bool _myoUnlocked = false;
    private StreamEmg _myoStreamEmg = StreamEmg.Enabled;

    private Thalmic.Myo.Myo _myo;
}
