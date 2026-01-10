using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent CarterGamesLabel = new GUIContent("Carter Games", "See more from Carter Games here!");
        private static readonly GUIContent SupportDevLabel = new GUIContent("Buy me a coffee?", "Support the developer with a small drink. Totally optional.");
        private static readonly GUIContent GithubLabel = new GUIContent("Github", "The repository for the asset.");
        private static readonly GUIContent DocsLabel = new GUIContent("Online Docs", "Open the assets latest online documentation.");
        private static readonly GUIContent ContactLabel = new GUIContent("Contact Dev", "Contact the developer for help. You'll usually get an answer within 24 hours.");

        private const string OpenLinkButtonLabel = "Open Link";
        
        private const string CarterGamesLink = "https://www.carter.games";
        private const string SupportDeveloperLink = "https://www.buymeacoffee.com/cartergames";
        private const string GithubLink = "https://www.github.com/CarterGames/SaveManager";
        private const string DocsLink = "https://www.carter.games/savemanager/";
        private const string ContactLink = "https://www.carter.games/contact";
        
        
        
        /// <summary>
        /// Draws the buttons section of the window.
        /// </summary>
        private static void DrawButtons()
        {
            // Docs.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(DocsLabel, DocsLink);
            
            if (GUILayout.Button(OpenLinkButtonLabel, GUILayout.Width(100)))
            {
                Application.OpenURL(DocsLink);
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            // Contact Dev.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(ContactLabel, ContactLink);
            
            if (GUILayout.Button(OpenLinkButtonLabel, GUILayout.Width(100)))
            {
                Application.OpenURL(ContactLink);
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            // Github.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(GithubLabel, GithubLink);
            
            if (GUILayout.Button(OpenLinkButtonLabel, GUILayout.Width(100)))
            {
                Application.OpenURL(GithubLink);
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            // Carter Games.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(CarterGamesLabel, CarterGamesLink);
            
            if (GUILayout.Button(OpenLinkButtonLabel, GUILayout.Width(100)))
            {
                Application.OpenURL(CarterGamesLink);
            }
            
            EditorGUILayout.EndHorizontal();
            
            
            // Buy me a coffee.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(SupportDevLabel, SupportDeveloperLink);
            
            if (GUILayout.Button(OpenLinkButtonLabel, GUILayout.Width(100)))
            {
                Application.OpenURL(SupportDeveloperLink);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}