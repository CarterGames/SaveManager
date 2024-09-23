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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CarterGames.Common
{
    /// <summary>
    /// A helper class for assembly related logic.
    /// </summary>
    public static class AssemblyHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Assembly[] saveManagerAssemblies;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets all the cart assemblies to use when checking in internally only.
        /// </summary>
        private static Assembly[] SaveManagerAssemblies
        {
            get
            {
                if (saveManagerAssemblies != null) return saveManagerAssemblies;
                saveManagerAssemblies = GetCartAssemblies();
                return saveManagerAssemblies;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The assemblies for the library.
        /// </summary>
        /// <returns>Returns all the assemblies for the cart.</returns>
        private static Assembly[] GetCartAssemblies()
        {
#if UNITY_EDITOR
            return new Assembly[3]
            {
                Assembly.Load("CarterGames.SaveManager.Editor"),
                Assembly.Load("CarterGames.SaveManager.Runtime"),
                Assembly.Load("CarterGames.SaveManager.Common"),
            };
#else
            return new Assembly[2]
            {
                Assembly.Load("CarterGames.SaveManager.Runtime"),
                Assembly.Load("CarterGames.SaveManager.Common")
            };
#endif
        }


        /// <summary>
        /// Gets the number of classes of the requested type in the project.
        /// </summary>
        /// <param name="internalCheckOnly">Check internally to the asset only.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>The total in the project.</returns>
        public static int CountClassesOfType<T>(bool internalCheckOnly = true)
        {
            var assemblies = internalCheckOnly ? SaveManagerAssemblies : AppDomain.CurrentDomain.GetAssemblies();
                
            return assemblies.SelectMany(x => x.GetTypes())
                .Count(x => x.IsClass && typeof(T).IsAssignableFrom(x));
        }
        
        
        /// <summary>
        /// Gets the number of classes of the requested type in the project.
        /// </summary>
        /// <param name="assemblies">The assemblies to check through.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>The total in the project.</returns>
        public static int CountClassesOfType<T>(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes())
                .Count(x => x.IsClass && typeof(T).IsAssignableFrom(x));
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <param name="internalCheckOnly">Check internally to the asset only.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<T> GetClassesOfType<T>(bool internalCheckOnly = true)
        {
            var assemblies = internalCheckOnly ? SaveManagerAssemblies : AppDomain.CurrentDomain.GetAssemblies();

            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x) && x.FullName != typeof(T).FullName)
                .Select(type => (T)Activator.CreateInstance(type));
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <param name="internalCheckOnly">Check internally to the asset only.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<Type> GetClassesNamesOfType<T>(bool internalCheckOnly = true)
        {
            var assemblies = internalCheckOnly ? SaveManagerAssemblies : AppDomain.CurrentDomain.GetAssemblies();

            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x) && x.FullName != typeof(T).FullName);
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <param name="assemblies">The assemblies to check through.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<T> GetClassesOfType<T>(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x) && x.FullName != typeof(T).FullName)
                .Select(type => (T)Activator.CreateInstance(type));
        }
    }
}