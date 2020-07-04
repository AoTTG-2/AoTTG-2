using UnityEngine;
using Zenject;

namespace Cannon
{
    internal class CannonInstaller : MonoInstaller
    {
        [SerializeField]
        private CannonBarrel.Settings barrelSettings;

        [SerializeField]
        private string cannonBallPrefabName = "Cannon/CannonBall";

        [SerializeField]
        private MannedCannonState.Settings mannedStateSettings;

        [SerializeField]
        private Transform
            @base,
            barrel,
            firePoint,
            playerPoint;

        [SerializeField]
        private Interactable
            mountInteractable,
            unmountInteractable;

        public override void InstallBindings()
        {
            Container.BindInstance(mannedStateSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<CannonStateManager>().AsSingle();
            Container.BindInterfacesTo<UnmannedCannonState>().AsSingle();

            Container.BindInterfacesTo<MannedCannonState>()
                .AsSingle()
                .WithArguments(
                    mountInteractable,
                    unmountInteractable,
                    firePoint,
                    playerPoint);

            Container.Bind<CannonBase>()
                .AsSingle()
                .WithArguments(@base);

            Container.BindInstance(cannonBallPrefabName).WhenInjectedInto<PhotonFactory<CannonFacade, Vector3, CannonBall>>();
            Container.BindFactory<CannonFacade, Vector3, Vector3, Quaternion, byte, CannonBall, CannonBall.Factory>()
                .FromFactory<PhotonFactory<CannonFacade, Vector3, CannonBall>>();

            Container.BindInstance(barrelSettings).AsSingle();
            Container.Bind<CannonBarrel>()
                .AsSingle()
                .WithArguments(
                    barrel,
                    firePoint);
        }
    }
}