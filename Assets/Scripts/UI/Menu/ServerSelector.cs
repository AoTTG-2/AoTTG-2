using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI class for the "Lobby" which contains a list of all available rooms.
    /// </summary>
    public class ServerSelector : UiNavigationElement
    {
        [SerializeField]
        private RectTransform map;

        [SerializeField]
        private float mapTransitionTime, mapFinalAlpha;

        private LTDescr map_positioner, map_scaler;
        private Vector3 default_map_scale, default_map_position;

        private Dictionary<RectTransform, LTDescr> map_faders;

        private void Awake()
        {
            this.map_faders = new Dictionary<RectTransform, LTDescr>();

            this.default_map_position = map.anchoredPosition;
            this.default_map_scale = map.localScale;
        }

        private Vector3 calculateScale(RectTransform subMap)
        {
            var screenProportion = (float) Screen.width / Screen.height;
            var max = Mathf.Max(subMap.sizeDelta.x, subMap.sizeDelta.y * screenProportion);
            return default_map_scale * (1800f / max);
        }

        private Vector3 calculatePosition(RectTransform subMap)
        {
            var anchored= subMap.anchoredPosition * 1.13f;
            return default_map_position - new Vector3(anchored.x - 300, anchored.y, 0);
        }

        private void processMapFaders(RectTransform active = null)
        {
            this.map_faders.StripKeysWithNullValues();

            if (active!=null && this.map_faders.ContainsKey(active)) 
            {
                LeanTween.cancel(this.map_faders[active].id);
                this.map_faders.Remove(active);
            }

            var keys = this.map_faders.Keys.ToArray();

            foreach (var key in keys)
            {
                var faderInfo = map_faders[key];
                if (faderInfo != null)
                {
                    LeanTween.cancel(faderInfo.id);
                    map_faders[key] = LeanTween.alpha(key, 0f, mapTransitionTime);
                }
            }

            if (active != null)
                map_faders.Add(active,LeanTween.alpha(active, mapFinalAlpha, mapTransitionTime));
        }

        private void stopCurrentAnim()
        {
            if (this.map_positioner != null)
            {
                LeanTween.cancel(this.map_positioner.id);
            }
            if (this.map_scaler != null)
            {
                LeanTween.cancel(this.map_scaler.id);
            }
            this.map_positioner = null;
            this.map_scaler = null;
        }

        public void MapMovement(RectTransform toPos = null)
        {
            this.stopCurrentAnim();

            Vector3 next_pos, next_scale;
            if(toPos == null)
            {
                next_pos = this.default_map_position;
                next_scale = this.default_map_scale;
            }
            else
            {
                next_pos = this.calculatePosition(toPos);
                next_scale = this.calculateScale(toPos);
            }

            this.map_scaler = LeanTween.scale(map, next_scale, mapTransitionTime);
            this.map_positioner = LeanTween.move(map, next_pos, mapTransitionTime);

            this.processMapFaders(toPos);
        }

        public override void Back()
        {
            base.Back();
            PhotonNetwork.Disconnect();
        }
    }
}