﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : MonoBehaviour {
	public void OnClick() {
		Debug.Log("データ削除");
		PlayerPrefs.DeleteAll();
	}
}
