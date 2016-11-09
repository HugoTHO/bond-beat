using UnityEngine;
using System.Collections;

public class SquaresFeedback : MonoBehaviour {

	public enum Stage { First, Second, Third, Fourth };

	public Stage stage;
	public KeyCode pressKey1, pressKey2;
	public float pressDelay;

	private bool delaying;
	private GameObject[] childs;
	private float delayStart;

	void Start () {

		childs = new GameObject[transform.childCount];

		for (int i = 0; i < childs.Length; i++) {
			childs[i] = this.transform.GetChild (i).gameObject;
			childs[i].SetActive(false);
		}

		stage = Stage.First;
		delaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ((!delaying || Time.time - delayStart < 0.1) && Input.GetKey(pressKey1)) {

			childs[(int)stage * 2].SetActive (true);

			if (!delaying) {
				delayStart = Time.time;
				delaying = true;
			}
		}

		if ((!delaying || Time.time - delayStart < 0.1) && Input.GetKey(pressKey2)) {

			childs[(int)stage * 2 + 1].SetActive (true);

			if (!delaying) {
				delayStart = Time.time;
				delaying = true;
			}
		}

		if (delaying) {
			
			if (Time.time - delayStart >= pressDelay) {
				
				if (childs [(int)stage * 2].activeSelf) {
					childs [(int)stage * 2].SetActive (false);
				}
				if (childs [(int)stage * 2 + 1].activeSelf) {
					childs [(int)stage * 2 + 1].SetActive (false);
				}

				delaying = false;
			}
		}
	}

	void FixedUpdate () {
	}
}
