using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMasterCS : MonoBehaviour {

	public enum Mode { Tutorial, Free } ;

	public Mode mode;
	public float NextBeat { get { return this.nextBeat; } }
	public float BeatTime { get { return this.beatTime; } }

	public int BPM, maxTime;
	public MoleculeFitterCS fitter;
	public Text help1, help2, help3;
	public AudioSource music;
	public InputFeedbackCS input1, input2;
	public GameObject finishScreen;

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
			help1.gameObject.SetActive (true);
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

		if ((int)Time.time == maxTime) {
			FinishGame ();
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
						switch (tutIndex) {
						case 1:
							help2.gameObject.SetActive (true);
							break;
						case 2:
							help3.gameObject.SetActive (true);
							break;
						default:
							Debug.Log ("Bug no Tutorial");
							break;
						}
						fitter.NewMolecule (dataBase.tutorialMolecules [tutIndex]);
					} else {

						mode = Mode.Free;

						int rand = Random.Range (0, dataBase.molecules.Count);

						MoleculeDataCS molecule = dataBase.molecules [rand];
						dataBase.molecules.RemoveAt (rand);

						fitter.NewMolecule (molecule);
					}
				}
			} else if (mode == Mode.Tutorial && moleculeDelay == 4) {
				if (help1.gameObject.activeSelf)
					help1.gameObject.SetActive (false);
				if (help2.gameObject.activeSelf)
					help2.gameObject.SetActive (false);
				if (help3.gameObject.activeSelf)
					help3.gameObject.SetActive (false);
			}
		}
	}

	public void PausarJogo() {
		music.Pause();
		Time.timeScale = 0;
		input1.paused = true;
		input2.paused = true;
	}

	public void DespausarJogo () {
		music.UnPause();
		Time.timeScale = 1;
		input1.paused = false;
		input2.paused = false;
	}

	private void FinishGame () {
		input1.paused = true;
		input2.paused = true;
		finishScreen.SetActive (true);
	}
}
