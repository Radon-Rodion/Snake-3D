using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesSwitcher : MonoBehaviour
{
    public void startGame(){
		SceneManager.LoadScene(1);
	}
	
	public void openMenu(){
		SceneManager.LoadScene(0);
	}
	
	public void gameOver(){
		SceneManager.LoadScene(2);
	}
}
