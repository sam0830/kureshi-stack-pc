using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputGestureManager : SingletonMonoBehaviour<InputGestureManager> 
{
  /// <summary>
  /// 登録済みのジェスチャー配列
  /// </summary>
  private List<InputGesture> _gestures = new List<InputGesture>();

  /// <summary>
  /// ジェスチャー情報
  /// </summary>
  private GestureInfo _gesture_info = new GestureInfo ();

  /// <summary>
  /// トレース中の位置情報
  /// </summary>
  private Queue<Vector3> _trace_position_queue = new Queue<Vector3> ();

  /// <summary>
  /// トレース中の時間情報
  /// </summary>
  private Queue<float> _trace_time_queue = new Queue<float> ();

  /// <summary>
  /// トレースデータ保持数
  /// </summary>
  private readonly int TRACE_QUE_COUNT = 20;

  /// <summary>
  /// 登録済みのジェスチャー配列
  /// </summary>
  /// <value>The gestures.</value>
  public List<InputGesture> Gestures
  {
    get { return _gestures; }
    set { _gestures = value; }
  }

  /// <summary>
  /// 現在処理中のジェスチャー情報
  /// </summary>
  /// <value>The active gesture.</value>
  private InputGesture ProcessingGesture 
  {
    get;
    set;
  }

  /// <summary>
  /// 直前のタッチId
  /// </summary>
  /// <value></value>
  private int TouchId {
    get;
    set;
  }

  /// <summary>
  /// Awake
  /// </summary>
  void Awake (){
    if (this != Instance) {
      // 既に存在しているなら削除
      Destroy (this);
    } else {
      // 音管理はシーン遷移では破棄させない
      DontDestroyOnLoad (this.gameObject);
    }
  }

  /// <summary>
  /// ジェスチャーの追加を行います
  /// </summary>
  /// <param name="gesture">Gesture.</param>
  public void RegisterGesture( InputGesture gesture )
  {
    var index = this._gestures.FindIndex (g => g.Order > gesture.Order);
    if (index < 0) {
      index = this._gestures.Count;
    }
    this._gestures.Insert (index, gesture);
  }

  /// <summary>
  /// ジェスチャーの解除を行います
  /// </summary>
  /// <param name="gesture">Gesture.</param>
  public void UnregisterGesture( InputGesture gesture )
  {
    this._gestures.Remove (gesture);
  }
  
  /// <summary>
  /// 
  /// </summary>
	void Update () 
  {
    this._gesture_info.IsDown = false;
    this._gesture_info.IsUp = false;
    this._gesture_info.IsDrag = false;

    // 入力チェック
    var isInput = IsTouchPlatform() ? InputForTouch( ref this._gesture_info ) : InputForMouse( ref this._gesture_info );
    if (!isInput) {
      return;   // 入力無し
    }

    // 各種移動
    if (this._gesture_info.IsDown) {
      DoDown (this._gesture_info);
    }
    if (this._gesture_info.IsDrag) {
      DoDrag (this._gesture_info);
    }
    if (this._gesture_info.IsUp) {
      DoUp (this._gesture_info);
    }
	}

  /// <summary>
  /// タッチパネル向けのプラットフォームかどうか取得します
  /// </summary>
  /// <returns>Android/iOSの場合にtrueを返します</returns>
  bool IsTouchPlatform()
  {
    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
      return true;
    }
    return false;
  }

  /// <summary>
  /// タッチされたタッチ情報を取得します
  /// </summary>
  /// <returns>タッチ情報を返します。何もタッチされていなければnullを返します</returns>
  Touch? GetTouch()
  {
    // 前回と同じタッチを追跡します
    // 新しいタッチの場合は最初のタッチを使用します
    if (Input.touchCount <= 0) {
      return null;
    }
    for (int n=0; n<Input.touchCount; n++) {
      if( this.TouchId == Input.touches[n].fingerId ) {
        return Input.touches[n];    // 前回と同じタッチ
      }
    }
    // 新規タッチ(タッチ開始時のみ)
    if (Input.touches [0].phase == TouchPhase.Began) {
      this.TouchId = Input.touches [0].fingerId;
      return Input.touches [0];
    }
    return null;
  }

  /// <summary>
  /// タッチ入力情報をGestureInfoへ変換します
  /// </summary>
  /// <returns>入力情報があればtrueを返します</returns>
  /// <param name="info"></param>
  bool InputForTouch( ref GestureInfo info )
  {
    // 基本的にタッチは1点のみ検出するインターフェースとする
    Touch? touch = GetTouch ();
    if (!touch.HasValue) {
      return false;
    }
    info.ScreenPosition = touch.GetValueOrDefault().position;
    info.DeltaPosition = touch.GetValueOrDefault().deltaPosition;
    switch (touch.GetValueOrDefault().phase) {
    case TouchPhase.Began:
      info.IsDown = true;
      break;
    case TouchPhase.Moved:
    case TouchPhase.Stationary:
      info.IsDrag = true;
      break;
    case TouchPhase.Ended:
    case TouchPhase.Canceled:
      info.IsUp = true;
      this.TouchId = -1;      // タッチ終了
      break;
    }
    return true;
  }

  /// <summary>
  /// マウス入力情報をGestureInfoへ変換します
  /// </summary>
  /// <returns>入力情報があればtrueを返します</returns>
  /// <param name="info"></param>
  bool InputForMouse( ref GestureInfo info )
  {
    // マウス用の処理
    if (Input.GetMouseButtonDown(0) ) {
      info.IsDown = true;
      info.DeltaPosition = new Vector3();
      info.ScreenPosition = Input.mousePosition;
    }
    if (Input.GetMouseButtonUp(0)) {
      info.IsUp = true;
      info.DeltaPosition = Input.mousePosition - info.ScreenPosition;
      info.ScreenPosition = Input.mousePosition;
    }
    if( Input.GetMouseButton(0) ) {
      info.IsDrag = true;
      info.DeltaPosition = Input.mousePosition - info.ScreenPosition;
      info.ScreenPosition = Input.mousePosition;
    }
    return true;
  }

  /// <summary>
  /// Down入力処理を行います
  /// </summary>
  /// <param name="info"></param>
  void DoDown (GestureInfo info)
  {
    // 登録済みジェスチャーの中で処理すべきものを調べます
    this.ProcessingGesture = _gestures.Find (ges => ges.IsGestureProcess (info));
    if (this.ProcessingGesture == null) {
      return;
    }

    // トレースクリア
    ClearTracePosition ();

    // OnGestureDown
    info.DeltaTime = 0;
    info.DragDistance = new Vector3 ();
    this.ProcessingGesture.OnGestureDown (info);
  }

  /// <summary>
  /// Drag入力処理を行います
  /// </summary>
  /// <param name="info"></param>
  void DoDrag (GestureInfo info)
  {
    if (this.ProcessingGesture == null) {
      return;
    }

    // トレース位置追加
    AddTracePosition (info.ScreenPosition);

    // OnGestureDrag
    info.DeltaTime = GetTraceDeltaTime ();
    info.DragDistance = GetTraceVector ( 0, 0 );
    this.ProcessingGesture.OnGestureDrag (info);
  }

  /// <summary>
  /// Up入力処理を行います
  /// </summary>
  /// <param name="info"></param>
  void DoUp (GestureInfo info)
  {
    if (this.ProcessingGesture == null) {
      return;
    }
    // OnGestureUp
    info.DeltaTime = GetTraceDeltaTime ();
    info.DragDistance = GetTraceVector ( 0, 0 );
    this.ProcessingGesture.OnGestureUp (info);

    // Flick判定
    var v1 = GetTraceVector ( 0, 0 );
    var v2 = GetTraceVector ( this._trace_position_queue.Count - 5, 0 );
    var dot = Vector3.Dot( v1.normalized, v2.normalized );
    if (dot > 0.9) {
      // Flick発生
      this.ProcessingGesture.OnGestureFlick (info);
    }

    // Gesture終了
    this.ProcessingGesture = null;
  }

  /// <summary>
  /// トレース情報をクリアします
  /// </summary>
  void ClearTracePosition()
  {
    this._trace_position_queue.Clear ();
    this._trace_time_queue.Clear ();
  }

  /// <summary>
  /// Drag中の入力位置を追加します
  /// </summary>
  /// <param name="trace_position"></param>
  void AddTracePosition ( Vector3 trace_position )
  {
    this._trace_position_queue.Enqueue (trace_position);
    this._trace_time_queue.Enqueue (Time.deltaTime);
    if (this._trace_position_queue.Count > TRACE_QUE_COUNT) {    // TRACE_QUE_COUNT個まで
      this._trace_position_queue.Dequeue();
      this._trace_time_queue.Dequeue();
    }
  }

  /// <summary>
  /// トレース経過時間を取得します
  /// </summary>
  /// <returns></returns>
  float GetTraceDeltaTime()
  {
    float delta = 0;
    var times = this._trace_time_queue.ToArray ();
    foreach (var t in times) {
      delta += t;
    }
    return delta;
  }

  /// <summary>
  /// トレースデータからベクトルを取得します
  /// </summary>
  /// <returns></returns>
  /// <param name="start_index_ofs"></param>
  /// <param name="end_index_ofs"></param>
  Vector3 GetTraceVector ( int start_index_ofs, int end_index_ofs )
  {
    var positions = this._trace_position_queue.ToArray ();
    var sindex = start_index_ofs;
    var eindex = positions.Length - 1 - end_index_ofs;
    if (sindex < 0) {
      sindex = 0;
    }
    if (eindex < 0) {
      eindex = positions.Length - 1;
    }
    if (sindex >= positions.Length) {
      sindex = positions.Length - 1;
    }
    if (eindex >= positions.Length) {
      eindex = positions.Length - 1;
    }
    if (sindex > eindex) {
      var temp = sindex;
      sindex = eindex;
      eindex = temp;
    }
    return positions[eindex] - positions[sindex];
  }
  
  /// <summary>
  /// デバッグ表示
  /// </summary>
  void OnGUI()
  {
    var info = this._gesture_info;
    int x = 150;
    int y = 20;
    GUI.Label( new Rect(x,y,300,20), "ScreenPosition = " + info.ScreenPosition.ToString() );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "DeltaPosition = " + info.DeltaPosition.ToString() );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "IsDown = " + info.IsDown.ToString() );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "IsDrag = " + info.IsDrag.ToString() );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "IsUp = " + info.IsUp.ToString() );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "DeltaTime = " + info.DeltaTime.ToString() );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "DragDistance = " + info.DragDistance.ToString()  );
    y += 20;
    GUI.Label( new Rect(x,y,300,20), "ProcessingGesture = " + (this.ProcessingGesture == null ? "null" : "live")  );
  }
}
