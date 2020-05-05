using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Renderer) )]
public class MiniMap_Blip : MonoBehaviour {
    public Transform    trackedTransform;
    public Color        color = Color.white;
    public float        yHeight = 5;

    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = color;
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.position = trackedTransform.position + Vector3.up*yHeight;
        transform.rotation = trackedTransform.rotation;
	}
}
