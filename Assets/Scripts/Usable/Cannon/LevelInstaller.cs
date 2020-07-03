using Zenject;

namespace Cannon
{
    internal sealed class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (PhotonNetwork.isMasterClient)
                Container.Bind<CannonRequestManager>().AsSingle();
        }
    }
}