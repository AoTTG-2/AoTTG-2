using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toorah.ScriptableVariables
{
    [CreateAssetMenu(fileName = "InGameCamera Variable", menuName = "Scriptable Variables/Single/InGameCamera")]
    public class InGameMainCameraVariable : ScriptableVariable<IN_GAME_MAIN_CAMERA> { }

    [System.Serializable]
    public class InGameMainCameraReference : VariableReference<IN_GAME_MAIN_CAMERA, InGameMainCameraVariable> { }
    
}
