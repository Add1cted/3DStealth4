using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( AudioSource ))]
public class AudioFootstepCollider : MonoBehaviour {
    static private List<AudioFootstepCollider> INSTANCES = new List<AudioFootstepCollider>();

    public bool         muteNextStep = true;

    private AudioSource source;

	private void Awake()
	{
        if (!INSTANCES.Contains(this)) {
            INSTANCES.Add(this);
        }
        source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
        AudioGroundSound groundSound = other.GetComponent<AudioGroundSound>();
        if (groundSound == null) {
            return;
        }

        // This stops the footsteps from making sound one time (e.g., at the beginning of a level)
        if (muteNextStep) {
            muteNextStep = false;
            return;
        }

        AudioClip clip = groundSound.GetClip();
        source.PlayOneShot(clip);
	}

    static public void MUTE_NEXT_STEPS() {
        foreach (AudioFootstepCollider instance in INSTANCES) {
            instance.muteNextStep = true;
        }
    }
}
