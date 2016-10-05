using UnityEngine;
using System.Collections;

public class HeroPlayerAction : PlayerAction
{
    public HeroPlayerAction()
    {
        //TODO: remove - test only
        TargetOFlagMask = (int)TargetOFlag.HeroYourself | (int)TargetOFlag.HeroFriend | (int)TargetOFlag.Grid | (int)TargetOFlag.StructureFriend | (int)TargetOFlag.CreepFriend;
    }
}
