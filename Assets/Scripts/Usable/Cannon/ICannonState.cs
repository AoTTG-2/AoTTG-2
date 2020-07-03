namespace Cannon
{
    internal interface ICannonState
    {
        void Enter();

        void Exit();

        void Update();
    }
}