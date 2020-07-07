using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (PhotonNetwork.isMasterClient)
                Container.Bind<CannonRequestManager>().AsSingle();
            
            // TODO: Modify PhotonFactory so it can take a dynamic prefabName.
            Container.BindFactory<int, Vector3, Quaternion, GameObject, BoomFactory>()
                .FromMethod((_, number, position, rotation) =>
                {
                    var boom = PhotonNetwork.Instantiate($"FX/boom{number}", position, rotation, 0);
                    foreach (var coll in boom.GetComponentsInChildren<EnemyCheckCollider>())
                        coll.dmg = 0;
                    return boom;
                });
        }
    }
}