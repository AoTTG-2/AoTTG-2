using Assets.Scripts.CustomMaps;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface ICustomMapService
    {
        List<CustomMap> GetCustomMaps();
        void Load(string mapName);
        void LoadScene(string sceneName);
        string ConvertLegacyMap(string legacyMap);
    }
}
