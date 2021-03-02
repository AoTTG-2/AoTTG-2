using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using System.Threading.Tasks;

namespace Assets.Scripts.Services.Interface
{
    public interface IPlaybackService : IService
    {
        bool Save(string filePath);
    }
}