using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Renderer) )]
public class AlertModeColorModifier : MonoBehaviour {
	[Header("Set in Inspector")]
	public int materialElementNumber = 0;
    public string colorName = "_EmissionColor";
    public Color colorToSwitchTo = Color.red;

    [Header("Set Dynamically")]
	public Material material;
	public Color originalColor;

	private bool inited = false;

	// Use this for initialization
	void Start () {
        // Find the Material by index
		material = null;
		Material[] mats = GetComponent<Renderer>().materials;
		if (materialElementNumber < mats.Length) {
			material = mats[materialElementNumber];
		}

		if (material == null) {
			Debug.LogError("GameObject "+gameObject.name+" does not have a material element #: "
                           +materialElementNumber);
            return;
		} else if (!material.HasProperty(colorName)) {
			Debug.LogError("Material "+material.name+" does not have color named: "+colorName);
			return;
		}

        originalColor = material.GetColor(colorName);

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
        material.SetColor(colorName, (alertMode) ? colorToSwitchTo : originalColor );
    }
	
}
