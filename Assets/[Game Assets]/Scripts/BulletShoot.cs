using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX3.Framework;
using EX3.Framework.Components;

public class BulletShoot : EnemyDamageController
{
    InstantiableObject _instantiableObjectController;

    private void Awake()
    {
        this._instantiableObjectController = GetComponent<InstantiableObject>();
        this.OnCollision += this.OnCollisionEvent;
    }

    private void OnCollisionEvent(Collision2D collision)
    {
        this._instantiableObjectController.Dispose();
    }
}
