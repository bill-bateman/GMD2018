using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public InputModule input_module;
	public float speed;
	public float jump_force = 500.0f;
	public Animator player_animator;
	public SpriteRenderer player_renderer;
	public float min_dist_to_ground = 0.5f;

	private Rigidbody2D rb;
	private bool is_on_ground, old_ground;
	private bool jumped;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		is_on_ground = jumped = false;
		old_ground = true; //fall
	}

	//called on entering collision (where one of the 2 objects must have isTrigger checked)
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "ground") {
			old_ground = is_on_ground;
			is_on_ground = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "ground") {
			old_ground = is_on_ground;
			is_on_ground = false;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "ground") {
			old_ground = is_on_ground;
			is_on_ground = true;
		}
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

		if (is_on_ground) {
			if (!old_ground) {
				//just landed
				jumped = false;
				player_animator.SetBool ("is_jumping", false);
				player_animator.SetBool ("is_falling", false);
			}

			//perform jump
			if (!jumped && input_module.get_jump ()) {
				jumped = true;
				is_on_ground = false; old_ground = true;
				Vector3 up = new Vector3 (0, 1);
				rb.AddForce (jump_force * up);
			}

		} 
		if (!is_on_ground) {
			if (old_ground) {
				//just jumped or fell
				if (jumped) {
					//just jumped
					player_animator.SetBool ("is_jumping", true);
				} else {
					//just fell 
					player_animator.SetBool ("is_falling", true);
				}
			}

			if (jumped && rb.velocity.y < 0) {
				//change from jumping to falling
				jumped = false;
				player_animator.SetBool ("is_falling", true);
			}

		}


		old_ground = is_on_ground;
	}
}
