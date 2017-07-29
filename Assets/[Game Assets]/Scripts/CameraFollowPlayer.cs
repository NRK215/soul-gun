using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    PlayerController _player;
    Camera _mainCamera;
    float _cameraZ;
    Vector3 _finalOffSet;

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    float _offSet = 2.5f;

    private void Awake()
    {
        this._mainCamera = Camera.main;
        this._player = FindObjectOfType<PlayerController>();
        this._cameraZ = this._mainCamera.transform.position.z;
    }

    private void Update()
    {
        this._finalOffSet = this._player.transform.right * this._offSet;
        this._finalOffSet.z = this._cameraZ;

        Vector3 finalPosition = this._player.transform.position + this._finalOffSet;

        Vector3 currentPosition = this._mainCamera.transform.position;
        currentPosition.x = Mathf.Lerp(currentPosition.x, finalPosition.x, Time.deltaTime * this._speed);
        this._mainCamera.transform.position = currentPosition;
    }
}
