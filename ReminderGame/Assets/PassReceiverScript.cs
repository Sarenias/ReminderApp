using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassReceiverScript : MonoBehaviour {

	AndroidJavaObject receiver;
	GameObject javaComm;
	AlarmScript script;

	void Start()
	{
		//Get communicator from last scene
		javaComm = GameObject.Find("JavaComm") as GameObject;

		//Get its attached script
		script = javaComm.GetComponent<AlarmScript> ();

		//Get script's UnityReceiver instance
		receiver = script.returnUnityReceiver ();
	}

	public void stopTimer()
	{
		//Get application context
		AndroidJavaClass jclass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jclass.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaObject context = activity.Call<AndroidJavaObject> ("getApplicationContext");

		//Stop alarm tone
		receiver.Call ("stop", context);

		//Get alarm time key
		string title = receiver.Call<long> ("returnID").ToString() + ".jpg";

		//Use time key as screenshot name
		Application.CaptureScreenshot(title);
		print (title);
		//Make string out of pathname
		string pathname = Application.persistentDataPath + "\\" + title;
		print (pathname);
		//Add pathname to database
		receiver.Call("addPathname", pathname, receiver.Call<long>("returnID"));
		//Restart scene1
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		//Restart paused alarms
		receiver.Call("restartActiveAlarms", context);
		//Destroy this object
		Destroy(javaComm);
	}

}
