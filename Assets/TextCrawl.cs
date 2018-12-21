using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextCrawl : MonoBehaviour
{
	public AudioSource audio_blip;

	private string display_text;
	private int char_count;


	public void ShowText(string s) {
		display_text = s;
		char_count = 0;
		audio_blip.Play(); audio_blip.loop = true;
		audio_blip.pitch = Random.Range(0.5f, 1.5f);
	}

    // Update is called once per frame
    void Update()
    {
		Debug.Log(char_count);
		Debug.Log(display_text);
		char_count++;
		if (char_count > display_text.Length) {
			char_count = display_text.Length;
			audio_blip.Stop(); audio_blip.loop = false;
		} else {
			audio_blip.pitch = Random.Range(0.5f, 1.5f);
		}
		gameObject.GetComponent<Text>().text = display_text.Substring(0, char_count);
    }
}