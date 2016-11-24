using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeInPartCS : MonoBehaviour {

	public float fadeTime = 0.5f;

	private bool showed;
	private float fadeStart;
	private List<Renderer> renderList;
	private List<TextMesh> textList;

	// Use this for initialization
	void Start () {
	
		showed = false;
		fadeStart = Time.time;

		renderList = new List<Renderer> ();
		textList = new List<TextMesh> ();

		foreach (Transform child in transform) {

			Renderer rAux = child.gameObject.GetComponent<Renderer>();
			Color cAux = rAux.material.color;
			cAux.a = 0;
			rAux.material.color = cAux;
			renderList.Add (rAux);

			if (child.childCount > 0) {
				TextMesh tAux = child.GetChild (0).GetComponent<TextMesh> ();
				Color tcAux = tAux.color;
				tcAux.a = 0;
				tAux.color = tcAux;
				textList.Add (tAux);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!showed) {
			
			float t = (Time.time - fadeStart) / fadeTime;

			if (t < 1.0f) {
				foreach (Renderer render in renderList) {
					Color cAux = render.material.color;
					cAux.a = t;
					render.material.color = cAux;
				}
				foreach (TextMesh text in textList) {
					Color cAux = text.color;
					cAux.a = t;
					text.color = cAux;
				}
			}
			else {
				foreach (Renderer render in renderList) {
					Color cAux = render.material.color;
					cAux.a = 1;
					render.material.color = cAux;
				}
				foreach (TextMesh text in textList) {
					Color cAux = text.color;
					cAux.a = 1;
					text.color = cAux;
				}

				showed = true;
			}
				
		}

	}
}
