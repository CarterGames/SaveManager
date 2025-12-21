using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SaveCaptureDefine
    {
        public string CaptureName { get; private set; }
        public TextAsset CaptureFile { get; private set; }


        public SaveCaptureDefine(TextAsset file)
        {
            CaptureName = file.name;
            CaptureFile = file;
        }
    }
}