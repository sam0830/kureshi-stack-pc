using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour , InputGesture {
	[SerializeField]
	private Vector3 CENTER_OF_GRAVITY = new Vector3(0f,0f,0f);
	private Rigidbody2D rigidboy2D;

	private bool _isSelected = false;

	public bool IsSelected {
		get { return _isSelected; }
		set { _isSelected = value; }
	}
	/// <summary>
    ///
    /// </summary>
    private void OnEnable() {
        InputGestureManager.Instance.RegisterGesture (this);
    }

	public void UnregisterGesture() {
		InputGestureManager.Instance.UnregisterGesture (this);
	}

	private void Start() {
		rigidboy2D = GetComponent<Rigidbody2D>();
		rigidboy2D.centerOfMass = CENTER_OF_GRAVITY;
	}

	private void Update() {

	}

	private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere (transform.position + transform.rotation * CENTER_OF_GRAVITY, 0.1f);
    }

	/// <summary>
    /// ジェスチャーの処理順番番号
    /// </summary>
    /// <value>0が一番速い、数値が大きくなると判定順番が遅くなる</value>
    public int Order {
      get { return 9999; }
    }

    /// <summary>
    /// 指定ジェスチャーが処理する必要があるかどうかを取得します
    /// </summary>
    /// <returns>処理する必要があるならtrueを返す</returns>
    /// <param name="info">Info.</param>
    public bool IsGestureProcess( GestureInfo info ) {
		if(GameSceneManager.Instance.ePlayType == GameSceneManager.PlayType.PAUSE) {
			return false;
		}
        return true;  // 常に処理する(仮)
    }

    /// <summary>
    /// Down時に呼び出されます
    /// </summary>
    /// <param name="info">Info.</param>
    public void OnGestureDown( GestureInfo info ) {
		if(GameSceneManager.Instance.ePlayType == GameSceneManager.PlayType.PAUSE) {
			_isSelected = false;
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(info.ScreenPosition);
        // Rayの当たったオブジェクトの情報を格納する
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100f);
        // オブジェクトにrayが当たった時
        if(hit.collider) {
			if(hit.collider.gameObject == this.gameObject) {
				_isSelected = true;
				return;
			}
		}
		_isSelected = false;
    }

    /// <summary>
    /// Up時に呼び出されます
    /// </summary>
    /// <param name="info">Info.</param>
    public void OnGestureUp( GestureInfo info ) {
		if(GameSceneManager.Instance.ePlayType == GameSceneManager.PlayType.PAUSE) {
			_isSelected = false;
			return;
		}
		_isSelected = false;
    }

    /// <summary>
    /// Drag時に呼び出されます
    /// </summary>
    /// <param name="info">Info.</param>
    public void OnGestureDrag( GestureInfo info ) {
		if(GameSceneManager.Instance.ePlayType == GameSceneManager.PlayType.PAUSE) {
			_isSelected = false;
			return;
		}
		if(!_isSelected) {
			return;
		}
		Vector3 pos = this.transform.position;
		Vector3 diff = Camera.main.ScreenToWorldPoint (new Vector3(0,0,1.0f)) - Camera.main.ScreenToWorldPoint(info.DeltaPosition);
		this.transform.position = new Vector3(pos.x-diff.x, pos.y, pos.z);
		float min = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x;
		float max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x;
		this.transform.position = new Vector3(
			Mathf.Clamp(this.transform.position.x, min, max),
			pos.y,
			pos.z
		);
    }

    /// <summary>
    /// Flick時に呼び出されます
    /// </summary>
    /// <param name="info">Info.</param>
    public void OnGestureFlick( GestureInfo info ) {
		Debug.Log("フリック");
    }

}
