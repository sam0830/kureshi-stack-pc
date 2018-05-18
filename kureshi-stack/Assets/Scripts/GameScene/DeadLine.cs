using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D coll) {
		if(SequenceManager.Instance.ePhaseType == SequenceManager.PhaseType.GAMEOVER) {
			return;
		}
		SequenceManager.Instance.SetGameOver();
	}
}
