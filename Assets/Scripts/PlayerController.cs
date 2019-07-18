using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float PlayerSpeed = 10.0f;
    public float JumpForce = 600.0f;
    private bool grounded = false;
	public int health = 3;
	private float popup_time;

	// related objects 
    public Transform groundCheck;
	private Animator animator;
	public Transform HUD;
	public Transform Equipped;
	public Transform Inventory;
	public Transform HeroSprite;
	public Transform Sword_Object;
	public Transform deaths_score;
	public Transform popup_message; 
			
	private Sword my_sword;
    private bool is_jumping = false;
    private Rigidbody2D body;

	private bool swinging = false;
	public float swing_speed = 12.0f;
	private bool swing_back = false;

	// sounds
	private AudioSource jump_sound; 
	private AudioSource swing_sound; 
	private AudioSource pickup_sound;
	private AudioSource lose_sound;
	private AudioSource hurt_sound;
	private AudioSource win_sound;

    // Use this for initialization
    void Start()
    {
		jump_sound = GameObject.Find("jump").GetComponent<AudioSource>();
		pickup_sound = GameObject.Find("pickup").GetComponent<AudioSource>();
		swing_sound = GameObject.Find("swing_sound").GetComponent<AudioSource>();
		lose_sound = GameObject.Find("lose_sound").GetComponent<AudioSource>();
		win_sound = GameObject.Find("win_sound").GetComponent<AudioSource>();
		hurt_sound = GameObject.Find("hurt_sound").GetComponent<AudioSource>();

		my_sword = Sword_Object.GetComponent<Sword>();
        body = GetComponent<Rigidbody2D>();
		animator = GameObject.Find("Player/Sprite").GetComponent<Animator>();
        body.isKinematic = false;
		
		health = PlayerPrefs.GetInt("heath",3);
		if(health < 1){
			health = 1;
			PlayerPrefs.SetInt("health",1);
		}
		// this will display correct hearts
		set_lives();
		deaths_score.gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("deaths",0).ToString() + "x";
    }
	
	public void attack(){
		if(health <= 0){
			return;
		}
		//print("ATTACKING");
		if(!swinging){
			swing_sound.Play(0);
			swinging = true;
		}
	}
	
	public void restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
		Time.timeScale = 1;
	}
	 
	public void die(){
		if(Time.timeScale == 0){
			return;
		}
		health = 0;
		set_lives();
		GameObject.Find("music").GetComponent<AudioSource>().Stop();
		var screen = GameObject.Find("death screen");
		screen.transform.localScale = new Vector3(1,1,1);
		var deaths = PlayerPrefs.GetInt("deaths",0);
		PlayerPrefs.SetInt("deaths",++deaths);
		PlayerPrefs.Save();

		// pause 
		Time.timeScale = 0;
		lose_sound.Play(0);
	}
	
	public void win(){
		var screen = GameObject.Find("win screen");
		var level = int.Parse(SceneManager.GetActiveScene().name);
		
		// show screen
		if(level == 3){
			GameObject.Find("end_menu").gameObject.transform.localScale = new Vector3(1,1,1);
			GameObject.Find("continue_menu").gameObject.transform.localScale = new Vector3(0,0,0);
		}
		screen.transform.localScale = new Vector3(1,1,1);
		win_sound.Play();
		level++;

		// stop music 
		GameObject.Find("music").GetComponent<AudioSource>().Stop();
		
		// set next level as this current level + 1 
		PlayerPrefs.SetString("next_level",level.ToString());
		
		// save health so increase difficulty
		PlayerPrefs.SetInt("health",health);
		PlayerPrefs.Save();

		print("saved next level as " + level );

		// pause 
		Time.timeScale = 0;
	}

	public void next(){
		var level = PlayerPrefs.GetString("next_level","1");
		if(level == "4"){
			PlayerPrefs.SetString("next_level","1");
			level = "1";
		}
		print("LOADING LEVEL " + level);
		SceneManager.LoadScene(level, LoadSceneMode.Single);
		Time.timeScale = 1;
	}

    // Update is called once per frame
    void Update()
    {
		if(Time.time > popup_time + 5.0f && popup_message.localScale.x == 1.0f){
			popup_message.localScale = new Vector3(0,0,0);
		}
        var x = CrossPlatformInputManager.GetAxis("Horizontal")* Time.deltaTime * PlayerSpeed;
		if(x == 0.0f){
			x = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerSpeed;
		}
        var y = CrossPlatformInputManager.GetAxis("Vertical");
		if(y == 0.0f){
			y = Input.GetAxis("Vertical") * 10;
		}

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if(!grounded){
			// check if on top of an enemy
        	grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Enemy"));
		}

        transform.Translate(x, 0, 0);
		animator.SetFloat("speed",Mathf.Abs(x));
		if(!swinging){
			if(x > 0.0f){
				if(HeroSprite.localScale.x != 2.0f){
					HeroSprite.localScale = new Vector3(2,2,2);
				}
			} else if (x < 0.0f && HeroSprite.localScale.x != (-2.0f)){
				HeroSprite.localScale = new Vector3(-2,2,2);
			}
		}


		if(Input.GetKeyDown(KeyCode.Space)){
			attack();
		}
	
		//print(Equipped.transform.eulerAngles.z);
		// attack swing 
		if(swinging){
			if(HeroSprite.localScale.x == 2.0f){
				if(!swing_back){
					Equipped.transform.Rotate(0,0,0-swing_speed,Space.Self);
					//print(Equipped.transform.eulerAngles.z);
					if(Equipped.transform.eulerAngles.z <= 240){
						//print("SWINGING BACK");
						swing_back = true;
					}
				}else{
					if(Equipped.transform.eulerAngles.z == (360 - swing_speed )){
						Equipped.transform.eulerAngles = new Vector3(0,0,0);
						swing_back = false;
						swinging = false;
					} else {
						Equipped.transform.Rotate(0,0,(0+swing_speed ),Space.Self);
					}
				}
			}
			else if (HeroSprite.localScale.x == -2.0f){
				if(!swing_back){
					Equipped.transform.Rotate(0,0,0-swing_speed,Space.Self);
					if(Equipped.transform.eulerAngles.z >= 120){
						swing_back = true;
					}
				}else{
					if(Equipped.transform.eulerAngles.z == (360 - swing_speed )){
						Equipped.transform.eulerAngles = new Vector3(0,0,0);
						swing_back = false;
						swinging = false;
					} else {
						Equipped.transform.Rotate(0,0,(0+swing_speed ),Space.Self);
					}
				}
			
			}
		}
		my_sword.is_swinging(swinging);
    }

	public void jump(){
		if(animator.GetBool("jumping") && grounded){
			animator.SetBool("jumping",false);
		} else if(!animator.GetBool("jumping") && !grounded){
			animator.SetBool("jumping",true);
		}
		
        if (grounded){ 
			jump_sound.Play(0);
            body.AddForce(new Vector2(0f, JumpForce));
        } 
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
		// prevent own weapon/damage from hurting myself
		if(coll.transform.parent == Equipped){
			return;
		}

		// picking up a key 
        if(coll.gameObject.CompareTag("Key")){
			pickup_sound.Play(0);
            Inventory.GetComponent<Inventory>().addItem(coll.gameObject);
			return;
        }

		// entering a door 
        if (coll.gameObject.CompareTag("Door")){
			if(!coll.gameObject.GetComponent<door>().bUnlocked){
				string need_key = coll.gameObject.GetComponent<door>().keyName;
				
				if(Inventory.GetComponent<Inventory>().tryKey(need_key) == false){
					popup_message.localScale = new Vector3(1,1,1);
					popup_time = Time.time;

					// locked!
					return;
				}
					
				coll.gameObject.GetComponent<door>().unlock();
			}

			win();

			return;
        }
		
		// Picking up a sword
		if(coll.gameObject.CompareTag("Sword")){
			if(swinging){
				return;
			}
			if(coll.gameObject.GetComponent<Sword>().damage <= my_sword.damage){
				return;
			} 
			coll.transform.parent = Equipped;
			coll.transform.localPosition = new Vector3(0, 0.2f, 0);
			return;
		}
    }
	
	public void TakeDamage(int amount, Vector2 force, bool bHeal = false){
		if(health == 0){
			return;
		}

		// Damage
		if(amount > 0 && !bHeal){
			body.AddForce(force);
		} else {
			return;
		}
		
		if(!bHeal){
			hurt_sound.Play();
			health -= amount;
		}else{
			health += amount;
		}
		
		if(health == 0){
			// die	
			die();
	 	}
		set_lives();
	}

	private void set_lives(){
		var i = 1;
		for(i = 1;i<4;i++){
			var heart = GameObject.Find("health/" + i.ToString()).transform;
			if(!heart){
				continue;
			}
			if(i <= health){
				heart.localScale = new Vector3(1,1,1);
			}
			else {
				heart.localScale = new Vector3(0,0,0);
			}
		} 
	}
}
