using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TittleRotateCS : MonoBehaviour {

	public float velocity;

	private RectTransform rectTransform;

	void Start () {

		rectTransform = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		rectTransform.Rotate( Vector3.forward * velocity );
	}
}
