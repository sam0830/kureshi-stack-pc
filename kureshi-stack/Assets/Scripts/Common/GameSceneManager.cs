using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

public class GameSceneManager : SingletonMonoBehaviour<GameSceneManager> {
	public enum PlayType {
		PLAY,
		PAUSE
	}

	private PlayType _ePlayType = PlayType.PLAY;

	public PlayType ePlayType {
		get { return _ePlayType; }
		set { _ePlayType = value; }
	}

	override protected void Awake() {
		base.Awake();
		DontDestroyOnLoad(this.gameObject);
	}

	private void Start() {
		AudioManager.Instance.PlayBGM (Constant.TITLE_BGM, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
	}

	private void Update() {

	}

	public void LoadGameScene() {
		SceneManager.LoadScene("game");
	}

	public void LoadTitleScene() {
		SceneManager.LoadScene("title");
	}

	public void LoadHowToScene() {
		SceneManager.LoadScene("howto");
	}

	public void LoadHighScoreScene() {
		SceneManager.LoadScene("highscore");
	}
}
