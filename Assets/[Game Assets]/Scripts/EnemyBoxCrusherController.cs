using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoxCrusherController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if ((collision.contacts[0].normal == Vector2.up && Physics2D.gravity.normalized.y == -1) ||
                (collision.contacts[0].normal == Vector2.down && Physics2D.gravity.normalized.y == 1))
            {
                collision.gameObject.GetComponent<DamageController>().ApplyDamage(999);
            }
        }
    }
}
