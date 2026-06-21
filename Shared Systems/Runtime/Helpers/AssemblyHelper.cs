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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CarterGames.Shared.SaveManager
{
    /// <summary>
    /// A helper class for assembly related logic.
    /// </summary>
    public static class AssemblyHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// It is intended to use this cache to avoid the use
        /// of <see cref="Assembly.GetTypes"/> in each call.
        /// </summary>
        private static readonly Dictionary<Type, List<Type>> TypeCache = new Dictionary<Type, List<Type>>();

        private static Assembly[] cachedAssemblies;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets all the cart assemblies to use when checking in internally only.
        /// </summary>
        private static IEnumerable<Assembly> CachedAssemblies
        {
            get
            {
                if (cachedAssemblies != null) return cachedAssemblies;
                cachedAssemblies = GetAssemblies();
                return cachedAssemblies;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The assemblies for the library.
        /// </summary>
        /// <returns>Returns all the assemblies.</returns>
        private static Assembly[] GetAssemblies()
        {
#if UNITY_EDITOR
            return ProjectAssemblyDefinition.ProjectEditorAssemblies;
#else
            return ProjectAssemblyDefinition.ProjectRuntimeAssemblies;
#endif
        }


        /// <summary>
        /// Gets the number of classes of the requested type in the project.
        /// </summary>
        /// <param name="internalCheckOnly">Check internally to the asset only.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>The total in the project.</returns>
        public static int CountClassesOfType<T>(bool internalCheckOnly = false)
        {
            return GetClassesNamesOfType<T>(internalCheckOnly).Count();
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <param name="internalCheckOnly">Check internally to the asset only.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<T> GetClassesOfType<T>(bool internalCheckOnly = false)
        {
            return GetClassesNamesOfType<T>(internalCheckOnly)
                .Select(type => (T)Activator.CreateInstance(type));
        }


        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <param name="internalCheckOnly">Check internally to the asset only.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<Type> GetClassesNamesOfType<T>(bool internalCheckOnly = false)
        {
            Type targetType = typeof(T);

            // We don't need to search in the project / assembly if we already know the type.
            if (TypeCache.TryGetValue(targetType, out List<Type> cachedTypes)) return cachedTypes;

#if UNITY_6000_4_OR_NEWER
            var assemblies = internalCheckOnly ? CachedAssemblies
                : UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies();
#else
            var assemblies = internalCheckOnly ? CachedAssemblies
                : AppDomain.CurrentDomain.GetAssemblies();
#endif

            // Searching all the implementation of the class
            var foundTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && !x.ContainsGenericParameters 
                    && targetType.IsAssignableFrom(x) && x != targetType)
                .ToList();

            // Caching for later
            TypeCache[targetType] = foundTypes;

            return foundTypes;
        }
    }
}