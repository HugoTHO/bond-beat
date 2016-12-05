using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreCS : MonoBehaviour {

	public int Score { get { return this.score; } }

	public float fadeOutVel = 0.5f;

	public Text scoreText, feedbackScore;
	public Slider scoreSlider;
	public Color positive, negative;

	private float fadeStart;
	private bool fading;
	private int score;

	void Start () {
		score = 0;
		scoreText.text = "0";
		scoreSlider.value = 0;
		fading = false;
	}

	void Update () {
		if (fading) {
			float t = (fadeOutVel - (Time.time - fadeStart)) / fadeOutVel;

			if (t < 0) {
				t = 0;
				fading = false;	
			}

			Color tmp = feedbackScore.color;
			tmp.a = t;
			feedbackScore.color = tmp;
		}
	}

	public void IncreaseScore (int amount) {
		score += amount;
		this.UpdateScore ();
		feedbackScore.text = "+" + amount.ToString ();
		feedbackScore.color = positive;

		fading = true;
		fadeStart = Time.time;
	}

	public void DecreaseScore (int amount) {
		score -= amount;

		if (score < 0)
			score = 0;
		
		this.UpdateScore ();
		feedbackScore.text = "-" + amount.ToString ();
		feedbackScore.color = negative;
		fading = true;
		fadeStart = Time.time;
	}

	private void UpdateScore () {

		scoreText.text = score.ToString ();

		if (scoreSlider.value < scoreSlider.maxValue) {
			scoreSlider.value = score;
			if (scoreSlider.value > scoreSlider.maxValue) {
				scoreSlider.value = scoreSlider.maxValue;
			}
		}
	}
}
