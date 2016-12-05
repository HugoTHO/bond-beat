﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerCS : MonoBehaviour {

	public Text timerText;
	public Slider timerSlider;
	private float startTime;

	void Start () {
		startTime = Time.time;
		timerSlider.value = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float t = Time.time - startTime;

		string minutes = ((int)t / 60).ToString();
		string seconds = (int)t % 60 > 9 ? ((int)t % 60).ToString():
										   "0" + ((int)t % 60).ToString();

		timerText.text = minutes + ":" + seconds;

		if (timerSlider.value < timerSlider.maxValue) {
			timerSlider.value = t;
		}
	}
}
