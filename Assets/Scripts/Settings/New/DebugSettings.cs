using Assets.Scripts.Settings.New.Types;
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
        public BoolSetting ShowColliders;
        /// <summary>
        /// Toggles whether or not the player should have collisions enabled or not
        /// </summary>
        public BoolSetting NoClip;
        /// <summary>
        /// Prevents all titans from being able to attack
        /// </summary>
        public BoolSetting TitanAttacks;
        /// <summary>
        /// Prevents all titans from moving
        /// </summary>
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
