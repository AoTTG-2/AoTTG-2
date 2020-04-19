namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class Attack
    {
        public bool IsFinished;

        public virtual bool CanAttack(MindlessTitan titan)
        {
            return true;
        }

        public virtual void Execute(MindlessTitan titan)
        {
        }
    }
}
