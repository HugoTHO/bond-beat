using UnityEngine;
using System.Collections;

public class InputFeedbackCS : MonoBehaviour {

	public enum Stage { First, Second, Third, Fourth };

	public bool paused;
	public KeyCode pressKey1, pressKey2;
	public float pressDelay;
	public int InputCode { get { return inputCode; } }

	private int inputCode;
	private bool delaying, blocked;
	private float delayStart;
	private Stage stage;
	private GameObject[] childs;
	private SpriteRenderer[] sprites;

	void Start () {

		childs = new GameObject[transform.childCount];
		sprites = new SpriteRenderer[transform.childCount];

		for (int i = 0; i < childs.Length; i++) {
			childs [i] = this.transform.GetChild (i).gameObject;
			sprites [i] = childs [i].GetComponent<SpriteRenderer> ();
			childs [i].SetActive(false);
		}

		stage = Stage.First;
		paused = false;
		delaying = false;
		blocked = false;
		inputCode = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (!blocked) {

			if (!paused && (!delaying || Time.time - delayStart < 0.025) && Input.GetKeyDown(pressKey1)) {

				childs[(int)stage * 2].SetActive (true);

				if (!delaying) {
					delayStart = Time.time;
					delaying = true;
				}
			}

			if (!paused && (!delaying || Time.time - delayStart < 0.025) && Input.GetKeyDown(pressKey2)) {

				childs[(int)stage * 2 + 1].SetActive (true);

				if (!delaying) {
					delayStart = Time.time;
					delaying = true;
				}
			}

			if (delaying) {

				float t = (pressDelay - (Time.time - delayStart)) / pressDelay;

				if (childs [(int)stage * 2].activeSelf) {
					Color tmp = sprites [(int)stage * 2].color;
					tmp.a = t;
					sprites [(int)stage * 2].color = tmp;
				}
				if (childs [(int)stage * 2 + 1].activeSelf) {
					Color tmp = sprites [(int)stage * 2 + 1].color;
					tmp.a = t;
					sprites [(int)stage * 2 + 1].color = tmp;
				}

				if (Time.time - delayStart >= 0.025f) {

					if (inputCode > 0)
						inputCode = -1;

					if (inputCode == 0) {
						if (childs [(int)stage * 2].activeSelf && childs [(int)stage * 2 + 1].activeSelf)
							inputCode = 3;
						else if (childs [(int)stage * 2 + 1].activeSelf)
							inputCode = 2;
						else
							inputCode = 1;
					}
				}

				if (Time.time - delayStart >= pressDelay) {
				
					if (childs [(int)stage * 2].activeSelf) {
						childs [(int)stage * 2].SetActive (false);
					}
					if (childs [(int)stage * 2 + 1].activeSelf) {
						childs [(int)stage * 2 + 1].SetActive (false);
					}

					delaying = false;
					inputCode = 0;
				}

			}
		}
	}

	public void NextStage () {

		delaying = false;
		inputCode = 0;

		if (childs [(int)stage * 2].activeSelf) {
			Color tmp = sprites [(int)stage * 2].color;
			tmp.a = 1f;
			sprites [(int)stage * 2].color = tmp;
		}
		if (childs [(int)stage * 2 + 1].activeSelf) {
			Color tmp = sprites [(int)stage * 2 + 1].color;
			tmp.a = 1f;
			sprites [(int)stage * 2 + 1].color = tmp;
		}

		switch (stage) {
			case Stage.First :
				stage = Stage.Second;
				break;
			case Stage.Second :
				stage = Stage.Third;
				break;
			case Stage.Third :
				stage = Stage.Fourth;
				break;
			case Stage.Fourth:
				Block ();
				break;
		}
	}

	public void Block () {
		blocked = true;
	}

	public void ResetState () {

		for (int i = 0; i < childs.Length; i++)
			childs [i].SetActive (false);

		transform.parent.GetChild(0).gameObject.SetActive (false);

		stage = Stage.First;
		blocked = false;
	}
}