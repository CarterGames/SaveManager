/*
 * Save Manager
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
using UnityEngine;
using UnityEngine.Networking;

namespace CarterGames.Shared.SaveManager.Editor
{
    /// <summary>
    /// The new v2 setup for checking asset versions. Queries an API instead of a JSON file directly.
    /// </summary>
    public static class VersioningAPI
    {
        /// <summary>
        /// Call to query for the assets latest version on the server.
        /// </summary>
        /// <param name="onSuccess">Logic to run on success.</param>
        /// <param name="onFailed">Logic to run on failure.</param>
        public static void Query(Action<VersionPacketSuccess> onSuccess, Action<VersionPacketError> onFailed)
        {
            var url = $"api.carter.games/v0/versioning/query?id={VersionInfo.Key}";
            
            var request = UnityWebRequest.Get(url);
            request.timeout = 5;

            var op = request.SendWebRequest();
            op.completed -= OnRequestCompleted;
            op.completed += OnRequestCompleted;

            return;

            void OnRequestCompleted(AsyncOperation operation)
            {
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    onFailed?.Invoke(VersionPacketError.Custom("Cannot connect to versioning API. Please try again later."));
                }
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        onSuccess?.Invoke(JsonUtility.FromJson<VersionPacketSuccess>(request.downloadHandler.text));
                    }
#pragma warning disable 0168
                    catch (Exception e)
#pragma warning restore 0168
                    {
                        onFailed?.Invoke(VersionPacketError.Custom("Couldn't parse data received."));
                    }
                }
                else
                {
                    onFailed?.Invoke(JsonUtility.FromJson<VersionPacketError>(request.downloadHandler.text));
                }
            }
        }
    }
}