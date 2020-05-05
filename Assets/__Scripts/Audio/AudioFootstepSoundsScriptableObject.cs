using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/AudioFootstepsSO", fileName = "AudioFootstepsSO_.asset")]
[System.Serializable]
public class AudioFootstepSoundsScriptableObject : ScriptableObject {
    public List<AudioClip>  clips = new List<AudioClip>();

    int lastClip = -1;

    public AudioClip GetClip(int n=-1) {
        // Check two edge cases
        if (clips == null || clips.Count == 0) {
            return null;
        }
        if (clips.Count == 1) {
            return clips[0];
        }
        // If n was not specified or was not valid, randomly choose a clip
        if (n < 0 || n >= clips.Count) {
            // Choose a random clip and try to make it not the same clip as last time
            do {
                n = Random.Range(0,clips.Count);
            } while (n == lastClip);
        }       
		lastClip = n;
        return clips[n];
    }
}
