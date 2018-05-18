using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleKureshiImage : MonoBehaviour {

	private static readonly Vector3 TARGET_POSITION = new Vector3(0, -613, 0);
	private static readonly Vector3 INITIAL_POSITION = new Vector3(0, -1300, 0);

	private const float SPRING_CONSTANT = 0.9f;
	private const float ATTENUATION_RATE = 0.1f;

	private Vector3 acc, vel, pos;
	private void Start() {
		acc = vel = Vector3.zero;
        pos = INITIAL_POSITION;
	}

	private void Update() {
		Vector3 diff = TARGET_POSITION - this.pos;
        this.acc = diff * 0.1f;
        this.vel += this.acc;
        this.vel *= 0.9f;
        this.pos += this.vel;
		transform.localPosition = this.pos;
	}
}
