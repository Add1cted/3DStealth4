//#define DEBUG_MiniMap_Camera_ShowMinMax

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Camera) )]
public class MiniMap_Camera : MonoBehaviour {
    [Header("Inscribed")]
    [Tooltip("If set to 0, this scales itself based on objects in the Ground and" +
             "Cover layers. If set >0, this is the OrthographicSize of the Camera.")]
    public float        camSize = 0;

    [Tooltip("If camSize==0, these are the layers to contain within the Camera view.")]
    // Note that we could have pulled the cullingMask from the Camera, but there 
    //  are some Layers that we want to render but don't necessarily want to zoom
    //  to (for instance, the EnemyLightCone layer).
    public LayerMask    layersToZoomTo;

    [Tooltip("What percent of the MiniMap should be left as a frame around the level?")]
    public float        zoomBorder = 0.1f;

    private Camera      cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
		GameManager.LEVEL_START_EVENT += FillCameraWithLevel;
	}

    /// <summary>
    /// This searches through ALL GameObjects for those that are on either the
    /// Ground or Cover layers. This is very slow, so it should only be done at 
    /// the beginning of the game or when a new level is loaded.
    /// </summary>
    private void FillCameraWithLevel()
    {
        // For this to work, the Camera must be orthographic
        if (!cam.orthographic) {
            Debug.LogWarning("For MiniMap Camera scaling to work automatically, "
                             + "the camera must be orthographic.");
            cam.orthographic = true;
        }

        // This line is really slow, the rest is pretty fast though
        GameObject[] allGOs = FindObjectsOfType<GameObject>();

        Vector3 xzMin = Vector3.zero, xzMax = Vector3.zero;
        Renderer tRend;
        int binaryZoomLayers = layersToZoomTo.value;
        int binaryLayer;
        bool foundSomethingToZoomTo = false;

        for (int i=0; i<allGOs.Length; i++) {
            // A little bit of binary bit shifting because that's fun…and fast!
            binaryLayer = 1 << allGOs[i].layer;

            // If the binary layer is 0010, and the binaryZoomLayers is 0110, then
            //  binaryLayer & binaryZoomLayers will be 0010, equal to binaryLayer.
            if ( (binaryLayer & binaryZoomLayers) == binaryLayer ) {
                // We've found a GameObject that we should zoom to.
                foundSomethingToZoomTo = true;

                // Use the bounds of the Renderer to get a good idea of extents.
                tRend = allGOs[i].GetComponent<Renderer>();
                if (tRend != null) {
                    xzMin.x = Mathf.Min(xzMin.x, tRend.bounds.min.x);
                    xzMin.z = Mathf.Min(xzMin.z, tRend.bounds.min.z);
                    xzMax.x = Mathf.Max(xzMax.x, tRend.bounds.max.x);
                    xzMax.z = Mathf.Max(xzMax.z, tRend.bounds.max.z);
                }
            }
        }

        // If there is nothing in the level that we wish to zoom to, then disable
        //  the MiniMap
        gameObject.SetActive(foundSomethingToZoomTo);
        if (!foundSomethingToZoomTo) return;

        // Center the camera in the scene
        Vector3 center = (xzMin + xzMax) / 2f;

#if DEBUG_MiniMap_Camera_ShowMinMax
        Debug.DrawLine(xzMin, center, Color.magenta, 60);
        Debug.DrawLine(xzMax, center, Color.magenta, 60);
#endif

        center.y = 16; // This number keeps this camera above the action.
        transform.position = center;

        // Scale the camera's orthographic size
        float ratioHW = (cam.rect.height*Screen.height) / (cam.rect.width*Screen.width);
        float width = xzMax.x - xzMin.x;
        float height = xzMax.z - xzMin.z;
        float size = Mathf.Max( width * ratioHW, height ) * 0.5f;
        size *= 1+zoomBorder;
        cam.orthographicSize = size;

    }
}
