using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeball : MonoBehaviour {
	public int speed = 5;
	public int damage = 1;
	public int lifetime = 0;
	private Vector3 player_loc;
	public Vector2 force = new Vector2(0,3000);
	public Vector3 direction; 

	// Use this for initialization
	void Start() {
		player_loc = GameObject.Find("Player").gameObject.transform.position;
		direction = (player_loc - transform.position).normalized;

		if(lifetime != 0){
			Destroy(gameObject,lifetime);
			Destroy(this, lifetime);
		} 
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(gameObject != null){
			if (coll.gameObject.CompareTag("Player")){
				coll.gameObject.GetComponent<PlayerController>().TakeDamage(damage,force);
				Destroy(gameObject);
				Destroy(this);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject != null && lifetime != 0){
			transform.position += direction * speed *Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position, player_loc, speed * Time.deltaTime);
			//transform.Translate(velocity * speed * Time.deltaTime);
		}
	}

}
