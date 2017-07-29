using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX3.Framework.Components
{
    /// <summary>
    /// Billboard effect.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("[EX3] Framework/Utils/Billboard")]
    public class Billboard : MonoBehaviour
    {
        public bool InvertDirection = false;

        void Update()
        {
            if (this.InvertDirection)
            {
                this.transform.forward = Camera.main.transform.forward;
            }
            else
            {
                this.transform.LookAt(Camera.main.transform);
            }
        }
    }

}