using System;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class BombPvpSkill : Skill
    {
        public BombPvpSkill(Hero hero) : base(hero)
        {
        }

        public override bool Use()
        {
            return true;
        }

        public override void OnUpdate()
        {
        }
    }
}
