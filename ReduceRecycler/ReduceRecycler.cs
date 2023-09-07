using System.Collections;

using UnityEngine;

using BepInEx;
using RoR2;

namespace ReduceRecycler {
	// [BepInDependency("com.rune580.riskofoptions")]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class ReduceRecycler : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ReduceRecycler";
		public const string PluginVersion = "0.1.0";

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ReduceRecycler)}");

			On.RoR2.EquipmentSlot.FireRecycle += (On.RoR2.EquipmentSlot.orig_FireRecycle orig, EquipmentSlot self) => {
				bool didRecyclerTransmute = orig(self);
				if (didRecyclerTransmute) {
					// Game state doesn't update quick enough OR until after this hook is done running (weird)
					StartCoroutine(ResetRecyclerCooldown(self));
				}

				return didRecyclerTransmute;
			};
		}

		private IEnumerator ResetRecyclerCooldown(EquipmentSlot equipment) {
			yield return new WaitForSeconds(0.5f);
			float cooldownSeconds = equipment.cooldownTimer;
			float stopwatchCurrentSeconds = Run.instance.GetRunStopwatch();

			if (!float.IsInfinity(cooldownSeconds)) {
				equipment.characterBody.inventory.DeductActiveEquipmentCooldown(cooldownSeconds);
				Run.instance.SetRunStopwatch(stopwatchCurrentSeconds + cooldownSeconds);
			}
		}
	}
}
