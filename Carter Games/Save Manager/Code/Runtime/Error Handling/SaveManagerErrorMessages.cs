using System.Collections.Generic;

namespace CarterGames.Assets.SaveManager
{
    public static class SaveManagerErrorMessages
    {
        private const string ErrorCodeFormat = "{0} ({1})";
        private const string ErrorCodeFormatWithMsg = "{0} ({1}): {2}";
        
        private static IReadOnlyDictionary<SaveManagerErrorCode, string> ErrorMessagesLookup =
            new Dictionary<SaveManagerErrorCode, string>()
            {
                { SaveManagerErrorCode.NoGlobalSaveObjectOfType, "No global save object of type {0} found." },
                { SaveManagerErrorCode.NoSlotSaveObjectOfType, "No save slot object of type {0} found." },
                { SaveManagerErrorCode.NoSaveValueFound, "No save value found under the key {0} of type {1}." },
                { SaveManagerErrorCode.NoSaveKeyAssigned, "No save key assigned to the a save value {0}, This value cannot be saved until a unique key is assigned." },
                { SaveManagerErrorCode.DuplicateSaveKeys, "Duplicate save keys detected, please ensure all save keys are unique." },
                { SaveManagerErrorCode.NoSlotsToLoad, "There are no slots defined in the save to load. Skipping save slot loading..." },
                { SaveManagerErrorCode.SaveEncryptionFailed, "The save encryption encountered an error. See more info in trace.\nHandler: {0}\nException: {1}\nTrace: {2}" },
                { SaveManagerErrorCode.SaveDecryptionFailed, "The save decryption encountered an error. See more info in trace.\nHandler: {0}\nException: {1}\nTrace: {2}" },
                { SaveManagerErrorCode.SaveValueLoadFailed, "The save value {0} of type {1} failed to load from the save.\nException: {2}\nTrace: {3}" },
                { SaveManagerErrorCode.SaveValueResetFailed, "The save value {0} of type {1} failed to reset.\nException: {2}\nTrace: {3}" },
                { SaveManagerErrorCode.SaveValueTypeMismatch, "The save value {0} failed to load as the type was not a match. Saved type {1}, expected {2}." },
            };


        public static string GetErrorMessageFormat(this SaveManagerErrorCode errorCode, params object[] parameters)
        {
            if (ErrorMessagesLookup.TryGetValue(errorCode, out var message))
            {
                var formattedMsg = string.Format(message, parameters);
                return string.Format(ErrorCodeFormatWithMsg, errorCode, (int) errorCode, formattedMsg);
            }

            return string.Format(ErrorCodeFormat, errorCode, (int) errorCode);
        }
    }
}