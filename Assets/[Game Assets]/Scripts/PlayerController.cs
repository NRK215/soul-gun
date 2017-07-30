using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX3.Framework;
using EX3.Framework.Components;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    readonly Quaternion _leftDirection = Quaternion.Euler(Vector2.up * 180f);
    readonly Quaternion _rightDirection = Quaternion.Euler(Vector2.up);

    Rigidbody2D _rigidBody;
    SpriteRenderer _sprite;
    Animator _animator;
    ObjectPool _shootsPool;
    DamageController _damageController;
    Timer _timer;
    CameraFollowPlayer _cameraFollowPlayer;

    float _horizontalVelocity;

    [SerializeField]
    float _horizontalForce = 1f;
    [Space]
    [Header("Weapon settings:")]
    [SerializeField]
    float _shootCandence = 0.5f;
    [SerializeField]
    float _shootSpeed = 1f;
    [SerializeField]
    int _shootDamage = 1;
    [SerializeField]
    int _maxShoots = 3;
    [SerializeField]
    float _shootLifeTime = 1f;
    [SerializeField]
    Transform _sourceShoot;

    public Transform ReSpawn;
    public GameObject particulas;

    public UnityEvent OnDeadEvent;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        this._rigidBody = GetComponent<Rigidbody2D>();
        this._sprite = GetComponent<SpriteRenderer>();
        this._animator = GetComponent<Animator>();
        this._shootsPool = GetComponentInChildren<ObjectPool>();
        this._damageController = GetComponent<DamageController>();

        this._timer = new Timer();

        this._shootsPool.MaxInstances = this._maxShoots;
        this._damageController.OnDead = this.OnDead;
    }

    private void Update()
    {
        this.UpdateMovement();
        this.CheckInputShoot();
    }

    private void FixedUpdate()
    {
        Vector2 currentVelocity = this._rigidBody.velocity;
        currentVelocity.x = this._horizontalVelocity;
        this._rigidBody.velocity = currentVelocity;
    }

    void UpdateMovement()
    {
        if (this.IsDead) return;

        if (GameManager.Instance.Input.MoveLeft.IsPressed)
        {
            this._horizontalVelocity = -this._horizontalForce;
            this.transform.rotation = this._leftDirection;
            this._animator.SetBool("Run", true);
        }
        else if (GameManager.Instance.Input.MoveRight.IsPressed)
        {
            this._horizontalVelocity = this._horizontalForce;
            this.transform.rotation = this._rightDirection;
            this._animator.SetBool("Run", true);
        }
        else
        {
            this._horizontalVelocity = 0f;
            this._animator.SetBool("Run", false);
        }
    }

    void CheckInputShoot()
    {
        if (this.IsDead) return;

        if (GameManager.Instance.Input.Shoot.IsDown)
        {
            Physics2D.gravity *= -1;
            this._sprite.flipY = !this._sprite.flipY;

            if (this._timer.Value >= this._shootCandence)
            {
                this._timer.Reset();
                var shoot = this._shootsPool.GetNewInstance(this._sourceShoot.position, this._sourceShoot.rotation, this._shootLifeTime);
                shoot?.GetComponent<BulletShoot>().SetParams("Enemy", this._shootDamage, "Player", "Shoot");
                var rigidBody = shoot.GetComponent<Rigidbody2D>();
                if (rigidBody)
                {
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(this._sourceShoot.right * this._shootSpeed, ForceMode2D.Impulse);

                    this._damageController.ApplyDamage(1);
                }
            }
        }
    }

    void OnDead()
    {
        this.IsDead = true;
        this.OnDeadEvent?.Invoke();

        Invoke("Reaparecer", 2);
    }

    public void Reaparecer()
    {
        this.transform.position = this.ReSpawn.position;
        _sprite.enabled = true;
        this.particulas.SetActive(false);
        this.IsDead = false;
        this._damageController.CurrentHealth = this._damageController.MaxHealth;
    }
}
