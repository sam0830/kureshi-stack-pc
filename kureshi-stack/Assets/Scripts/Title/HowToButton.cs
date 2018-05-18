using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class HowToButton : MonoBehaviour {

	public void OnClick() {
		AudioManager.Instance.PlaySE(Constant.ICON_SE);
		GameSceneManager.Instance.LoadHowToScene();
	}
}
