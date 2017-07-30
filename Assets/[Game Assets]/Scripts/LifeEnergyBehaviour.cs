using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EX3.Framework;
using EX3.Framework.Components;

[RequireComponent(typeof(DamageController))]
public class LifeEnergyBehaviour : MonoBehaviour
{
    DamageController _damageController;

    Timer _timer;
    bool _isCoroutineRunning = false;

    Coroutine _delayBeforeRechargeCoroutine;

    [SerializeField]
    int _unitsPerShoot = 1;
    [SerializeField]
    int _maxEnergy = 10;
    [SerializeField]
    int _rechargeUnitsPerSecond = 1;
    [SerializeField]
    float _delayBeforeRecharge = 1f;
    [SerializeField]
    Event _OnCollisionEvent;

    private void Awake()
    {
        this._damageController = GetComponent<DamageController>();
        this._damageController.MaxHealth = this._damageController.CurrentHealth = this._maxEnergy;

        this._timer = new Timer();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (this._damageController.CurrentHealth < this._damageController.MaxHealth)
        {
            if ((collision.contacts[0].normal == Vector2.up && Physics2D.gravity.normalized.y == -1) ||
                (collision.contacts[0].normal == Vector2.down && Physics2D.gravity.normalized.y == 1))
            {
                if (this._timer.Value >= this._delayBeforeRecharge && !this._isCoroutineRunning)
                {
                    StartCoroutine(this.RechargueEnergyCoroutine());
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this._timer.Reset();
    }

    IEnumerator RechargueEnergyCoroutine()
    {
        var wait = new WaitForSeconds(1f);

        this._isCoroutineRunning = true;

        yield return new WaitForSeconds(this._delayBeforeRecharge);

        while (this._damageController.CurrentHealth < this._maxEnergy)
        {
            this._damageController.ApplyDamage(-1);
            yield return wait;
        }

        this._isCoroutineRunning = false;
    }
}
