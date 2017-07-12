using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDB : MonoBehaviour {

	void Start()
	{
		//Insures this script will survive
		DontDestroyOnLoad (this);
	}

	void OnApplicationQuit()
	{
		//Get application context
		AndroidJavaClass jclass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jclass.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaObject context = activity.Call<AndroidJavaObject> ("getApplicationContext");

		//Close database with DatabaseCloser
		AndroidJavaObject jo = new AndroidJavaObject ("android.unity.dynamicalarmapp.DatabaseCloser", context);

		Destroy (this);
	}
}
