using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {
	public int heal_amount = 1;
	
    void OnTriggerEnter2D(Collider2D col){ 
		if(col.gameObject.CompareTag("Player")){
			if(col.gameObject.GetComponent<PlayerController>().health != 3){
				col.gameObject.GetComponent<PlayerController>().TakeDamage(heal_amount,new Vector2(0,0),true);
				Destroy(gameObject);
			}
		}
	}
}
