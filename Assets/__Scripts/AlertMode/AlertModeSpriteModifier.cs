using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeSpriteModifier : MonoBehaviour {
    [Header("Set in Inspector")]
    public Color            colorToSwitchTo = Color.red;
    public Sprite           spriteToSwitchTo;
    [Range(0,1)]
    [Tooltip("0 = None of new color | 1 = All new color")]
    public float            colorBlend = 1;

    [Header("Set Dynamically")]
	public SpriteRenderer[] spriteRenderers;
    public Color[]          originalColors;
	public Sprite[]         originalSprites;

    private bool inited = false;


    // Use this for initialization
    void Start () {
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers.Length == 0) {
			Debug.LogError("GameObject "+gameObject.name+" and children do not" +
                           " contain a SpriteRenderer component");
            return;
        }

		originalColors = new Color[spriteRenderers.Length];
		originalSprites = new Sprite[spriteRenderers.Length];
        for (int i=0; i<spriteRenderers.Length; i++) {
			originalColors[i] = spriteRenderers[i].color;
			originalSprites[i] = spriteRenderers[i].sprite;
        }

        // Register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;

        inited = true;
		AlertModeStatusChange(false);
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

        Color col;
		for (int i=0; i<spriteRenderers.Length; i++) {
            if (alertMode) {
                col = Color.Lerp(originalColors[i], colorToSwitchTo, colorBlend);
                spriteRenderers[i].color = col;
				spriteRenderers[i].sprite = spriteToSwitchTo;
			} else {
				spriteRenderers[i].color = originalColors[i];
                spriteRenderers[i].sprite = originalSprites[i];
            }
        }
    }
}