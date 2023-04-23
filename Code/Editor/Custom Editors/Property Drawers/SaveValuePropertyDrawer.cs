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
            EditorGUILayout.BeginVertical();

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);

            DrawResetButton(property);
            
            EditorGUILayout.EndHorizontal();
            
            if (property.isExpanded)
            {
                EditorGUILayout.BeginVertical("Box");

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(property.FindPropertyRelative("key"), new GUIContent("Key"));
                EditorGUI.EndDisabledGroup();
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(property.FindPropertyRelative("value"), new GUIContent("Value"));
                if (EditorGUI.EndChangeCheck())
                {
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    SaveManager.Save();
                }
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUI.indentLevel--;
            
            EditorGUILayout.EndVertical();
            
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
        

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.standardVerticalSpacing;
        }


        private static void ResetToDefault(SerializedProperty prop)
        {
            var type = prop.serializedObject.targetObject.GetType();
            
            var serialized2 = ScriptableObject.CreateInstance(type);
            var serializedCopy = new SerializedObject(serialized2);
            Object.DestroyImmediate(serialized2);
            
            prop.serializedObject.CopyFromSerializedPropertyIfDifferent(serializedCopy.FindProperty(prop.propertyPath));
        }


        private void DrawResetButton(SerializedProperty property)
        {
            GUI.backgroundColor = UtilEditor.Red;
            
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog($"Reset {property.FindPropertyRelative("key").stringValue}",
                        $"Are you sure you want to reset {property.FindPropertyRelative("key").stringValue} to its default value.",
                        "Reset", "Cancel"))
                {
                    ResetToDefault(property.FindPropertyRelative("value"));
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    SaveManager.Save();
                    return;
                }
            }

            GUI.backgroundColor = UtilEditor.SettingsAssetEditor.BackgroundColor;
        }
    }
}