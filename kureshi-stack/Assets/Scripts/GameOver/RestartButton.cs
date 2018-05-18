using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RestartButton : MonoBehaviour {
	/**
	 * 戻るボタンの初期位置(移動前の位置)
	 * @type {Vector3}
	 */
	private static readonly Vector3 RESTART_BUTTON_INITIAL_POSITION = new Vector3(0f, -855f, 0f);

	/**
	 * 戻るボタンの移動後の位置
	 * @type {Vector3}
	 */
	private static readonly Vector3 RESTART_BUTTON_TARGET_POSITION = new Vector3(0f, -300f, 0f);

	private RectTransform rectTransform;

	private void Start() {
		rectTransform = GetComponent<RectTransform>();
		rectTransform.localPosition = RESTART_BUTTON_INITIAL_POSITION;
	}

	private void Update() {
		rectTransform.localPosition = Vector3.MoveTowards(
			rectTransform.localPosition,
			RESTART_BUTTON_TARGET_POSITION,
			300f*Time.deltaTime);
	}
	public void OnClick() {
		Debug.Log("ボタンを押した");
		AudioManager.Instance.PlaySE(Constant.ICON_SE);
		//AdsManager.Instance.ShowRewardedAd();
		//AdsManager.Instance.ShowVideo();
		GameSceneManager.Instance.LoadTitleScene();
	}
}
