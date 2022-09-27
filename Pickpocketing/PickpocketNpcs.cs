using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AresFramework.Model.Entities.Npcs;
using AresFramework.Model.Entities.Players;
using AresFramework.Model.Entities.Skills;
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
    public static bool Pickpocket(Player player, Npc npc)
    {

        var npcId = npc.Id;
        
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

        
        // Random chance here
        // TODO: real formula
        var successRandom = new Random().Next(1, 5);
        if (successRandom == 1)
        {
            var rndDamage = new Random().Next(
                pickpocketNpc.StunDamage().Start.Value, 
                pickpocketNpc.StunDamage().End.Value);
            player.SendMessage($"You failed to pickpocket the {npc.DefinitionOrDefault().Name}.");
            return true;
        }
        
        // Success
        double experienceGained = pickpocketNpc.Experience();
        player.SkillSet.AddExperience(Skill.Thieving, experienceGained);
        player.SendMessage("You successfully pickpocket the {npcName}");
        
        return true;
    }
    
    public static void MapPickpocketNpc(IPickpocketNpc pickpocketNpc)
    {
        foreach (var npc in pickpocketNpc.ApplicableNpcs())
        {
            if (PickpocketingNpcs.ContainsKey(npc))
            {
                Log.Warn($"The id {npc} has already been mapped!");
                continue;
            }
            Log.Debug($"Mapped pickpocket npc id {npc} to {pickpocketNpc.GetType().FullName}");
            PickpocketingNpcs.Add(npc, pickpocketNpc);
        }
    }
    
    public static void LoadNpcs()
    {
        try
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
        catch (Exception ex)
        {
            Log.Info(ex, "error happened");
        }
    }
    
    private static byte[] ReadResource(string name)
    {
        // Determine path
        var assembly = Assembly.GetExecutingAssembly();
        string[] files = assembly.GetManifestResourceNames();
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