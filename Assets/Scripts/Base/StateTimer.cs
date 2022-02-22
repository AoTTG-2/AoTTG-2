using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StateTimer : MonoBehaviour
{
    protected float totalTimeInState;
    protected int maxTimer;
    protected float timer;

    public bool IsActiveState { get { return timer > 0; } }

    public StateTimer()
    {
        maxTimer = 15;
    }

    protected virtual void FixedUpdate()
    {
        CalcTotalTime();
        SubtractTime();
        SetState();
    }

    protected virtual void Awake()
    {
        enabled = true;
    }

    private void CalcTotalTime()
    {
        if (IsActiveState)
        {
            totalTimeInState += Time.deltaTime;
        }
        else if (totalTimeInState > 0)
        {
            totalTimeInState = 0;
        }
    }

    private void SubtractTime()
    {
        var deltaTime = Time.deltaTime;
        var result = timer - deltaTime;
        timer = result < 0 ? 0 : result;
    }

    protected virtual void SetState()
    {
    }

    public void AddTime(int time)
    {
        var total = timer + time;
        timer = (total < maxTimer) ? total : maxTimer;
    }
}
