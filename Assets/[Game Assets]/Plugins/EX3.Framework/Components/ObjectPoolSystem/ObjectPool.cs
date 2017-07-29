using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using EX3.Framework;
using System.Linq;

namespace EX3.Framework.Components
{
    /// <summary>
    /// Object pool dispatcher.
    /// </summary>
    /// <remarks>Manage a list of instances of GameObjects (based on InstantiateObject class) for optimizing performance.</remarks>
    [AddComponentMenu("[EX3] Framework/Utils/Object Pool System/Object Pool")]
    public class ObjectPool : MonoBehaviour
    {
        #region Private vars
        InstantiableObject[] _instances;
        int _iterator = 0;
        #endregion

        #region Public vars
        [Header("Change this value in runtime will not take any effect.")]
        public InstantiableObject PrefabModel;
        [Header("Change this value in runtime will not take any effect.")]
        public int MaxInstances;
        [Header("Return the next instance in use (useful with bullet decals)")]
        [SerializeField]
        bool _iterableCycling = false;
        [ReadOnly]
        public int Count;
        #endregion

        #region Properties
        /// <summary>
        /// Return if this pool is full of instances.
        /// </summary>
        public bool IsFull { get { return this.Count == this.MaxInstances; } }

        /// <summary>
        /// Return all instances of this Object Pool.
        /// </summary>
        public ReadOnlyCollection<InstantiableObject> Instances { get { return this._instances.ToList().AsReadOnly(); } }

        /// <summary>
        /// Return all active instances of this Object Pool.
        /// </summary>
        public ReadOnlyCollection<InstantiableObject> ActiveInstances { get { return this._instances.Where(e => !e.isFree).ToList().AsReadOnly(); } }
        #endregion

        #region Initializers
        void Awake()
        {
            if (this.PrefabModel)
            {
                this._instances = new InstantiableObject[this.MaxInstances];
                for (int i = 0; i < this.MaxInstances; i++)
                {
                    InstantiableObject instance = Instantiate(this.PrefabModel, Vector3.zero, Quaternion.identity);
                    {
                        instance.pool = this;
                        instance.isFree = true;
                        instance.gameObject.SetActive(false);
                    }
                    this._instances[i] = instance;
                }
            }
            else
            {
                throw new System.Exception(string.Format("No Prefab set in this ObjectPool. GameObject '{0}'", this.gameObject.name));
            }
        }
        #endregion

        #region Methods & functions
        /// <summary>
        /// Get the first free instance available.
        /// </summary>
        /// <returns>Return an enabled and active instance. If this ObjectPool is full, return null.</returns>
        public InstantiableObject GetNewInstance()
        {
            return this.GetNewInstance(Vector3.zero);
        }

        /// <summary>
        /// Get the first free instance available.
        /// </summary>
        /// <param name="position">Start position of the new instance.</param>
        /// <returns>Return an enabled and active instance. If this ObjectPool is full, return null.</returns>
        public InstantiableObject GetNewInstance(Vector3 position)
        {
            return this.GetNewInstance(position, Quaternion.identity);
        }

        /// <summary>
        /// Get the first free instance available.
        /// </summary>
        /// <param name="position">Start position of the new instance.</param>
        /// <param name="rotation">Start rotation of the new instance.</param>
        /// <param name="lifeTime">Lifetime, in seconds, after the instances is disabled. Set 0 to manual disabled. Not take effect if IterableCycling is true.</param>
        /// <returns>Return an enabled and active instance. If this ObjectPool is full (and IterableCycling is false), return null.</returns>
        public InstantiableObject GetNewInstance(Vector3 position, Quaternion rotation, float lifeTime = 0f)
        {
            if (this.PrefabModel)
            {
                if (this._iterableCycling)
                {
                    if (++this._iterator == this._instances.Length)
                    {
                        this._iterator = 0;
                    }

                    var instance = this._instances[this._iterator];
                    instance.isFree = false;
                    instance.transform.position = position;
                    instance.transform.rotation = rotation;
                    instance.gameObject.SetActive(true);
                    if (++this.Count >= this.MaxInstances)
                    {
                        this.Count = this.MaxInstances;
                    }
                    return instance;
                }
                else
                { 
                    if (!this.IsFull)
                    {
                        foreach (InstantiableObject instance in this._instances)
                        {
                            if (instance.isFree)
                            {
                                instance.isFree = false;
                                instance.transform.position = position;
                                instance.transform.rotation = rotation;
                                instance.gameObject.SetActive(true);
                                if (lifeTime > 0)
                                {
                                    instance.Dispose(lifeTime);
                                }
                                this.Count++;
                                return instance;
                            }
                        }
                    }
                    return null; 
                }
            }
            else
            {
                throw new System.Exception(string.Format("No Prefab set in this ObjectPool. GameObject '{0}'", this.gameObject.name));
            }
        }

        /// <summary>
        /// Recycling all instances.
        /// </summary>
        /// <remarks>This call disabled and deactivated all instances.</remarks>
        public void RecyclingAll()
        {
            if (this.PrefabModel)
            {
                foreach (InstantiableObject instance in this._instances)
                {
                    instance.isFree = true;
                    instance.gameObject.SetActive(false);
                }

                if (this.MaxInstances != this._instances.Length)
                {
                    this.Awake();
                }
            }
            else
            {
                throw new System.Exception(string.Format("No Prefab set in this ObjectPool. GameObject '{0}'", this.gameObject.name));
            }
        }

        /// <summary>
        /// Releasing instances when this GameObject is destroying.
        /// </summary>
        private void OnDestroy()
        {
            foreach (InstantiableObject instance in this._instances)
            {
                Destroy(instance);
            }
        }
        #endregion
    } 
}