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
				bool result = orig(self);
				// Game state doesn't update quick enough OR until after this hook is done running (weird)
				StartCoroutine(ResetRecyclerCooldown(self));
				return result;
			};

		}

		private IEnumerator ResetRecyclerCooldown(EquipmentSlot equipment) {
			yield return new WaitForSeconds(0.5f);
			equipment.characterBody.inventory.DeductActiveEquipmentCooldown(equipment.cooldownTimer);
		}
	}
}
