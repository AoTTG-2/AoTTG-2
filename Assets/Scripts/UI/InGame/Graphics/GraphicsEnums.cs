using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class GraphicsEnums : MonoBehaviour
    {

        public enum AntiAliasing
        {
            Disabled,
            _2xMultiSampling = 2,
            _4xMultiSampling = 4,
            _8xMultiSampling = 8
        }

    }
}
