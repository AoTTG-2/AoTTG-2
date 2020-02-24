using System;

public class RCCondition
{
    private int operand;
    private RCActionHelper parameter1;
    private RCActionHelper parameter2;
    private int type;

    public RCCondition(int sentOperand, int sentType, RCActionHelper sentParam1, RCActionHelper sentParam2)
    {
        this.operand = sentOperand;
        this.type = sentType;
        this.parameter1 = sentParam1;
        this.parameter2 = sentParam2;
    }

    private bool boolCompare(bool baseBool, bool compareBool)
    {
        int operand = this.operand;
        if (operand == 2)
        {
            return (baseBool == compareBool);
        }
        return ((operand == 5) && (baseBool != compareBool));
    }

    public bool checkCondition()
    {
        switch (this.type)
        {
            case 0:
                return this.intCompare(this.parameter1.returnInt(null), this.parameter2.returnInt(null));

            case 1:
                return this.boolCompare(this.parameter1.returnBool(null), this.parameter2.returnBool(null));

            case 2:
                return this.stringCompare(this.parameter1.returnString(null), this.parameter2.returnString(null));

            case 3:
                return this.floatCompare(this.parameter1.returnFloat(null), this.parameter2.returnFloat(null));

            case 4:
                return this.playerCompare(this.parameter1.returnPlayer(null), this.parameter2.returnPlayer(null));

            case 5:
                return this.titanCompare(this.parameter1.returnTitan(null), this.parameter2.returnTitan(null));
        }
        return false;
    }

    private bool floatCompare(float baseFloat, float compareFloat)
    {
        switch (this.operand)
        {
            case 0:
                if (baseFloat >= compareFloat)
                {
                    return false;
                }
                return true;

            case 1:
                if (baseFloat > compareFloat)
                {
                    return false;
                }
                return true;

            case 2:
                if (baseFloat != compareFloat)
                {
                    return false;
                }
                return true;

            case 3:
                if (baseFloat < compareFloat)
                {
                    return false;
                }
                return true;

            case 4:
                if (baseFloat <= compareFloat)
                {
                    return false;
                }
                return true;

            case 5:
                if (baseFloat == compareFloat)
                {
                    return false;
                }
                return true;
        }
        return false;
    }

    private bool intCompare(int baseInt, int compareInt)
    {
        switch (this.operand)
        {
            case 0:
                if (baseInt >= compareInt)
                {
                    return false;
                }
                return true;

            case 1:
                if (baseInt > compareInt)
                {
                    return false;
                }
                return true;

            case 2:
                if (baseInt != compareInt)
                {
                    return false;
                }
                return true;

            case 3:
                if (baseInt < compareInt)
                {
                    return false;
                }
                return true;

            case 4:
                if (baseInt <= compareInt)
                {
                    return false;
                }
                return true;

            case 5:
                if (baseInt == compareInt)
                {
                    return false;
                }
                return true;
        }
        return false;
    }

    private bool playerCompare(PhotonPlayer basePlayer, PhotonPlayer comparePlayer)
    {
        int operand = this.operand;
        if (operand == 2)
        {
            return (basePlayer == comparePlayer);
        }
        return ((operand == 5) && (basePlayer != comparePlayer));
    }

    private bool stringCompare(string baseString, string compareString)
    {
        switch (this.operand)
        {
            case 0:
                if (!(baseString == compareString))
                {
                    return false;
                }
                return true;

            case 1:
                if (!(baseString != compareString))
                {
                    return false;
                }
                return true;

            case 2:
                if (!baseString.Contains(compareString))
                {
                    return false;
                }
                return true;

            case 3:
                if (baseString.Contains(compareString))
                {
                    return false;
                }
                return true;

            case 4:
                if (!baseString.StartsWith(compareString))
                {
                    return false;
                }
                return true;

            case 5:
                if (baseString.StartsWith(compareString))
                {
                    return false;
                }
                return true;

            case 6:
                if (!baseString.EndsWith(compareString))
                {
                    return false;
                }
                return true;

            case 7:
                if (baseString.EndsWith(compareString))
                {
                    return false;
                }
                return true;
        }
        return false;
    }

    private bool titanCompare(TITAN baseTitan, TITAN compareTitan)
    {
        int operand = this.operand;
        if (operand == 2)
        {
            return (baseTitan == compareTitan);
        }
        return ((operand == 5) && (baseTitan != compareTitan));
    }

    public enum castTypes
    {
        typeInt,
        typeBool,
        typeString,
        typeFloat,
        typePlayer,
        typeTitan
    }

    public enum operands
    {
        lt,
        lte,
        e,
        gte,
        gt,
        ne
    }

    public enum stringOperands
    {
        equals,
        notEquals,
        contains,
        notContains,
        startsWith,
        notStartsWith,
        endsWith,
        notEndsWith
    }
}

