using UnityEngine;
using Zenject;

namespace Logging
{
    internal sealed class LoggerFactory : PlaceholderFactory<Object, ILogger> {}
}