using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StartButton : MonoBehaviour {

	public void OnClick() {
		AudioManager.Instance.PlaySE(Constant.Start_SE);
	   	GameSceneManager.Instance.LoadGameScene();
	}
}
