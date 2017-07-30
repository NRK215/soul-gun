using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBalas : MonoBehaviour {

	// Use this for initialization
	void Start () {

      Invoke("muerte", 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   public void OnCollisionEnter2D(Collision2D collision)
   {
      Invoke("muerte", 2);
      GetComponent<SpriteRenderer>().enabled = false;

   }


   void muerte()
   {

      Destroy(this.gameObject);

   }

}
