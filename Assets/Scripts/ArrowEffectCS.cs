using UnityEngine;
using System.Collections;

public class ArrowEffectCS : MonoBehaviour {

	public enum Orientation { Right, Left, Up, Down };

	public float EffectTime = 1.2f, movDistance = 0.1f;
	public Orientation orientation;

	private bool fadeIn, inEffect;
	private float fadeStart;
	private SpriteRenderer sprite;
	private Vector3 originalPosition;

	void Start () {
	
		fadeIn = true;
		inEffect = false;

		sprite = gameObject.GetComponent<SpriteRenderer> ();

		Color tmp = sprite.color;
		tmp.a = 0.0f;
		sprite.color = tmp;

		originalPosition = transform.localPosition;
	}

	void Update () {

		if (fadeIn) {
			if (inEffect) {

				float t = (Time.time - fadeStart) / (EffectTime / 2.0f);
				Color tmp = sprite.color;
				Vector3 auxVect = originalPosition;

				if (t < 1) {
					tmp.a = t;
				} else {
					tmp.a = 1.0f;
					fadeIn = false;
					inEffect = false;
				}
				sprite.color = tmp;

				switch (orientation) {
					case Orientation.Right:
						auxVect.x += t < 1? t * movDistance : movDistance; 
						break;
					case Orientation.Left:
						auxVect.x -= t < 1? t * movDistance : movDistance;
						break;
					case Orientation.Up:
						auxVect.y += t < 1? t * movDistance : movDistance;
						break;
					case Orientation.Down:
						auxVect.y -= t < 1? t * movDistance : movDistance;
						break;
				}

				transform.localPosition = auxVect;

			} else {
				inEffect = true;
				fadeStart = Time.time;
			}
		}
		else {
			if (inEffect) {

				float t = (Time.time - fadeStart) / (EffectTime/2.0f);
				Color tmp = sprite.color;
				Vector3 auxVect = originalPosition;

				if (t < 1) {
					tmp.a = 1.0f - t;
				}
				else {
					tmp.a = 0.0f;
					fadeIn = true;
					inEffect = false;
				}
				sprite.color = tmp;

				switch (orientation) {
				case Orientation.Right:
					auxVect.x += t < 1? (1.0f - t) * movDistance : 0; 
					break;
				case Orientation.Left:
					auxVect.x -= t < 1? (1.0f - t) * movDistance : 0;
					break;
				case Orientation.Up:
					auxVect.y += t < 1? (1.0f - t) * movDistance : 0;
					break;
				case Orientation.Down:
					auxVect.y -= t < 1? (1.0f - t) * movDistance : 0;
					break;
				}

				transform.localPosition = auxVect;
			}
			else {
				inEffect = true;
				fadeStart = Time.time;
			}
		}

	}
}
