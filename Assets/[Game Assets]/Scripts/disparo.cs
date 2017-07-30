using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disparo : MonoBehaviour
{


   public GameObject bala;
   public GameObject target;
   public Transform salida;
   public float bulletSpeed = 10;
   public float time_ = 2;
   // Use this for initialization
   IEnumerator Start()
   {
      while (true)
      {
         yield return new WaitForSeconds(time_);
         var bullet = Instantiate(bala, salida.position, salida.rotation);
         bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.right * bulletSpeed, ForceMode2D.Impulse);
      }

   }

   private void Update()
   {
      LookAtPlayer();
   }

   void LookAtPlayer()
   {
      this.transform.right = (target.transform.position - this.transform.position).normalized;

   }
      
}



