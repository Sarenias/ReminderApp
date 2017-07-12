/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        private TrackableBehaviour mTrackableBehaviour;
        private Canvas canvasObject;

        void Start()
        {
			// Register Vuforia life-cycle callbacks:
			VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            
        }

		void OnVuforiaStarted ()
		{
			bool focusModeSet = CameraDevice.Instance.SetFocusMode(
				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

			if (!focusModeSet)
			{
				Debug.Log("Failed to set focus mode (unsupported mode).");
			}
			canvasObject = (Canvas)FindObjectOfType(typeof(Canvas));
			canvasObject.enabled = false;

			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
		}
			
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            print(newStatus);
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED)
            {
                canvasObject.enabled = true;
            }
            else
            {
                canvasObject.enabled = false;
            }
        }
			
    }
}
