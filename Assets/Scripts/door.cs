using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour {

	public string nextScene = "SampleScene";
	
	// the key required to 'unlock' this door 
	public string keyName = "Iron";
	public bool bUnlocked = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void unlock(){
		if(!bUnlocked){
			// play sound ?
			bUnlocked = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D coll)
    {

    }
}
