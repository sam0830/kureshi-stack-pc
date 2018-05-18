using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

/**
 * ゲームの流れを管理するクラス
 * 1. 3Dモデルが画面の一定位置に出現
 * 2. カウントダウン開始
 * 3. ユーザの入力受付(ドラッグ: 移動, )
 * 4. カウントダウン終了後落下(Rigidbody dynamic)
 * 5. 静止の待ち時間
 * 6. 1に戻る
 * 6.1 ゲームオーバーならゲームオーバーの瞬間の画像を背景に遷移
 * @type {class}
 */
public class SequenceManager : SingletonMonoBehaviour<SequenceManager> {
	public enum PhaseType{
		INITIAL,
		SETPOSITION,
		CAMERAMOVE,
		USER,
		WAIT,
		END,
		GAMEOVER,
	}

	/**
	 * フェース
	 * @type {PhaseType}
	 */
	public PhaseType _ePhaseType =  PhaseType.INITIAL;

	/**
	 * モデルの出現場所
	 */
	private static readonly Vector3 KURESHI_INITIAL_POSITION = new Vector3(0f, 3f, 1f);

	/**
	 * モデルの待機場所
	 * @type {Vector3}
	 */
	private static readonly Vector3 KURESHI_WAIT_POSITION = new Vector3(0f, 1f, 1f);

	/**
	 * カメラの移動する高さ
	 * @type {Vector3}
	 */
	private static readonly Vector3 CAMERA_MOVE_HEIGHT = new Vector3(0f, 1f, 0f);

	/**
	 * ポップアップしたゲームオブジェクト
	 * @type {GameObject}
	 */
	private GameObject popupObject;
	/**
	 * 経過時間
	 * @type {float}
	 */
	private float _userTime = USER_TIME;

	/**
	 * 待ち時間
	 * @type {float}
	 */
	private float waitTime = 0;

	/**
	 * ユーザが操作可能な時間
	 * @type {float}
	 */
	[SerializeField]
	private const float USER_TIME = 10.0f;
	/**
	 * 静止のための待ち時間
	 * @type {float}
	 */
	[SerializeField]
	private const float WAIT_TIME = 3.0f;

	/**
	 * 現在のスコア保存
	 * @type {int}
	 */
	private int _score = 0;

	 /**
	  * ハイスコア更新かどうか
	  * @type {bool}
	  */
	private bool _isHighScoreUpdated = false;

	/**
	 * ゲーム開始時のハイスコア
	 * @type {int}
	 */
	private int _currentHighScore;

	[SerializeField]
	private Canvas gameoverCanvas;

	[SerializeField]
	private GameObject mainCamera;

	[SerializeField]
	private List<GameObject> _kureshiList;

	private Vector3 cameraTargetPos;
	private Vector3 kureshiTargetPos = KURESHI_WAIT_POSITION;
	private Vector3 kureshiInitialPos = KURESHI_INITIAL_POSITION;

	public float UserTime {
		get { return _userTime; }
		set { _userTime = value; }
	}

	public int Score {
		get { return _score; }
		set { _score = value; }
	}

	public PhaseType ePhaseType {
		get { return _ePhaseType; }
	}

	public bool IsHighScoreUpdated {
		get { return _isHighScoreUpdated; }
	}

	public int CurrentHighScore {
		get { return _currentHighScore; }
	}

	private void Start() {
		if(gameoverCanvas == null) {
			// TODO: Resources.OnLoad
		}
		if(mainCamera == null) {
			mainCamera = GameObject.Find(Constant.CAMERA_NAME);
		}
		//Time.timeScale = 1.0f; // 前回ゲームオーバーの場合時間が止まっている
		cameraTargetPos = mainCamera.transform.position;
		gameoverCanvas.GetComponent<Canvas>().worldCamera = mainCamera.GetComponent<Camera>();
		_currentHighScore = PlayerPrefs.GetInt(Constant.HIGH_SCORE_KEY, 0);
	}

	private void Update() {
		switch(_ePhaseType) {
			case PhaseType.INITIAL:
				InitialProcess();
				break;
			case PhaseType.CAMERAMOVE:
				CameraMoveProcess();
				break;
			case PhaseType.SETPOSITION:
				SetPositionProcess();
				break;
			case PhaseType.USER:
				UserProcess();
				break;
			case PhaseType.WAIT:
				WaitProcess();
				break;
			case PhaseType.END:
				EndProcess();
				break;
			case PhaseType.GAMEOVER:
				GameOverProcess();
				break;
			default:
				break;
		}
		Debug.DrawLine(new Vector3(-3.5f, mainCamera.transform.position.y-1, 0f),
		new Vector3(3.5f, mainCamera.transform.position.y-1, 0f),Color.red);
	}

	/**
	 * 呉氏のオブジェクトを初期位置に生成するメソッド
	 * 場合によってよってはカメラの位置移動処理へ遷移
	 */
	private void InitialProcess() {
		// 呉氏が十分積まれているとき
		if(IsFullyStacked()) {
			cameraTargetPos = mainCamera.transform.position + CAMERA_MOVE_HEIGHT;
			kureshiTargetPos = kureshiTargetPos + Vector3.up;
			kureshiInitialPos = kureshiInitialPos + Vector3.up;
		}

		popupObject = Instantiate(_kureshiList[Random.Range(0, _kureshiList.Count)],
		kureshiInitialPos,
		Quaternion.Euler(0, 0, 0));
		RotationSlider.Instance.SliderValue = 0;
		_ePhaseType = PhaseType.CAMERAMOVE;
	}

	/**
	 * カメラ移動中に毎フレーム呼ばれるメソッド
	 */
	 private void CameraMoveProcess() {
 		mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, cameraTargetPos, Time.deltaTime);
 		if(mainCamera.transform.position == cameraTargetPos) {
			if(IsFullyStacked()) {
				_ePhaseType = PhaseType.INITIAL;
			}
 			_ePhaseType = PhaseType.SETPOSITION;
 		}
 	}

	private void SetPositionProcess() {
		// 呉氏の初期位置への移動
		popupObject.transform.position = Vector3.MoveTowards(popupObject.transform.position, kureshiTargetPos, Time.deltaTime*4.0f);
		if(popupObject.transform.position == kureshiTargetPos) {
			AudioManager.Instance.PlaySE(Constant.SET_POSITION_SE);
			_ePhaseType = PhaseType.USER;
		}
	}

	private void UserProcess() {
		// 操作可能時間中はx軸のみのドラッグが可能
		_userTime -= Time.deltaTime;
		popupObject.transform.rotation = Quaternion.Euler(0, 0, -RotationSlider.Instance.SliderValue);
		if(_userTime <= 0) {
			_userTime = USER_TIME;
			popupObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			popupObject.GetComponent<ObjectHandler>().UnregisterGesture();
			popupObject.GetComponent<ObjectHandler>().IsSelected = false;
			popupObject.GetComponent<PolygonCollider2D>().isTrigger = false;
			_ePhaseType = PhaseType.WAIT;
		}
		return;
	}

	/**
	 * ユーザ操作終了後の待ち時間
	 */
	private void WaitProcess() {
		waitTime += Time.deltaTime;
		if(waitTime >= WAIT_TIME) {
			waitTime = 0;
			_ePhaseType = PhaseType.END;
		}
		return;
	}

	private void EndProcess() {
		_score++;
		popupObject.tag = Constant.STACKED_TAG_NAME;
		_ePhaseType = PhaseType.INITIAL;
	}

	private void GameOverProcess() {
		// ゲームオーバー画面で毎フレーム呼ばれる
	}

	public void SetGameOver() {
		Debug.Log("ゲームオーバー");
		popupObject.GetComponent<ObjectHandler>().UnregisterGesture();
		// ハイスコア更新している場合
		if(_score > PlayerPrefs.GetInt (Constant.HIGH_SCORE_KEY, 0)) {
			PlayerPrefs.SetInt(Constant.HIGH_SCORE_KEY, _score);
			_isHighScoreUpdated = true;
		}
		/**
		 * ゲームオーバービューを表示
		 */
		// Time.timeScale = 0;
		Instantiate(gameoverCanvas);
		_ePhaseType = PhaseType.GAMEOVER;
	}

	private bool IsFullyStacked() {
		RaycastHit2D[] hits = Physics2D.LinecastAll(
			new Vector3(-3.5f, mainCamera.transform.position.y-1, 0f),
			new Vector3(3.5f, mainCamera.transform.position.y-1, 0f)
		);
		foreach(RaycastHit2D hit in hits) {
			if(hit.collider.tag == Constant.STACKED_TAG_NAME) {
				return true;
			}
		}
		return false;
	}

}
