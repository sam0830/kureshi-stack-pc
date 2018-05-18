using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	private Text scoreText;

	private const string PREFIX_STRING = "Score:";

	private void Start() {
		scoreText = GetComponent<Text>();
		scoreText.text = PREFIX_STRING + ((int)SequenceManager.Instance.Score).ToString();
	}

	private void Update() {
		scoreText.text = PREFIX_STRING + ((int)SequenceManager.Instance.Score).ToString();
	}
}
