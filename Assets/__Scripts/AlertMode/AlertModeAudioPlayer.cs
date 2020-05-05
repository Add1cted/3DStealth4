using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(AudioSource) )]
public class AlertModeAudioPlayer : MonoBehaviour {
	AudioSource  audioSource;

    // Use this for initialization
    void Start ()
	{
        audioSource = GetComponent<AudioSource>();
        // Register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;
    }

    private void OnDestroy()
    {
        // De-register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate -= AlertModeStatusChange;
    }

    void AlertModeStatusChange(bool alertMode)
	{
		if (alertMode) {
			audioSource.Play();
		} else {
			audioSource.Stop();
		}      
    }
}
