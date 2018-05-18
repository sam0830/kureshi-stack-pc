using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common {
	public static class Constant {

		/**
		 * 呉氏オブジェクトのタグ名
		 * @type {string}
		 */
		public const string STACKED_TAG_NAME = "Stacked";
		public const string UNTAGGED_TAG_NAME = "Untagged";

		/**
		 * ハイスコアのキー
		 * @type {string}
		 */
		public const string HIGH_SCORE_KEY = "high_score";

		/**
		 * タイトルBGMのキー
		 * @type {string}
		 */
		public const string TITLE_BGM = "TitleBGM";
		/**
		 * SEのキー
		 * @type {string}
		 */
		public const string SET_POSITION_SE = "SetPositionSE";

		/**
		 * IconのSEキー
		 * @type {string}
		 */
		public const string ICON_SE = "IconSE";

		/**
		 * StartのSEキー
		 * @type {string}
		 */
		public const string Start_SE = "StartSE";

		/**
		 * BGMの音量のキー
		 * @type {String}
		 */
		public const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
		/**
		 * SEの音量のキー
		 * @type {String}
		 */
		public const string SE_VOLUME_KEY  = "SE_VOLUME_KEY";

		/**
		 * シーン上のカメラの名前
		 * @type {string}
		 */
		public const string CAMERA_NAME = "MainCamera";
	}

	public static class UIString {
		/**
		 * ハイスコア文字列
		 * @type {string}
		 */
		public const string HIGH_SCORE_PREFIX_STRING = "HighScore\n";

		/**
		 * 現在のスコア文字列
		 * @type {string}
		 */
		public const string CURRENT_SCORE_PREFIX_STRING = "Your Score\n";
	}
}
