using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PersonScript : MonoBehaviour
{
	public GameObject speech_bubble;
	public TextCrawl speech_text;

	private int index;
	private string[] string_array = { 
		"A portal?",
		"My dog!",
		"Not again..."
	};

    // Start is called before the first frame update
    void Start()
    {
		index = 0;
    }

	public void NextText() {
		speech_bubble.SetActive(true);
		speech_text.ShowText(string_array[index]);
		index++;
	}

	public void NextTaskString(string s) {
		speech_bubble.SetActive(true);
		speech_text.ShowText(s);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
