using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Exercise_Video : MonoBehaviour
{
    public VideoPlayer videoPlayer1, videoPlayer2;
    public VideoClip dumbbelCurl1, dumbbelCurl2;
	public Text exercise_name;

	public void Start()
	{
		if(UI_Panel_Manager.exercise == ExerciseType.Dumbbell_curl){
			exercise_name.text = "Dumbbel Curl";
		}
		else if(UI_Panel_Manager.exercise == ExerciseType.Dumbbell_kick_back){
			exercise_name.text = "Triceps Kickback";
		}
		VideoPlay();
	}
	public void VideoPlay(){
        if(UI_Panel_Manager.exercise == ExerciseType.Dumbbell_curl){
			videoPlayer1.clip = dumbbelCurl1;
			videoPlayer2.clip = dumbbelCurl2;
			videoPlayer1.Play();
			videoPlayer2.Play();
		}
	}
}
