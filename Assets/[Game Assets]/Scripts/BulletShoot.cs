using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX3.Framework;
using EX3.Framework.Components;
using UnityEngine.Events;

public class BulletShoot : EnemyDamageController
{
    InstantiableObject _instantiableObjectController;

    public UnityEvent OnBulletCollisionEvent;

    private void Awake()
    {
        this._instantiableObjectController = GetComponent<InstantiableObject>();
        this.OnCollision += this.OnCollisionEvent;
    }

    private void OnCollisionEvent(Collision2D collision)
    {
        this.OnBulletCollisionEvent.Invoke();
        this._instantiableObjectController.Dispose();
    }
}
