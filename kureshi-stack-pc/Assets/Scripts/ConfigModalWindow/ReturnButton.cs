using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButton : MonoBehaviour {
	private const string TITLE_BGM = "TitleBGM";
	[SerializeField]
	private Canvas configModalWindow;

	private void Start() {
		if(configModalWindow == null) {
			configModalWindow = GameObject.Find("ConfigModalWindow").GetComponent<Canvas>();
		}
	}

	public void OnClick() {
		AudioManager.Instance.PlayBGM(TITLE_BGM);
		configModalWindow.enabled = false;
	}
}
