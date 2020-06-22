//Stores the outfit options
public struct CharacterOutfit
{
    Sex sex;
    SkinColor skinColor;
    int hairID;
    Odmg odmg;
    OutfitType outfitType;
    int outfitTextureID;
    Division division;
    bool capeEnabled;
    bool hoodEnabled;
    bool scarfEnabled;
}

//Stores and manages the stats of the character
public class CharacterStat
{
    #region Fields
    private static readonly int MaxPoints = 400;
    private static readonly int MinStat = 75;
    private static readonly int MaxStat = 125;

    private int availablePoints;
    private int gas;
    private int equipment;
    private int acceleration;
    private int speed;
    #endregion

    #region Properties
    public int Gas
    {
        get { return gas; }

        set
        {
            int difference = ClampedStatDifference(gas, value);
            gas += difference;
            availablePoints -= difference;
        }
    }

    public int Equipment
    {
        get { return equipment; }

        set
        {
            int difference = ClampedStatDifference(equipment, value);
            int remainder = difference % 25;

            equipment += difference / 4;
            availablePoints -= (difference - remainder);
        }
    }

    public int Acceleration
    {
        get { return acceleration; }

        set
        {
            int difference = ClampedStatDifference(acceleration, value);
            acceleration += difference;
            availablePoints -= difference;
        }
    }

    public int Speed
    {
        get { return speed; }

        set
        {
            int difference = ClampedStatDifference(speed, value);
            speed += difference;
            availablePoints -= difference;
        }
    }
    #endregion

    #region Constructors
    CharacterStat()
    {
        SetDefault();
    }

    CharacterStat(int gas, int equipment, int acceleration, int speed)
    {
        this.gas = gas;
        this.equipment = equipment;
        this.acceleration = acceleration;
        this.speed = speed;

        CalculateAvailablePoints();

        if (availablePoints < 0)
            SetDefault();
    }
    #endregion

    #region Methods
    public void SetDefault()
    {
        availablePoints = 0;
        gas = 100;
        equipment = 100;
        acceleration = 100;
        speed = 100;
    }

    private void CalculateAvailablePoints()
    {
        availablePoints = MaxPoints - (gas + equipment + acceleration + speed);
    }

    /* Calculate the amount to add to a stat given the current and new value.
       Clamps the amount based on stat constraints and available points */
    private int ClampedStatDifference(int oldStat, int newStat)
    {
        int difference;

        //Get the clamped difference in stat value
        if (newStat < MinStat)
            difference = MinStat - oldStat;
        else if (newStat > MaxStat)
            difference = MaxStat - oldStat;
        else
            difference = newStat - oldStat;

        //If the stat difference costs more than the available points, use all of the remaining points
        if (difference > availablePoints)
            difference = availablePoints;

        return difference;
    }
    #endregion
}