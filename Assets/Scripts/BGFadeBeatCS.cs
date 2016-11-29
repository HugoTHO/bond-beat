using UnityEngine;
using System.Collections;

public class BGFadeBeatCS : MonoBehaviour {

	public int BPM;

	private float EffectTime, nextBeat;
	private SpriteRenderer sprite;

	void Start () {
	
		EffectTime = 60.0f / BPM;
		nextBeat = Time.time + EffectTime;

		sprite = gameObject.GetComponent<SpriteRenderer> ();
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
			tmp.a = t;
		} else {
			tmp.a = (1 - t);
		}

		sprite.color = tmp;
	}

}