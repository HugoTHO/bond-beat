using UnityEngine;
using System.Collections;

public class DontDestroyCS : MonoBehaviour {

	void Awake () {
	
		DontDestroyOnLoad (transform.gameObject);
	}
}
