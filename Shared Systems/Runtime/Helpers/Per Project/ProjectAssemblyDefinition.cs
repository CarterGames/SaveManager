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

namespace CarterGames.Shared.SaveManager
{
    internal static class ProjectAssemblyDefinition
    {
#if UNITY_EDITOR
        public static Assembly[] ProjectEditorAssemblies { get; } = new Assembly[]
        {
            Assembly.Load("CarterGames.SaveManager.Editor"),
            Assembly.Load("CarterGames.SaveManager.Runtime"),
            Assembly.Load("CarterGames.Shared.SaveManager.Editor"),
            Assembly.Load("CarterGames.Shared.SaveManager")
        };
#else
        public static Assembly[] ProjectRuntimeAssemblies { get; } = new Assembly[]
        {
            Assembly.Load("CarterGames.SaveManager.Runtime"),
            Assembly.Load("CarterGames.Shared.SaveManager")
        };
#endif
    }
}