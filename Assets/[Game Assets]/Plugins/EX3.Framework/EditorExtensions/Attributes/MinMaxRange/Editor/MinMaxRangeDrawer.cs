using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EX3.Framework
{
    /* MinMaxRangeDrawer.cs
    * by Eddie Cameron – For the public domain
    * http://www.grapefruitgames.com/blog/2013/11/a-min-max-range-for-unity/
    * ———————————————————–
    * — EDITOR SCRIPT : Place in a subfolder named ‘Editor’ —
    * ———————————————————–
    * Renders a MinMaxRange field with a MinMaxRangeAttribute as a slider in the inspector
    * Can slide either end of the slider to set ends of range
    * Can slide whole slider to move whole range
    * Can enter exact range values into the From: and To: inspector fields
    *
    * Edited by EX3 (remove slider and set fixed position to Min & Max float fields)
    */
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 16;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
            if (property.type != "MinMaxRange")
                Debug.LogWarning("Use only with MinMaxRange type");
            else
            {
                var range = attribute as MinMaxRangeAttribute;
                var minValue = property.FindPropertyRelative("Min");
                var maxValue = property.FindPropertyRelative("Max");
                var newMin = minValue.floatValue;
                var newMax = maxValue.floatValue;

                var xDivision = position.width * 0.33f;
                var yDivision = position.height * 0.5f;
                EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision), label);

                EditorGUI.LabelField(new Rect(position.xMax - 180f, position.y, xDivision, yDivision), "Min: ");
                newMin = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.xMax - 150f, position.y, 50f, yDivision), newMin), range.minLimit, newMax);

                EditorGUI.LabelField(new Rect(position.xMax - 84f, position.y, xDivision, yDivision), "Max: ");
                newMax = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.xMax - 50f, position.y, 50f, yDivision), newMax), newMin, range.maxLimit);

                minValue.floatValue = newMin;
                maxValue.floatValue = newMax;
            }
        }
    }
}