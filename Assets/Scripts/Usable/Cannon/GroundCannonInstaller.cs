namespace Cannon
{
    internal sealed class GroundCannonInstaller : CannonInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.BindInterfacesTo<MovingCannonState>().AsSingle();
        }
    }
}