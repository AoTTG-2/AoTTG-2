using Photon;

namespace Assets.Scripts.Characters
{
    public abstract class Entity : MonoBehaviour
    {
        public Faction Faction { get; set; }
    }
}
