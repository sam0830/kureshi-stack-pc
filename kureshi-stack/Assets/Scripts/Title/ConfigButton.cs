using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class ConfigButton : MonoBehaviour {

	[SerializeField]
	private Canvas configModalWindow;
	[SerializeField]
	private Slider bgmSlider;
	[SerializeField]
	private Slider seSlider;
	
	private void Start() {
		if(bgmSlider == null) {
			bgmSlider = GameObject.Find("ConfigModalWindow/BGMSlider").GetComponent<Slider>();
		}
		if(seSlider == null) {
			seSlider = GameObject.Find("ConfigModalWindow/SESlider").GetComponent<Slider>();
		}
		if(configModalWindow == null) {
			configModalWindow = GameObject.Find("ConfigModalWindow").GetComponent<Canvas>();
		}
		bgmSlider.value = PlayerPrefs.GetFloat(Constant.BGM_VOLUME_KEY, 1.0f);
		seSlider.value = PlayerPrefs.GetFloat(Constant.SE_VOLUME_KEY, 1.0f);
		configModalWindow.enabled = false;
	}

	private void ShowConfigModalWindow() {
		if(configModalWindow != null) {
			configModalWindow.enabled = true;
		}
	}

	public void OnClick() {
		AudioManager.Instance.PlaySE(Constant.ICON_SE);
		AudioManager.Instance.FadeOutBGM();
		ShowConfigModalWindow();
	}
}
