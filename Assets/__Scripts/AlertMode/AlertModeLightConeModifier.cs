using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeLightConeModifier : MonoBehaviour {
	[Header("Set in Inspector")]
    public Color colorToSwitchTo = Color.red;
	public bool copyAlphaFromOriginal = true;

    [Header("Set Dynamically")]
    public LightCone[] cones;
    public Color[] originalColors;

    private bool inited = false;


	// Use this for initialization
	void Start () {
		cones = GetComponentsInChildren<LightCone>();
        if (cones.Length == 0) {
			Debug.LogError("GameObject "+gameObject.name+" and children do not contain " +
                           "a LightCone component");
            return;
		}

		originalColors = new Color[cones.Length];
		for (int i=0; i<cones.Length; i++) {
			originalColors[i] = cones[i].color;
		}
        
        // Register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;

        inited = true;
	}

    private void OnDestroy()
    {
        // De-register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate -= AlertModeStatusChange;
    }

    void AlertModeStatusChange(bool alertMode) {
        if (!inited) {
            // If this did not init properly. Do not try to do this
            return;
        }

        if (alertMode) {
			Color color;
			for (int i=0; i<cones.Length; i++) {
				color = colorToSwitchTo;
				if (copyAlphaFromOriginal) {
					color.a = originalColors[i].a;
				}
				cones[i].color = color;
			}
        } else {
			for (int i=0; i<cones.Length; i++) {
				// The following line uses a ternary operator to choose the color.
				cones[i].color = originalColors[i];
			}
		}
    }
}
