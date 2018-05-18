using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropButton : MonoBehaviour {

	/**
	 * UserTimeを0以下にすれば
	 * UserPhaseなら落下
	 * それ以外なら任意のタイミングでリセットされるので関係ない
	 */
	public void OnClick() {
		if(SequenceManager.Instance.ePhaseType != SequenceManager.PhaseType.USER) { return; }
		SequenceManager.Instance.UserTime = 0f;
	}
}
