using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rigidBody;
    SpriteRenderer _sprite;
    float _horizontalVelocity;

    [SerializeField]
    float _horizontalForce = 1f;

    private void Awake()
    {
        this._rigidBody = GetComponent<Rigidbody2D>();
        this._sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.Instance.Input.Shoot.IsDown)
        {
            Physics2D.gravity *= -1;
            this._sprite.flipY = !this._sprite.flipY;
        }

        if (GameManager.Instance.Input.MoveLeft.IsPressed)
        {
            this._horizontalVelocity = -this._horizontalForce;
            this._sprite.flipX = true;
        }
        else if (GameManager.Instance.Input.MoveRight.IsPressed)
        {
            this._horizontalVelocity = this._horizontalForce;
            this._sprite.flipX = false;
        }
        else
        {
            this._horizontalVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 currentVelocity = this._rigidBody.velocity;
        currentVelocity.x = this._horizontalVelocity;
        this._rigidBody.velocity = currentVelocity;
    }
}
