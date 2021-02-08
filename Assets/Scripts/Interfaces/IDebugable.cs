using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    /// <summary>
    /// A Debugabble entity, can return a Debug String from <see cref="IDebugable.GetDebugString"/>
    /// </summary>
    public interface IDebugable
    {
        /// <summary>
        /// Get a Debug string from this entity
        /// </summary>
        /// <returns>Debug string</returns>
        string GetDebugString(StringBuilder sb);
    }
}