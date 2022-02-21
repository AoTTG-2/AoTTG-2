using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Base
{
    public class SingeltonMonoBehaviour<Tclass> : MonoBehaviour where Tclass : class
    {
        public static Tclass Instance;

        protected virtual void Awake()
        {
            CheckSingleton();
        }

        private void CheckSingleton()
        {
            if (Instance is null)
            {
                Instance = this as Tclass;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
