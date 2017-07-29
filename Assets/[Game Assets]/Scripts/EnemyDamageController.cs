using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX3.Framework;
using EX3.Framework.Components;

public class EnemyDamageController : MonoBehaviour
{
    [SerializeField]
    int _damage;
    [SerializeField]
    string _targetTag;
    [SerializeField]
    string[] _ignoreTags;
    [SerializeField]
    bool _ignoreUntagged = false;

    public delegate void OnCollisionEnterHandler(Collision2D collision);

    public OnCollisionEnterHandler OnCollision { get; set; }

    public void SetParams(string targetTag, int damage, params string[] ignoreTags)
    {
        this._targetTag = targetTag;
        this._damage = damage;
        this._ignoreTags = ignoreTags;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(this._targetTag))
        {
            collision.gameObject.GetComponent<DamageController>()?.ApplyDamage(this._damage);
        }

        if (!this.CheckIgnoreTags(collision))
        {
            this.OnCollision?.Invoke(collision);
        }
    }

    bool CheckIgnoreTags(Collision2D collision)
    {
        for (int i = 0; i < this._ignoreTags.Length; i++)
        {
            if (collision.gameObject.CompareTag(this._ignoreTags[i]))
            {
                return true;
            }
        }

        return this._ignoreUntagged && collision.gameObject.CompareTag("Untagged");
    }
}