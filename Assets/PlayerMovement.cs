using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public InputModule input_module;
	public float speed;
	public float jump_force = 500.0f;
	public Animator player_animator;
	public SpriteRenderer player_renderer;

	private Rigidbody2D rb;
	private bool is_on_ground;

	// Use this for initialization
	void Start () {
		rb = GetComponentInChildren<Rigidbody2D> ();
		is_on_ground = true;
	}
	
	// FixedUpdate is called at constant intervals
	void FixedUpdate () {
		/*** Player motion ***/
		if (input_module.is_moving ()) {
			//horizontal movement
			transform.localPosition += new Vector3 (input_module.get_movement () * (speed * Time.deltaTime), 0); //control movement speed
			player_renderer.flipX = input_module.get_movement()<0 ? true : false;
			player_animator.SetBool ("is_walking", true);
		} else {
			player_animator.SetBool("is_walking", false);
		}


		if (input_module.get_jump () && is_on_ground) {
			Vector3 up = new Vector3 (0, 1);
			rb.AddForce (jump_force * up);
		}
	}
}
