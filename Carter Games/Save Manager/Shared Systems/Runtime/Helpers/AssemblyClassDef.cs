/*
 * Copyright (c) 2025 Carter Games
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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Linq;
using UnityEngine;

namespace CarterGames.Shared.SaveManager
{
	/// <summary>
	/// A class for storing info about a class so it can be referenced from its assembly & type names.
	/// </summary>
	[Serializable]
	public sealed class AssemblyClassDef
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		[SerializeField] private string assembly;
		[SerializeField] private string type;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Properties
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Gets if the class is valid or not.
		/// </summary>
		public bool IsValid => !string.IsNullOrEmpty(assembly) && !string.IsNullOrEmpty(type);

		
		/// <summary>
		/// The assembly string stored.
		/// </summary>
		public string Assembly => assembly;
		
		
		/// <summary>
		/// The type string stored.
		/// </summary>
		public string Type => type;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Creates a new definition when called.
		/// </summary>
		/// <param name="assembly">The assembly to reference.</param>
		/// <param name="type">The type to reference.</param>
		public AssemblyClassDef(string assembly, string type)
		{
			this.assembly = assembly;
			this.type = type;
		}

		
		/// <summary>
		/// Creates a new definition when called.
		/// </summary>
		public static AssemblyClassDef FromType<T>()
		{
			return new AssemblyClassDef(typeof(T).Assembly.FullName, typeof(T).FullName);
		}

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Converts System.Type to a AssemblyClassDef instance.
		/// </summary>
		/// <param name="type">The type to convert.</param>
		/// <returns>The created AssemblyClassDef from the type.</returns>
		public static implicit operator AssemblyClassDef(Type type)
		{
			return new AssemblyClassDef(type.Assembly.FullName, type.FullName);
		}
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Gets the type stored in this AssemblyClassDef.
		/// </summary>
		/// <typeparam name="T">The type to make.</typeparam>
		/// <returns>The made type or the types default on failure.</returns>
		public T GetDefinedType<T>()
		{
			if (!IsValid)
			{
				Debug.LogError("[GetDefinedType]: Data not valid to generate the defined type");
				return default;
			}
			
			try
			{
				return AssemblyHelper.GetClassesOfType<T>(false).FirstOrDefault(t =>
					t.GetType().Assembly.FullName == Assembly && t.GetType().FullName == Type);
			}
#pragma warning disable
			catch (Exception e)
			{
				Debug.LogError(
					"[GetDefinedType]: Failed to generate type from stored data. If you have refactored the class selected, please reselect it to update the record.");

				return default;
			}
#pragma warning restore
		}
	}
}