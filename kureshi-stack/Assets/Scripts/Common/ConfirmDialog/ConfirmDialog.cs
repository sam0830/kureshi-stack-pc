using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConfirmDialog : SingletonMonoBehaviour<ConfirmDialog> {

	private Canvas canvas;

	override protected void Awake() {
		base.Awake();
		DontDestroyOnLoad(this.gameObject);
	}

	private void Start() {
		canvas = GetComponent<Canvas>();
	}

	private void Update() {
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Escape)) {
			// ダイアログを表示
			canvas.enabled = true;
			Time.timeScale = 0f;
			GameSceneManager.Instance.ePlayType = GameSceneManager.PlayType.PAUSE;
			return;
		}
#else
		// プラットフォームがアンドロイドかチェック
		if (Application.platform == RuntimePlatform.Android) {
			// エスケープキー取得
			if (Input.GetKeyDown(KeyCode.Escape)) {
				// ダイアログを表示
				canvas.enabled = true;
				Time.timeScale = 0f;
				GameSceneManager.Instance.ePlayType = GameSceneManager.PlayType.PAUSE;
				return;
			}
		}
#endif
	}

	public void SetConfirmDialogEnabled(bool flag = false) {
		canvas.enabled = flag;
	}


}
