using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class GroundCannonInstaller : CannonInstaller
    {
        [SerializeField]
        private Interactable
            startMovingInteractable,
            stopMovingInteractable;

        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInstances(startMovingInteractable, stopMovingInteractable);
            
            Container.Bind(typeof(CannonState),
                    typeof(IInitializable),
                    typeof(IDisposable))
                .To<MovingCannonState>()
                .AsSingle()
                .WithArguments(
                    startMovingInteractable,
                    stopMovingInteractable);
        }
    }
}