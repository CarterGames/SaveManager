using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Backups;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SearchProviderSaveBackupLocations : SearchProvider<ISaveBackupLocation>
    {
        private static SearchProviderSaveBackupLocations Instance;
        private IEnumerable<ISaveBackupLocation> cache;
        
        protected override string ProviderTitle => "Backup Save Locations";
        

        public override bool HasOptions => true;
        
        
        public override List<SearchGroup<ISaveBackupLocation>> GetEntriesToDisplay()
        {
            if (cache == null)
            {
                cache = AssemblyHelper.GetClassesOfType<ISaveBackupLocation>().Where(t => !t.GetType().IsInterface);
            }
            

            var list = new List<SearchGroup<ISaveBackupLocation>>();
            var items = new List<SearchItem<ISaveBackupLocation>>();
			
            foreach (var entry in cache)
            {
                if (ToExclude.Contains(entry)) continue;
                items.Add(SearchItem<ISaveBackupLocation>.Set(entry.GetType().Name, entry));
            }
            
            list.Add(new SearchGroup<ISaveBackupLocation>(items));
			
            return list;
        }
        
        
        public static SearchProviderSaveBackupLocations GetProvider()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<SearchProviderSaveBackupLocations>();
            }

            return Instance;
        }
    }
}