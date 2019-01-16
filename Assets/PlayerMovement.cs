using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public InputModule input_module;
	public GameObject spawn_point;
	public BearMovement bear_script;
	public float speed;
	public float climb_speed;
	public float jump_force = 500.0f;
	public Animator player_animator;
	public SpriteRenderer player_renderer;
	public float min_dist_to_ground = 0.5f;

	public Text sign_prompt;
	public GameObject sign_popup;
	public TextCrawl sign_text_crawl;

	public AudioSource background_music;
	public AudioClip forest_music;
	public AudioClip bear_music;
	public AudioClip death_music;

	public AudioSource bear_fx;
	public AudioSource fx_source;
	public AudioClip walking_fx;
	public AudioClip climbing_fx;

	public AudioSource fall_music;
	public AudioClip cave_intro;
	public AudioClip cave_song;

	public bool is_cave;

	public string nextSceneName;

	private Rigidbody2D rb;
	private bool is_on_ground, old_ground;
	private bool jumped;
	private bool in_vines, is_climbing;
	private bool in_sign, reading_sign;
	private bool bear_mode;
	private bool is_dying;

	private float dying_counter;

	private bool fx_walking;
	private bool fx_climbing;

	private string[] sign_text_array = {
		"Use the left and right arrow keys to move and the spacebar to jump.",
		"Use the up and down arrow keys to climb on vines.",
		"There's no prize (´･ω･`) ",
		"BEWARE OF BEAR"
	};

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		is_on_ground = jumped = in_vines = is_climbing = in_sign = reading_sign = bear_mode = is_dying = false;
		old_ground = true; //fall to ground at start
		sign_prompt.enabled = false;
		sign_popup.SetActive(false);

		transform.position = spawn_point.transform.position;

		if (is_cave) {
			background_music.clip = cave_intro;
			background_music.Play();
		}
	}

	private string position_to_string_text(Vector3 p) {
		if (p.x < 2 && p.y < 3) return sign_text_array[0]; //movement / jump sign
		if (p.x > 5 && p.x < 10) return sign_text_array[1]; //climbing sign 
		if (p.x < -4 && p.y > 18) return sign_text_array[2]; //troll sign
		if (p.x > 20) return sign_text_array[3]; //bear sign
		return "OOPSIE WOOPSIE!! Uwu We made a fucky wucky!! A wittle fucko boingo!";
	}

	public bool in_bear_zone() { return bear_mode; }
	public float get_x() { return transform.position.x; }
	public bool is_dead() { return is_dying; }

	private void reset_on_death() {
		transform.position = spawn_point.transform.position;

		//reset animation stuff
		is_on_ground = jumped = in_vines = is_climbing = in_sign = reading_sign = bear_mode = is_dying = false;
		old_ground = true; //fall to ground at start
		player_animator.SetBool ("is_walking", false);
		player_animator.SetBool ("is_falling", true);
		player_animator.SetBool ("is_jumping", false);
		player_animator.SetBool ("is_climbing", false);
		player_animator.SetBool ("is_climbing_idle", false);

		//not dying any more
		player_renderer.color = Color.white;
		player_animator.enabled = true;
		is_dying = false;

		//reset bear
		bear_mode = false;
		if (bear_script != null) bear_script.reset_on_player_death();

		//reset music
		if (is_cave) {
			background_music.clip = cave_intro;
			background_music.Play();
		} else {
			background_music.clip = forest_music;
			background_music.Play();
		}
	}

	//called on entering collision (where one of the 2 objects must have isTrigger checked)
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "ground") {
			old_ground = is_on_ground;
			is_on_ground = true;

			if (is_cave && fall_music.isPlaying && background_music.clip != cave_song) {
				background_music.clip = cave_song;
				background_music.loop = true;
				background_music.Play();
			}
		} else if (other.tag == "climbable") {
			in_vines = true;
		} else if (other.tag == "sign") {
			in_sign = true;
		} else if (other.tag == "bear_zone" && !bear_mode) {
			bear_mode = true;

			background_music.clip = bear_music;
			background_music.Play();

			bear_fx.Play();
		} else if (other.tag == "bear" || other.tag == "point") {
			//DEATH
			if (!is_dying) {
				is_dying = true;
				dying_counter = 3;
				player_renderer.color = Color.red;
				player_animator.enabled = false;

				background_music.clip = death_music;
				background_music.Play();

				if (other.tag == "bear") bear_fx.Play();

				fx_source.loop = false;
			}
		} else if (other.tag == "portal") {
			//next level
			SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
		} else if (other.tag == "cave_fall") {
			fall_music.Play();
			background_music.Stop();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "ground") {
			old_ground = is_on_ground;
			is_on_ground = false;
		} else if ((other.tag == "climbable") && in_vines) {
			in_vines = is_climbing = false;
			player_animator.SetBool("is_climbing", false);
			rb.gravityScale = 1.0f;
		} else if (other.tag == "sign") {
			in_sign = false; reading_sign = false;
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

		if (is_dying) {
			dying_counter -= Time.deltaTime;
			rb.velocity = new Vector3(0, 0);
			if (dying_counter < 0) {
				reset_on_death();
			} else return;
		};

		/*** Player motion ***/
		if (input_module.is_moving ()) {
			//horizontal movement
			transform.localPosition += new Vector3 (input_module.get_movement () * (speed * Time.deltaTime), 0); //control movement speed
			player_renderer.flipX = input_module.get_movement()<0 ? true : false;
			player_animator.SetBool ("is_walking", true);

			fx_walking = true;
		} else {
			player_animator.SetBool("is_walking", false);
			fx_walking = false;
		}

		bool _jump = input_module.get_jump();
		if (is_on_ground) {
			if (!old_ground) {
				//just landed
				jumped = false;
				player_animator.SetBool ("is_jumping", false);
				player_animator.SetBool ("is_falling", false);
				fx_walking = true;
			}

			//perform jump
			if (!jumped && _jump) {
				jumped = true;
				is_on_ground = false; old_ground = true;
				Vector3 up = new Vector3 (0, 1);
				rb.AddForce (jump_force * up);
			}

		} 
		if (!is_on_ground) {
			fx_walking = false;
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
		if (in_vines && input_module.is_pressing_climb_button()) {
			is_climbing = true; player_animator.SetBool("is_climbing", true);
		}
		if (is_climbing) {
			rb.gravityScale = 0.0f;
			//move
			int c = input_module.get_climbing_movement();
			rb.velocity = new Vector3(0, c * climb_speed * Time.deltaTime);
			player_animator.SetBool("is_climbing_idle", c==0);
			fx_climbing = c!=0;
		} else {
			rb.gravityScale = 1.0f;
			player_animator.SetBool("is_climbing", false);
			fx_climbing = false;
		}

		old_ground = is_on_ground;

		//signs
		if (in_sign && !reading_sign) { 
			reading_sign = input_module.is_pressing_enter(); 
			if (reading_sign)  {
				sign_popup.SetActive(true);
				sign_text_crawl.ShowText(position_to_string_text(transform.position));
			}
		}
		sign_prompt.enabled = in_sign && !reading_sign; //display sign prompt

		//display sign text
		sign_popup.SetActive(in_sign && reading_sign);

		//fx
		if (fx_climbing) {
			if (!fx_source.loop || fx_source.clip != climbing_fx) {
				fx_source.loop = true;
				fx_source.clip = climbing_fx;
				fx_source.Play();
			}
		} else if (fx_walking) {
			if (!fx_source.loop || fx_source.clip != walking_fx) {
				fx_source.loop = true;
				fx_source.clip = walking_fx;
				fx_source.Play();
			}
		} else {
			fx_source.loop = false;
		}
	}
}
