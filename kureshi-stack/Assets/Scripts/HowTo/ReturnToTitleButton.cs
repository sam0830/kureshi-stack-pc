using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToTitleButton : MonoBehaviour {

	void Start () {

	}

	/**
	 * タイトルシーン読み込み
	 */
	public void OnClick() {
		GameSceneManager.Instance.LoadTitleScene();
	}
}
