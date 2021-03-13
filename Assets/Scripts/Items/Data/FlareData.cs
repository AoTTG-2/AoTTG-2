using UnityEngine;

namespace Assets.Scripts.Items.Data
{
    [CreateAssetMenu(fileName = "Flare", menuName = "Items/Flare", order = 1)]
    public class FlareData : ItemData
    {
        [SerializeField] private Color color = Color.cyan;

        public Item ToItem()
        {
            return new Flare(color, this);
        }
    }
}
