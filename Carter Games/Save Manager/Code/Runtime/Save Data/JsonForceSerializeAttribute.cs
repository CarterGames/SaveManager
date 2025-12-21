using System;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonForceSerializeAttribute : PropertyAttribute
    {
        public JsonForceSerializeAttribute() { }
    }
}