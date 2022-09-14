using System.Linq;
using AresFramework.Model.Entity.Action.Interactions.Npcs;
using AresFramework.Plugins.Thieving.Pickpocketing;

namespace AresFramework.Plugins.Thieving.Actions;

public class PickpocketingNpcsAction : NpcInteractionAction
{
    
    public override void BuildInteractions()
    {
        Map("pickpocket", (player, npc, option) =>
        {
            PickpocketNpcs.Pickpocket(player, npc.Id);
        });
    }
    
    public override int[] AttachedNpcs()
    {
        return PickpocketNpcs.PickpocketingNpcs.Keys.ToArray();
    }
}