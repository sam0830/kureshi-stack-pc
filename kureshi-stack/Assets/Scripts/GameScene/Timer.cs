using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private Text timerText;

	private int _prevSeconds;

	private void Start() {
		timerText = GetComponent<Text>();
		_prevSeconds = (int)SequenceManager.Instance.UserTime;
		timerText.text = _prevSeconds.ToString();
	}

	private void Update() {
		if((int)SequenceManager.Instance.UserTime != _prevSeconds) {
			timerText.text = ((int)SequenceManager.Instance.UserTime).ToString();
		}
	}

}
