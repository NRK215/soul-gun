using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX3.Framework.Components
{
    /// <summary>
    /// Manages a collider, setted as trigger, for uses like a collision sensor.
    /// </summary>
    [AddComponentMenu("[EX3] Framework/Utils/Trigger Sensor")]
    public class TriggerSensor : MonoBehaviour
    {
        #region Enums
        public enum TriggerSensorBehaviour
        {
            OnTriggerEnter,
            OnTriggerExit,
            OnTriggerStay
        }
        #endregion

        #region Inspector fields
        [Header("Set the OnTrigger behaviour:")]
        [SerializeField]
        TriggerSensorBehaviour _onTriggerBehaviour = TriggerSensorBehaviour.OnTriggerEnter;
        [SerializeField]
        bool _ignorePlayerTag = false;
        #endregion

        #region Read only inspector fields for quick visual debug
#if UNITY_EDITOR
        [Header("Quick debug view:")]
        [ReadOnly]
        public bool _hasContact;
        [ReadOnly]
        public Collider _colliderContact;
#endif
        #endregion

        #region Properties
        /// <summary>
        /// Return if the sensor has contact with other game object.
        /// </summary>
        public bool HasContact { get; private set; }
        #endregion

        #region Methods & Functions
        private bool IsPlayer(Collider other)
        {
            return other.CompareTag("Player") || this._ignorePlayerTag;
        }

        void SetQuickDebugInfo(bool hasContact, Collider other)
        {
#if UNITY_EDITOR
            this._hasContact = this.HasContact;
            this._colliderContact = other;
#endif
        }
        #endregion

        #region Events
        private void OnTriggerEnter(Collider other)
        {
            this.HasContact = (this._onTriggerBehaviour == TriggerSensorBehaviour.OnTriggerEnter) && this.IsPlayer(other);
            this.SetQuickDebugInfo(this.HasContact, other);
        }

        private void OnTriggerExit(Collider other)
        {
            this.HasContact = (this._onTriggerBehaviour == TriggerSensorBehaviour.OnTriggerExit) && this.IsPlayer(other);
            this.SetQuickDebugInfo(this.HasContact, other);
        }

        private void OnTriggerStay(Collider other)
        {
            this.HasContact = (this._onTriggerBehaviour == TriggerSensorBehaviour.OnTriggerStay) && this.IsPlayer(other);
            this.SetQuickDebugInfo(this.HasContact, other);
        }
        #endregion
    } 
}
