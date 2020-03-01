using System;

public class RCActionHelper
{
    public int helperClass;
    public int helperType;
    private RCActionHelper nextHelper;
    private object parameters;

    public RCActionHelper(int sentClass, int sentType, object options)
    {
        this.helperClass = sentClass;
        this.helperType = sentType;
        this.parameters = options;
    }

    public void callException(string str)
    {
        FengGameManagerMKII.instance.chatRoom.addLINE(str);
    }

    public bool returnBool(object sentObject)
    {
        object parameters = sentObject;
        if (this.parameters != null)
        {
            parameters = this.parameters;
        }
        switch (this.helperClass)
        {
            case 0:
                return (bool) parameters;

            case 1:
            {
                RCActionHelper helper = (RCActionHelper) parameters;
                switch (this.helperType)
                {
                    case 0:
                        return this.nextHelper.returnBool(FengGameManagerMKII.intVariables[helper.returnString(null)]);

                    case 1:
                        return (bool) FengGameManagerMKII.boolVariables[helper.returnString(null)];

                    case 2:
                        return this.nextHelper.returnBool(FengGameManagerMKII.stringVariables[helper.returnString(null)]);

                    case 3:
                        return this.nextHelper.returnBool(FengGameManagerMKII.floatVariables[helper.returnString(null)]);

                    case 4:
                        return this.nextHelper.returnBool(FengGameManagerMKII.playerVariables[helper.returnString(null)]);

                    case 5:
                        return this.nextHelper.returnBool(FengGameManagerMKII.titanVariables[helper.returnString(null)]);
                }
                return false;
            }
            case 2:
            {
                PhotonPlayer player = (PhotonPlayer) parameters;
                if (player != null)
                {
                    HERO hero;
                    switch (this.helperType)
                    {
                        case 0:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.team]);

                        case 1:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.RCteam]);

                        case 2:
                            return !((bool) player.CustomProperties[PhotonPlayerProperty.dead]);

                        case 3:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.isTitan]);

                        case 4:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.kills]);

                        case 5:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.deaths]);

                        case 6:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.max_dmg]);

                        case 7:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.total_dmg]);

                        case 8:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.customInt]);

                        case 9:
                            return (bool) player.CustomProperties[PhotonPlayerProperty.customBool];

                        case 10:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.customString]);

                        case 11:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.customFloat]);

                        case 12:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.name]);

                        case 13:
                            return this.nextHelper.returnBool(player.CustomProperties[PhotonPlayerProperty.guildName]);

                        case 14:
                        {
                            int iD = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[iD];
                                return this.nextHelper.returnBool(hero.transform.position.x);
                            }
                            return false;
                        }
                        case 15:
                        {
                            int key = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(key))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[key];
                                return this.nextHelper.returnBool(hero.transform.position.y);
                            }
                            return false;
                        }
                        case 0x10:
                        {
                            int num6 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num6))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num6];
                                return this.nextHelper.returnBool(hero.transform.position.z);
                            }
                            return false;
                        }
                        case 0x11:
                        {
                            int num7 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num7))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num7];
                                return this.nextHelper.returnBool(hero.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude);
                            }
                            return false;
                        }
                    }
                }
                return false;
            }
            case 3:
            {
                TITAN titan = (TITAN) parameters;
                if (titan != null)
                {
                    switch (this.helperType)
                    {
                        case 0:
                            return this.nextHelper.returnBool(titan.abnormalType);

                        case 1:
                            return this.nextHelper.returnBool(titan.myLevel);

                        case 2:
                            return this.nextHelper.returnBool(titan.currentHealth);

                        case 3:
                            return this.nextHelper.returnBool(titan.transform.position.x);

                        case 4:
                            return this.nextHelper.returnBool(titan.transform.position.y);

                        case 5:
                            return this.nextHelper.returnBool(titan.transform.position.z);
                    }
                }
                return false;
            }
            case 4:
            {
                RCActionHelper helper2 = (RCActionHelper) parameters;
                RCRegion region = (RCRegion) FengGameManagerMKII.RCRegions[helper2.returnString(null)];
                switch (this.helperType)
                {
                    case 0:
                        return this.nextHelper.returnBool(region.GetRandomX());

                    case 1:
                        return this.nextHelper.returnBool(region.GetRandomY());

                    case 2:
                        return this.nextHelper.returnBool(region.GetRandomZ());
                }
                return false;
            }
            case 5:
                switch (this.helperType)
                {
                    case 0:
                    {
                        int num2 = (int) parameters;
                        return Convert.ToBoolean(num2);
                    }
                    case 1:
                        return (bool) parameters;

                    case 2:
                    {
                        string str = (string) parameters;
                        return Convert.ToBoolean(str);
                    }
                    case 3:
                    {
                        float num3 = (float) parameters;
                        return Convert.ToBoolean(num3);
                    }
                }
                return false;
        }
        return false;
    }

    public float returnFloat(object sentObject)
    {
        object parameters = sentObject;
        if (this.parameters != null)
        {
            parameters = this.parameters;
        }
        switch (this.helperClass)
        {
            case 0:
                return (float) parameters;

            case 1:
            {
                RCActionHelper helper = (RCActionHelper) parameters;
                switch (this.helperType)
                {
                    case 0:
                        return this.nextHelper.returnFloat(FengGameManagerMKII.intVariables[helper.returnString(null)]);

                    case 1:
                        return this.nextHelper.returnFloat(FengGameManagerMKII.boolVariables[helper.returnString(null)]);

                    case 2:
                        return this.nextHelper.returnFloat(FengGameManagerMKII.stringVariables[helper.returnString(null)]);

                    case 3:
                        return (float) FengGameManagerMKII.floatVariables[helper.returnString(null)];

                    case 4:
                        return this.nextHelper.returnFloat(FengGameManagerMKII.playerVariables[helper.returnString(null)]);

                    case 5:
                        return this.nextHelper.returnFloat(FengGameManagerMKII.titanVariables[helper.returnString(null)]);
                }
                return 0f;
            }
            case 2:
            {
                PhotonPlayer player = (PhotonPlayer) parameters;
                if (player != null)
                {
                    HERO hero;
                    switch (this.helperType)
                    {
                        case 0:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.team]);

                        case 1:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.RCteam]);

                        case 2:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.dead]);

                        case 3:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.isTitan]);

                        case 4:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.kills]);

                        case 5:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.deaths]);

                        case 6:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.max_dmg]);

                        case 7:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.total_dmg]);

                        case 8:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.customInt]);

                        case 9:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.customBool]);

                        case 10:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.customString]);

                        case 11:
                            return (float) player.CustomProperties[PhotonPlayerProperty.customFloat];

                        case 12:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.name]);

                        case 13:
                            return this.nextHelper.returnFloat(player.CustomProperties[PhotonPlayerProperty.guildName]);

                        case 14:
                        {
                            int iD = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[iD];
                                return hero.transform.position.x;
                            }
                            return 0f;
                        }
                        case 15:
                        {
                            int key = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(key))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[key];
                                return hero.transform.position.y;
                            }
                            return 0f;
                        }
                        case 0x10:
                        {
                            int num7 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num7))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num7];
                                return hero.transform.position.z;
                            }
                            return 0f;
                        }
                        case 0x11:
                        {
                            int num8 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num8))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num8];
                                return hero.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude;
                            }
                            return 0f;
                        }
                    }
                }
                return 0f;
            }
            case 3:
            {
                TITAN titan = (TITAN) parameters;
                if (titan != null)
                {
                    switch (this.helperType)
                    {
                        case 0:
                            return this.nextHelper.returnFloat(titan.abnormalType);

                        case 1:
                            return titan.myLevel;

                        case 2:
                            return this.nextHelper.returnFloat(titan.currentHealth);

                        case 3:
                            return titan.transform.position.x;

                        case 4:
                            return titan.transform.position.y;

                        case 5:
                            return titan.transform.position.z;
                    }
                }
                return 0f;
            }
            case 4:
            {
                RCActionHelper helper2 = (RCActionHelper) parameters;
                RCRegion region = (RCRegion) FengGameManagerMKII.RCRegions[helper2.returnString(null)];
                switch (this.helperType)
                {
                    case 0:
                        return region.GetRandomX();

                    case 1:
                        return region.GetRandomY();

                    case 2:
                        return region.GetRandomZ();
                }
                return 0f;
            }
            case 5:
                switch (this.helperType)
                {
                    case 0:
                    {
                        int num3 = (int) parameters;
                        return Convert.ToSingle(num3);
                    }
                    case 1:
                        return Convert.ToSingle((bool) parameters);

                    case 2:
                        float num4;
                        if (float.TryParse((string) parameters, out num4))
                        {
                            return num4;
                        }
                        return 0f;

                    case 3:
                        return (float) parameters;
                }
                return (float) parameters;
        }
        return 0f;
    }

    public int returnInt(object sentObject)
    {
        object parameters = sentObject;
        if (this.parameters != null)
        {
            parameters = this.parameters;
        }
        switch (this.helperClass)
        {
            case 0:
                return (int) parameters;

            case 1:
            {
                RCActionHelper helper = (RCActionHelper) parameters;
                switch (this.helperType)
                {
                    case 0:
                        return (int) FengGameManagerMKII.intVariables[helper.returnString(null)];

                    case 1:
                        return this.nextHelper.returnInt(FengGameManagerMKII.boolVariables[helper.returnString(null)]);

                    case 2:
                        return this.nextHelper.returnInt(FengGameManagerMKII.stringVariables[helper.returnString(null)]);

                    case 3:
                        return this.nextHelper.returnInt(FengGameManagerMKII.floatVariables[helper.returnString(null)]);

                    case 4:
                        return this.nextHelper.returnInt(FengGameManagerMKII.playerVariables[helper.returnString(null)]);

                    case 5:
                        return this.nextHelper.returnInt(FengGameManagerMKII.titanVariables[helper.returnString(null)]);
                }
                return 0;
            }
            case 2:
            {
                PhotonPlayer player = (PhotonPlayer) parameters;
                if (player != null)
                {
                    HERO hero;
                    switch (this.helperType)
                    {
                        case 0:
                            return (int) player.CustomProperties[PhotonPlayerProperty.team];

                        case 1:
                            return (int) player.CustomProperties[PhotonPlayerProperty.RCteam];

                        case 2:
                            return this.nextHelper.returnInt(player.CustomProperties[PhotonPlayerProperty.dead]);

                        case 3:
                            return (int) player.CustomProperties[PhotonPlayerProperty.isTitan];

                        case 4:
                            return (int) player.CustomProperties[PhotonPlayerProperty.kills];

                        case 5:
                            return (int) player.CustomProperties[PhotonPlayerProperty.deaths];

                        case 6:
                            return (int) player.CustomProperties[PhotonPlayerProperty.max_dmg];

                        case 7:
                            return (int) player.CustomProperties[PhotonPlayerProperty.total_dmg];

                        case 8:
                            return (int) player.CustomProperties[PhotonPlayerProperty.customInt];

                        case 9:
                            return this.nextHelper.returnInt(player.CustomProperties[PhotonPlayerProperty.customBool]);

                        case 10:
                            return this.nextHelper.returnInt(player.CustomProperties[PhotonPlayerProperty.customString]);

                        case 11:
                            return this.nextHelper.returnInt(player.CustomProperties[PhotonPlayerProperty.customFloat]);

                        case 12:
                            return this.nextHelper.returnInt(player.CustomProperties[PhotonPlayerProperty.name]);

                        case 13:
                            return this.nextHelper.returnInt(player.CustomProperties[PhotonPlayerProperty.guildName]);

                        case 14:
                        {
                            int iD = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[iD];
                                return this.nextHelper.returnInt(hero.transform.position.x);
                            }
                            return 0;
                        }
                        case 15:
                        {
                            int key = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(key))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[key];
                                return this.nextHelper.returnInt(hero.transform.position.y);
                            }
                            return 0;
                        }
                        case 0x10:
                        {
                            int num7 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num7))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num7];
                                return this.nextHelper.returnInt(hero.transform.position.z);
                            }
                            return 0;
                        }
                        case 0x11:
                        {
                            int num8 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num8))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num8];
                                return this.nextHelper.returnInt(hero.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude);
                            }
                            return 0;
                        }
                    }
                }
                return 0;
            }
            case 3:
            {
                TITAN titan = (TITAN) parameters;
                if (titan != null)
                {
                    switch (this.helperType)
                    {
                        case 0:
                            return (int) titan.abnormalType;

                        case 1:
                            return this.nextHelper.returnInt(titan.myLevel);

                        case 2:
                            return titan.currentHealth;

                        case 3:
                            return this.nextHelper.returnInt(titan.transform.position.x);

                        case 4:
                            return this.nextHelper.returnInt(titan.transform.position.y);

                        case 5:
                            return this.nextHelper.returnInt(titan.transform.position.z);
                    }
                }
                return 0;
            }
            case 4:
            {
                RCActionHelper helper2 = (RCActionHelper) parameters;
                RCRegion region = (RCRegion) FengGameManagerMKII.RCRegions[helper2.returnString(null)];
                switch (this.helperType)
                {
                    case 0:
                        return this.nextHelper.returnInt(region.GetRandomX());

                    case 1:
                        return this.nextHelper.returnInt(region.GetRandomY());

                    case 2:
                        return this.nextHelper.returnInt(region.GetRandomZ());
                }
                return 0;
            }
            case 5:
                switch (this.helperType)
                {
                    case 0:
                        return (int) parameters;

                    case 1:
                        return Convert.ToInt32((bool) parameters);

                    case 2:
                        int num4;
                        if (int.TryParse((string) parameters, out num4))
                        {
                            return num4;
                        }
                        return 0;

                    case 3:
                    {
                        float num3 = (float) parameters;
                        return Convert.ToInt32(num3);
                    }
                }
                return (int) parameters;
        }
        return 0;
    }

    public PhotonPlayer returnPlayer(object objParameter)
    {
        object parameters = objParameter;
        if (this.parameters != null)
        {
            parameters = this.parameters;
        }
        switch (this.helperClass)
        {
            case 1:
            {
                RCActionHelper helper = (RCActionHelper) parameters;
                return (PhotonPlayer) FengGameManagerMKII.playerVariables[helper.returnString(null)];
            }
            case 2:
                return (PhotonPlayer) parameters;
        }
        return (PhotonPlayer) parameters;
    }

    public string returnString(object sentObject)
    {
        object parameters = sentObject;
        if (this.parameters != null)
        {
            parameters = this.parameters;
        }
        switch (this.helperClass)
        {
            case 0:
                return (string) parameters;

            case 1:
            {
                RCActionHelper helper = (RCActionHelper) parameters;
                switch (this.helperType)
                {
                    case 0:
                        return this.nextHelper.returnString(FengGameManagerMKII.intVariables[helper.returnString(null)]);

                    case 1:
                        return this.nextHelper.returnString(FengGameManagerMKII.boolVariables[helper.returnString(null)]);

                    case 2:
                        return (string) FengGameManagerMKII.stringVariables[helper.returnString(null)];

                    case 3:
                        return this.nextHelper.returnString(FengGameManagerMKII.floatVariables[helper.returnString(null)]);

                    case 4:
                        return this.nextHelper.returnString(FengGameManagerMKII.playerVariables[helper.returnString(null)]);

                    case 5:
                        return this.nextHelper.returnString(FengGameManagerMKII.titanVariables[helper.returnString(null)]);
                }
                return string.Empty;
            }
            case 2:
            {
                PhotonPlayer player = (PhotonPlayer) parameters;
                if (player != null)
                {
                    HERO hero;
                    switch (this.helperType)
                    {
                        case 0:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.team]);

                        case 1:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.RCteam]);

                        case 2:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.dead]);

                        case 3:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.isTitan]);

                        case 4:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.kills]);

                        case 5:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.deaths]);

                        case 6:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.max_dmg]);

                        case 7:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.total_dmg]);

                        case 8:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.customInt]);

                        case 9:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.customBool]);

                        case 10:
                            return (string) player.CustomProperties[PhotonPlayerProperty.customString];

                        case 11:
                            return this.nextHelper.returnString(player.CustomProperties[PhotonPlayerProperty.customFloat]);

                        case 12:
                            return (string) player.CustomProperties[PhotonPlayerProperty.name];

                        case 13:
                            return (string) player.CustomProperties[PhotonPlayerProperty.guildName];

                        case 14:
                        {
                            int iD = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[iD];
                                return this.nextHelper.returnString(hero.transform.position.x);
                            }
                            return string.Empty;
                        }
                        case 15:
                        {
                            int key = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(key))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[key];
                                return this.nextHelper.returnString(hero.transform.position.y);
                            }
                            return string.Empty;
                        }
                        case 0x10:
                        {
                            int num6 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num6))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num6];
                                return this.nextHelper.returnString(hero.transform.position.z);
                            }
                            return string.Empty;
                        }
                        case 0x11:
                        {
                            int num7 = player.ID;
                            if (FengGameManagerMKII.heroHash.ContainsKey(num7))
                            {
                                hero = (HERO) FengGameManagerMKII.heroHash[num7];
                                return this.nextHelper.returnString(hero.GetComponent<UnityEngine.Rigidbody>().velocity.magnitude);
                            }
                            return string.Empty;
                        }
                    }
                }
                return string.Empty;
            }
            case 3:
            {
                TITAN titan = (TITAN) parameters;
                if (titan != null)
                {
                    switch (this.helperType)
                    {
                        case 0:
                            return this.nextHelper.returnString(titan.abnormalType);

                        case 1:
                            return this.nextHelper.returnString(titan.myLevel);

                        case 2:
                            return this.nextHelper.returnString(titan.currentHealth);

                        case 3:
                            return this.nextHelper.returnString(titan.transform.position.x);

                        case 4:
                            return this.nextHelper.returnString(titan.transform.position.y);

                        case 5:
                            return this.nextHelper.returnString(titan.transform.position.z);
                    }
                }
                return string.Empty;
            }
            case 4:
            {
                RCActionHelper helper2 = (RCActionHelper) parameters;
                RCRegion region = (RCRegion) FengGameManagerMKII.RCRegions[helper2.returnString(null)];
                switch (this.helperType)
                {
                    case 0:
                        return this.nextHelper.returnString(region.GetRandomX());

                    case 1:
                        return this.nextHelper.returnString(region.GetRandomY());

                    case 2:
                        return this.nextHelper.returnString(region.GetRandomZ());
                }
                return string.Empty;
            }
            case 5:
                switch (this.helperType)
                {
                    case 0:
                    {
                        int num2 = (int) parameters;
                        return num2.ToString();
                    }
                    case 1:
                    {
                        bool flag2 = (bool) parameters;
                        return flag2.ToString();
                    }
                    case 2:
                        return (string) parameters;

                    case 3:
                    {
                        float num3 = (float) parameters;
                        return num3.ToString();
                    }
                }
                return string.Empty;
        }
        return string.Empty;
    }

    public TITAN returnTitan(object objParameter)
    {
        object parameters = objParameter;
        if (this.parameters != null)
        {
            parameters = this.parameters;
        }
        switch (this.helperClass)
        {
            case 1:
            {
                RCActionHelper helper = (RCActionHelper) parameters;
                return (TITAN) FengGameManagerMKII.titanVariables[helper.returnString(null)];
            }
            case 3:
                return (TITAN) parameters;
        }
        return (TITAN) parameters;
    }

    public void setNextHelper(RCActionHelper sentHelper)
    {
        this.nextHelper = sentHelper;
    }

    public enum helperClasses
    {
        primitive,
        variable,
        player,
        titan,
        region,
        convert
    }

    public enum mathTypes
    {
        add,
        subtract,
        multiply,
        divide,
        modulo,
        power
    }

    public enum other
    {
        regionX,
        regionY,
        regionZ
    }

    public enum playerTypes
    {
        playerType,
        playerTeam,
        playerAlive,
        playerTitan,
        playerKills,
        playerDeaths,
        playerMaxDamage,
        playerTotalDamage,
        playerCustomInt,
        playerCustomBool,
        playerCustomString,
        playerCustomFloat,
        playerName,
        playerGuildName,
        playerPosX,
        playerPosY,
        playerPosZ,
        playerSpeed
    }

    public enum titanTypes
    {
        titanType,
        titanSize,
        titanHealth,
        positionX,
        positionY,
        positionZ
    }

    public enum variableTypes
    {
        typeInt,
        typeBool,
        typeString,
        typeFloat,
        typePlayer,
        typeTitan
    }
}

