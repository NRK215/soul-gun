using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EX3.Framework.Components
{
    /// <summary>
    /// Animation Curve Effect.
    /// </summary>
    /// <remarks>Able to apply animation based on animation curves for translations, rotations and scale, based even on RigidBody if you want.</remarks>
    [AddComponentMenu("[EX3] Framework/Utils/Animation Curve Effect")]
    public class AnimationCurveEffect : MonoBehaviour
    {
        #region Structs
        [System.Serializable]
        public struct AnimationAxisParameters
        {
            #region Fields
            public bool AnimateAxis;
            public AnimationCurve Curve;

            /// <summary>
            /// Return the animation curve duration.
            /// </summary>
            public float Duration
            {
                get
                {
                    return (this.Curve.keys.Length > 0) ? this.Curve.keys[this.Curve.keys.Length - 1].time : 0f;
                }
            }

            /// <summary>
            /// Return if the curve animation is looped (at the end key).
            /// </summary>
            public bool IsLooped
            {
                get
                {
                    return this.Curve.postWrapMode == WrapMode.Loop;
                }
            }
            #endregion
        }

        [System.Serializable]
        public struct AnimationCurveVector
        {
            #region Fields
            public bool Active;
            public AnimationAxisParameters X;
            public AnimationAxisParameters Y;
            public AnimationAxisParameters Z;
            #endregion

            #region Properties
            /// <summary>
            /// Calculate the vector result of the current curve value by time x speed in each active axis.
            /// </summary>
            public Vector3 VectorAnimation
            {
                get
                {
                    return new Vector3(this.X.AnimateAxis ? this.X.Curve.Evaluate(Time.time) : 0f,
                                       this.Y.AnimateAxis ? this.Y.Curve.Evaluate(Time.time) : 0f,
                                       this.Z.AnimateAxis ? this.Z.Curve.Evaluate(Time.time) : 0f);
                }
            }
            #endregion
        }
        #endregion

        #region Public vars
        public AnimationCurveVector Translation;
        public AnimationCurveVector Rotation;
        public AnimationCurveVector Scale;
        public bool UsesRigidBody = false;
        public Rigidbody RigidBody;
        #endregion

        #region Initializers
        private void Reset()
        {
            this.Translation = new AnimationCurveVector();
            this.Translation.X = this.Translation.Y = this.Translation.Z = new AnimationAxisParameters() { AnimateAxis = false };

            this.Rotation = new AnimationCurveVector();
            this.Rotation.X = this.Rotation.Y = this.Rotation.Z = new AnimationAxisParameters() { AnimateAxis = false };

            this.Scale = new AnimationCurveVector();
            this.Scale.X = this.Scale.Y = this.Scale.Z = new AnimationAxisParameters() { AnimateAxis = false };
        }
        #endregion

        #region Update logic
        public void Update()
        {
            if (!this.UsesRigidBody)
            {
                if (this.Translation.Active)
                {
                    this.transform.position += this.Translation.VectorAnimation;
                }
                if (this.Rotation.Active)
                {
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + this.Rotation.VectorAnimation);
                }
                if (this.Scale.Active)
                {
                    this.transform.localScale += this.Scale.VectorAnimation;
                }
            }
        }

        /// <summary>
        /// Rigidbody transforms logic (with physics response).
        /// </summary>
        private void FixedUpdate()
        {
            if (this.UsesRigidBody)
            {
                this.CheckForRigidBody();

                if (this.Translation.Active)
                {
                    this.RigidBody.MovePosition(this.RigidBody.position + this.Translation.VectorAnimation);
                }
                if (this.Rotation.Active)
                {
                    this.RigidBody.MoveRotation(Quaternion.Euler(this.RigidBody.rotation.eulerAngles + this.Rotation.VectorAnimation));
                }
                if (this.Scale.Active)
                {
                    this.transform.localScale += this.Scale.VectorAnimation;
                }
            }
        }
        #endregion

        #region Methods & Functions
        void CheckForRigidBody()
        {
            if (!this.RigidBody)
            {
                // Search first in this gameObject:
                this.RigidBody = GetComponent<Rigidbody>();
                if (!this.RigidBody)
                {
                    // Search in the parent:
                    this.RigidBody = GetComponentInParent<Rigidbody>();
                    if (!this.RigidBody)
                    {
                        // Search in the root tranform:
                        this.RigidBody = this.transform.root.GetComponent<Rigidbody>();

                        if (!this.RigidBody)
                        {
                            Debug.LogError("No RigidBody found on this gameObject, parent or root transform.");
                        }
                    }
                }
            }
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AnimationCurveEffect))]
    public class AnimationCurveEffectEditor : Editor
    {
        #region Internal vars
        AnimationCurveEffect _instance;
        #endregion

        #region Events
        public override void OnInspectorGUI()
        {
            if (!this._instance)
            {
                this._instance = (AnimationCurveEffect)target;
            }

            EditorGUILayout.HelpBox("Able to apply animation based on animation curves for translations, rotations and scale, based even on RigidBody if you want.", MessageType.Info);

            this.DrawToggleGroup("Translation", ref this._instance.Translation);
            this.DrawToggleGroup("Rotation", ref this._instance.Rotation);
            this.DrawToggleGroup("Scale", ref this._instance.Scale);

            EditorGUILayout.Separator();

            this._instance.UsesRigidBody = EditorGUILayout.BeginToggleGroup("Uses RigidBody?", this._instance.UsesRigidBody);
            {
                if (this._instance.UsesRigidBody)
                {
                    EditorGUILayout.ObjectField("RigidBody", this._instance.RigidBody, typeof(Rigidbody), true);
                    EditorGUILayout.HelpBox("If this field is empty, this component try to find a RigidBody on this gameObject, his parent and his root transform.", MessageType.Info);
                }
            }
            EditorGUILayout.EndToggleGroup();
        }
        #endregion

        #region Methods & Functions
        void DrawToggleGroup(string label, ref AnimationCurveEffect.AnimationCurveVector vector)
        {
            vector.Active = EditorGUILayout.BeginToggleGroup(label, vector.Active);
            {
                if (vector.Active)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        // Column margin:
                        EditorGUILayout.BeginVertical(GUILayout.Width(10f));
                        {
                            EditorGUILayout.Separator();
                        }
                        EditorGUILayout.EndVertical();

                        // Column fields:
                        EditorGUILayout.BeginVertical();
                        {
                            this.DrawAnimationCurveAxis("X", ref vector.X);
                            this.DrawAnimationCurveAxis("Y", ref vector.Y);
                            this.DrawAnimationCurveAxis("Z", ref vector.Z);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                }
            }
            EditorGUILayout.EndToggleGroup();
        }

        void DrawAnimationCurveAxis(string label, ref AnimationCurveEffect.AnimationAxisParameters axisParams)
        {
            EditorGUILayout.BeginHorizontal();
            {
                axisParams.AnimateAxis = EditorGUILayout.ToggleLeft(label, axisParams.AnimateAxis, GUILayout.MaxWidth(26f));
                EditorGUILayout.CurveField(axisParams.Curve);
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
#endif 
}