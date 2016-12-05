using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateScoreCS : MonoBehaviour {

	public ScoreCS score;

	private bool update;

	void Start () {
		update = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!update) {
			gameObject.GetComponent<Text> ().text = score.Score.ToString ();
		}
	}
}
