using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing : AI {
	public Transform proj_start;
	public GameObject projectile;
	private Animator animator;
	private float next_hit_time = 0.0f;
	private float hit_interval = 1.5f;
	public int projectile_lifetime = 5;

	void Start(){
		animator = gameObject.GetComponent<Animator>();
		foreach (Transform child in transform){
			if(child.name == "attack_sound"){
				attack_sound = child.GetComponent<AudioSource>();
			}
			if(child.name == "hit_sound"){
				hit_sound = child.GetComponent<AudioSource>();
			}
		}
	}

	public override void TakeDamage(int amount, Vector2 force, bool bHeal = false){
		//
	}

	void Update(){
		if(health == 0){
			return;
		}

		if(Time.time > next_hit_time){
			var proj = (GameObject)Instantiate(projectile, proj_start.position, Quaternion.identity, transform);
			proj.GetComponent<projectile>().lifetime = projectile_lifetime;
			attack_sound.Play();
			next_hit_time = Time.time + hit_interval;
		}
	}
}
