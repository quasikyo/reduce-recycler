using BepInEx;

namespace ReduceRecycler {
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[BepInDependency("com.rune580.riskofoptions")]
	public class ReduceRecycler : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ReduceRecycler";
		public const string PluginVersion = "0.1.0";


		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ReduceRecycler)}");
		}

	}
}
