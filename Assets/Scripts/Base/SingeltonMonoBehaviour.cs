using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Inheritance from this class implements the singeltion pattern for a <see cref="MonoBehaviour"/> class.
/// </summary>
/// <typeparam name="Tclass"></typeparam>
public abstract class SingeltonMonoBehaviour<Tclass> : MonoBehaviour where Tclass : class
{
    #region Public Properties
    /// <summary>
    /// Gets active instance of this class.
    /// </summary>
    public static Tclass Instance;
    #endregion

    #region Monobehaviours
    protected virtual void Awake()
    {
        CheckSingleton();
    }
    #endregion

    #region Private Properties
    private void CheckSingleton()
    {
        if (Instance is null)
        {
            Instance = this as Tclass;
            DontDestroyOnLoad(gameObject);
        }
        else if (this as Tclass != Instance)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
