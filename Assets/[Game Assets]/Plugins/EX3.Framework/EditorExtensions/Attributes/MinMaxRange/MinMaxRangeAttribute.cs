using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX3.Framework
{
    /* MinMaxRangeAttribute.cs
    * by Eddie Cameron – For the public domain
    * http://www.grapefruitgames.com/blog/2013/11/a-min-max-range-for-unity/
    * —————————-
    * Use a MinMaxRange class to replace twin float range values (eg: float minSpeed, maxSpeed; becomes MinMaxRange speed)
    * Apply a [MinMaxRange( minLimit, maxLimit )] attribute to a MinMaxRange instance to control the limits and to show a
    * slider in the inspector
    * 
    * Edited by EX3 (Change public var names to Min & Max)
    */
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public float minLimit, maxLimit;

        public MinMaxRangeAttribute(float minLimit, float maxLimit)
        {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
        }
    }

    [System.Serializable]
    public class MinMaxRange
    {
        public float Min, Max;

        public float GetRandomValue()
        {
            return Random.Range(Min, Max);
        }
    }
}

