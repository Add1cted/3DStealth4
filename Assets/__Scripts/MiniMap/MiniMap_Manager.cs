using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains a list of Gameobject tags to be tracked and the color to make them.
/// Also manages MiniMap_Blip creation, assignment, and destruction.
/// </summary>
public class MiniMap_Manager : MonoBehaviour {
    [Header("Inscribed")]
    public List<TrackTagInfo>       tagsToTrack;

    private List<MiniMap_Blip>      activeBlips = new List<MiniMap_Blip>();

    // Use this for initialization
	void Start () {
		GameManager.LEVEL_START_EVENT += AssignBlips;
		GameManager.LEVEL_END_EVENT += DestroyActiveBlips;
	}
	

	void AssignBlips() {
        if (activeBlips != null && activeBlips.Count > 0) {
            DestroyActiveBlips();
        }

        GameObject blipGO;
        MiniMap_Blip blip;

        // foreach used to cause lots of garbage collection issues, but that has
        //  largely been resolved in recent versions of Unity
        foreach (TrackTagInfo tC in tagsToTrack) {
            GameObject[] tGOs = GameObject.FindGameObjectsWithTag(tC.tag);
            // If no GameObjects with that tag were found, continue
            if (tGOs.Length == 0) {
                Debug.LogWarning("MiniMap_Manager:AssignBlips() – No GameObjects"
                                 + "were found with the tag \""+tC.tag+"\"");
                continue;
            }
            foreach (GameObject tGO in tGOs) {
                blipGO = Instantiate<GameObject>(tC.prefab);
                blip = blipGO.GetComponent<MiniMap_Blip>();
                blip.transform.SetParent(transform.parent);
                blip.color = tC.color;
                blip.trackedTransform = tGO.transform;
                activeBlips.Add(blip);
            }
        }   
	}


    private void DestroyActiveBlips()
    {
        MiniMap_Blip blip;
        while (activeBlips.Count > 0) {
            blip = activeBlips[activeBlips.Count-1];
            activeBlips.Remove(blip);
            Destroy(blip.gameObject);
        }
    }


    [System.Serializable]
    public struct TrackTagInfo {
        public string       tag;
        public Color        color;
        public GameObject   prefab;
    }

}
