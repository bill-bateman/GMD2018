using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalMovement : MonoBehaviour
{
	public float speed;
	public float move_time_1, stop_time_1, move_time_2, stop_time_2, move_time_3;
	public PersonScript person;

	public GameObject dog_object;
	public GameObject person_object;

	private bool shown_text_1, shown_text_2, shown_text_3;

	void Start() {
		shown_text_1 = shown_text_2 = shown_text_3 = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "dog") {
			Destroy(dog_object);
			if (!shown_text_2) {
				shown_text_2 = true;
				person.NextText();
			}
		} else if (other.tag == "person") {
			Destroy(person_object);
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		bool move;
		if (move_time_1 > 0) {
			move = true;
			move_time_1 -= Time.deltaTime;
		} else if (stop_time_1 > 0) {
			if (!shown_text_1) {
				shown_text_1 = true;
				person.NextText();
			}
			move = false;
			stop_time_1 -= Time.deltaTime;
		} else if (move_time_2 > 0) {
			move = true;
			move_time_2 -= Time.deltaTime;
		} else if (stop_time_2 > 0) {
			move = false;
			stop_time_2 -= Time.deltaTime;
		} else if (move_time_3 > 0) { 
			move = true; 
			move_time_3 -= Time.deltaTime;
			if (!shown_text_3) {
				shown_text_3 = true;
				person.NextText();
			}
		} else {
			move=true;
			SceneManager.LoadScene("Level1", LoadSceneMode.Single);
		}

		if (move) transform.position += new Vector3(-1 * speed * Time.deltaTime, 0);
    }
}
