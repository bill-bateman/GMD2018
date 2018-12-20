using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : MonoBehaviour
{
	public PlayerMovement player;
	public float movement_speed;
	public Animator bear_animator;
	public SpriteRenderer bear_renderer;

	public GameObject start_position;

    // Start is called before the first frame update
    void Start()
    {
		bear_animator.SetBool("bear_move", false);
		transform.position = start_position.transform.position;
    }

	public void reset_on_player_death() {
		bear_animator.SetBool("bear_move", false);
		transform.position = start_position.transform.position;
		bear_renderer.flipX = false;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (!player.in_bear_zone() || player.is_dead()) {
			bear_animator.SetBool("bear_move", false);
			return;
		}

		//track player's x location
		float x = transform.position.x;
		float px = player.get_x();

		if (Mathf.Abs(x - px) > 0.5) {
			bear_animator.SetBool("bear_move", true);
			Vector3 direction;
			if (x < px) {
				bear_renderer.flipX = false;
				direction = new Vector3(+1, 0); 
			} else {
				bear_renderer.flipX = true;
				direction = new Vector3(-1, 0);
			}
			transform.position += direction * (movement_speed * Time.deltaTime);
		}
		else {
			bear_animator.SetBool("bear_move", false);
		}
    }
}
