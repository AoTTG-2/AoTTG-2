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

        [SerializeField] private GroundCannonBase.Settings groundBaseSettings;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.BindInstance(groundBaseSettings).AsSingle();
            Container.Rebind<CannonBase>()
                .To<GroundCannonBase>()
                .AsSingle()
                .WithArguments(
                    Base,
                    leftWheel,
                    rightWheel);
            
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