using System.Globalization;
using UnityEngine;

/*
 * 
 *  Save Manager
 *							  
 *	Save Manager DataTypeHelper
 *      A helper class to make converting types to and from string easier, this is only for the editor code and should not be used elsewhere...
 *			
 *  Warning:
 *	    Please refrain from editing this script as it will cause issues to the asset...
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
    public static class DataTypeHelper
    {
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an int for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Int</returns>
        public static int IntConvert(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new int() 
                : int.Parse(value);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an float for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Float</returns>
        public static float FloatConvert(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new float() 
                : float.Parse(value);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an short for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Short</returns>
        public static short ShortConvert(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new short() 
                : short.Parse(value);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an long for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Long</returns>
        public static long LongConvert(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new long() 
                : long.Parse(value);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an double for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Double</returns>
        public static double DoubleConvert(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new double() 
                : double.Parse(value);
        }

        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an byte for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Byte</returns>
        public static byte ByteConvert(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new byte() 
                : byte.Parse(value);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an bool for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Bool</returns>
        public static bool BoolConvert(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            var _value = value.Trim().ToLower();

            return _value.Equals("true");
        }
        

        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an Vector2 for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Vector2</returns>
        public static Vector2 Vector2Convert(string value)
        {
            if (string.IsNullOrEmpty(value)) return new Vector2();
            var _temp = value.Split(',');
            
            if (_temp.Length < 2)
                return new Vector2();
            
            var _array = new float[2];
            var _xValue = (_temp[0].Substring(1, _temp[0].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _yValue = (_temp[1].Substring(0, _temp[1].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();

            _array[0] = float.TryParse(_xValue, out var _xResult) 
                ? _xResult
                : 0f;
            
            _array[1] = float.TryParse(_yValue, out var _yResult) 
                ? _yResult
                : 0f;

            return new Vector2(_array[0], _array[1]);
        }


        /// <summary>
        /// FOR EDITOR ONLY!
        /// Fixes issues with the Vector2 string to allow it to be written to the file.
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Corrected String</returns>
        public static string CorrectVector2Format(string value)
        {
            var _temp = value.Split(',');
            
            if (_temp.Length < 2) return "0f, 0f";
            
            var _array = new string[2];
            _array[0] = _temp[0] + "f";

            var _startTemp = _temp[1].Substring(0, _temp[1].Length - 1);
            var _endTemp = _temp[1].Substring(_temp[1].Length - 1, 1);

            _array[1] = $"{_startTemp}f{_endTemp}";
            return $"{_array[0]},{_array[1]}";
        }


        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an Vector3 for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Vector3</returns>
        public static Vector3 Vector3Convert(string value)
        {
            if (string.IsNullOrEmpty(value)) return new Vector3();
            var _temp = value.Split(',');


            if (_temp.Length < 3)
                return new Vector3();

            var _array = new float[3];

            var _xValue = (_temp[0].Substring(1, _temp[0].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _yValue = (_temp[1].Substring(1, _temp[1].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _zValue = (_temp[2].Substring(1, _temp[2].Length - 2), CultureInfo.InvariantCulture.NumberFormat).ToString();

            _array[0] = float.TryParse(_xValue, out var _xResult) 
                ? _xResult
                : 0f;
            
            _array[1] = float.TryParse(_yValue, out var _yResult) 
                ? _yResult
                : 0f;
            
            _array[2] = float.TryParse(_zValue, out var _zResult) 
                ? _zResult 
                : 0f;
            
            return new Vector3(_array[0], _array[1], _array[2]);
        }

        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Fixes issues with the Vector3 string to allow it to be written to the file.
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Corrected String</returns>
        public static string CorrectVector3Format(string value)
        {
            var _temp = value.Split(',');
            
            if (_temp.Length < 3) return "0f, 0f, 0f";
            
            var _array = new string[3];
            _array[0] = _temp[0] + "f";
            _array[1] = _temp[1] + "f";

            var _startTemp = _temp[2].Substring(0, _temp[2].Length - 1);
            var _endTemp = _temp[2].Substring(_temp[2].Length - 1, 1);

            _array[2] = $"{_startTemp}f{_endTemp}";
            return $"{_array[0]},{_array[1]},{_array[2]}";
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an Vector4 for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Vector4</returns>
        public static Vector4 Vector4Convert(string value)
        {
            if (string.IsNullOrEmpty(value)) return new Vector4();
            var _temp = value.Split(',');

            if (_temp.Length < 4)
                return new Vector4();

            var _array = new float[4];

            var _xValue = (_temp[0].Substring(1, _temp[0].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _yValue = (_temp[1].Substring(1, _temp[1].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _zValue = (_temp[2].Substring(1, _temp[2].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _wValue = (_temp[3].Substring(1, _temp[3].Length - 2), CultureInfo.InvariantCulture.NumberFormat).ToString();

            
            _array[0] = float.TryParse(_xValue, out var _xResult) 
                ? _xResult
                : 0f;
            
            _array[1] = float.TryParse(_yValue, out var _yResult) 
                ? _yResult
                : 0f;
            
            _array[2] = float.TryParse(_zValue, out var _zResult) 
                ? _zResult 
                : 0f;
            
            _array[3] = float.TryParse(_wValue, out var _wResult) 
                ? _wResult 
                : 0f;
            
            return new Vector4(_array[0], _array[1], _array[2], _array[3]);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Fixes issues with the Vector4 string to allow it to be written to the file.
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Corrected String</returns>
        public static string CorrectVector4Format(string value)
        {
            var _temp = value.Split(',');

            if (_temp.Length < 4) return "0f, 0f, 0f, 0f";
            
            var _array = new string[4];
            _array[0] = _temp[0] + "f";
            _array[1] = _temp[1] + "f";
            _array[2] = _temp[2] + "f";

            var _startTemp = _temp[3].Substring(0, _temp[3].Length - 1);
            var _endTemp = _temp[3].Substring(_temp[3].Length - 1, 1);
            
            _array[3] = $"{_startTemp}f{_endTemp}";
            return $"{_array[0]},{_array[1]},{_array[2]},{_array[3]}";
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Converts a string into an Color for use in the editor tool. 
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Color</returns>
        public static Color ColorConvert(string value)
        {
            if (string.IsNullOrEmpty(value)) return new Color();
            var _temp = value.Split(',');

            if (_temp.Length < 4)
                return new Color();
            
            var _array = new float[4];

            var _xValue = (_temp[0].Substring(5, _temp[0].Length - 5), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _yValue = (_temp[1].Substring(1, _temp[1].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _zValue = (_temp[2].Substring(1, _temp[2].Length - 1), CultureInfo.InvariantCulture.NumberFormat).ToString();
            var _wValue = (_temp[3].Substring(1, _temp[3].Length - 2), CultureInfo.InvariantCulture.NumberFormat).ToString();
            
            _array[0] = float.TryParse(_xValue, out var _xResult) 
                ? _xResult
                : 0f;
            
            _array[1] = float.TryParse(_yValue, out var _yResult) 
                ? _yResult
                : 0f;
            
            _array[2] = float.TryParse(_zValue, out var _zResult) 
                ? _zResult 
                : 0f;
            
            _array[3] = float.TryParse(_wValue, out var _wResult) 
                ? _wResult 
                : 0f;
            
            return new Color(_array[0], _array[1], _array[2], _array[3]);
        }
        
        
        /// <summary>
        /// FOR EDITOR ONLY!
        /// Fixes issues with the Color string to allow it to be written to the file.
        /// </summary>
        /// <param name="value">string to read</param>
        /// <returns>Corrected String</returns>
        public static string CorrectColorFormat(string value)
        {
            var _temp = value.Split(',');
            
            if (_temp.Length < 4) return "0f, 0f, 0f, 0f";
            
            var _array = new string[4];
            
            _array[0] = _temp[0].Substring(4, _temp[0].Length - 4) + "f";
            _array[1] = _temp[1] + "f";
            _array[2] = _temp[2] + "f";

            var _startTemp = _temp[3].Substring(0, _temp[3].Length - 1);
            var _endTemp = _temp[3].Substring(_temp[3].Length - 1, 1);

            _array[3] = $"{_startTemp}f{_endTemp}";
            return $"{_array[0]},{_array[1]},{_array[2]},{_array[3]}";
        }
    }
}