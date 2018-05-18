using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBGMButton : MonoBehaviour {
	[SerializeField]
	private Slider bgmSlider;

	private const string TRY_BGM_NAME = "TryBGM";

	private void Start() {
		if(bgmSlider == null) {
			bgmSlider = GameObject.Find("ConfigModalWindow/BGMSlider").GetComponent<Slider>();
		}
	}

	public void OnClick() {
		AudioManager.Instance.TryAudio(TRY_BGM_NAME, bgmSlider.value);
	}
}
