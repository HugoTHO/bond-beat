using UnityEngine;
using System.Collections.Generic;

public class MoleculeDataCS {

	public string Name { get { return name; } }
	public GameObject Prefab { get { return prefab; } }
	public List<InstructionCS> Instructions { get { return instructions; } }

	private string name;
	GameObject prefab;
	private List<InstructionCS> instructions;

	public MoleculeDataCS (string name, string prefab, List<InstructionCS> instructions ) {
		this.name = name;
		try {
			this.prefab = Resources.Load<GameObject> ("Molecules/" + prefab);
		}
		catch (KeyNotFoundException) {
			Debug.Log ("Loading de " + prefab + " falhou!");
			this.prefab = null;
		}
		this.instructions = instructions;
	}

}
