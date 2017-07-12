using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlarmScript : MonoBehaviour {
    AndroidJavaObject receiver;
	bool broadcastReceived = false;

    void Start()
    {
		//Make sure this object survives next scene load
		DontDestroyOnLoad(this);
		//Get instance of UnityReceiver (middleman between Java code and Unity)
        AndroidJavaClass jclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jclass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass unityReceive = new AndroidJavaClass("android.unity.dynamicalarmapp.UnityReceiver");
		receiver = unityReceive.CallStatic<AndroidJavaObject>("createInstance", context);

		//Create object StartMain, which starts main activity in DynamicAlarmApp plugin
        AndroidJavaObject jo = new AndroidJavaObject("android.unity.dynamicalarmapp.StartMain", activity);
    }

    void Update()
    {
		//Check for full alarm 
		//If full alarm has gone off, stop checking
		if (!broadcastReceived) {
			broadcastReceived = checkReceiverStatus ();
		}
    }

	public AndroidJavaObject returnUnityReceiver()
	{
		//Return instance of receiver
		return receiver	;
	}

	bool checkReceiverStatus()
	{
		//Set bool to full alarm status in receiver (false = alarm has not gone off)
		bool finished = receiver.CallStatic<bool> ("getStatus");
		//If alarm has finished
		if (finished == true) { 
			//Change bool back
			receiver.CallStatic("resetAlarm");
			//Load next scene
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			return true;
		}
		return false;
	}




}
