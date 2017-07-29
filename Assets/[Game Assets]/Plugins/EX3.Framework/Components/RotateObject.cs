using UnityEngine;
using System.Collections;

namespace EX3.Framework.Components
{
    [AddComponentMenu("[EX3] Framework/Utils/Rotate Object")]
    public class RotateObject : MonoBehaviour
    {
        #region Enums
        public enum RotationAxis
        {
            Right,
            Up,
            Forward
        }
        #endregion

        #region Public vars
        public float Speed = 1f;
        public RotationAxis Axis = RotationAxis.Up;
        public bool InLocalCoordinates = false;
        #endregion

        #region Update logic
        void Update()
        {
            Vector3 axis = Vector3.zero;
            switch (this.Axis)
            {
                case RotationAxis.Right:
                    axis = Vector3.right;
                    break;
                case RotationAxis.Up:
                    axis = Vector3.up;
                    break;
                case RotationAxis.Forward:
                    axis = Vector3.forward;
                    break;
            }
            this.transform.Rotate(axis * Time.deltaTime * Speed, this.InLocalCoordinates ? Space.Self : Space.World);
        } 
        #endregion
    } 
}
