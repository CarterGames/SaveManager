using UnityEngine;
using System;

/*
 * 
 *  Save Manager
 *							  
 *	Save Manager Alt Data Types Script
 *      Stores a collection of additional save types that can't be saved by default.
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

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Holds the values required to save a Vector2
    /// </summary>
    [Serializable]
    public struct SaveVector2
    {
        public float x;
        public float y;
        
        public SaveVector2(float vecX, float vecY)
        {
            x = vecX;
            y = vecY;
        }

        public override string ToString()
            => $"{x} {y}";

        public static implicit operator Vector2(SaveVector2 rValue)
            => new Vector2(rValue.x, rValue.y);

        public static implicit operator SaveVector2(Vector2 rValue)
            => new SaveVector2(rValue.x, rValue.y);
        
        public static SaveVector2 operator +(SaveVector2 a, SaveVector2 b) 
            => new SaveVector2(a.x + b.x, a.y + b.y);
 
        public static SaveVector2 operator -(SaveVector2 a, SaveVector2 b)
            => new SaveVector2(a.x - b.x, a.y - b.y);
 
        public static SaveVector2 operator -(SaveVector2 a)
            => new SaveVector2(-a.x, -a.y);
 
        public static SaveVector2 operator *(SaveVector2 a, float m)
            => new SaveVector2(a.x * m, a.y * m);
 
        public static SaveVector2 operator *(float m, SaveVector2 a)
            => new SaveVector2(a.x * m, a.y * m);
 
        public static SaveVector2 operator /(SaveVector2 a, float d)
            => new SaveVector2(a.x / d, a.y / d);
    }

    /// <summary>
    /// Holds the values required to save a Vector3
    /// </summary>
    [Serializable]
    public struct SaveVector3
    {
        public float x;
        public float y;
        public float z;

        public SaveVector3(float vecX, float vecY, float vecZ)
        {
            x = vecX;
            y = vecY;
            z = vecZ;
        }

        public override string ToString()
            => $"{x} {y} {z}";

        public static implicit operator Vector3(SaveVector3 rValue)
            => new Vector3(rValue.x, rValue.y, rValue.z);

        public static implicit operator SaveVector3(Vector3 rValue)
            => new SaveVector3(rValue.x, rValue.y, rValue.z);

        public static SaveVector3 operator +(SaveVector3 a, SaveVector3 b) 
            => new SaveVector3(a.x + b.x, a.y + b.y, a.z + b.z);
 
        public static SaveVector3 operator -(SaveVector3 a, SaveVector3 b) 
            => new SaveVector3(a.x - b.x, a.y - b.y, a.z - b.z);
 
        public static SaveVector3 operator -(SaveVector3 a)
            => new SaveVector3(-a.x, -a.y, -a.z);
 
        public static SaveVector3 operator *(SaveVector3 a, float m)
            => new SaveVector3(a.x * m, a.y * m, a.z * m);
 
        public static SaveVector3 operator *(float m, SaveVector3 a)
            => new SaveVector3(a.x * m, a.y * m, a.z * m);
 
        public static SaveVector3 operator /(SaveVector3 a, float d)
            => new SaveVector3(a.x / d, a.y / d, a.z / d);
    }

    /// <summary>
    /// Holds the values required to save a Vector4
    /// </summary>
    [Serializable]
    public struct SaveVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SaveVector4(float vecX, float vecY, float vecZ, float vecW)
        {
            x = vecX;
            y = vecY;
            z = vecZ;
            w = vecW;
        }

        public override string ToString()
            => $"{x} {y} {z} {w}";

        public static implicit operator Vector4(SaveVector4 rValue)
            => new Vector4(rValue.x, rValue.y, rValue.z, rValue.w);

        public static implicit operator SaveVector4(Vector4 rValue)
            => new Vector4(rValue.x, rValue.y, rValue.z, rValue.w);
        
        public static SaveVector4 operator +(SaveVector4 a, SaveVector4 b) 
            => new SaveVector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
 
        public static SaveVector4 operator -(SaveVector4 a, SaveVector4 b) 
            => new SaveVector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
 
        public static SaveVector4 operator -(SaveVector4 a)
            => new SaveVector4(-a.x, -a.y, -a.z, -a.w);
 
        public static SaveVector4 operator *(SaveVector4 a, float m)
            => new SaveVector4(a.x * m, a.y * m, a.z * m, a.w * m);
 
        public static SaveVector4 operator *(float m, SaveVector4 a)
            => new SaveVector4(a.x * m, a.y * m, a.z * m, a.w * m);
 
        public static SaveVector4 operator /(SaveVector4 a, float d)
            => new SaveVector4(a.x / d, a.y / d, a.z / d, a.w / d);
    }

    /// <summary>
    /// Holds the values required to save a Color
    /// </summary>
    [Serializable]
    public struct SaveColor
    {
        public float r;
        public float g;
        public float b;
        public float a;
        
        public SaveColor(float cR, float cG, float cB, float cA)
        {
            r = cR;
            g = cG;
            b = cB;
            a = cA;
        }

        public override string ToString()
        {
            return $"{r} {g} {b} {a}";
        }
        
        public static implicit operator Color(SaveColor rValue)
            => new Color(rValue.r, rValue.g, rValue.b, rValue.a);

        public static implicit operator SaveColor(Color rValue)
            => new SaveColor(rValue.r, rValue.g, rValue.b, rValue.a);
    }
    
    /// <summary>
    /// Holds the values required to save a Color32
    /// </summary>
    [Serializable]
    public struct SaveColor32
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
        
        
        public SaveColor32(byte cR, byte cG, byte cB, byte cA)
        {
            r = cR;
            g = cG;
            b = cB;
            a = cA;
        }

        public override string ToString()
        {
            return $"{r} {g} {b} {a}";
        }
        
        public static implicit operator Color32(SaveColor32 rValue)
            => new Color32(rValue.r, rValue.g, rValue.b, rValue.a);

        public static implicit operator SaveColor32(Color32 rValue)
            => new SaveColor32(rValue.r, rValue.g, rValue.b, rValue.a);
    }
    
    /// <summary>
    /// Holds the values required to save a Quaternion
    /// </summary>
    [Serializable]
    public struct SaveQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SaveQuaternion(float vecX, float vecY, float vecZ, float vecW)
        {
            x = vecX;
            y = vecY;
            z = vecZ;
            w = vecW;
        }

        public override string ToString()
            => $"{x} {y} {z} {w}";

        public static implicit operator Quaternion(SaveQuaternion rValue)
            => new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);

        public static implicit operator SaveQuaternion(Quaternion rValue)
            => new SaveQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }

    /// <summary>
    /// Holds the values required to save a Sprite
    /// </summary>
    [Serializable]
    public struct SaveSprite
    {
        public string name;
        public int x;
        public int y;
        public byte[] data;

        public SaveSprite(int sX, int sY, byte[] sData, string sName)
        {
            name = sName;
            x = sX;
            y = sY;
            data = sData;
        }

        public override string ToString()
            => $"{name}";

        public static implicit operator Sprite(SaveSprite rValue)
        {
            var _tex = new Texture2D(rValue.x, rValue.y);
            _tex.LoadImage(rValue.data);
            var _sprite = Sprite.Create(_tex, new Rect(0f, 0f, _tex.width, _tex.height), Vector2.one);
            _sprite.name = rValue.name;
            return _sprite;
        }

        public static implicit operator SaveSprite(Sprite rValue)
        {
            var _tex = rValue.texture;
            return new SaveSprite(_tex.width, _tex.height,
                _tex.EncodeToPNG(), rValue.name);
        }
    }
}