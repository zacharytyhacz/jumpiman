using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void play(){
		// get last level played
		// load that level
		var level = PlayerPrefs.GetString("next_level","1");
		SceneManager.LoadScene(level, LoadSceneMode.Single);
		print("load " + level);
	}
}
