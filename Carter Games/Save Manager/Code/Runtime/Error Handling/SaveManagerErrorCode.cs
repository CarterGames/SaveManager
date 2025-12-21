namespace CarterGames.Assets.SaveManager
{
    public enum SaveManagerErrorCode
    {
        NoGlobalSaveObjectOfType = 1200,
        NoSaveValueFound = 1201,
        NoSaveKeyAssigned = 1202,
        DuplicateSaveKeys = 1203,
        SaveValueLoadFailed = 1204,
        SaveValueResetFailed = 1205,
        SaveValueTypeMismatch = 1206,
        NoSlotsToLoad = 1220,
        SlotCapReached = 1221,
        SlotIdAlreadyExists = 1222,
        NoSlotSaveObjectOfType = 1223,
        BackupRestoreFailed = 1250,
        SaveEncryptionFailed = 1260,
        SaveDecryptionFailed = 1261,
        SaveCaptureLoadFailed = 1300,
        SaveCaptureSaveFailed = 1301,
    }
}