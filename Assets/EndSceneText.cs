using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneText : MonoBehaviour
{
	public string displayText;
	public int delay;
	public PersonScript ps;

	private int count;

	void Start() { count = 0; }

    // Update is called once per frame
    void Update()
    {
		count++;
		if (count == delay) {
			ps.NextTaskString(displayText);
		}
    }
}
