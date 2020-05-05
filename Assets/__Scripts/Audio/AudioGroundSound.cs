using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGroundSound : MonoBehaviour {
    public AudioFootstepSoundsScriptableObject footstepsSO;

    public AudioClip GetClip() {
        if (footstepsSO == null) {
            return null;
        }
        return footstepsSO.GetClip();
    }
}
