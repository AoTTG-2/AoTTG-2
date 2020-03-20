using System;
using System.Collections;

public class IComparerRacingResult : IComparer
{
    int IComparer.Compare(object x, object y)
    {
        float time = ((RacingResult) x).time;
        float num2 = ((RacingResult) y).time;
        if ((time == num2) || (Math.Abs((float) (time - num2)) < float.Epsilon))
        {
            return 0;
        }
        if (time < num2)
        {
            return -1;
        }
        return 1;
    }
}

