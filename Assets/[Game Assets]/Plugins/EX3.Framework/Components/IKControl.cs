using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX3.Framework.Components
{
    /// <summary>
    /// Inverse Kinematic control (Mecanim).
    /// </summary>
    /// <remarks>Allow to apply own settings to IK position, rotation, hints and head look at.</remarks>
    [AddComponentMenu("[EX3] Framework/Utils/IK Control")]
    public class IKControl : MonoBehaviour
    {
        #region Internal vars
        Animator _animator;
        #endregion

        #region Public vars
        public int AnimatorLayer = 0;

        [Header("IK Position & Rotation settings:")]
        public bool ApplyIKPosition = true;
        public Transform IKTarget;
        public AvatarIKGoal IKGoal;
        [Range(0f, 1f)]
        public float PositionWeight = 0f;

        public bool ApplyIKRotation = false;
        public bool UseTransformRotation = true;
        public Vector3 Rotation = Vector3.zero;
        [Range(0f, 1f)]
        public float RotationWeight = 0f;

        [Header("IK Hint settings:")]
        public bool ApplyIKHint = false;
        public Transform HintTarget;
        public AvatarIKHint IKHint;
        [Range(0f, 1f)]
        public float HintWeight = 0f;

        [Header("Head Look At settings:")]
        public bool ApplyLookAt = false;
        public Transform LookAtTarget;
        [Range(0f, 1f)]
        public float LookAtWeight = 0f;
        #endregion

        #region Initializers
        void Awake()
        {
            this._animator = GetComponent<Animator>();
        }
        #endregion

        #region Update logic
        private void OnAnimatorIK(int layerIndex)
        {
            if (layerIndex == this.AnimatorLayer)
            {
                if (this.ApplyLookAt)
                {
                    this._animator.SetLookAtPosition(this.LookAtTarget.position);
                    this._animator.SetLookAtWeight(this.LookAtWeight);
                }

                if (this.ApplyIKHint && this.HintTarget)
                {
                    this._animator.SetIKHintPosition(this.IKHint, this.HintTarget.position);
                    this._animator.SetIKHintPositionWeight(this.IKHint, this.HintWeight);
                }

                if (this.IKTarget)
                {
                    if (this.ApplyIKPosition)
                    {
                        this._animator.SetIKPosition(this.IKGoal, this.IKTarget.position);
                        this._animator.SetIKPositionWeight(this.IKGoal, this.PositionWeight);
                    }

                    if (this.ApplyIKRotation)
                    {
                        this._animator.SetIKRotation(this.IKGoal, this.UseTransformRotation ? this.IKTarget.rotation : Quaternion.Euler(this.Rotation));
                        this._animator.SetIKRotationWeight(this.IKGoal, this.RotationWeight);
                    }
                }
            }
        }
        #endregion
    } 
}
