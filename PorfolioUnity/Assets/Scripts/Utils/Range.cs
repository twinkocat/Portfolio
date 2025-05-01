using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
// ReSharper disable InconsistentNaming
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable SuggestVarOrType_BuiltInTypes

#endif
/// <summary>
/// A value type that represents a range of floating point values.
/// </summary>
/// <example>
/// Ranges are implicitly convertible to and from floats and support most basic
/// arithmetic with them.
/// <code>
/// Range r1 = 5f;                  // Converted to a range of [5, 5]
/// Range r2 = r1.Expanded(5);      // Expanded range to [0, 10]
/// Range r3 = r2 + 4;              // Shifted range to [4, 14]
/// Range r4 = r3 - 9;              // Shifted range to [-5, 5]
/// Range r5 = r4 * 20;             // Multiplied range to [-100, 100]
/// Range r6 = r5 / 10;             // Divided range to [-10, 10]
/// </code>
/// </example>
[Serializable]
public struct Range {

  [SerializeField] float _min;
  [SerializeField] float _max;

  /// <summary>
  /// Gets the smallest value in the range.
  /// </summary>
  public float Min => _min;

  /// <summary>
  /// Gets the largest value in the range.
  /// </summary>
  public float Max => _max;

  /// <summary>
  /// Gets the midpoint value of the range.
  /// </summary>
  public float Center => (Max + Min) / 2f;

  /// <summary>
  /// Gets the size of the range.
  /// </summary>
  public float Size => Max - Min;

  /// <summary>
  /// Creates a range scoped to the value of only one value.
  /// </summary>
  /// <param name="val">the center value of the range</param>
  /// <example>
  /// <code>
  /// new Range(0)         // A range of [0, 0]
  /// new Range(5)         // A range of [5, 5]
  /// new Range(1000, 100) // A range of [100, 100]
  /// </code>
  /// </example>
  public Range(float val) : this(val, val) {}

  /// <summary>
  /// Creates a range based on an interval of values.
  /// </summary>
  /// <param name="a">one extremum of the range.</param>
  /// <param name="b">one extremum of the range.</param>
  /// <example>
  /// <code>
  /// new Range(0, 5)   // A range of [0, 5]
  /// new Range(5, 0)   // Another way to express [0, 5]
  /// new Range(5, 100) // A range of [5, 100]
  /// new Range(100, 5) // Another way to express of [5, 100]
  /// </code>
  /// </example>
  public Range(float a, float b) {
    if (a > b) {
      _min = b;
      _max = a;
    } else {
      _max = b;
      _min = a;
    }
  }

  /// <summary>
  /// Creates a new Range expanded from the current range.
  /// </summary>
  /// <remarks>
  /// Can be used to shrink the new range by providiing a negative size.
  /// If the absolute value of <paramref cref="size"/> is larger than the
  /// <see cref="Size"/> of the Range, the range will
  /// </remarks>
  /// <param name="size">the size</param>
  /// <returns></returns>
  public Range Expanded(float size) {
    var extents = size / 2f;
    return new Range(Min - extents, Max + extents);
  }

  /// <summary>
  /// Gets whether the range is close to a single value and approximately the same
  /// as a given floating point value.
  /// </summary>
  /// <param name="value">the target value to check against.</param>
  /// <returns>true if the entire range is approximately the target value, false otherwise.</returns>
  public bool Approximately(float value) {
    return Mathf.Approximately(Size, 0f) && Mathf.Approximately(Center, value);
  }

  /// <summary>
  /// Clamps a value to the limits of the range.
  /// </summary>
  /// <param name="value">the value to clamp.</param>
  /// <returns>the clamped value.</returns>
  public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

  /// <summary>
  /// Uniformily samples a value from the region.
  /// </summary>
  /// <returns>the randomly sampled value.</returns>
  public float GetValue() => Random.Range(Min, Max);

  public int GetValueRoundToInt() => Mathf.RoundToInt(GetValue());
  
  public int GetValueFloorToInt() => Mathf.FloorToInt(GetValue());
  
  public static implicit operator Range(float val) => new(val);

  public static Range operator +(Range lhs, Range rhs) => new(lhs.Min + rhs.Min, lhs.Max + rhs.Max);
  public static Range operator -(Range lhs, Range rhs) => new(lhs.Min - rhs.Min, lhs.Max - rhs.Max);
  
  public static Range operator *(Range lhs, Range rhs) {
    float[] values = new float[4];
    
    values[0] = lhs.Min * rhs.Min;
    values[1] = lhs.Max * rhs.Min;
    values[2] = lhs.Min * rhs.Max;
    values[3] = lhs.Max * rhs.Max;
    float min = float.MaxValue, max = float.MinValue;
    
    for (var i = 0; i < 4; i++) {
      min = Math.Min(min, values[i]);
      max = Math.Max(max, values[i]);
    }
    return new Range { _min = min, _max = max };
  }
  public static Range operator /(Range lhs, Range rhs) => lhs * (1f / rhs);
  public static Range operator *(Range lhs, float rhs) => new Range { _min = lhs._min * rhs, _max = lhs._max * rhs };
  public static Range operator /(Range lhs, float rhs) => new Range { _min = lhs._min / rhs, _max = lhs._max / rhs };
  public static Range operator /(float lhs, Range rhs)  {
    rhs._min = rhs._min == 0f ? float.NegativeInfinity : lhs / rhs._min;
    rhs._max = rhs._max == 0f ? float.PositiveInfinity : lhs / rhs._max;
    return rhs;
  }

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Range))]
internal class RangeDrawer : PropertyDrawer {

  const float buttonSize = 30f;

  Dictionary<string, bool> _propertyType;

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    var min = property.FindPropertyRelative("_min");
    var max = property.FindPropertyRelative("_max");

    _propertyType = _propertyType ?? (_propertyType = new Dictionary<string, bool>());

    bool isRange;
    if (!_propertyType.TryGetValue(property.propertyPath, out isRange)) {
        isRange = !Mathf.Approximately(min.floatValue, max.floatValue);
        _propertyType.Add(property.propertyPath, isRange);
    }

    _propertyType[property.propertyPath] = DrawRangeEditor(position, property, label, isRange);
  }

  static bool DrawRangeEditor(Rect position, SerializedProperty property, GUIContent label, bool isRange) {
    var fieldPosition = position;
    var buttonPosition = position;
    fieldPosition.width -= buttonSize;
    buttonPosition.x += fieldPosition.width;
    buttonPosition.width = buttonSize;

    var min = property.FindPropertyRelative("_min");
    var max = property.FindPropertyRelative("_max");

    EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
    EditorGUI.BeginProperty(position, label, property);
    if (isRange) {
        MultiFloatField(fieldPosition, label, 
                        new[] { new GUIContent("-"), new GUIContent("+")},
                        new[] { min, max });
    } else {
      var multiEditing = min.hasMultipleDifferentValues || max.hasMultipleDifferentValues;
      EditorGUI.showMixedValue = multiEditing;
      var value = EditorGUI.FloatField(fieldPosition, label ,min.floatValue);
      if (!min.hasMultipleDifferentValues) {
        min.floatValue = value;
        max.floatValue = value;
      }
    }
    EditorGUI.showMixedValue = false;
    if (GUI.Button(buttonPosition, isRange ? "\u2194" : "\u2022")) {
        isRange = !isRange;
        if (!isRange) {
            var average = (min.floatValue + max.floatValue) / 2;
            min.floatValue = average;
            max.floatValue = average;
        }
    }
    EditorGUI.EndProperty();
    return isRange;
  }

  static void MultiFloatField(Rect position, GUIContent label, GUIContent[] subLabels, SerializedProperty[] properties) {
    int controlId = GUIUtility.GetControlID("foldout".GetHashCode(), FocusType.Passive, position);
    position = MultiFieldPrefixLabel(position, controlId, label, subLabels.Length);
    position.height = 17f;
    MultiFloatField(position, subLabels, properties);
  }

  static void MultiFloatField(Rect position, GUIContent[] subLabels, SerializedProperty[] properties) {
    int length = properties.Length;
    float num = (position.width - (float) (length - 1) * 2f) / (float) length;
    Rect position1 = new Rect(position);
    position1.width = num;
    float labelWidth1 = EditorGUIUtility.labelWidth;
    int indentLevel = EditorGUI.indentLevel;
    EditorGUIUtility.labelWidth = 13f;
    EditorGUI.indentLevel = 0;
    for (int index = 0; index < properties.Length; ++index) {
      EditorGUI.PropertyField(position1, properties[index], subLabels[index], false);
      position1.x += num + 2f;
    }
    EditorGUIUtility.labelWidth = labelWidth1;
    EditorGUI.indentLevel = indentLevel;
  }

  static bool LabelHasContent(GUIContent label) {
    if (label == null || label.text != string.Empty)
      return true;
    return label.image != null;
  }

  static Rect MultiFieldPrefixLabel(Rect totalPosition, int id, GUIContent label, int columns) {
    if (!LabelHasContent(label))
      return EditorGUI.IndentedRect(totalPosition);
    var indent = EditorGUI.indentLevel * 15f;
    if (EditorGUIUtility.wideMode) {
      Rect labelPosition = new Rect(totalPosition.x + indent, totalPosition.y, EditorGUIUtility.labelWidth - indent, 16f);
      Rect rect = totalPosition;
      rect.xMin += EditorGUIUtility.labelWidth;
      if (columns > 1) {
        --labelPosition.width;
        --rect.xMin;
      }
      if (columns == 2) {
        float num = (float) (((double) rect.width - 4.0) / 3.0);
        rect.xMax -= num + 2f;
      }
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id);
      return rect;
    }
    Rect labelPosition1 = new Rect(totalPosition.x + indent, totalPosition.y, totalPosition.width - indent, 16f);
    Rect rect1 = totalPosition;
    rect1.xMin += indent + 15f;
    rect1.yMin += 16f;
    EditorGUI.HandlePrefixLabel(totalPosition, labelPosition1, label, id);
    return rect1;
  }

}

#endif