using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.IO;

public class MoleculeDataBaseCS : MonoBehaviour {

	public TextAsset moleculeDatabaseFile;

	public List<MoleculeDataCS> tutorialMolecules;
	public List<MoleculeDataCS> molecules;

	void Awake () {
		tutorialMolecules = new List<MoleculeDataCS>();
		molecules = new List<MoleculeDataCS>();
		ConstructDataBase ();
	}

	private void ConstructDataBase () {
		XmlDocument xmlDocument = new XmlDocument ();
		xmlDocument.LoadXml (moleculeDatabaseFile.text);
		XmlNodeList itemList = xmlDocument.GetElementsByTagName ("Molecule");

		foreach (XmlNode itemInfo in itemList) {
			
			XmlNodeList itemContent = itemInfo.ChildNodes;

			int difficulty = -1;
			List<InstructionCS> instrList = new List<InstructionCS> ();
			string name = "", prefabName = "";

			foreach (XmlNode content in itemContent) {

				switch (content.Name) {
				case "Name" :
					name = content.InnerText;
					break;
				case "Difficulty" :
					difficulty = int.Parse(content.InnerText);
					break;
				case "Instructions":
					XmlNodeList subContent = content.ChildNodes;

					foreach (XmlNode instrNode in subContent) {
						List<int> intList = new List<int> ();
						XmlNodeList subSubContent = instrNode.ChildNodes;

						foreach (XmlNode instrInfo in subSubContent) {
							intList.Add (int.Parse (instrInfo.InnerText));
						}

						instrList.Add(new InstructionCS(intList));
					}
					break;
				case "Prefab" :
					prefabName = content.InnerText;
					break;
				}
			}

			switch (difficulty) {
			case 0:
				tutorialMolecules.Add (new MoleculeDataCS (name, prefabName, instrList));
				break;
			case 1:
				molecules.Add (new MoleculeDataCS (name, prefabName, instrList));
				break;
			default:
				Debug.Log ("Problema no XML!");
				break;
			}
		}
	}
}
