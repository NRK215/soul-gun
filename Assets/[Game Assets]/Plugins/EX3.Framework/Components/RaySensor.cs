using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX3.Framework.Components
{
    /// <summary>
    /// Ray sensor.
    /// </summary>
    /// <remarks>This ray direction always is the tranform forward.</remarks>
    [AddComponentMenu("[EX3] Framework/Utils/Ray Sensor")]
    public class RaySensor : MonoBehaviour
    {
        #region Internal vars
        Ray _ray;
        #endregion

        #region Inspector fields
        [SerializeField]
        float _maxDistance = 1f;
        [SerializeField]
        LayerMask _collisionMask = Physics.AllLayers;
        [SerializeField]
        Color _gizmoColor = Color.red;
        [SerializeField]
        bool _rayCastAll = false;
        [SerializeField]
        bool _drawGizmo = false;
        #endregion

        #region Read only inspector fields for quick visual debug
#if UNITY_EDITOR
        [Header("Quick debug view:")]
        [ReadOnly]
        public bool _hasContact;
        [ReadOnly] public float _distance;
        [Space]
        [Space]
        [ReadOnly]
        public Vector3 _point;
        [ReadOnly] public Vector3 _normal;
        [ReadOnly] public Vector3 _barycentricCoordinate;
        [ReadOnly] public int _triangleIndex;
        [ReadOnly] public Vector2 _textureCoord;
        [ReadOnly] public Vector2 _textureCoord2;
        [ReadOnly] public Vector2 _lightmapCoord;
        [ReadOnly] public Collider _collider;
#endif
        #endregion

        #region Properties
        /// <summary>
        /// The RayCast operation has contact with any game object?
        /// </summary>
        public bool HasContact { get; private set; }

        /// <summary>
        /// Result of the single RayCast (default behaviour).
        /// </summary>
        public RaycastHit Hit { get; private set; }

        /// <summary>
        /// Result of the RayCastAll (if the flag is active).
        /// </summary>
        public RaycastHit[] Hits { get; private set; }

        /// <summary>
        /// Get the distance from the origin to the hit point (only in single RayCast).
        /// </summary>
        /// <remarks>If not has contact hit, this property return infinite.</remarks>
        public float Distance
        {
            get
            {
                return this.HasContact ? this.Hit.distance : Mathf.Infinity;
            }
        }
        #endregion

        #region Initializers
        private void Awake()
        {
            this._ray = new Ray(this.transform.position, this.transform.forward);
        }
        #endregion

        #region Update logic
        private void FixedUpdate()
        {
            this._ray = new Ray(this.transform.position, this.transform.forward);

            if (this._rayCastAll)
            {
                this.Hits = Physics.RaycastAll(this._ray, this._maxDistance, this._collisionMask);
                this.HasContact = this.Hits.Length > 0;

                this.SetQuickDebugInfo(this.HasContact, this.Hits[0]);
            }
            else
            {
                RaycastHit hit;
                this.HasContact = Physics.Raycast(this._ray, out hit, this._maxDistance, this._collisionMask);
                this.Hit = this.HasContact ? hit : new RaycastHit();

                this.SetQuickDebugInfo(this.HasContact, this.Hit);
            }
        }
        #endregion

        #region Methods & Functions
        void SetQuickDebugInfo(bool hasContact, RaycastHit hit)
        {
#if UNITY_EDITOR
            this._hasContact = hasContact;
            this._distance = this.Distance;

            if (hasContact)
            {
                this._barycentricCoordinate = hit.barycentricCoordinate;
                this._collider = hit.collider;
                this._lightmapCoord = hit.lightmapCoord;
                this._normal = hit.normal;
                this._point = hit.point;
                this._textureCoord = hit.textureCoord;
                this._textureCoord2 = hit.textureCoord2;
                this._triangleIndex = hit.triangleIndex;
            }
#endif
        }
        #endregion

        #region Events
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (this._drawGizmo)
            {
                Gizmos.color = this._gizmoColor;
                Gizmos.DrawLine(this.transform.position, this.HasContact ? this.Hit.point : this.transform.position + this.transform.forward * this._maxDistance);
            }
        }
#endif
        #endregion
    }

}