using System;
using AresFramework.Model.Items;

namespace AresFramework.Plugins.Thieving.Pickpocketing;

public record PickpocketData(int[] npcs, int levelRequired, double experience, int stunTimer, DamageRange stunDamage,
    Item[] rewards)
{
    public PickpocketData() : this(null, 0, 0, 0, null, null) {}
}

public record DamageRange(int min, int max)
{
    public DamageRange() : this(1, 1){}
}
