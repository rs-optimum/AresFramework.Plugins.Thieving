using System;
using AresFramework.Model.Entity;
using AresFramework.Model.Items;

namespace AresFramework.Plugins.Thieving.Pickpocketing;

public sealed class DefaultPickpocketNpc : IPickpocketNpc
{
    
    private int _levelRequired;
    private double _experience;
    private int[] _applicableNpcs = new int[]{};
    private Item[] _loot;
    private int _stunTicks;
    private Range _damageRange;

    public DefaultPickpocketNpc(int levelRequired, double experience, int[] applicableNpcs, Item[] loot, int stunTicks, Range damageRange)
    {
        _levelRequired = levelRequired;
        _experience = experience;
        _applicableNpcs = applicableNpcs;
        _loot = loot;
        _stunTicks = stunTicks;
        _damageRange = damageRange;
    }

    /// <summary>
    /// This is here to initialize and stop throwing exception
    /// </summary>
    public DefaultPickpocketNpc()
    {
        
    }

    public int LevelRequired()
    {
        return _levelRequired;
    }

    public double Experience()
    {
        return _experience;
    }

    public Predicate<Player> HasRequirements()
    {
        return (player) => true;
    }

    public int StunTicks()
    {
        return _stunTicks;
    }
    
    public Range StunDamage()
    {
        return _damageRange;
    }

    public double GetChance(Player player)
    {
        return 100;
    }

    public int[] ApplicableNpcs()
    {
        return _applicableNpcs;
    }

    public Item[] Loot()
    {
        return _loot;
    }
}