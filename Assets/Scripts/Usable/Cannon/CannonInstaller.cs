using UnityEngine;
using Zenject;

namespace Cannon
{
    internal class CannonInstaller : MonoInstaller
    {
        [SerializeField]
        private CannonBarrel.Settings barrelSettings;

        [SerializeField]
        private MannedCannonState.Settings mannedStateSettings;

        [SerializeField]
        private string cannonBallPrefabName = "Cannon/CannonBall";

        public override void InstallBindings()
        {
            Container.BindInstance(mannedStateSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<CannonStateManager>().AsSingle();
            Container.BindInterfacesTo<UnmannedCannonState>().AsSingle();
            Container.BindInterfacesTo<MannedCannonState>().AsSingle();

            Container.Bind<CannonBase>().AsSingle();

            Container.BindInstance(cannonBallPrefabName).WhenInjectedInto<PhotonFactory<CannonFacade, Vector3, CannonBall>>();
            Container.BindFactory<CannonFacade, Vector3, Vector3, Quaternion, byte, CannonBall, CannonBall.Factory>()
                .FromFactory<PhotonFactory<CannonFacade, Vector3, CannonBall>>();

            Container.BindInstance(barrelSettings).AsSingle();
            Container.Bind<CannonBarrel>().AsSingle();
        }
    }
}