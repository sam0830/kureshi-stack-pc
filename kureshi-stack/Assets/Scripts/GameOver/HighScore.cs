using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class HighScore : MonoBehaviour {
	/**
	 * ハイスコアオブジェクトの初期位置(移動前の位置)
	 * @type {Vector3}
	 */
	private static readonly Vector3 HIGH_SCORE_TEXT_INITIAL_POSITION = new Vector3(-700f, 260f, 0f);

	/**
	 * ハイスコアオブジェクトの移動後の位置
	 * @type {Vector3}
	 */
	private static readonly Vector3 HIGH_SCORE_TEXT_TARGET_POSITION = new Vector3(-150f, 260f, 0f);

	[SerializeField]
	private Text highScoreText;

	private RectTransform rectTransform;

	private void Start() {
		highScoreText = transform.Find("Text").gameObject.GetComponent<Text>();
		highScoreText.text = UIString.HIGH_SCORE_PREFIX_STRING + ((int)SequenceManager.Instance.CurrentHighScore).ToString();
		rectTransform = GetComponent<RectTransform>();
		rectTransform.localPosition = HIGH_SCORE_TEXT_INITIAL_POSITION;
	}

	private void Update() {
		rectTransform.localPosition = Vector3.MoveTowards(
			rectTransform.localPosition,
			HIGH_SCORE_TEXT_TARGET_POSITION,
			300f*Time.deltaTime);
	}

}
