/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CarterGames.Assets.SaveManager.Editor
{
    [CustomPropertyDrawer(typeof(SaveValueBase), true)]
    public sealed class SaveValuePropertyDrawer : PropertyDrawer
    {
        // Note EditorGUILayout works here as its applying to a base class and any inheriting classes will behave.
        // Its a super weird instance where it does work, but saves a lot of code for the rects etc.
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject == null) return;
            
            EditorGUI.BeginProperty(position, label, property);
 
            var container = EditorGUILayout.BeginVertical(property.isExpanded ? "HelpBox" : "Box");
    
            position.height = container.height;
            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);
            
            DrawResetButton(property);
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
    
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                
                EditorGUILayout.BeginVertical("Box");
    
                if (PerUserSettings.ShowSaveKeys)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(property.Fpr("key"), new GUIContent("Key"));
                    EditorGUI.EndDisabledGroup();
                }
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(property.Fpr("value"), new GUIContent("Value"));
                if (EditorGUI.EndChangeCheck())
                {
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    SaveManager.Save();
                }
                
                if (PerUserSettings.ShowDefaultValues)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(property.Fpr("defaultValue"), new GUIContent("Default Value"));
                    EditorGUI.EndDisabledGroup();
                }
                
                EditorGUILayout.EndVertical();
                
                EditorGUI.indentLevel--;
            }
            
            GUILayout.Space(2f);
            
            EditorGUILayout.EndVertical();
            EditorGUI.EndProperty();
        }
        
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.standardVerticalSpacing;
        }
        
    
        private static void ResetToDefault(SerializedProperty prop)
        {
            var field = prop.serializedObject.targetObject.GetType()
                .GetField(prop.propertyPath.Split('.')[0].Trim(),
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Undo.RecordObject(prop.serializedObject.targetObject, "Save Value reset to default value");
            
            field.FieldType
                .GetMethod("ResetValue", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(field.GetValue(prop.serializedObject.targetObject), new object[1] { true });
        }
        
        
        private void DrawResetButton(SerializedProperty property)
        {
            GUI.backgroundColor = UtilEditor.Red;
            
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog($"Reset {property.name}",
                        $"Are you sure you want to reset {property.name} to its default value.",
                        "Reset", "Cancel"))
                {
                    ResetToDefault(property);
                    
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    
                    SaveManager.Save();
                    return;
                }
            }
    
            GUI.backgroundColor = Color.white;
        }
    }
}