using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(EnemyNav) )]
public class AlertModeEnemyStateModifier : MonoBehaviour {
	private EnemyNav        enemy;
    
    // Use this for initialization
    void Start () {
		enemy = GetComponent<EnemyNav>();
        // Register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;
    }

    private void OnDestroy()
    {
        // De-register this to know when AlertMode is entered (or exited)
        AlertModeManager.alertModeStatusChangeDelegate -= AlertModeStatusChange;
    }

    void AlertModeStatusChange(bool alertMode) {
		enemy.mode = alertMode ? EnemyNav.eMode.chase : EnemyNav.eMode.stopChase;
    }
}
