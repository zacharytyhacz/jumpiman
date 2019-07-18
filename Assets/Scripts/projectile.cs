using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
	public int speed = 5;
	public int damage = 1;
	public int lifetime = 0;
	public Vector2 force = new Vector2(1000,1500);

	// Use this for initialization
	void Start () {
		if(lifetime != 0){
			Destroy(gameObject,lifetime);
			Destroy(this, lifetime);
		} 
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject != null){
			var x = Time.deltaTime * speed;  
			transform.Translate(x,0,0);
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(gameObject != null){
			if (coll.gameObject.CompareTag("Player")){
				force.x = (force.x * transform.parent.localScale.x * -1);
				coll.gameObject.GetComponent<PlayerController>().TakeDamage(damage,force);
				Destroy(gameObject);
				Destroy(this);
			}
		}
	}
}
