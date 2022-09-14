using System;
using AresFramework.Model.Entity;
using AresFramework.Model.Items;
using JetBrains.Annotations;

namespace AresFramework.Plugins.Thieving.Pickpocketing.Impl;


[UsedImplicitly]
public class ElfPickpocketNpc : IPickpocketNpc
{
    public int LevelRequired()
    {
        return 90;
    }

    public double Experience()
    {
        return 353.3;
    }

    public Predicate<Player> HasRequirements()
    {
        return player =>
        {
            if (!player.Username.Equals("optimum"))
            {
                player.SendMessage("You need to be called optimum to thiev this npc");
                return false;
            }

            return true;
        };
    }
    
    public int StunTicks()
    {
        return 10;
    }

    public Range StunDamage()
    {
        return new Range(5, 5);
    }

    public double GetChance(Player player)
    {
        return 0.2f;
    }

    public int[] ApplicableNpcs()
    {
        return new int[] { 102 };
    }
    
    public Item[] Loot()
    {
        return new[] {new Item(995, 20000)};
    }
}