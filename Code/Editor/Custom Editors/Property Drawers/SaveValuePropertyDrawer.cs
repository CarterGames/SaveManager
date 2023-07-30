/*
 * Copyright (c) 2018-Present Carter Games
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

using System.Reflection;
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

            if (Event.current.isMouse && Event.current.button == 1)
            {
                OnPropertyContextMenu(new GenericMenu(), property);
            }
            
            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.BeginVertical();
    
            EditorGUI.indentLevel++;
    
            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);
    
            DrawResetButton(property);
            
            EditorGUILayout.EndHorizontal();
    
            if (property.isExpanded)
            {
                EditorGUILayout.BeginVertical("Box");
    
                if (UtilEditor.SettingsAssetEditor.ShowSaveKeys)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(property.FindPropertyRelative("key"), new GUIContent("Key"));
                    EditorGUI.EndDisabledGroup();
                }
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(property.FindPropertyRelative("value"), new GUIContent("Value"));
                if (EditorGUI.EndChangeCheck())
                {
                    SaveManager.Save();
                }
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUI.indentLevel--;
            
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
                .GetField(prop.propertyPath.Split('.')[0],
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            field.GetValue(prop.serializedObject.targetObject).GetType()
                .GetMethod("ResetValue", BindingFlags.Public | BindingFlags.Instance).Invoke(field.GetValue(prop.serializedObject.targetObject), null);
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
                    ResetToDefault(property.FindPropertyRelative("value"));
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    SaveManager.Save();
                    return;
                }
            }
    
            GUI.backgroundColor = UtilEditor.SettingsAssetEditor.BackgroundColor;
        }
        

        void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            Debug.Log(property);
            Debug.Log(property.FindPropertyRelative("value").type);
            
            var propertyCopy = property.Copy();
            menu.AddItem(new GUIContent("Copy Value"), false, () =>
            {
                GUIUtility.systemCopyBuffer = property.FindPropertyRelative("value").ToString();
            });
            
            menu.ShowAsContext();
        }
    }
}