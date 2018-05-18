using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
	private const float START_POINT_X = 6.0f;
	private const float END_POINT_X = -6.0f;

	private const float MOVE_SPEED = 0.5f;

	private void Start() {

	}

	private void Update() {
		if(transform.position.x <= END_POINT_X) {
			transform.position = new Vector3(START_POINT_X, transform.position.y, 1.0f);
		}
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(END_POINT_X, transform.position.y, 1.0f), MOVE_SPEED*Time.deltaTime);
	}
}
