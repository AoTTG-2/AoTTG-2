using System;
using System.Collections;

[Obsolete("Use System.Linq")]
public class IComparerPVPchkPtID : IComparer
{
    int IComparer.Compare(object x, object y)
    {
        float id = ((PVPcheckPoint) x).id;
        float num2 = ((PVPcheckPoint) y).id;
        if ((id == num2) || (Math.Abs((float) (id - num2)) < float.Epsilon))
        {
            return 0;
        }
        if (id < num2)
        {
            return -1;
        }
        return 1;
    }
}

