using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

/*
 * 
 *  Save Manager
 *							  
 *	Save Manager Data Editor
 *      The editor script for the Save Data Editor Window.
 *			
 *  Warning:
 *	    Please refrain from editing this script as it will cause issues to the assets...
 *			
 *  Written by:
 *      Jonathan Carter
 *
 *  Published By:
 *      Carter Games
 *      E: hello@carter.games
 *      W: https://www.carter.games
 *		
 *  Version: 1.1.0
 *	Last Updated: 24/07/2021 (d/m/y)							
 * 
 */

namespace CarterGames.Assets.SaveManager.Editor
{
    public enum CollectionTypes
    {
        None,
        Array,
        List,
        Queue,
        Stack,
    };
    
    
    
    public enum DataTypes
    {
        IntValue,
        FloatValue,
        ShortValue,
        LongValue,
        DoubleValue,
        ByteValue,
        BoolValue,
        StringValue,
        Vector2Value,
        Vector3Value,
        Vector4Value,
        ColorValue,
        QuaternionValue,
        SpriteValue,
        ClassValue
    };
    
    
    
    
    /// <summary>
    /// Controls what is shown on the save data editor window tool.
    /// </summary>
    public class SaveDataEditor : EditorWindow
    {
        private readonly Color32 redCol = new Color32(190, 42, 42, 255);
        
        private readonly string FieldPrefix = "        public";
        private readonly string FieldEndLine = ";";
        private List<string> SaveFieldTypes = new List<string>(6) {"Vector2", "Vector3", "Vector4", "Color", "Quaternion", "Sprite"};
        private readonly string[] CollectionStringValues = new string[5] {"[]", "List<", ">", "Queue<", "Stack<"};

        private readonly string[] TypesNotClasses = new string[14]
        {
            "int", "float", "short", "long", "double", "byte", "bool", "string", "Vector2", "Vector3", "Vector4", "Color", "Quaternion", "Sprite"
        };
        
        private readonly string FileNotFoundWarning =
            "Save Manager | CG: Warning Code 1: Save Data class not found at expected directory (Assets/Scripts/Save/...)\n NOTE: Ignore this warning if you have yet to generate a save class from the editor.";

        private readonly string UnableToReadDefaultValueWarning = "Save Manager | CG: Warning Code 2: Unable to read default value from class, will set to default for the field type.";
        
        private CultureInfo nfi;
        
        
        // The position on the tab menu variable
        private int tabPos;

        
        // Variables used in the generate class window
        private bool isCreatingFile;

        private List<SaveManagerField> fields;
        private List<SaveManagerField> readFields;
        
        
        // Variables used in the read class window
        private bool hasReadFile;
        private List<string> readLines;
        private List<DataTypes> readTypes;
        private List<string> readValueNames;
        private List<string> readDefaultValue;
        private List<CollectionTypes> readCollection;
        private List<string> readClassNames;

        
        // Tools for the deselecting of panels
        public Rect deselectWindow;

        
        // Variable used in scroll views
        private Vector2 scrollPos;

        

        /// <summary>
        /// Static | Adds the button to call the editor window under Tools/SaveDataEditor
        /// </summary>
        [MenuItem("Tools/Save Manager | CG/Save Data Editor", priority = 3)]
        public static void ShowWindow()
        {
            GetWindow<SaveDataEditor>("Save Data Editor");
        }
        
        
        /// <summary>
        /// Resets the save file for the user when they press the option in the menu...
        /// </summary>
        [MenuItem("Tools/Save Manager | CG/Reset Save File", priority = 4)]
        public static void ResetSaveFile()
        {
            SaveManager.ResetSaveFile();
        }
        
        
        /// <summary>
        /// Resets the save file for the user when they press the option in the menu...
        /// </summary>
        [MenuItem("Tools/Save Manager | CG/Delete Save File", priority = 5)]
        public static void DeleteSaveFile()
        {
            SaveManager.DeleteSaveFile();
        }


        private void OnEnable()
        {
            nfi = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            nfi.NumberFormat.CurrencyDecimalSeparator = ",";
            
            // Reads the file that currently exists...
            ReadFile();
        }


        /// <summary>
        /// Shows the GUI on the editor window.
        /// </summary>
        public void OnGUI()
        {
            deselectWindow = new Rect(0, 0, position.width, position.height);

            ShowAssetLogo();


            // Label that shows the name of the asset
            CarterEditor.HorizontalCentered(() =>
            {
                CarterEditor.BoldCompact("Save Manager");
            });
            
   
            EditorGUILayout.Space();

            // The tab menu used to decide what is shown on the editor window.
            CarterEditor.HorizontalCentered(() =>
            {
                tabPos = GUILayout.Toolbar(tabPos, new string[] { "Save Data", "About" }, GUILayout.MaxWidth(800f), GUILayout.MaxHeight(25f));

            });
            

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            switch (tabPos)
            {
                case 0:
                    TabOneDisplay();    // show the creation menu
                    break;
                case 1:
                    ShowAboutTab();     // show the about asset menu
                    break;
                default:
                    break;
            }

            // defines the min/max size of the editor window.
            SetMinMaxWindowSize();

            // Makes it so you can deselect elements in the window by adding a button the size of the window that you can't see under everything
            //make sure the following code is at the very end of OnGUI Function
            if (GUI.Button(deselectWindow, "", GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }

        
        
        /// <summary>
        /// Defines the min and max size for the editor window
        /// </summary>
        private void SetMinMaxWindowSize()
        {
            EditorWindow editorWindow = this;
            editorWindow.minSize = new Vector2(400f, 500f);
            editorWindow.maxSize = new Vector2(800f, 750f);
        }
        
        

        /// <summary>
        /// Shows the asset logo or and alt text if it is not in the correct folder.
        /// </summary>
        private void ShowAssetLogo()
        {
            EditorGUILayout.Space();

            CarterEditor.HorizontalCentered(() =>
            {
                // Shows either the Carter Games Logo or an alternative for if the icon is deleted when you import the package
                if (Resources.Load<Texture2D>("Carter Games/Save Manager/LogoSM"))
                {
                    if (GUILayout.Button(Resources.Load<Texture2D>("Carter Games/Save Manager/LogoSM"), GUIStyle.none,
                        GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        GUI.FocusControl(null);
                    }
                }
                else
                {
                    CarterEditor.HorizontalCentered(() =>
                    {
                        EditorGUILayout.LabelField("Carter Games", EditorStyles.boldLabel, GUILayout.Width(100));
                    });
                }
            });
            
            EditorGUILayout.Space();
        }

        
        
        
        /// <summary>
        /// Shows the Generate class display
        /// </summary>
        private void TabOneDisplay()
        {
            EditorGUILayout.HelpBox("Create a new SaveData class here.\n\nYou may just write it yourself, however if you wish " +
                "for the asset to work with the data you want to save, we advise you use the provided editor to generate it. " +
                "To begin, press the add field button, and repeat for each field you need. When done, press the Generate Class button.", MessageType.Info, true);

            GUI.backgroundColor = Color.green;

            CarterEditor.HorizontalCentered(() =>
            {
                if (fields != null)
                    if (fields.Count > 0)
                        isCreatingFile = true;
                
                // Adds a field to the editor...
                if (GUILayout.Button("+ Add Field", GUILayout.Width(90f)))
                {
                    if (!isCreatingFile)
                    {
                        isCreatingFile = true;
                        fields = new List<SaveManagerField> {new SaveManagerField()};
                    }
                    else
                        fields.Add(new SaveManagerField());
                }

                GUI.backgroundColor = Color.white;

            });
            
            EditorGUILayout.Space();

            /*
             * 
             *      Stuff that displays the fields...
             * 
             */

            if (!isCreatingFile) return;
            if (fields.Count <= 0) return;
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.ExpandHeight(true));

            for (var i = 0; i < fields.Count; i++)
            {
                RenderSaveManagerFieldLine(fields[i], i);

                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
            

            
            
            GUI.backgroundColor = Color.green;
            
            CarterEditor.HorizontalCentered(() =>
            {
                if (GUILayout.Button("Generate New SaveData Class", GUILayout.MaxWidth(200f)))
                    GenerateClass();
            });
            
            GUI.backgroundColor = Color.white;
        }




        /// <summary>
        /// Renders a field in the save manager editor...
        /// </summary>
        /// <param name="field">The field data to render</param>
        /// <param name="element">The element in the list of fields to show</param>
        private void RenderSaveManagerFieldLine(SaveManagerField field, int element)
        {
            CarterEditor.VerticalBoxed(() =>
            {
                var _type = field.dataType.ToString().Replace("Value", "");
                    
                CarterEditor.HorizontalCentered(() =>
                {
                    if (field.collectionType.Equals(CollectionTypes.None))
                        CarterEditor.BoldCompact($"{_type} | {field.variableName}");
                    else
                        CarterEditor.BoldCompact($"{_type} {field.collectionType.ToString()} | {field.variableName}");
                });


                CarterEditor.Horizontal(() =>
                {
                    CarterEditor.Vertical(() => RenderLeftFieldsOnVariable(field));
                    
                    CarterEditor.Vertical(() => RenderRightFieldsOnVariable(field));
                    
                    GUI.backgroundColor = Color.red;

                    if (GUILayout.Button(" - ", GUILayout.MaxWidth(CarterEditor.TextWidth("   -  "))))
                        RemoveElement(element);

                    GUI.backgroundColor = Color.white;
                });
            });
        }




        /// <summary>
        /// Renders the left part of a field 
        /// </summary>
        /// <param name="field">The field to render</param>
        private void RenderLeftFieldsOnVariable(SaveManagerField field)
        {
            CarterEditor.Horizontal(() =>
            {
                CarterEditor.Compact( "Variable Name:");

                field.variableName = EditorGUILayout.TextField(field.variableName);
            });


            if (!field.dataType.Equals(DataTypes.ClassValue) && !field.dataType.Equals(DataTypes.SpriteValue) && field.collectionType.Equals(CollectionTypes.None))
            {
                CarterEditor.Horizontal(() =>
                {
                    CarterEditor.Compact("Default Value:");

                    switch (field.dataType)
                    {
                        case DataTypes.IntValue:
                            field.defaultValue = EditorGUILayout.IntField("", DataTypeHelper.IntConvert(field.defaultValue)).ToString();
                            break;
                        case DataTypes.FloatValue:
                            field.defaultValue = EditorGUILayout
                                .FloatField("", DataTypeHelper.FloatConvert(field.defaultValue)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case DataTypes.ShortValue:
                            field.defaultValue = EditorGUILayout
                                .IntField("", DataTypeHelper.ShortConvert(field.defaultValue)).ToString();
                            break;
                        case DataTypes.LongValue:
                            field.defaultValue = EditorGUILayout
                                .FloatField("", DataTypeHelper.LongConvert(field.defaultValue)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case DataTypes.DoubleValue:
                            field.defaultValue = EditorGUILayout
                                .DoubleField("", DataTypeHelper.DoubleConvert(field.defaultValue)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case DataTypes.ByteValue:
                            field.defaultValue = EditorGUILayout.IntField("", DataTypeHelper.ByteConvert(field.defaultValue)).ToString();
                            break;
                        case DataTypes.BoolValue:
                            field.defaultValue = EditorGUILayout
                                .Toggle("", DataTypeHelper.BoolConvert(field.defaultValue)).ToString();
                            break;
                        case DataTypes.StringValue:
                            field.defaultValue = EditorGUILayout.TextField(field.defaultValue);
                            break;
                        case DataTypes.Vector2Value:
                            field.defaultValue = EditorGUILayout
                                .Vector2Field("", DataTypeHelper.Vector2Convert(field.defaultValue)).ToString("G");
                            break;
                        case DataTypes.Vector3Value:
                            field.defaultValue = EditorGUILayout
                                .Vector3Field("", DataTypeHelper.Vector3Convert(field.defaultValue)).ToString("G");
                            break;
                        case DataTypes.Vector4Value:
                            field.defaultValue = EditorGUILayout
                                .Vector4Field("", DataTypeHelper.Vector4Convert(field.defaultValue)).ToString("G");
                            break;
                        case DataTypes.ColorValue:
                            field.defaultValue = EditorGUILayout
                                .ColorField("", DataTypeHelper.ColorConvert(field.defaultValue)).ToString();
                            break;
                        case DataTypes.QuaternionValue:
                            field.defaultValue = EditorGUILayout
                                .Vector4Field("", DataTypeHelper.Vector4Convert(field.defaultValue)).ToString("G");
                            break;
                        default:
                            break;
                    }
                });
            }


            CarterEditor.Horizontal(() =>
            {
                if (field.dataType != DataTypes.ClassValue) return;
                            
                CarterEditor.Compact("Class Name:");

                field.className = EditorGUILayout.TextField(field.className);
            });
        }
        
        



        /// <summary>
        /// Renders the right part of a field
        /// </summary>
        /// <param name="field">The field to render</param>
        private void RenderRightFieldsOnVariable(SaveManagerField field)
        {
            CarterEditor.Horizontal(() =>
            {
                CarterEditor.Compact("Data Type:");

                GUI.backgroundColor = Color.yellow;
                field.ChangeType(
                    (DataTypes) EditorGUILayout.EnumPopup(field.dataType));
                

                GUI.backgroundColor = Color.green;

                if (GUILayout.Button(" + ", GUILayout.MaxWidth(CarterEditor.TextWidth("  +  "))))
                    fields.Add(new SaveManagerField());

                GUI.backgroundColor = Color.white;
            });


            CarterEditor.Horizontal(() =>
            {
                CarterEditor.Compact("Collection Type:");

                GUI.backgroundColor = Color.yellow;
                field.collectionType =
                    (CollectionTypes) EditorGUILayout.EnumPopup(field.collectionType);
            });
        }
        
        


        /// <summary>
        /// Removes the field at the position entered...
        /// </summary>
        /// <param name="i"></param>
        private void RemoveElement(int i)
        {
            fields.Remove(fields[i]);
        }





        /// <summary>
        /// Generates the SaveData.cs class based on the values inputted
        /// </summary>
        private void GenerateClass(bool isRead = false)
        {
            var copyPath = "Assets/Scripts/Save/SaveData.cs";

            // Creates directories if needed...
            if (Directory.Exists(Application.dataPath + "/Scripts/"))
            {
                if (!Directory.Exists(Application.dataPath + "/Scripts/Save/"))
                {
                    AssetDatabase.CreateFolder("Assets/Scripts", "Save");
                }
            }
            else
            {
                AssetDatabase.CreateFolder("Assets", "Scripts");
                AssetDatabase.CreateFolder("Assets/Scripts", "Save");
            }



            if (File.Exists(copyPath))
                File.Delete(copyPath);

            using (StreamWriter _outfile = new StreamWriter(copyPath))
            {
                WriteHeaderOfClass(_outfile);


                if (fields.Count > 0)
                {
                    for (var i = 0; i < fields.Count; i++)
                    {
                        _outfile.WriteLine(GetFieldString(fields[i], i));
                    }
                }

                _outfile.WriteLine("    }");
                _outfile.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }




        /// <summary>
        /// Writes the header of the class...
        /// </summary>
        /// <param name="outfile"></param>
        /// <returns></returns>
        private void WriteHeaderOfClass(StreamWriter outfile)
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("");
            outfile.WriteLine("// *** Class Generated By SaveDataEditor ***");
            outfile.WriteLine("namespace CarterGames.Assets.SaveManager");
            outfile.WriteLine("{");
            outfile.WriteLine("    [Serializable]");
            outfile.WriteLine("    public class SaveData");
            outfile.WriteLine("    {");
        }
        
        
        

        
        /// <summary>
        /// Gets the string for the field to be written to the file.
        /// </summary>
        /// <param name="field">The field to read</param>
        /// <param name="pos">The position in the list, only used if there is no variable name...</param>
        /// <returns>String</returns>
        private string GetFieldString(SaveManagerField field, int pos)
        {
            var _line = string.Empty;
            var _type = field.dataType.ToString().Replace("Value", "");

            // Makes a name for the variable is the field is blank...
            if (string.IsNullOrEmpty(field.variableName) || string.IsNullOrWhiteSpace(field.variableName))
                field.variableName = _type.ToLower() + pos;

            if (field.collectionType.Equals(CollectionTypes.None))
            {
                if (TypeContains(_type))
                {
                    _line = field.dataType.Equals(DataTypes.ClassValue)
                        ? $"{FieldPrefix} {field.className} {field.variableName}"
                        : $"{FieldPrefix} {_type} {field.variableName}";
                    
                    
                    if (field.defaultValue != null)
                    {
                        if (field.defaultValue.Length > 0)
                        {
                            if (field.dataType.Equals(DataTypes.Vector2Value))
                                return $"{_line} = new {_type}{DataTypeHelper.CorrectVector2Format(field.defaultValue)}{FieldEndLine}";
                            else if (field.dataType.Equals(DataTypes.Vector3Value))
                                return $"{_line} = new {_type}{DataTypeHelper.CorrectVector3Format(field.defaultValue)}{FieldEndLine}";
                            else if (field.dataType.Equals(DataTypes.Vector4Value))
                                return $"{_line} = new {_type}{DataTypeHelper.CorrectVector4Format(field.defaultValue)}{FieldEndLine}";
                            else if (field.dataType.Equals(DataTypes.ColorValue))
                                return $"{_line} = new {_type}{DataTypeHelper.CorrectColorFormat(field.defaultValue)}{FieldEndLine}";
                            else if (field.dataType.Equals(DataTypes.QuaternionValue))
                                return $"{_line} = new {_type}{DataTypeHelper.CorrectVector4Format(field.defaultValue)}{FieldEndLine}";
                            
                            return $"{_line} = new {_type}{field.defaultValue}{FieldEndLine}";
                        }
                        
                        return _line + FieldEndLine;
                    }
                    
                    return _line + FieldEndLine;
                }
                
                _line = field.dataType.Equals(DataTypes.ClassValue)
                    ? $"{FieldPrefix} {field.className} {field.variableName}"
                    : $"{FieldPrefix} {_type.ToLower()} {field.variableName}";

                if (field.defaultValue != null)
                {
                    if (field.defaultValue.Length > 0)
                    {
                        if (field.dataType == DataTypes.StringValue)
                            return $"{_line} = \"{field.defaultValue}\"{FieldEndLine}";
                        else if (field.dataType == DataTypes.FloatValue)
                            return $"{_line} = {field.defaultValue}f{FieldEndLine}";
                        else
                            return $"{_line} = {field.defaultValue}{FieldEndLine}";
                    }

                    return _line + FieldEndLine;
                }
                
                return _line + FieldEndLine;
            }


            switch (field.collectionType)
            {
                case CollectionTypes.Array:

                    if (TypeContains(_type))
                        return $"{FieldPrefix} Save{_type}[] {field.variableName}{FieldEndLine}";

                    return field.dataType.Equals(DataTypes.ClassValue)
                        ? $"{FieldPrefix} {field.className}[] {field.variableName}{FieldEndLine}"
                        : $"{FieldPrefix} {_type.ToLower()}[] {field.variableName}{FieldEndLine}";

                case CollectionTypes.List:

                    if (TypeContains(_type))
                        return $"{FieldPrefix} List<Save{_type}> {field.variableName}{FieldEndLine}";

                    return field.dataType.Equals(DataTypes.ClassValue)
                        ? $"{FieldPrefix} List<{field.className}> {field.variableName}{FieldEndLine}"
                        : $"{FieldPrefix} List<{_type.ToLower()}> {field.variableName}{FieldEndLine}";

                case CollectionTypes.Queue:

                    if (TypeContains(_type))
                        return $"{FieldPrefix} Queue<Save{_type}> {field.variableName}{FieldEndLine}";

                    return field.dataType.Equals(DataTypes.ClassValue)
                        ? $"{FieldPrefix} Queue<{field.className}> {field.variableName}{FieldEndLine}"
                        : $"{FieldPrefix} Queue<{_type.ToLower()}> {field.variableName}{FieldEndLine}";

                case CollectionTypes.Stack:

                    if (TypeContains(_type))
                        return $"{FieldPrefix} Stack<Save{_type}> {field.variableName}{FieldEndLine}";

                    return field.dataType.Equals(DataTypes.ClassValue)
                        ? $"{FieldPrefix} Stack<{field.className}> {field.variableName}{FieldEndLine}"
                        : $"{FieldPrefix} Stack<{_type.ToLower()}> {field.variableName}{FieldEndLine}";
                default:
                    return "Error";
            }
        }




        private bool TypeContains(string toCheck)
        {
            foreach (var _t in SaveFieldTypes)
            {
                if (toCheck.Equals(_t)) return true;
            }

            return false;
        }




        /// <summary>
        /// Shows the about tab
        /// </summary>
        private void ShowAboutTab()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            CarterEditor.HorizontalCentered(() =>
            {
                EditorGUILayout.LabelField("Version: 1.1.0", GUILayout.Width(90));
            });
            

            CarterEditor.HorizontalCentered(() =>
            {
                EditorGUILayout.LabelField("Released: 06/07/2021", GUILayout.Width(140));
            });


            EditorGUILayout.Space();

            
            
            CarterEditor.HorizontalCentered(() =>
            {
                if (GUILayout.Button("Documentation", GUILayout.Width(100)))
                {
                    Application.OpenURL("https://carter.games/savemanager");
                }
                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("Discord", GUILayout.Width(65f)))
                {
                    Application.OpenURL("https://carter.games/discord");
                }
                GUI.backgroundColor = redCol;
                if (GUILayout.Button("Report Issue", GUILayout.Width(100f)))
                {
                    Application.OpenURL("https://carter.games/report");
                }
                GUI.backgroundColor = Color.white;
            });
            

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("Save Manager is a tool to help beginners save and load their games with ease. The asset allow the saving and loading of multiple data types.\n\n" +
                "should you need any help with the asset, please get in touch either via our community discord server or via email (hello@carter.games)", MessageType.Info);

            EditorGUILayout.Space();
        }

        
        
        /// <summary>
        /// Reads the SaveData.cs file and gets the data types, variable names and if they are array or not there.
        /// </summary>
        private void ReadFile()
        {
            if (!File.Exists("Assets/Scripts/Save/SaveData.cs"))
            {
                Debug.LogWarning(FileNotFoundWarning);
                return;
            }
            if (hasReadFile) return;
            
            var _fileData = File.ReadAllLines("Assets/Scripts/Save/SaveData.cs");
            Debug.Log(_fileData.Length);
            readLines = new List<string>();

            for (int i = 0; i < _fileData.Length; i++)
            {
                if (!_fileData[i].Contains("public")) continue;
                
                Debug.Log(_fileData[i]);
                readLines.Add(_fileData[i]);
            }

            ConvertStringsToData();
        }

        /// <summary>
        /// Converts the read lines from the ReadFile() method to their respective types so it can be used in the editor
        /// </summary>
        private void ConvertStringsToData()
        {
            readFields = new List<SaveManagerField>();
            readTypes = new List<DataTypes>();
            readValueNames = new List<string>();
            readCollection = new List<CollectionTypes>();
            readClassNames = new List<string>();
            readDefaultValue = new List<string>();

            for (int i = 0; i < readLines.Count; i++)
            {
                if (i <= 0) continue;

                var _line = readLines[i].Trim();
                var _splitLine = _line.Split(' ');


                // Gets the data type/class type....
                if (!IsClass(readLines[i].Trim().Split(' ')[1]))
                {
                    readTypes.Add((DataTypes) Enum.Parse(typeof(DataTypes),
                        readLines[i].Trim().Split(' ')[1].Replace("Save", "").Replace("[]", "").Replace("List<", "")
                            .Replace(">", "").Replace("Queue<", "").Replace("Stack<", "") + "Value", true));
                    readClassNames.Add("");
                }
                else
                {
                    readTypes.Add(DataTypes.ClassValue);

                    var _className = _splitLine[1];
                    _className = ReplaceWithBlank(_className, CollectionStringValues);
                    readClassNames.Add(_className);
                }


                // Gets the collection type...
                if (readLines[i].Contains("List<"))
                    readCollection.Add(CollectionTypes.List);
                else if (readLines[i].Contains("[]"))
                    readCollection.Add(CollectionTypes.Array);
                else if (readLines[i].Contains("Queue<"))
                    readCollection.Add(CollectionTypes.Queue);
                else if (readLines[i].Contains("Stack<"))
                    readCollection.Add(CollectionTypes.Stack);
                else
                    readCollection.Add(CollectionTypes.None);


                // Gets the variable name....
                var _name = _splitLine[2];
                _name = ReplaceWithBlank(_name, new char[1] {';'}, true);
                readValueNames.Add(_name);


                // Gets the default value...
                if (readClassNames[i - 1] != string.Empty)
                {
                    Debug.Log("Was Class");
                    readDefaultValue.Add(string.Empty);
                    
                    readFields.Add(new SaveManagerField(readValueNames[i - 1], readTypes[i - 1], readCollection[i - 1],
                        readDefaultValue[i - 1], readClassNames[i-1]));
                }
                else
                {
                    var _type = readTypes[i - 1].ToString().Replace("Value", "");

                    if (readCollection[i - 1].Equals(CollectionTypes.None))
                    {
                        // Checks to see if the type is a vector/color/etc.....
                        if (TypeContains(_type))
                        {
                            // Turns the default value into something readable....
                            // If it is a sprite or class it returns an empty string...
                            var _value = string.Empty;

                            if (!readTypes[i - 1].Equals(DataTypes.SpriteValue))
                            {
                                if (_line.Contains("("))
                                    _value =
                                        $"({_line.Split('(')[1].Substring(0, _line.Split('(')[1].Length - 1).Replace('f', ' ')}";
                                else
                                {
                                    Debug.LogWarning(UnableToReadDefaultValueWarning);
                                    _value = string.Empty;
                                }
                            }
                            else
                                _value = string.Empty;

                            readDefaultValue.Add(_value);
                        }
                        // If the type is a string, it gets rid of the "" from the string declaration...
                        else if (readTypes[i - 1].Equals(DataTypes.StringValue))
                        {
                            var _value = _splitLine[4].Substring(0, _splitLine[4].Length - 1);

                            _value = ReplaceWithBlank(_value, new char[1] {'"'}, true);

                            readDefaultValue.Add(_value);
                        }
                        // If the type is float or double, it gets rid of the "f" from the end of the line...
                        else if (readTypes[i - 1].Equals(DataTypes.FloatValue) ||
                                 readTypes[i - 1].Equals(DataTypes.DoubleValue))
                        {
                            readDefaultValue.Add(_splitLine[4].Substring(0, _splitLine[4].Length - 2));
                        }
                        // Else it returns the line as it is without the end line char...
                        else
                        {
                            readDefaultValue.Add(_splitLine[4].Substring(0, _splitLine[4].Length - 1));
                        }
                    }
                    // If the collection type if not a single value it just returns an empty string...
                    else
                        readDefaultValue.Add(string.Empty);

                    Debug.Log(
                        $"{readTypes[i - 1]} {readCollection[i - 1]} {readValueNames[i - 1]} = {readDefaultValue[i - 1]}");

                    readFields.Add(new SaveManagerField(readValueNames[i - 1], readTypes[i - 1], readCollection[i - 1],
                        readDefaultValue[i - 1]));
                }
            }

            // Sets the fields to the read fields...
            hasReadFile = true;
            fields = readFields;
        }

        /// <summary>
        /// Checks to see if the dataType is a class or not, Used in the ConvertStringsToData() method
        /// </summary>
        /// <param name="checkValue">string value to check</param>
        /// <returns></returns>
        private bool IsClass(string checkValue)
        {
            return !TypesNotClasses.Contains(checkValue);
        }




        private string ReplaceWithBlank(string value, char[] toReplace, bool shouldTrim = false)
        {
            for (int i = 0; i < toReplace.Length; i++)
            {
                value = value.Replace(toReplace[i], ' ');
            }

            return shouldTrim 
                ? value.Trim() 
                : value;
        }
        
        
        private string ReplaceWithBlank(string value, string[] toReplace, bool shouldTrim = false)
        {
            for (int i = 0; i < toReplace.Length; i++)
            {
                value = value.Replace(toReplace[i], "");
            }

            return shouldTrim 
                ? value.Trim() 
                : value;
        }
        
    }
    



    [Serializable]
    public class SaveManagerField
    {
        public string variableName;
        public DataTypes dataType;
        public CollectionTypes collectionType;
        public string defaultValue;
        public string className;
        

        public SaveManagerField(){}

        public SaveManagerField(string vName, DataTypes dType, CollectionTypes cType, string defValue)
        {
            variableName = vName;
            dataType = dType;
            collectionType = cType;
            defaultValue = defValue;
        }


        public SaveManagerField(string vName, DataTypes dType, CollectionTypes cType, string defValue, string cName)
        {
            variableName = vName;
            dataType = dType;
            collectionType = cType;
            defaultValue = defValue;
            className = cName;
        }


        public void ChangeType(DataTypes newType)
        {
            if (newType == dataType) return;
            defaultValue = string.Empty;
            dataType = newType;
        }
    }



    public static class CarterEditor
    {
        /// <summary>
        /// Make a EditorGUILayout.BeginHorizontal...
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method....</remarks>
        public static void Horizontal(Action blockElements)
        {
            EditorGUILayout.BeginHorizontal();
            blockElements();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Make a EditorGUILayout.BeginHorizontal...with a Box GUIStyle...
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method....</remarks>
        public static void HorizontalBoxed(Action blockElements)
        {
            EditorGUILayout.BeginHorizontal("box");
            blockElements();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Make a EditorGUILayout.BeginHorizontal...with content that is centered...
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method....</remarks>
        public static void HorizontalCentered(Action blockElements)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            blockElements();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Make a EditorGUILayout.BeginVertical...
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method....</remarks>
        public static void Vertical(Action blockElements)
        {
            EditorGUILayout.BeginVertical();
            blockElements();
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Make a EditorGUILayout.BeginVertical...with a Box GUIStyle...
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method....</remarks>
        public static void VerticalBoxed(Action blockElements)
        {
            EditorGUILayout.BeginVertical("Box");
            blockElements();
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Make a EditorGUILayout.BeginVertical...with content that is centered...
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method....</remarks>
        public static void VerticalCentered(Action blockElements)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            blockElements();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        public static void Plain(string labelString, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(labelString, options);
        }


        public static void Bold(string labelString, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(labelString, EditorStyles.boldLabel, options);
        }


        public static void Compact(string labelString)
        {
            labelString += " ";
            EditorGUILayout.LabelField(labelString, GUILayout.Width(TextWidth(labelString)));
        }


        public static float TextWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
        
        
        public static void BoldCompact(string labelString)
        {
            labelString += "      ";
            EditorGUILayout.LabelField(labelString, EditorStyles.boldLabel, GUILayout.Width(TextWidth(labelString)));
        }
    }
}