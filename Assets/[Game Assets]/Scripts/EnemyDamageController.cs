using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX3.Framework;
using EX3.Framework.Components;
using UnityEngine.Events;

public class EnemyDamageController : MonoBehaviour
{
    [SerializeField]
    int _damage = 1;
    [SerializeField]
    string _targetTag = "Player";
    [SerializeField]
    string[] _ignoreTags;
    [SerializeField]
    bool _ignoreUntagged = false;
    [SerializeField]
    bool _ignoreSameTag = true;

    public UnityEvent OnCollisionEvent;

    public delegate void OnCollisionEnterHandler(Collision2D collision);

    public OnCollisionEnterHandler OnCollision { get; set; }

    private void Awake()
    {
        this._ignoreTags = new string[System.Convert.ToInt32(this._ignoreUntagged) + System.Convert.ToInt32(this._ignoreSameTag)];

        int index = 0;
        if (this._ignoreUntagged)
        {
            this._ignoreTags[index] = "Untagged";
            index++;
        }

        if (this._ignoreSameTag)
        {
            this._ignoreTags[index] = this.tag;
        }
    }

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
            collision.gameObject.GetComponent<DamageController>()?.ApplyDamage(999);
        }

        if (!this.CheckIgnoreTags(collision))
        {
            this.OnCollisionEvent?.Invoke();
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
        return false;
    }
}