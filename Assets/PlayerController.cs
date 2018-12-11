using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//movement variables
	private float movement;
	private bool btn_jump, btn_left, btn_right;

	//keyboard bindings
	private KeyCode keyboard_jump_button = KeyCode.Space;
	private KeyCode keyboard_movement_left = KeyCode.LeftArrow;
	private KeyCode keyboard_movement_right = KeyCode.RightArrow;


	// Getter functions

	public bool get_jump() {
		if (btn_jump) {
			btn_jump = false;
			return true;
		} 
		return btn_jump;
	}

	public bool is_moving() {
		return btn_left ^ btn_right;
	}

	public float get_movement() { return movement; }



	// Use this for initialization
	void Start () {
		btn_jump = btn_left = btn_right = false;
		movement = 0.0f;
	}
		
	// Update is called once per frame
	void Update () {
		btn_left = Input.GetKey (keyboard_movement_left);
		btn_right = Input.GetKey (keyboard_movement_right);

		//left-right movement
		if (btn_left ^ btn_right) {
			movement = (btn_left ? -1.0f : 1.0f);
		} else {
			movement = 0.0f;
		}

		//jump
		if (!btn_jump) btn_jump = (Input.GetKeyDown(keyboard_jump_button));
	}
}
