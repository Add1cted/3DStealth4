using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof( ThirdPersonWallCover ))]
public class AudioMixerCoverSnapshots : MonoBehaviour {
    public AudioMixerSnapshot   noCoverSnap, inCoverSnap;

    ThirdPersonWallCover        tpwc;
    bool                        inCoverCache = false;

	// Use this for initialization
	void Start () {
        tpwc = GetComponent<ThirdPersonWallCover>();
        noCoverSnap.TransitionTo(0);
	}
	
	void Update () {
        bool inCoverCurr = (tpwc.inCover > -1);
        if (inCoverCurr != inCoverCache) {
            if (inCoverCurr) {
                inCoverSnap.TransitionTo(0);
            } else {
                noCoverSnap.TransitionTo(0);
            }
            inCoverCache = inCoverCurr;
        }
	}
}
