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

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// Implement to make a search provider for something.
    /// You still have to have a way to open it, but it will show the values entered.
    /// </summary>
    /// <typeparam name="T">The type to provide from the search selection.</typeparam>
    public abstract class SearchProvider<T> : ScriptableObject, ISearchWindowProvider
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The title to add to the search provider when open.
        /// </summary>
        protected abstract string ProviderTitle { get; }
        
        
        /// <summary>
        /// A list of entries to exclude from the search.
        /// </summary>
        protected List<T> ToExclude { get; set; } = new List<T>();
        
        
        /// <summary>
        /// Gets if the proivder has options to show.
        /// </summary>
        public abstract bool HasOptions { get; }


        /// <summary>
        /// The width of the search provider window.
        /// </summary>
        private float WindowWidth { get; set; } = -1;
        
        
        /// <summary>
        /// Gets any addition entries to show that can be shown such as group entries.
        /// </summary>
        private List<SearchTreeEntry> AdditionalEntries
        {
            get
            {
                var list = new List<SearchTreeEntry>();

                foreach (var entries in GetEntriesToDisplay())
                {
                    if (!entries.IsValidGroup)
                    {
                        foreach (var value in entries.Values)
                        {
                            if (ToExclude.Contains(value.Value)) continue;
                            list.Add(SearchHelper.CreateEntry(value.Key, 1, value.Value));
                        }
                    }
                    else
                    {
                        list.Add(SearchHelper.CreateGroup(entries.Key, 1));
                        
                        foreach (var value in entries.Values)
                        {
                            if (ToExclude.Contains(value.Value)) continue;
                            list.Add(SearchHelper.CreateEntry(value.Key, 2, value.Value));
                        }
                    }
                }

                return list;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raised when a selection is made.
        /// </summary>
        public readonly Evt<SearchTreeEntry> SelectionMade = new Evt<SearchTreeEntry>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Opens the search window when called.
        /// </summary>
        public void Open()
        {
            ToExclude.Clear();
            
            if (WindowWidth.Equals(-1))
            {
                WindowWidth = Mathf.Min(AdditionalEntries
                    .Select(t => GUIWidth(t.content.text))
                    .Max() + 35, 1000f);
            }
            
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), WindowWidth), this);
        }
        
        
        /// <summary>
        /// Opens the search window when called.
        /// </summary>
        /// <param name="currentValue">The current value to not show.</param>
        public void Open(T currentValue)
        {
            ToExclude.Clear();
            
            if (currentValue != null)
            {
                ToExclude.Add(currentValue);
            }
            
            if (WindowWidth.Equals(-1))
            {
                WindowWidth = Mathf.Min(GetEntriesToDisplay()
                    .Select(t => GUIWidth(t.Key))
                    .Max() + 35, 1000f);
            }
            
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), WindowWidth), this);
        }
        
        
        /// <summary>
        /// Creates the search tree when called.
        /// </summary>
        /// <param name="context">The context for the window to target on.</param>
        /// <returns>The entries to show.</returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchList = new List<SearchTreeEntry>();
            
            searchList.Add(new SearchTreeGroupEntry(new GUIContent(ProviderTitle), 0));
            searchList.AddRange(AdditionalEntries);

            return searchList;
        }
        
        
        /// <summary>
        /// Runs when a selection is made.
        /// </summary>
        /// <param name="searchTreeEntry">The tree entry pressed.</param>
        /// <param name="context">The window context.</param>
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry == null) return false;
            SelectionMade.Raise(searchTreeEntry);
            return true;
        }
        
        
        /// <summary>
        /// The entries the search provider can display.
        /// </summary>
        /// <returns>A list of entries to show.</returns>
        public abstract List<SearchGroup<T>> GetEntriesToDisplay();

        public virtual List<T> GetValidValues()
        {
            return null;
        }
        
        
        private static float GUIWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x + 10f;
        }
    }
}