using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingGameComponent : MonoBehaviour
    {
        protected RacingGamemode Gamemode { get; } = FengGameManagerMKII.Gamemode as RacingGamemode;

        protected virtual void Awake()
        {
            if (Gamemode == null)
                Destroy(this.gameObject);
        }
    }
}
