using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeAmbientLightModifier : MonoBehaviour {
	[Header("Set in Inspector")]
    public Color colorToSwitchTo = Color.red;

    [Header("Set Dynamically")]
	public Color originalColor;

	private bool inited = false;

	// Use this for initialization
	void Start () {
        originalColor = RenderSettings.ambientLight;

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
            // If this did not init properly. Do not try to use the material
            return;
        }
        // The following line uses a ternary operator to choose the color.
        RenderSettings.ambientLight = alertMode ? colorToSwitchTo : originalColor;
    }
	
}
