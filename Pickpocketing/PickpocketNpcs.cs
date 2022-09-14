using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AresFramework.Model.Entity;
using AresFramework.Model.Entity.Skills;
using Newtonsoft.Json;
using NLog;

namespace AresFramework.Plugins.Thieving.Pickpocketing;

public static class PickpocketNpcs
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    public static readonly Dictionary<int, IPickpocketNpc> PickpocketingNpcs = new Dictionary<int, IPickpocketNpc>();
    
    /// <summary>
    /// Starts pickpocketing the a specific npc
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public static bool Pickpocket(Player player, int npcId)
    {
        PickpocketingNpcs.TryGetValue(npcId, out var pickpocketNpc);
        
        if (pickpocketNpc == null)
        {
            player.SendMessage("Nothing interesting happens.");
            return false;
        }
        
        Log.Debug($"Attempting to thief {npcId} with thieving level {player.SkillSet.GetSkill(Skill.Thieving)}");

        if (pickpocketNpc.LevelRequired() > player.SkillSet.GetCurrentLevel(Skill.Thieving))
        {
            player.SendMessage($"You need a thieving level of {pickpocketNpc.LevelRequired()} to pickpocket this npc.");
            return false;
        }

        if (!pickpocketNpc.HasRequirements().Invoke(player))
        {
            return false;
        }

        double experienceGained = pickpocketNpc.Experience();
        player.SkillSet.AddExperience(Skill.Thieving, experienceGained);
        player.SendMessage("You successfully pickpocket the {npcName}");
        
        return true;
    }

    public static void MapPickpocketNpc(IPickpocketNpc pickpocketNpc)
    {
        foreach (var npcs in pickpocketNpc.ApplicableNpcs())
        {
            if (PickpocketingNpcs.ContainsKey(npcs))
            {
                Log.Warn($"The pickpocketable npc id {npcs} already exists");
                continue;
            }
            Log.Debug($"Mapped the following id {npcs} to the following class {pickpocketNpc.GetType().Name}");
            PickpocketingNpcs.Add(npcs, pickpocketNpc);
        }
    }
    
    public static void LoadNpcs()
    {
        var readAll = Encoding.UTF8.GetString(ReadResource("pickpocket.json"));
        
        var npcs = JsonConvert.DeserializeObject<List<PickpocketData>>(readAll);
        foreach (var npc in npcs)
        {
            var defaultJsonNpc = new DefaultPickpocketNpc(npc.levelRequired, npc.experience, npc.npcs, npc.rewards, npc.stunTimer,
                new Range(npc.stunDamage.min, npc.stunDamage.max));
            MapPickpocketNpc(defaultJsonNpc);
        }
    }
    
    private static byte[] ReadResource(string name)
    {
        // Determine path
        var assembly = Assembly.GetExecutingAssembly();
        string fileName = assembly.GetManifestResourceNames()
            .SingleOrDefault(e => e.Contains(name));
        using Stream stream = assembly.GetManifestResourceStream(fileName);
        
        using (MemoryStream reader = new MemoryStream())
        {
            stream.CopyTo(reader);
            return reader.ToArray();
        }
    }
}