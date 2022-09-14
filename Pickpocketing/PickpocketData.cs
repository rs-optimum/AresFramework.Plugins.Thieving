using System;
using AresFramework.Model.Items;

namespace AresFramework.Plugins.Thieving.Pickpocketing;

public record PickpocketData(int[] npcs, int levelRequired, double experience, int stunTimer, DamageRange stunDamage, Item[] rewards);

public record DamageRange(int min, int max);
