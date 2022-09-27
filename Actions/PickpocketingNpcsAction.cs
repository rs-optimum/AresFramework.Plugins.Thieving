using System.Linq;
using AresFramework.Model.Plugins.Entities.Npcs.Interactions;
using AresFramework.Plugins.Thieving.Pickpocketing;

namespace AresFramework.Plugins.Thieving.Actions;

public class PickpocketingNpcsAction : NpcInteractionsPlugin
{
    
    public override void BuildInteractions()
    {
        Map("pickpocket", (player, npc, option) =>
        {
            PickpocketNpcs.Pickpocket(player, npc);
        });
    }
    
    public override int[] AttachedNpcs()
    {
        return PickpocketNpcs.PickpocketingNpcs.Keys.ToArray();
    }
}