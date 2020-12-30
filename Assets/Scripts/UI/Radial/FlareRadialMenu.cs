using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Data;
using Assets.Scripts.Services;
using System.Collections.Generic;

namespace Assets.Scripts.UI.Radial
{
    public class FlareRadialMenu : RadialMenu
    {
        public List<FlareData> flares;

        protected override void Start()
        {
            var pieces = new List<RadialElement>();
            //foreach (var flare in flares)
            //{
            //    var element = Instantiate(RadialElementPrefab, transform);
            //    element.Icon.sprite = flare.Icon;
            //    pieces.Add(element);
            //}

            foreach (var flare in Service.Inventory.GetItems<Flare>())
            {
                var element = Instantiate(RadialElementPrefab, transform);
                element.Icon.sprite = flare.Data.Icon;
                element.IconText.text = flare.Data.Name;
                pieces.Add(element);
            }

            Label.text = "Flares";

            Pieces = pieces.ToArray();

            StartCoroutine(SpawnButtons());
        }

        protected override void OnElementClicked(RadialElement element, int index)
        {
            var hero = Service.Player.Self as Hero;
            if (hero == null) return;
            Service.Inventory.GetItems<Flare>()[index].Use(hero);
            Destroy(gameObject);
        }

        protected virtual void OnDisable()
        {
            Destroy(gameObject);
        }
    }
}
