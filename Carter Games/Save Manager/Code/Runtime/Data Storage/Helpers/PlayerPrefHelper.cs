using System.Collections.Generic;
using System.Linq;

namespace CarterGames.Assets.SaveManager.Helpers
{
    public static class PlayerPrefHelper
    {
        private const int MaxChunkSize = 75; // Technically 100 is the max, 75 is just for safety xD
        
        
        public static IEnumerable<string> ConvertToChunkCollection(string json)
        {
            var iterations = json.Length / MaxChunkSize + 1;
            var chunkArray = new string[iterations];
            var data = new string(json.ToCharArray()).Trim(' ');

            for (var i = 0; i < iterations; i++)
            {
                var takenData = new string(data.Take(MaxChunkSize).ToArray());
                chunkArray[i] = takenData;

                var totalToRemove = data.Length >= MaxChunkSize ? MaxChunkSize : data.Length - 1;
                
                if (totalToRemove <= 0) break;

                data = data.Remove(0, totalToRemove);
            }

            return chunkArray;
        }


        public static string CovertCollectionToString(IEnumerable<string> data)
        {
            var result = string.Empty;

            foreach (var entry in data)
            {
                result += entry;
            }
            
            return result;
        }
    }
}