using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationSlider : SingletonMonoBehaviour<RotationSlider> {
	private Slider slider;

	public float SliderValue { get; set;}

	override protected void Awake() {
		base.Awake();
	}

	private void Start() {
		slider = GetComponent<Slider>();
	}

	private void Update() {
		//slider.value = SliderValue;
		if(Input.GetKey(KeyCode.Z)) {
			slider.value -= 2f;
		}
		if(Input.GetKey(KeyCode.X)) {
			slider.value += 2f;
		}
	}

	public void OnValueChanged() {
		SliderValue = slider.value;
	}
}
