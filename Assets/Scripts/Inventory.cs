using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public int max_swords = 3;
	public int max_keys = 3;
	public GameObject equipped;
	
	public bool addItem(GameObject new_item){
		if(new_item.tag == "Key" || new_item.tag == "Sword"){
			var rb = (new_item.gameObject.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D);
			
			rb.simulated = false;
			new_item.transform.parent = transform;
			new_item.transform.localPosition = new Vector3(0, 0, 0);
			
			return true;
		}
		
		return false;
	}
	
	public GameObject[] getItems(){
        var items = new GameObject[(max_swords + max_keys)];
		var i = 0;
		
		foreach(GameObject child in transform){
			items[i] = child;
			i++;
		}
		
		return items;
	}
	
	public GameObject[] getSwords(){
        var items = new GameObject[max_swords];
		var i = 0;
		
		foreach(GameObject child in transform){
			if(child.tag == "Sword"){
				i++;
				items[i] = child;
			}
			
		}
		
		return items;
	}
	
	
	
	public bool drop(string name){
		return false;
	}
	
	public bool tryKey(string name){
		print("I need " + name);
		foreach(Transform child in transform){
			if(child.tag == "Key"){
				print("I have " + child.GetComponent<Key>().keyName);
				if(child.GetComponent<Key>().keyName == name){
					return true;
				}
			}
		}
		print("I do not have " + name);
		return false;
	}
	
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		//
	}
}
