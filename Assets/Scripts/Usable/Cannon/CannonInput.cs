using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Cannon
{
    internal sealed class CannonInput
    {
        public Vector2 GetAxes()
        {
            var left = InputManager.Key(InputCannon.Left) ? -1f : 0f;
            var right = InputManager.Key(InputCannon.Right) ? 1f : 0f;
            var up = InputManager.Key(InputCannon.Up) ? 1f : 0f;
            var down = InputManager.Key(InputCannon.Down) ? -1f : 0f;
            return new Vector2(
                left + right,
                up + down);
        }
    }
}