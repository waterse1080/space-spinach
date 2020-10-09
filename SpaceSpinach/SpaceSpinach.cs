using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;

namespace SpaceSpinach {
    [BepInDependency(R2API.R2API.PluginGUID)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class SpaceSpinach : BaseUnityPlugin {
        private const string ModVer = "0.1.2";
        private const string ModName = "SpaceSpinach";
        public const string ModGuid = "com.WeeabooTrash.SpaceSpinach";

        internal new static ManualLogSource Logger; // allow access to the logger across the plugin classes

        public void Awake() {
            Logger = base.Logger;
            Assets.Init();
            Hooks.Init();
        }
    }
}
