using UnityEngine;
using System.Collections.Generic;

public class InstructionCS {

	public int OptA { get { return optA; } }
	public int OptB { get { return optB; } }

	private int optA, optB, ansA, ansB;
	
	public InstructionCS (List<int> array) {

		this.optA = array [0];
		this.optB = array [1];
		this.ansA = array [2];
		this.ansB = array [3];
	}

	public int getAnswer (int option) {
		return option == optA ? ansA : ansB;
	}
}
