using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneCS : MonoBehaviour {

	public void LoadTutorial() {
		SceneManager.LoadScene ("Tutorial");
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene ("Main Menu");
	}

	public void LoadFreeMode(){
		SceneManager.LoadScene ("Free Mode");
	}

	public void RestartScene (){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void Fechar () {
		Application.Quit ();
	}
}
