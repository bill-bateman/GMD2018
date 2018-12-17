using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour {

	//movement variables
	private float movement;
	private bool btn_jump, btn_left, btn_right, btn_up, btn_down;

	//keyboard bindings
	private KeyCode keyboard_jump_button = KeyCode.Space;
	private KeyCode keyboard_movement_left = KeyCode.LeftArrow;
	private KeyCode keyboard_movement_right = KeyCode.RightArrow;
	private KeyCode keyboard_movement_up = KeyCode.UpArrow;
	private KeyCode keyboard_movement_down = KeyCode.DownArrow;


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
	public bool is_pressing_up() { return btn_up; }
	public bool is_pressing_down() { return btn_down; }
	public bool is_pressing_climb_button() { return btn_up ^ btn_down; }

	public float get_movement() { return movement; }
	public int get_climbing_movement() { if (btn_up && !btn_down) {return 1;} else if (btn_down) {return -1;} else {return 0;} } 



	// Use this for initialization
	void Start () {
		btn_jump = btn_left = btn_right = btn_up = btn_down = false;
		movement = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		btn_left = Input.GetKey (keyboard_movement_left);
		btn_right = Input.GetKey (keyboard_movement_right);
		btn_up = Input.GetKey (keyboard_movement_up);
		btn_down = Input.GetKey (keyboard_movement_down);

		//left-right movement
		if (btn_left ^ btn_right) {
			movement = (btn_left ? -1.0f : 1.0f);
		} else {
			movement = 0.0f;
		}

		//jump
		btn_jump = (Input.GetKeyDown(keyboard_jump_button));
	}
}
