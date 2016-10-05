using System.Collections;

public partial class PlayerAction
{
    protected enum TargetOFlag : int
    {
        Empty           = 0,
        Grid            = 1,
        HeroEnemy       = 1 << 1,
        HeroFriend      = 1 << 2,
        HeroYourself    = 1 << 3,
        StructureEnemy  = 1 << 4,
        StructureFriend = 1 << 5,
        CreepEnemy      = 1 << 6,
        CreepFriend     = 1 << 7,
    }

    protected enum PlayerActionTag : int
    {
        HeroAction = 1,
    }

    protected int TargetOFlagMask;
}
