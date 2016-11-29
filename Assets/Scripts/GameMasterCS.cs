using UnityEngine;
using System.Collections;

public class GameMasterCS : MonoBehaviour {

	public float NextBeat { get {return this.nextBeat;} }

	public int BPM;
	public MoleculeFitterCS fitter;

	private float beatTime, nextBeat;
	private MoleculeDataBaseCS dataBase;

	void Start () {
		beatTime = 60.0f / BPM;
		nextBeat = Time.time + beatTime;

		dataBase = gameObject.GetComponent<MoleculeDataBaseCS> ();

		fitter.NewMolecule (dataBase.molecules [0]);
	}
	
	void Update () {
		if (Time.time >= nextBeat)
			nextBeat += beatTime;
	
	}
}
