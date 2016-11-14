using UnityEngine;
using System.Collections;

public class BeatBondCS : MonoBehaviour {

	public float beatTime, beatDistance = 1.42f;
	public GameObject[] prefabs;
	public int[] posCode;
	public Transform spawnPointA, spawnPointB;

	private float velocity, nextBeat;
	private GameObject[] objRefs;
	private int[] objBeats;
	private Vector3 posA, posB;


	// Use this for initialization
	void Start () {

		velocity = beatDistance / beatTime;
		nextBeat = Time.time + beatTime;

		posA = spawnPointA.transform.position;
		posB = spawnPointB.transform.position;

		objRefs = new GameObject[prefabs.Length];
		objBeats = new int[prefabs.Length];
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Time.time >= nextBeat) {

			for (int i = 0; i < objRefs.Length; i++) {
				
				if (objRefs [i] == null) {
					if (i == 0) {
						objRefs[i] = NewBeatLine (prefabs [i], posCode [i]);
						objBeats[i] = 0;
					} else if (objBeats [i - 1] == 2) {
						objRefs [i] = NewBeatLine (prefabs [i], posCode [i]);
						objBeats [i] = 0;
					}
				} else {
					
					objBeats[i] += 1;

					if (objBeats [i] == 8) {
						DestroyLine (objRefs [i], posCode [i]);
						objRefs [i] = null;
					}
				}
			}

			nextBeat = Time.time + beatTime;


		}
	}

	private GameObject NewBeatLine (GameObject prefab, int posCode) {

		GameObject aux;

		switch (posCode) {
		case 1: 
			aux = (GameObject) Instantiate (prefab, posA, Quaternion.identity, this.transform);
			break;
		case 2:
			aux = (GameObject) Instantiate (prefab, posB, Quaternion.identity, this.transform);
			break;
		case 3:
			aux = new GameObject();
			aux.AddComponent<Rigidbody2D> ();

			Transform tAux = aux.transform;
			tAux.SetParent(this.transform);
			tAux.position = this.transform.position;

			Instantiate (prefab, posA, Quaternion.identity, tAux);
			Instantiate (prefab, posB, Quaternion.identity, tAux);
			break;
		default:
			aux = new GameObject ();
			aux.AddComponent<Rigidbody2D> ();
			Debug.Log ("Comportamento Inesperado do Método 'NewBeatLine'");
			break;
		}
			
		Rigidbody2D rbAux = aux.GetComponent<Rigidbody2D> ();
		rbAux.gravityScale = 0;
		rbAux.velocity = Vector2.down * velocity;

		return aux;
	}

	private void DestroyLine (GameObject reference, int posCode){

		switch (posCode) {
		case 1:
		case 2:
			GameObject.Destroy (reference);
			break;
		case 3:
			GameObject.Destroy (reference.transform.GetChild (0).gameObject);
			GameObject.Destroy (reference.transform.GetChild (1).gameObject);
			GameObject.Destroy (reference);
			break;
		}
	}
}
