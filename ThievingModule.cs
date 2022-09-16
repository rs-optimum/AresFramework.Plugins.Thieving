using System.Threading.Tasks;
using AresFramework.Plugin.Loaders;
using AresFramework.Plugin.Module;
using AresFramework.Plugins.Thieving.Pickpocketing;
using JetBrains.Annotations;
using NLog;

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
        PluginLoader.InitializeTypes<IPickpocketNpc>(typeof(IPickpocketNpc), (pickpocketNpc, name, assembly) =>
        {
            PickpocketNpcs.MapPickpocketNpc(pickpocketNpc);
        });
        
        PickpocketNpcs.LoadNpcs();
        return Task.CompletedTask;
    }
}