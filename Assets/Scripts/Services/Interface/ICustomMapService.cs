using Assets.Scripts.CustomMaps;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface ICustomMapService
    {
        /// <summary>
        /// Returns a list of all Custom Maps located within the Custom Maps folder
        /// </summary>
        /// <returns></returns>
        List<CustomMap> GetCustomMaps();
        /// <summary>
        /// Loads a Custom Map
        /// </summary>
        /// <param name="mapName"></param>
        void Load(string mapName);
        /// <summary>
        /// Loads a Scene
        /// </summary>
        /// <param name="sceneName"></param>
        void LoadScene(string sceneName);
        /// <summary>
        /// Converts an AoTTG RC 2015 Custom Map to the AoTTG2 Custom Map format
        /// </summary>
        /// <param name="legacyMap"></param>
        /// <returns></returns>
        string ConvertLegacyMap(string legacyMap);
    }
}
