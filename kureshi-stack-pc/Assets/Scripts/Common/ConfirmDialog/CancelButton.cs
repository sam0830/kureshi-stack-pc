using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour {

	public void OnClick() {
		// Canvas.enabled = false;
		ConfirmDialog.Instance.SetConfirmDialogEnabled();
		Time.timeScale = 1.0f;
		GameSceneManager.Instance.ePlayType = GameSceneManager.PlayType.PLAY;
	}
}
