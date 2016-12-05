using UnityEngine;
using System.Collections;

public class BeatBondCS : MonoBehaviour {

	public enum Stage { first, second, third, fourth };

	public float NextBeat { set {this.nextBeat = value; } }
	public MoleculeFitterCS Fitter { set {this.fitter = value; } }

	public int bondCode, BPM;
	public float beatDistance = 1.43f;
	public GameObject[] prefabs;
	public int[] posCode;
	public Transform spawnPointA, spawnPointB;

	private int[] objBeats;
	private float beatTime, velocity, nextBeat;
	private bool matched, colorMark;
	private Stage stage;
	private GameObject[] objRefs;
	private Vector3 posA, posB;
	private MoleculeFitterCS fitter;
	private InputFeedbackCS inputs;
	private BGFadeBeatCS beatfeedback;

	// Use this for initialization
	void Start () {

		beatTime = 60.0f / BPM;
		velocity = beatDistance / beatTime;

		posA = spawnPointA.transform.position;
		posB = spawnPointB.transform.position;

		stage = Stage.first;
		matched = false;
		colorMark = false;

		objRefs = new GameObject[prefabs.Length];
		objBeats = new int[prefabs.Length];
	
		foreach (Transform child in transform.parent.parent) {
			if (child.CompareTag ("Input"))
				inputs = child.GetComponent<InputFeedbackCS> ();
			if (child.CompareTag ("BackGround")) {
				beatfeedback = child.GetComponent<BGFadeBeatCS> ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!matched) {

			if (Time.time >= nextBeat) {

				for (int i = (int)stage; i < objRefs.Length; i++) {
				
					if (objRefs [i] == null) {
						if (i == 0) {
							objRefs [i] = NewBeatLine (prefabs [i], posCode [i]);
							objBeats [i] = 0;
						} else if (objBeats [i - 1] == 2) {
							objRefs [i] = NewBeatLine (prefabs [i], posCode [i]);
							objBeats [i] = 0;
						}
					} else {
					
						objBeats [i] += 1;

						if (objBeats [i] == 8) {
							DestroyLine (objRefs [i], posCode [i]);
							objRefs [i] = null;
						}
					}
				}
				nextBeat += beatTime;
			}

			if (objRefs [(int)stage] != null) {
				float targetPosY = objRefs [(int)stage].transform.localPosition.y;

				if (targetPosY <= -11.15f + (int)stage * 1.42f) {
					if (!colorMark) {
						beatfeedback.ResetColor ();
						beatfeedback.intence = 1;
					}
					if( stage != Stage.first)
						ResetState ();
				} else {
					if (targetPosY >= -11.15f + (int)stage * 1.42f && targetPosY <= -10.15f + (int)stage * 1.42f) {
						if (!colorMark) {
							beatfeedback.ModifyColor ();
							beatfeedback.intence = 2;
						}
						if (inputs.InputCode == posCode [(int)stage])
							NextStage ();
					}
				}
			}
		}
	}

	public void ResetBGColor () {
		beatfeedback.ResetColor ();
		beatfeedback.intence = 1;
	}

	private void NextStage () {
		
		Rigidbody2D rbAux = objRefs[(int)stage].GetComponent<Rigidbody2D> ();
		rbAux.velocity = Vector2.zero;
		Vector3 auxVec = objRefs [(int)stage].transform.localPosition;
		auxVec.y = -10.65f + (int)stage * 1.42f;
		objRefs [(int)stage].transform.localPosition = auxVec;

		inputs.NextStage ();

		switch (stage) {
			case Stage.first :
				stage = Stage.second;
				break;
			case Stage.second :
				stage = Stage.third;
				break;
			case Stage.third :
				stage = Stage.fourth;
				break;
			case Stage.fourth:
				Match ();
				break;
			}

		if ((int)stage == objRefs.Length) {
			Match ();
		}
	}

	private void ResetState () {

		for (int i = 0; i < (int)stage; i++) {
			DestroyLine (objRefs [i], posCode [i]);
		}

		stage = Stage.first;
		inputs.ResetState ();
	}

	private void Match () {
		inputs.Block ();
		matched = true;
		fitter.NextPart (bondCode, this);
		beatfeedback.ResetColor ();
		beatfeedback.intence = 1;
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
