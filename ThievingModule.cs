using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AresFramework.Plugin.Loaders;
using AresFramework.Plugin.Module;
using AresFramework.Plugins.Thieving.Pickpocketing;
using JetBrains.Annotations;
using NLog;
using Polly;

namespace AresFramework.Plugins.Thieving;

[UsedImplicitly]
[PluginModule(
    "Thieving", 
    "1.0.0.0", 
    "Everything regarding base thieving goes in this project", 
    "Optimum")]
public class ThievingModule : IPluginModule
{
    
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    
    public Task Initialize()
    {
        PluginLoader.InitializeTypes<IPickpocketNpc>(typeof(IPickpocketNpc), (pickpocketNpc, name) =>
        {
            PickpocketNpcs.MapPickpocketNpc(pickpocketNpc);
        }); 
        
        Log.Info($"Loaded {PickpocketNpcs.PickpocketingNpcs.Count} custom pickpocket npcs");
        var retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetry(retryCount: 3, sleepDurationProvider: _ => TimeSpan.FromSeconds(1));
        
        var attempt = 0;
        retryPolicy.Execute(() =>
        {
            Log.Info($"Attempt {++attempt}");

            //throw new Exception();
        });
        PickpocketNpcs.LoadNpcs();
        return Task.CompletedTask;
    }
}