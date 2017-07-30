using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

   // Use this for initialization


   DamageController _damageController;
   public GameObject explosion;
   public GameObject _Muerte;
   public GameObject visual;
   SpriteRenderer _visual;
   PlayerController _PlayerController;
   public void Awake()
   {
      this._damageController = GetComponent<DamageController>();
      this._damageController.OnDead = this.OnDead;
     _visual  = GetComponent<SpriteRenderer>();


   }

       
	private void explosion_()
   {
         explosion.SetActive(true);

   }

  void OnDead()
   {


      GetComponent<BoxCollider2D>().enabled = false;
      Animator anim = _Muerte.GetComponent<Animator>();

      anim.enabled = false;
      
      _visual.enabled = !_visual.enabled;
      explosion_();
     
      Invoke("destruir",1);

   }

   void destruir()
   {

      Destroy(_Muerte);
      

   }

}
