using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal class CannonInstaller : MonoInstaller
    {
        [SerializeField] private CannonBall.Settings cannonBallSettings;
        [SerializeField] private CannonBase.Settings baseSettings;
        [SerializeField] private CannonBarrel.Settings barrelSettings;
        [SerializeField] private string cannonBallPrefabName = "Cannon/CannonBall";
        [SerializeField] private MannedCannonState.Settings mannedStateSettings;

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
            Container.BindInterfacesAndSelfTo<CannonOwnershipManager>().AsSingle();
            
            Container.BindInstance(mannedStateSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<CannonStateManager>().AsSingle();
            Container.Bind(typeof(CannonState),
                typeof(IInitializable),
                typeof(IDisposable))
                .To<UnmannedCannonState>()
                .AsSingle();

            Container.Bind(typeof(MannedCannonState),
                    typeof(CannonState),
                    typeof(IInitializable),
                    typeof(IDisposable))
                .To<MannedCannonState>()
                .AsSingle()
                .WithArguments(
                    mountInteractable,
                    unmountInteractable,
                    firePoint,
                    playerPoint);

            Container.BindInstances(mountInteractable, unmountInteractable);
            Container.Bind(typeof(CannonState),
                    typeof(IInitializable),
                    typeof(IDisposable))
                .To<RemoteControlledCannonState>()
                .AsSingle();

            Container.BindInstance(baseSettings).AsSingle();
            Container.Bind<CannonBase>()
                .AsSingle()
                .WithArguments(@base);

            Container.BindInstance(cannonBallPrefabName).WhenInjectedInto<PhotonFactory<int, Vector3, CannonBall>>();
            Container.BindFactory<int, Vector3, Vector3, Quaternion, byte, CannonBall, CannonBall.Factory>()
                .FromFactory<PhotonFactory<int, Vector3, CannonBall>>();

            Container.BindInstance(cannonBallSettings).AsSingle();
            Container.BindInstance(barrelSettings).AsSingle();
            Container.Bind<CannonBarrel>()
                .AsSingle()
                .WithArguments(
                    barrel,
                    firePoint);
        }
    }
}