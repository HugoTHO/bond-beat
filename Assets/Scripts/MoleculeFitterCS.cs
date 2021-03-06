﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MoleculeFitterCS : MonoBehaviour {

	public GameObject StreamARef { get { return this.strARef; } }
	public GameObject StreamBRef { get { return this.strBRef; } }

	public GameMasterCS master;
	public Text moleculeName;
	public ScoreCS score;
	public Transform moleculeSpawn, streamA, streamB;
	public RectTransform signA, signB;
	public InputFeedbackCS squaresA, squaresB;
	public GameObject[] beatBonds, beatSigns;
	public GameObject[] help;

	private bool delaying = false, tutorial = false, tutorialCheck = true;
	private int current, tutorialIndex = 0;
	private float interMolecDelay;
	private MoleculeDataCS molecule;
	private GameObject moleculeRef, strARef, strBRef, sigARef, sigBRef;
	private List<GameObject> moleculeParts, arrows;

	void Update () {
		if (delaying) {
			if (Time.time >= interMolecDelay) {
				moleculeName.text = "";
				delaying = false;
			}
		}
	}

	public void NewMolecule (MoleculeDataCS molecule) {

		this.molecule = molecule;

		if (tutorialCheck) {
			if (this.molecule.Name == "metano") {
				tutorial = true;
			}
			tutorialCheck = false;
		}

		current = 0;
		StartFit ();
	}

	private void StartFit () {

		moleculeParts = new List<GameObject> ();
		arrows = new List<GameObject> ();

		moleculeRef = (GameObject) GameObject.Instantiate (molecule.Prefab, moleculeSpawn, false);

		foreach (Transform pAux in moleculeRef.transform.GetChild(0))
			moleculeParts.Add (pAux.gameObject);

		foreach (Transform aAux in moleculeRef.transform.GetChild(1))
			arrows.Add (aAux.gameObject);

		moleculeName.text = molecule.Name;

		if (tutorial) {
			if (tutorialIndex > 0)
				help [tutorialIndex - 1].SetActive (false);
			if (tutorialIndex == 3) {
				tutorial = false;
			} else {
				help [tutorialIndex++].SetActive (true);
			}
		}

		if (molecule.Instructions [0].OptB > 0) {

			int rand = Random.Range (0, 2);

			strARef = (GameObject)GameObject.Instantiate (beatBonds [molecule.Instructions [0].OptA - 1], rand == 0 ? streamA : streamB, false);
			BeatBondCS auxRefA = strARef.GetComponent<BeatBondCS> ();
			auxRefA.NextBeat = master.NextBeat;
			auxRefA.Fitter = this;
			sigARef = (GameObject)GameObject.Instantiate (beatSigns [molecule.Instructions [0].OptA - 1], rand == 0 ? signA : signB, false);
		
			strBRef = (GameObject)GameObject.Instantiate (beatBonds [molecule.Instructions [0].OptB - 1], rand == 0 ? streamB : streamA, false);
			BeatBondCS auxRefB = strBRef.GetComponent<BeatBondCS> ();
			auxRefB.NextBeat = master.NextBeat;
			auxRefB.Fitter = this;
			sigBRef = (GameObject)GameObject.Instantiate (beatSigns [molecule.Instructions [0].OptB - 1], rand == 0 ? signB : signA, false);

		} else {
			
			strARef = (GameObject)GameObject.Instantiate (beatBonds [molecule.Instructions [0].OptA - 1], streamA, false);
			BeatBondCS auxRef = strARef.GetComponent<BeatBondCS> ();
			auxRef.NextBeat = master.NextBeat;
			auxRef.Fitter = this;
			sigARef = (GameObject)GameObject.Instantiate (beatSigns [molecule.Instructions [0].OptA - 1], signA, false);

			squaresB.Block();
		}
	}

	public void NextPart (int answer, BeatBondCS beatBond) {

		if (molecule.Instructions [current].getAnswer (answer) != -1) {

			if (beatBond.gameObject == strARef) {
				if (strBRef != null) {
					strBRef.GetComponent<BeatBondCS> ().ResetBGColor ();
					GameObject.DestroyImmediate (strBRef);
				}
			} else {
				if (strARef != null) {
					strARef.GetComponent<BeatBondCS> ().ResetBGColor ();
					GameObject.DestroyImmediate (strARef);
				}
			}

			GameObject.Destroy (beatBond.gameObject, master.NextBeat - Time.time);

			GameObject.DestroyImmediate (sigARef);
			GameObject.DestroyImmediate (sigBRef);

			arrows [current].SetActive (false);

			current = molecule.Instructions [current].getAnswer (answer);

			squaresA.ResetState ();
			squaresB.ResetState ();

			moleculeParts [current].SetActive (true);

			if (tutorial) {
				help [tutorialIndex - 1].SetActive (false);
			}

			if (current < molecule.Instructions.Count) {

				arrows [current].SetActive (true);

				score.IncreaseScore (100);

				if (tutorial) {
					help [tutorialIndex++].SetActive (true);
				}

				if (molecule.Instructions [current].OptB > 0) {

					int rand = Random.Range (0, 2);

					strARef = (GameObject)GameObject.Instantiate (beatBonds [molecule.Instructions [current].OptA - 1], rand == 0 ? streamA : streamB, false);
					BeatBondCS auxRefA = strARef.GetComponent<BeatBondCS> ();
					auxRefA.NextBeat = master.NextBeat;
					auxRefA.Fitter = this;
					sigARef = (GameObject)GameObject.Instantiate (beatSigns [molecule.Instructions [current].OptA - 1], rand == 0 ? signA : signB, false);

					strBRef = (GameObject)GameObject.Instantiate (beatBonds [molecule.Instructions [current].OptB - 1], rand == 0 ? streamB : streamA, false);
					BeatBondCS auxRefB = strBRef.GetComponent<BeatBondCS> ();
					auxRefB.NextBeat = master.NextBeat;
					auxRefB.Fitter = this;
					sigBRef = (GameObject)GameObject.Instantiate (beatSigns [molecule.Instructions [current].OptB - 1], rand == 0 ? signB : signA, false);

				} else {

					strARef = (GameObject)GameObject.Instantiate (beatBonds [molecule.Instructions [current].OptA - 1], streamA, false);
					BeatBondCS auxRef = strARef.GetComponent<BeatBondCS> ();
					auxRef.NextBeat = master.NextBeat;
					auxRef.Fitter = this;
					sigARef = (GameObject)GameObject.Instantiate (beatSigns [molecule.Instructions [current].OptA - 1], signA, false);

					squaresB.Block ();
				}
			} else {

				float tAux = master.NextBeat - Time.time + master.BeatTime * 3;

				GameObject.Destroy (moleculeRef, tAux);

				interMolecDelay = Time.time + tAux;

				delaying = true;

				score.IncreaseScore (500);

				master.NewMolecule ();
			}


		} else {
			
			if (squaresA.transform == beatBond.transform.parent.parent.GetChild (4)) {
				squaresA.ResetState ();
				squaresA.Block ();
			} else {
				squaresB.ResetState ();
				squaresB.Block ();
			}
			beatBond.transform.parent.parent.GetChild (0).gameObject.SetActive (true);

			score.DecreaseScore (100);

			GameObject.Destroy (beatBond.gameObject, master.NextBeat - Time.time);
		}
	}
}
