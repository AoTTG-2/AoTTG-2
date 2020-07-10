using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class GroundCannonInstaller : CannonInstaller
    {
        [SerializeField] private Transform movePoint;

        [SerializeField]
        private Interactable
            startMovingInteractable,
            stopMovingInteractable;

        [SerializeField] private Transform
            leftWheel,
            rightWheel;

        [SerializeField] private GroundCannonWheels.Settings wheelSettings;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.BindInstance(wheelSettings).AsSingle();
            Container.Bind<GroundCannonWheels>()
                .AsSingle()
                .WithArguments(
                    Base,
                    leftWheel,
                    rightWheel);

            Container.Rebind<CannonBase>()
                .To<GroundCannonBase>()
                .AsSingle()
                .WithArguments(Base);
            
            Container.BindInstances(startMovingInteractable, stopMovingInteractable);
            
            Container.Bind(typeof(CannonState),
                    typeof(IInitializable),
                    typeof(IDisposable))
                .To<MovingCannonState>()
                .AsSingle()
                .WithArguments(
                    startMovingInteractable,
                    stopMovingInteractable,
                    movePoint);
        }
    }
}