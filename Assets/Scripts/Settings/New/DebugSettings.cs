using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.Utility;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Debug", order = 1)]
    public class DebugSettings : BaseSettings
    {
        /// <summary>
        /// Toggles <see cref="VisibleCollider"/> on GameObjects
        /// </summary>
        [UiElement("DEBUG_COLLIDER", "DEBUG_COLLIDER_DESC")]
        public BoolSetting ShowColliders;
        /// <summary>
        /// Toggles whether or not the player should have collisions enabled or not
        /// </summary>
        [UiElement("DEBUG_NOCLIP", "DEBUG_NOCLIP_DESC")]
        public BoolSetting NoClip;
        /// <summary>
        /// Prevents all titans from being able to attack
        /// </summary>
        [UiElement("DEBUG_TITAN_ATTACK", "DEBUG_TITAN_ATTACK_DESC")]
        public BoolSetting TitanAttacks;
        /// <summary>
        /// Prevents all titans from moving
        /// </summary>
        [UiElement("DEBUG_TITAN_MOVEMENT", "DEBUG_TITAN_MOVEMENT_DESC")]
        public BoolSetting TitanMovement;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is DebugSettings debug)) throw new ArgumentException();
            if (debug.ShowColliders != null) ShowColliders.Value = debug.ShowColliders.Value;
            if (debug.NoClip != null) NoClip.Value = debug.NoClip.Value;
            if (debug.TitanAttacks != null) TitanAttacks.Value = debug.TitanAttacks.Value;
            if (debug.TitanMovement != null) TitanMovement.Value = debug.TitanMovement.Value;
        }
    }
}
