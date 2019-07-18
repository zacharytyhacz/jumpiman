using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eye_monster : AI {
	public Transform proj_start;
	public GameObject projectile;
	private float next_hit_time = 0.0f;
	private float hit_interval = 1.5f;
	public int projectile_lifetime = 5;
	private bool bHasTarget = false;
	private float next_change_dir_time = 0.0f;
	public float change_dir_interval = 2.0f;
	private Vector3 velocity = new Vector3(-1.2f,-1.9f,0);
	//public int speed = 3;
	private Transform maxHeight;
	private Transform minHeight;
	//public Rigidbody2D body;

	void Start(){
		foreach (Transform child in transform){
			if(child.name == "attack_sound"){
				attack_sound = child.GetComponent<AudioSource>();
			}
			if(child.name == "hit_sound"){
				hit_sound = child.GetComponent<AudioSource>();
			}
			if(child.name == "maxHeight"){
				maxHeight = child;
			}
			if(child.name == "minHeight"){
				minHeight = child;
			}
		}

		body = GetComponent<Rigidbody2D>();
	}

	public override void TakeDamage(int amount, Vector2 force, bool bHeal = false){
		base.TakeDamage(amount,force,bHeal);
	}


	void Update(){
		if(health == 0){
			return;
		}

		// move around toward player 
		if(Vector3.Distance(transform.position, GameObject.Find("Player").gameObject.transform.position) < 8.0f){
			if(!bHasTarget){
				//print("got target");
				bHasTarget = true;
			}
		} else if(bHasTarget){
			// player too far away
			//print("player too far away");
			transform.position = Vector3.MoveTowards(transform.position,GameObject.Find("Player").gameObject.transform.position, 2.0f * speed * Time.deltaTime);
			return;
		} else {
			return;
		}

		// check if too high 
		if(transform.position.y > (GameObject.Find("Player").gameObject.transform.position.y + 8.0f)){
			velocity.y = Random.Range(-3.9f,-1.9f);
			next_change_dir_time = Time.time + change_dir_interval;
			//print("i am too high");
			
		// check if too low 
		}else if(transform.position.y < (GameObject.Find("Player").gameObject.transform.position.y  + 1.5f)){
			velocity.y = Random.Range(0.9f,2.5f);
			next_change_dir_time = Time.time + change_dir_interval;
			//print("i am too low");

		// check if ready to change directions
		} else if(Time.time > next_change_dir_time){
			velocity = new Vector3(Random.Range(-3.0f,3.0f), Random.Range(-2.0f,2.0f),0);
			next_change_dir_time = Time.time + change_dir_interval;
		}
		transform.Translate(velocity * speed * Time.deltaTime);
		transform.localScale = new Vector3((3 * (transform.position.x > GameObject.FindWithTag("Player").gameObject.transform.position.x ? 1 : -1)),3,3);

		if(bHasTarget && Time.time > next_hit_time){
			var proj = (GameObject)Instantiate(projectile, proj_start.position, Quaternion.identity, transform.parent);
			//var proj = (GameObject)Instantiate(projectile, transform, true);
			proj.GetComponent<eyeball>().lifetime = projectile_lifetime;
			attack_sound.Play();
			next_hit_time = Time.time + hit_interval;
		}
	}
}
