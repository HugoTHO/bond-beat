using UnityEngine;
using System.Collections;

public class GameMasterCS : MonoBehaviour {

	public enum Mode { Tutorial, Free } ;

	public Mode mode;
	public float NextBeat { get { return this.nextBeat; } }
	public float BeatTime { get { return this.beatTime; } }

	public int BPM;
	public MoleculeFitterCS fitter;

	private bool counting;
	private int tutIndex, moleculeDelay;
	private float beatTime, nextBeat;
	private MoleculeDataBaseCS dataBase;

	void Start () {
		beatTime = 60.0f / BPM;
		nextBeat = Time.time + beatTime;

		dataBase = gameObject.GetComponent<MoleculeDataBaseCS> ();

		if (mode == Mode.Free) {
			int rand = Random.Range (0, dataBase.molecules.Count);

			MoleculeDataCS molecule = dataBase.molecules [rand];
			dataBase.molecules.RemoveAt (rand);

			fitter.NewMolecule (molecule);
		} else {
			tutIndex = 0;
			fitter.NewMolecule (dataBase.tutorialMolecules[0]);
		}

		counting = false;
		moleculeDelay = 0;
	}
	
	void Update () {
		if (Time.time >= nextBeat) {
			nextBeat += beatTime;
			if (counting) {
				moleculeDelay++;
				NewMolecule ();
			}
		}
	}

	public void NewMolecule () {

		if (!counting) {
			counting = true;
		} else {
			if (moleculeDelay >= 5) {
				
				counting = false;
				moleculeDelay = 0;

				if (mode == Mode.Free) {
					int rand = Random.Range (0, dataBase.molecules.Count);

					MoleculeDataCS molecule = dataBase.molecules [rand];
					dataBase.molecules.RemoveAt (rand);

					fitter.NewMolecule (molecule);
				} else {
					tutIndex++;
					if (tutIndex < 3) {
						fitter.NewMolecule (dataBase.tutorialMolecules [tutIndex]);
					} else {
						mode = Mode.Free;

						int rand = Random.Range (0, dataBase.molecules.Count);

						MoleculeDataCS molecule = dataBase.molecules [rand];
						dataBase.molecules.RemoveAt (rand);

						fitter.NewMolecule (molecule);
					}
				}
			}
		}
	}
}
