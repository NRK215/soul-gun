using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rigidBody;
    Vector2 _initialGravity;
    float _playerGravity;
    float _targetGravity;

    [SerializeField]
    float _horizontalForce = 1f;

    private void Awake()
    {
        this._rigidBody = GetComponent<Rigidbody2D>();
        this._initialGravity = Physics2D.gravity;
    }

    private void Update()
    {
        this._rigidBody.position += ((Vector2.right * Input.GetAxis("Horizontal")) * this._horizontalForce) * Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            Physics2D.gravity = -this._initialGravity;
            this._targetGravity = this._initialGravity.y / 3f;
        }
        else
        {
            Physics2D.gravity = this._initialGravity;
            this._targetGravity = -this._initialGravity.y / 3f;
        }

        if (this._rigidBody.position.y > 0)
        {
            this._targetGravity = -this._initialGravity.y / 3f;
        }

        this._targetGravity = Mathf.Lerp(this._targetGravity, this._playerGravity, Time.deltaTime);
        print(this._targetGravity);
    }

    private void FixedUpdate()
    {
        this._rigidBody.velocity += Vector2.down * this._targetGravity;
    }
}
