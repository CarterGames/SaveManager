/*
 * Save Manager (3.x)
 * Copyright (c) 2025-2026 Carter Games
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version. 
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. 
 *
 * You should have received a copy of the GNU General Public License along with this program.
 * If not, see <https://www.gnu.org/licenses/>. 
 */

using System.Reflection;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;

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
            
            var container = EditorGUILayout.BeginVertical("Box");
            GUIContent labelContent;
            
            var msg = string.Empty;
            var hasIssue = HasIssue(property, out msg);
            
            if (hasIssue)
            {
                labelContent = new GUIContent($" {label.text}", EditorArtHandler.GetIcon(SaveManagerConstants.WarningIcon));
            }
            else
            {
                labelContent = label;
            }
            
            position.height = container.height;
            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, labelContent);
            GUILayout.FlexibleSpace();
            
            if (hasIssue)
            {
                EditorGUILayout.HelpBox(msg, MessageType.None);
            }
            
            DrawResetButton(property);
            
            EditorGUILayout.EndHorizontal();
    
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                
                EditorGUILayout.BeginVertical("Box");
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(property.Fpr("value"), new GUIContent("Value"));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(property.serializedObject.targetObject, "Save Value changed");
                    
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    
                    EditorSaveManager.TrySetDirty();
                }
                
                if (property.Fpr("hasDefaultValue").boolValue)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(property.Fpr("defaultValue"), new GUIContent("Default Value"));
                    EditorGUI.EndDisabledGroup();
                }
                
                EditorGUILayout.EndVertical();
                
                EditorGUI.indentLevel--;
            }
            
            GUILayout.Space(1f);
            
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
            GUI.backgroundColor = Color.red;
            
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog($"Reset {property.name}",
                        $"Are you sure you want to reset {property.name} to its default value.",
                        "Reset", "Cancel"))
                {
                    ResetToDefault(property);
                    
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    
                    EditorSaveManager.TrySetDirty();
                    return;
                }
            }
    
            GUI.backgroundColor = Color.white;
        }


        private bool HasIssue(SerializedProperty property, out string message)
        {
            message = string.Empty;
            
            if (string.IsNullOrEmpty(property.Fpr("key").stringValue))
            {
                message = "No save key assigned, this value cannot be saved.";
                return true;
            }
            
            return false;
        }
    }
}