using System;
using AresFramework.Model.Entity;
using AresFramework.Model.Items;

namespace AresFramework.Plugins.Thieving.Pickpocketing;

public interface IPickpocketNpc
{
    /// <summary>
    /// The level required to pickpocket this npc
    /// </summary>
    public int LevelRequired();
    
    /// <summary>
    /// The experience to add
    /// </summary>
    /// <returns></returns>
    public double Experience();
    
    /// <summary>
    /// Checks if a player can thief or not, this will contain special requirements
    /// </summary>
    public Predicate<Player> HasRequirements();

    public int StunTicks();
    
    public Range StunDamage();
    
    /// <summary>
    /// Will return the change the player has
    /// </summary>
    /// <returns></returns>
    public double GetChance(Player player);

    
    public int[] ApplicableNpcs();
    
    
    public Item[] Loot();
    
}