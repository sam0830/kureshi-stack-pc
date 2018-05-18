using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySEButton : MonoBehaviour {
	[SerializeField]
	private Slider seSlider;

	private const string TRY_SE_NAME = "IconSE";

	private void Start() {
		if(seSlider == null) {
			seSlider = GameObject.Find("ConfigModalWindow/SESlider").GetComponent<Slider>();
		}
	}

	public void OnClick() {
		AudioManager.Instance.TryAudio(TRY_SE_NAME, seSlider.value);
	}
}
