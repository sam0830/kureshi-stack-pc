using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StartButton : MonoBehaviour {

	private void Update() {
		if(Input.GetKeyDown("return")) {
			OnClick();
		}
	}

	public void OnClick() {
		AudioManager.Instance.PlaySE(Constant.Start_SE);
	   	GameSceneManager.Instance.LoadGameScene();
	}
}
