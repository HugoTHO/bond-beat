using UnityEngine;
using System.Collections;

public class BGFadeBeatCS : MonoBehaviour {

	public int BPM;
	public float intence = 1;
	public Color modifiedColor;


	private float EffectTime, nextBeat;
	private SpriteRenderer sprite;
	private Color originalColor;

	void Start () {
	
		EffectTime = 60.0f / BPM;
		nextBeat = Time.time + EffectTime;

		sprite = gameObject.GetComponent<SpriteRenderer> ();
		originalColor = sprite.color;

		Color tmp = sprite.color;
		tmp.a = 0.0f;
		sprite.color = tmp;
	}

	void Update () {
		
		if (Time.time >= nextBeat)
			nextBeat += EffectTime;
		
		float t = (nextBeat - Time.time) / EffectTime;

		Color tmp = sprite.color;

		if (t < 0.5f) {
			tmp.a = t * intence;
		} else {
			tmp.a = (1 - t) * intence;
		}

		sprite.color = tmp;
	}

	public void ModifyColor() {

		Color tmp = sprite.color;
		tmp.r = modifiedColor.r;
		tmp.g = modifiedColor.g;
		tmp.b = modifiedColor.b;

		sprite.color = tmp;
	}

	public void ResetColor() {
	
		Color tmp = sprite.color;
		tmp.r = originalColor.r;
		tmp.g = originalColor.g;
		tmp.b = originalColor.b;

		sprite.color = tmp;
	}
}