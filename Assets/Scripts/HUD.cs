using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

	private bool bInventory = false;
	private bool bMenu 		= false;
	
	private PlayerController 	Player;
	private Inventory 			PlayerInventory;
	
	// Use this for initialization
	void Start (){
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}
	
	public void test(){
	}
	
	
	public void openInventory(){
		var inv = GameObject.Find("Inventory");
		
		if(!bInventory){
			var i = 0;
			
			foreach(GameObject item in PlayerInventory.getSwords()){
				if(item != null){
					var new_item = Instantiate(GameObject.Find("Item"));
					
					// move down
					var offset = new_item.transform.position;
					offset.y -= 130.0f * i;
					new_item.transform.position = offset;
					i++;
				}
			}
			
			bInventory = true;
			inv.transform.localScale += new Vector3(1,1,1);
			Debug.Log("opening inventory");
		} else {
			bInventory = false;
			Debug.Log("Closing Inventory");
			inv.transform.localScale -= new Vector3(1,1,1);
		}
		
		inv.SetActive(bInventory);
	}
	
	public void closeInventory(){
		GameObject.Find("Inventory").SetActive(false);
		bInventory = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
