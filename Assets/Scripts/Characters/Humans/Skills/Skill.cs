using Assets.Scripts.Characters.Humans.Equipment;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    /// <summary>
    /// Abstract class for the HERO skills
    /// </summary>
    public abstract class Skill
    {
        protected readonly Hero Hero;

        protected Skill(Hero hero)
        {
            Hero = hero;
        }

        public List<EquipmentType> CompatibleEquipmentTypes = new List<EquipmentType>();

        public float Cooldown { get; set; }

        /// <summary>
        /// Returns true if the Skill is currently used
        /// </summary>
        public bool IsActive { get; protected set; }
        /// <summary>
        /// Logic when the skill is used
        /// </summary>
        /// <returns></returns>
        public abstract bool Use();
        /// <summary>
        /// Determines what should happen to the skill on <see cref="Hero.Update"/>
        /// </summary>
        public abstract void OnUpdate();
        /// <summary>
        /// Determines what should happen to this skill on <see cref="Hero.FixedUpdate"/>
        /// </summary>
        public virtual void OnFixedUpdate() { }

        //TODO
        // Skills seem to check on Hero State:
        // Grabbed: Jean & Eren
        // Idle: Eren, Marco, Armin, Sasha, Mikasa, Levi, Petra

        // Special skill: bomb, which is used for bomb pvp.

        // Some skills check whether or not the player is on the ground
        // None of the skills currently are working for AHSS
        // AHSS skill would be dual shot

        public static Skill Create(HeroSkill skill, Hero hero)
        {
            switch (skill)
            {
                case HeroSkill.Jean:
                    return new JeanSkill(hero);
                case HeroSkill.Levi:
                    return new LeviSkill(hero);
                case HeroSkill.Petra:
                    return new PetraSkill(hero);
                case HeroSkill.Mikasa:
                    return new MikasaSkill(hero);
                default:
                    Debug.LogWarning($"Hero Skill {skill} is not implemented. Using default hero Skill (Blade Throw)...");
                    return new BladeThrowSkill(hero);
            }
        }
    }
}
