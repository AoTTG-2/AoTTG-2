using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtensionMethods
    {
        /// <summary>
        /// Get Component of <paramref name="go"/> and return via out parameter <paramref name="c"/>. Can be used as Fluent Syntax
        /// </summary>
        /// <example>
        /// <code>gameObject.GetComponent<Renderer>(out var renderer).GetComponent<RigidBody>(out var rigidBody);</code>
        /// </example>
        /// <typeparam name="T">Generic Parameter type of <see cref="Component"/></typeparam>
        /// <param name="go">Extension Method Parameter <see cref="GameObject"/></param>
        /// <param name="c">Out Parameter of type <typeparamref name="T"/></param>
        /// <returns><paramref name="go"/> to be used as Fluent Syntax</returns>
        public static GameObject GetComponent<T>(this GameObject go, out T c) where T : Component
        {
            c = go.GetComponent<T>();
            return go;
        }
    }
}