using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SearchProviderSaveEncryptionHandlers : SearchProvider<ISaveEncryptionHandler>
    {
        private static SearchProviderSaveEncryptionHandlers Instance;
        private IEnumerable<ISaveEncryptionHandler> cache;
        
        protected override string ProviderTitle => "Save Encryption Handlers";
        

        public override bool HasOptions => true;
        
        
        public override List<SearchGroup<ISaveEncryptionHandler>> GetEntriesToDisplay()
        {
            if (cache == null)
            {
                cache = AssemblyHelper.GetClassesOfType<ISaveEncryptionHandler>().Where(t => !t.GetType().IsInterface);
            }
            

            var list = new List<SearchGroup<ISaveEncryptionHandler>>();
            var items = new List<SearchItem<ISaveEncryptionHandler>>();
			
            foreach (var entry in cache)
            {
                if (ToExclude.Contains(entry)) continue;
                items.Add(SearchItem<ISaveEncryptionHandler>.Set(entry.GetType().Name, entry));
            }
            
            list.Add(new SearchGroup<ISaveEncryptionHandler>(items));
			
            return list;
        }
        
        
        public static SearchProviderSaveEncryptionHandlers GetProvider()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<SearchProviderSaveEncryptionHandlers>();
            }

            return Instance;
        }
    }
}