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

using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    /// <summary>
    /// Contains settings that are specific to just the editor and are not used at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "Editor Settings", menuName = "Carter Games/Save Manager/Editor Settings Asset", order = 3)]
    public sealed class SettingsAssetEditor : SaveManagerAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private int saveEditorTabPos;
        [SerializeField] private bool saveEditorProfileCreator;
        [SerializeField] private bool saveEditorProfileViewer;
        [SerializeField] private string lastProfileName;
        [SerializeField] private bool showSaveKeys;
        
        // Are used, just not directly referenced!
        [SerializeField] private string lastSaveObjectName;
        [SerializeField] private string lastSaveObjectFileName;
        [SerializeField] private bool justCreatedSaveObject;
        
        [SerializeField] private Color backgroundColor = Color.white;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets/Sets the tab pos of the save editor window.
        /// </summary>
        public int TabPos
        {
            get => saveEditorTabPos;
            set => saveEditorTabPos = value;
        }
        
        
        /// <summary>
        /// Gets/Sets if the creator tab is expanded.
        /// </summary>
        public bool ProfileCreatorExpanded
        {
            get => saveEditorProfileCreator;
            set => saveEditorProfileCreator = value;
        }
        
        
        /// <summary>
        /// Gets/Sets if the viewer tab is expanded.
        /// </summary>
        public bool ProfileViewerExpanded
        {
            get => saveEditorProfileViewer;
            set => saveEditorProfileViewer = value;
        }
        
        
        /// <summary>
        /// Gets/Sets the last save profile name used.
        /// </summary>
        public string LastSaveProfileName
        {
            get => lastProfileName;
            set => lastProfileName = value;
        }
        
        
        /// <summary>
        /// Gets the normal GUI.Background color and caches it for use.
        /// </summary>
        public Color BackgroundColor => backgroundColor;
        
        
        /// <summary>
        /// Gets/Sets the tab pos of the save editor window.
        /// </summary>
        public bool ShowSaveKeys
        {
            get => showSaveKeys;
            set => showSaveKeys = value;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the editor settings when called.
        /// </summary>
        public void Initialize()
        {
            backgroundColor = GUI.backgroundColor;
        }
    }
}