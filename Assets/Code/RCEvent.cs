using System;
using System.Collections.Generic;

public class RCEvent
{
    private RCCondition condition;
    private RCAction elseAction;
    private int eventClass;
    private int eventType;
    public string foreachVariableName;
    public List<RCAction> trueActions;

    public RCEvent(RCCondition sentCondition, List<RCAction> sentTrueActions, int sentClass, int sentType)
    {
        this.condition = sentCondition;
        this.trueActions = sentTrueActions;
        this.eventClass = sentClass;
        this.eventType = sentType;
    }

    public void checkEvent()
    {
        int num2;
        switch (this.eventClass)
        {
            case 0:
                for (num2 = 0; num2 < this.trueActions.Count; num2++)
                {
                    this.trueActions[num2].doAction();
                }
                break;

            case 1:
                if (!this.condition.checkCondition())
                {
                    if (this.elseAction != null)
                    {
                        this.elseAction.doAction();
                    }
                    break;
                }
                for (num2 = 0; num2 < this.trueActions.Count; num2++)
                {
                    this.trueActions[num2].doAction();
                }
                break;

            case 2:
                switch (this.eventType)
                {
                    case 0:
                        foreach (TITAN titan in FengGameManagerMKII.instance.getTitans())
                        {
                            if (FengGameManagerMKII.titanVariables.ContainsKey(this.foreachVariableName))
                            {
                                FengGameManagerMKII.titanVariables[this.foreachVariableName] = titan;
                            }
                            else
                            {
                                FengGameManagerMKII.titanVariables.Add(this.foreachVariableName, titan);
                            }
                            foreach (RCAction action in this.trueActions)
                            {
                                action.doAction();
                            }
                        }
                        return;

                    case 1:
                        foreach (PhotonPlayer player in PhotonNetwork.playerList)
                        {
                            if (FengGameManagerMKII.playerVariables.ContainsKey(this.foreachVariableName))
                            {
                                FengGameManagerMKII.playerVariables[this.foreachVariableName] = player;
                            }
                            else
                            {
                                FengGameManagerMKII.titanVariables.Add(this.foreachVariableName, player);
                            }
                            foreach (RCAction action in this.trueActions)
                            {
                                action.doAction();
                            }
                        }
                        return;
                }
                break;

            case 3:
                while (this.condition.checkCondition())
                {
                    foreach (RCAction action in this.trueActions)
                    {
                        action.doAction();
                    }
                }
                break;
        }
    }

    public void setElse(RCAction sentElse)
    {
        this.elseAction = sentElse;
    }

    public enum foreachType
    {
        titan,
        player
    }

    public enum loopType
    {
        noLoop,
        ifLoop,
        foreachLoop,
        whileLoop
    }
}

