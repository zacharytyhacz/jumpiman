using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	public string 	swordName = "Kindness";
	public int 		damage = 1;
	public Vector2 	force = new Vector2(2400,3000);
	private bool	allow_hit;
	
    void OnTriggerEnter2D(Collider2D col){ 
		if(!allow_hit){
			return;
		}
		if(col.gameObject.CompareTag("Enemy")){
			force.x = (force.x * (GameObject.Find("Player/Sprite").gameObject.transform.localScale.x / 2));
			col.gameObject.GetComponent<AI>().TakeDamage(damage,force);
		}
	}
	public void is_swinging(bool bswinging){
		if(bswinging != allow_hit){
			allow_hit = bswinging;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
