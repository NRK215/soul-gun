using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX3.Framework;

namespace EX3.Framework.Components
{
    /// <summary>
    /// Instantiate object.
    /// </summary>
    /// <remarks>Use this in the GameObjects that will be instantiate many times. This instance is manage by an ObjectPool class. Never destroy it!</remarks>
    public abstract class InstantiableObject : MonoBehaviour
    {
        #region Internal vars
        internal ObjectPool pool;
        internal bool isFree;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Call this method when you ended with this instance.
        /// </summary>
        /// <remarks>This call tell to the owner ObjectPool that this instance is free for create new instance.</remarks>
        public void Dispose()
        {
            this.isFree = true;
            this.gameObject.SetActive(false);
            this.pool.Count--;
        }

        /// <summary>
        /// Call this method when you ended with this instance before pass a specify ammout of time.
        /// </summary>
        /// <param name="time">Time in seconds to dispose this object.</param>
        /// <remarks>This call tell to the owner ObjectPool that this instance is free for create new instance.</remarks>
        public void Dispose(float time)
        {
            Invoke("Dispose", time);
        } 
        #endregion
    }
}