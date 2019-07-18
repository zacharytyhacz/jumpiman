using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
	// knows where player is at all times 
	public bool bOmniscient = false;
	public Transform player_loc;

	// damage to inflict
	public int damage = 1;

	// force in x and y directions on inflicting damage 
	public Vector2 force = new Vector2(0,0);

	public int health = 2;

	// time to stay active 
	// default e.g. = after died, stay around for 5 more seconds;
	public float post_death_time = 5.0f;

	// movement speed
	public float speed = 25.0f;
	public bool bStationary = false;

	// max distance to see player
	public float vision_distance = 1000.0f;

	// jump height if can jump
	protected float jumpZ = 100.0f;
	public bool bCanJump = false;

	// detects ground/edge
	protected Transform edgeCheck;

	// detects whats in front of me 
	protected Transform frontCheck;

	// next time in seconds this AI can attack
	protected float next_hit_time;

	// interval to give damage 
	public float hit_interval = 1.5f;

	// if on ground
	protected bool grounded = true;

	// this AI's collision body 
	protected Rigidbody2D body;

	// look direction 
	protected bool look_right = false;

	//======= sounds =========//
	protected AudioSource attack_sound;
	protected AudioSource hit_sound;
	
	void Start () {
		//player_loc = GameObject.FindWithTag("Player").gameObject.transform;
		var rand = Random.Range(0,2);
		if(rand == 0){
			flip();
		}
		foreach (Transform child in transform){
			if(child.name == "edgeCheck"){
				edgeCheck = child;
			}
			if(child.name == "frontCheck"){
				frontCheck = child;
			}
			if(child.name == "attack_sound"){
				attack_sound = child.GetComponent<AudioSource>();
			}
			if(child.name == "hit_sound"){
				hit_sound = child.GetComponent<AudioSource>();
			}
		}
		body = GetComponent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D coll){
	}

	// change directions 
	protected void flip(){
		look_right = !look_right;
		if(transform.localScale.x == -2.0f){
			transform.localScale = new Vector3(2,2,2);
		}else {
			transform.localScale = new Vector3(-2,2,2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(health == 0){
			return;
		}
		// detect if player is in front 
		if(Time.time > next_hit_time){
			var bHitPlayer = Physics2D.Linecast(transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Player"));
			if(bHitPlayer){
				print("I AM HITTING THE PLAYER");
				attack_sound.Play();
				// make sure horizontal force is same direction as what this AI is facing
				force.x = force.x * (transform.localScale.x / Mathf.Abs(transform.localScale.x));
				GameObject.Find("Player").GetComponent<PlayerController>().TakeDamage(damage,force);
				next_hit_time = Time.time + hit_interval;
				flip();
			}
		}
		if(!bStationary){
			// detects edge
        	grounded = Physics2D.Linecast(transform.position, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground"));
			if(!grounded){
				flip();
			}else if (Physics2D.Linecast(transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Ground"))){
				// detect whats in front 
				flip();
			}
			var x = Time.deltaTime * speed;
			if(look_right){
				x *= -1;
			}

			transform.Translate(x, 0, 0);
		}
	}
	
	public virtual void die(){
		transform.Rotate(0,0,180, Space.Self);
		GetComponent<BoxCollider2D>().isTrigger = true;
		if(body){
			body.constraints = RigidbodyConstraints2D.None;
		}
		Destroy(gameObject,post_death_time);
	}

	public virtual void TakeDamage(int amount, Vector2 force, bool bHeal = false){
		if(amount <= 0 || health == 0){
			return;
		}
		
		if(!bHeal){
			health -= amount;
			body.AddForce(force);
			hit_sound.Play();
		}else{
			health += amount;
		}

		if(health == 0){
			// die;
			die();
		}
		
		print("ENEMY TOOK DAMAGE");
	}
}
