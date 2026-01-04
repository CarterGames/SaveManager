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

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// An enum of all the possible intended issues the asset can have.
    /// </summary>
    public enum SaveManagerErrorCode
    {
        NoGlobalSaveObjectOfType = 1200,
        NoSaveValueFound = 1201,
        NoSaveKeyAssigned = 1202,
        DuplicateSaveKeys = 1203,
        SaveValueLoadFailed = 1204,
        SaveValueResetFailed = 1205,
        SaveValueTypeMismatch = 1206,
        GameLoadFailed = 1210,
        NoSlotsToLoad = 1220,
        SlotCapReached = 1221,
        SlotIdAlreadyExists = 1222,
        NoSlotSaveObjectOfType = 1223,
        BackupRestoreFailed = 1250,
        SaveEncryptionFailed = 1260,
        SaveDecryptionFailed = 1261,
        LegacyLoadFailed = 1270,
        SaveCaptureLoadFailed = 1300,
        SaveCaptureSaveFailed = 1301,
    }
}