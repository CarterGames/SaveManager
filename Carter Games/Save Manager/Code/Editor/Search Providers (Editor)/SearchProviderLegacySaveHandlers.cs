using System.Collections.Generic;
using System.Linq;
using CarterGames.Assets.SaveManager.Legacy;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SearchProviderLegacySaveHandlers : SearchProvider<ILegacySaveHandler>
    {
        private static SearchProviderLegacySaveHandlers Instance;
        private IEnumerable<ILegacySaveHandler> cache;
        
        protected override string ProviderTitle => "Legacy Save Handlers";
        

        public override bool HasOptions => true;
        
        
        public override List<SearchGroup<ILegacySaveHandler>> GetEntriesToDisplay()
        {
            if (cache == null)
            {
                cache = AssemblyHelper.GetClassesOfType<ILegacySaveHandler>().Where(t => !t.GetType().IsInterface);
            }
            

            var list = new List<SearchGroup<ILegacySaveHandler>>();
            var items = new List<SearchItem<ILegacySaveHandler>>();
			
            foreach (var entry in cache)
            {
                if (ToExclude.Contains(entry)) continue;
                items.Add(SearchItem<ILegacySaveHandler>.Set(entry.GetType().Name, entry));
            }
            
            list.Add(new SearchGroup<ILegacySaveHandler>(items));
			
            return list;
        }
        
        
        public static SearchProviderLegacySaveHandlers GetProvider()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<SearchProviderLegacySaveHandlers>();
            }

            return Instance;
        }
    }
}