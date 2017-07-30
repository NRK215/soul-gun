using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageController : MonoBehaviour
{
    [SerializeField]
    int _health = 1;

    public int MaxHealth { get { return this._health; } set { this._health = value; } }
    public int CurrentHealth { get; set; }

    public System.Action<int> OnDamageReceived;
    public System.Action OnDead;

    public UnityEvent<int> OnDamageReceivedEvent;
    public UnityEvent OnDeadEvent;

    private void Awake()
    {
        this.CurrentHealth = this._health;
    }

    public void ApplyDamage(int damage)
    {
        this.CurrentHealth = (int)Mathf.Clamp(this.CurrentHealth - damage, 0f, this._health);

        this.OnDamageReceivedEvent?.Invoke(damage);
        this.OnDamageReceived?.Invoke(damage);

        if (this.CurrentHealth == 0)
        {
            this.OnDeadEvent?.Invoke();
            this.OnDead?.Invoke();
        }
    }
}
