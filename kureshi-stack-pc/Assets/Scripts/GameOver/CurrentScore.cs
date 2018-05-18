using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class CurrentScore : MonoBehaviour {
	/**
	 * ハイスコアオブジェクトの初期位置(移動前の位置)
	 * @type {Vector3}
	 */
	public static readonly Vector3 CURRENT_SCORE_TEXT_INITIAL_POSITION = new Vector3(650f, 26.0f, 0f);

	/**
	 * ハイスコアオブジェクトの移動後の位置
	 * @type {Vector3}
	 */
	public static readonly Vector3 CURRENT_SCORE_TEXT_TARGET_POSITION = new Vector3(165f, 26.0f, 0f);

	[SerializeField]
	private Text currentScoreText;

	private RectTransform rectTransform;

	// ハイスコア更新時の演出用変数
	private const float SPRING_CONSTANT = 0.9f;
	private const float ATTENUATION_RATE = 0.1f;

	private Vector3 acc, vel, pos;

	private void Start() {
		currentScoreText = transform.Find("Text").gameObject.GetComponent<Text>();
		currentScoreText.text = UIString.CURRENT_SCORE_PREFIX_STRING + ((int)SequenceManager.Instance.Score).ToString();
		rectTransform = GetComponent<RectTransform>();
		rectTransform.localPosition = CURRENT_SCORE_TEXT_INITIAL_POSITION;
		if(SequenceManager.Instance.IsHighScoreUpdated) {
			acc = vel = Vector3.zero;
			pos = CURRENT_SCORE_TEXT_INITIAL_POSITION;
		}
	}

	private void Update() {
		if(SequenceManager.Instance.IsHighScoreUpdated) {
			Vector3 diff = CURRENT_SCORE_TEXT_TARGET_POSITION - this.pos;
	        this.acc = diff * 0.1f;
	        this.vel += this.acc;
	        this.vel *= 0.9f;
	        this.pos += this.vel;
			rectTransform.localPosition = this.pos;
			return;
		}
		rectTransform.localPosition = Vector3.MoveTowards(
			rectTransform.localPosition,
			CURRENT_SCORE_TEXT_TARGET_POSITION,
			300f*Time.deltaTime);
	}

}
