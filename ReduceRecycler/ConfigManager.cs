using BepInEx;
using BepInEx.Configuration;

using RiskOfOptions;
using RiskOfOptions.Options;

namespace ReduceRecycler {
	internal static class ConfigManager {
		private static ConfigFile OptionsConfig { get; set; }

		internal static ConfigEntry<bool> EnableOnlyAfterTeleporter { get; set; }

		static ConfigManager() {
			OptionsConfig = new ConfigFile(Paths.ConfigPath + "\\ReduceRecycler.cfg", true);
			ModSettingsManager.SetModDescription("Customize the behavior of the recycler equipment to shorten the time between uses.");

			EnableOnlyAfterTeleporter = OptionsConfig.Bind(
				"Behavior",
				"Enable Only After Teleporter",
				false,
				"Enable no-cooldown only after the teleporter for the stage has been completed."
			);
			ModSettingsManager.AddOption(new CheckBoxOption(EnableOnlyAfterTeleporter));
		}
	}
}
