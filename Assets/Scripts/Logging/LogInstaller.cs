using UnityEngine;
using Zenject;

namespace Logging
{
    internal sealed class LogInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, ILogger, LoggerFactory>()
                .FromMethod((_, context) => new Logger(new LogHandlerWithFallbackContext(context)));
        }
    }
}