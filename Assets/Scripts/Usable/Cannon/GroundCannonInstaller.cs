using UnityEngine;

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
            Container.BindInterfacesTo<MovingCannonState>()
                .AsSingle()
                .WithArguments(
                    startMovingInteractable,
                    stopMovingInteractable);
        }
    }
}