using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

   // Use this for initialization


   DamageController _damageController;
   public GameObject explosion;
   public PlayerController _PlayerController;
   public GameObject _Spawn; 
   public void Awake()
   {
      this._damageController = GetComponent<DamageController>();
      this._damageController.OnDead = this.OnDead;
    


   }

       
	private void explosion_()
   {
         explosion.SetActive(true);

   }

  void OnDead()
   {
      _PlayerController.ReSpawn.transform.position = _Spawn.transform.position;

      GetComponent<BoxCollider2D>().enabled = false;

      explosion_();
     
 

   }

  

}
