using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public struct ActionInput
    {
        public KeyCode DefaultKey;
        public KeyCode AltKey;

        public bool IsPressed { get { return UnityEngine.Input.GetKey(this.DefaultKey) || UnityEngine.Input.GetKey(this.AltKey); } }
        public bool IsDown { get { return UnityEngine.Input.GetKeyDown(this.DefaultKey) || UnityEngine.Input.GetKeyDown(this.AltKey); } }
        public bool IsUp { get { return UnityEngine.Input.GetKeyUp(this.DefaultKey) || UnityEngine.Input.GetKeyUp(this.AltKey); } }
    }

    [System.Serializable]
    public struct InputMap
    {
        public ActionInput MoveLeft;
        public ActionInput MoveRight;
        public ActionInput Shoot;

        public void SetDefault()
        {
            this.MoveLeft = new ActionInput() { DefaultKey = KeyCode.A, AltKey = KeyCode.LeftArrow };
            this.MoveRight = new ActionInput() { DefaultKey = KeyCode.D, AltKey = KeyCode.RightArrow };
            this.Shoot = new ActionInput() { DefaultKey = KeyCode.Space, AltKey = KeyCode.LeftControl };
        }
    }

    public InputMap Input;

    private void Reset()
    {
        this.Input.SetDefault();
    }

    private void Awake()
    {
        GameManager.Instance = this;
    }
}
