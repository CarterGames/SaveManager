using System.Collections.Generic;
using System.Linq;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SearchProviderSaveLocations : SearchProvider<ISaveDataLocation>
    {
        private static SearchProviderSaveLocations Instance;
        private IEnumerable<ISaveDataLocation> cache;
        
        protected override string ProviderTitle => "Save Locations";
        

        public override bool HasOptions => true;
        
        
        public override List<SearchGroup<ISaveDataLocation>> GetEntriesToDisplay()
        {
            if (cache == null)
            {
                cache = AssemblyHelper.GetClassesOfType<ISaveDataLocation>().Where(t => !t.GetType().IsInterface);
            }
            

            var list = new List<SearchGroup<ISaveDataLocation>>();
            var items = new List<SearchItem<ISaveDataLocation>>();
			
            foreach (var entry in cache)
            {
                if (ToExclude.Contains(entry)) continue;
                items.Add(SearchItem<ISaveDataLocation>.Set(entry.GetType().Name, entry));
            }
            
            list.Add(new SearchGroup<ISaveDataLocation>(items));
			
            return list;
        }
        
        
        public static SearchProviderSaveLocations GetProvider()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<SearchProviderSaveLocations>();
            }

            return Instance;
        }
    }
}