using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BGMとSEの管理をするマネージャ。シングルトン。
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager> {
	//ボリューム保存用のkeyとデフォルト値
	private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
	private const string SE_VOLUME_KEY  = "SE_VOLUME_KEY";
	private const float  BGM_VOLUME_DEFULT = 1.0f;
	private const float  SE_VOLUME_DEFULT  = 1.0f;

	//BGMがフェードするのにかかる時間
	public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
	public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
	private float bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

	//次流すBGM名、SE名
	private string nextBGMName;
	private string nextSEName;

	//BGMをフェードアウト中か
	private bool _isFadeOut = false;

	//BGM用、SE用に分けてオーディオソースを持つ
	public AudioSource AttachBGMSource, AttachSESource;

	//全Audioを保持
	private Dictionary<string, AudioClip> bgmDic, seDic;

	//=================================================================================
	//初期化
	//=================================================================================

	override protected void Awake () {
		base.Awake();

		DontDestroyOnLoad (this.gameObject);

		//リソースフォルダから全SE&BGMのファイルを読み込みセット
		bgmDic = new Dictionary<string, AudioClip> ();
		seDic  = new Dictionary<string, AudioClip> ();

		object[] bgmList = Resources.LoadAll ("Audio/BGM");
		object[] seList  = Resources.LoadAll ("Audio/SE");

		foreach (AudioClip bgm in bgmList) {
			bgmDic [bgm.name] = bgm;
		}
		foreach (AudioClip se in seList) {
			seDic [se.name] = se;
		}
	}

	private void Start ()
	{
		AttachBGMSource.volume = PlayerPrefs.GetFloat (BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
		AttachSESource.volume  = PlayerPrefs.GetFloat (SE_VOLUME_KEY,  SE_VOLUME_DEFULT);
	}

	//=================================================================================
	//SE
	//=================================================================================

	/// <summary>
	/// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
	/// </summary>
	public void PlaySE (string seName, float delay = 0.0f)
	{
		if (!seDic.ContainsKey (seName)) {
			Debug.Log (seName + "という名前のSEがありません");
			return;
		}

		nextSEName = seName;
		Invoke ("DelayPlaySE", delay);
	}

	private void DelayPlaySE ()
	{
		AttachSESource.PlayOneShot (seDic [nextSEName] as AudioClip);
	}

	//=================================================================================
	//BGM
	//=================================================================================

	/// <summary>
	/// 指定したファイル名のBGMを流す。ただし既に流れている場合は前の曲をフェードアウトさせてから。
	/// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
	/// </summary>
	public void PlayBGM (string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
	{
		if (!bgmDic.ContainsKey (bgmName)) {
			Debug.Log (bgmName + "という名前のBGMがありません");
			return;
		}

		//現在BGMが流れていない時はそのまま流す
		if (!AttachBGMSource.isPlaying) {
			nextBGMName = "";
			AttachBGMSource.clip = bgmDic [bgmName] as AudioClip;
			AttachBGMSource.Play ();
			return;
		}
		//違うBGMが流れている時は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れている時はスルー
		//if (AttachBGMSource.clip.name != bgmName) {
			nextBGMName = bgmName;
			FadeOutBGM (fadeSpeedRate);
		//}

	}

	public void TryAudio(string seName, float volume) {
		if (!seDic.ContainsKey (seName)) {
			Debug.Log (seName + "という名前のSEがありません");
			return;
		}

		if (!AttachSESource.isPlaying) {
			AttachSESource.PlayOneShot (seDic [seName] as AudioClip, volume);
			return;
		}

	}

	/// <summary>
	/// 現在流れている曲をフェードアウトさせる
	/// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
	/// </summary>
	public void FadeOutBGM (float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
	{
		bgmFadeSpeedRate = fadeSpeedRate;
		_isFadeOut = true;
	}

	private void Update ()
	{
		if (!_isFadeOut) {
			return;
		}

		//徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
		AttachBGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
		if (AttachBGMSource.volume <= 0) {
			AttachBGMSource.Stop ();
			AttachBGMSource.volume = PlayerPrefs.GetFloat (BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
			_isFadeOut = false;

			if (!string.IsNullOrEmpty (nextBGMName)) {
				PlayBGM (nextBGMName);
			}
		}

	}

	//=================================================================================
	//音量変更
	//=================================================================================

	/// <summary>
	/// BGMとSEのボリュームを別々に変更&保存
	/// </summary>
	public void ChangeVolume (float BGMVolume, float SEVolume)
	{
		AttachBGMSource.volume = BGMVolume;
		AttachSESource.volume  = SEVolume;

		PlayerPrefs.SetFloat (BGM_VOLUME_KEY,  BGMVolume);
		PlayerPrefs.SetFloat (SE_VOLUME_KEY,   SEVolume);
	}
}
