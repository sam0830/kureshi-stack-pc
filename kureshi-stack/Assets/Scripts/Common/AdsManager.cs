using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : SingletonMonoBehaviour<AdsManager> {

	private const string ANDROID_GAME_ID = "1745755";
	private const string IOS_GAME_ID = "";

	override protected void Awake() {
		base.Awake();
		DontDestroyOnLoad(this.gameObject);
		string gameID = null;
#if UNITY_ANDROID
    	gameID = ANDROID_GAME_ID;
#elif UNITY_IOS
        gameID = IOS_GAME_ID;
#endif
		Advertisement.Initialize(gameID);
	}

	public void ShowRewardedAd() {
		if (Advertisement.IsReady("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	public void ShowVideo() {
		if(Advertisement.IsReady("video")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("video", options);
		}
	}

	private void HandleShowResult(ShowResult result) {
		switch (result) {
			case ShowResult.Finished:
				Debug.Log ("The ad was successfully shown.");
				break;
			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
		}
	}
}
