using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
	public GameObject to_track;
	public float speed;

	private float prev_x;

    // Start is called before the first frame update
    void Start()
    {
		prev_x = to_track.transform.position.x;
    }

    // FixedUpdate is called at a constant rate
    void FixedUpdate()
    {
		float x = to_track.transform.position.x;

		transform.position += new Vector3((x - prev_x) * speed, 0);

		prev_x = x;
    }
}
