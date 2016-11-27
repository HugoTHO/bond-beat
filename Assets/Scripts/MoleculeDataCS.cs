using UnityEngine;
using System.Collections.Generic;

public class MoleculeDataCS {

	public string Name { get { return name; } }
	public string Prefab { get { return prefab; } }
	public List<InstructionCS> Instructions { get { return instructions; } }

	private string name, prefab;
	private List<InstructionCS> instructions;

	public MoleculeDataCS (string name, string prefab, List<InstructionCS> instructions ) {
		this.name = name;
		this.prefab = prefab;
		this.instructions = instructions;
	}

}
