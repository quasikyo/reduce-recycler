﻿using System.Collections;

using UnityEngine;

using BepInEx;
using RoR2;
using R2API.Utils;

namespace ReduceRecycler {
	[BepInDependency("com.rune580.riskofoptions")]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	public class ReduceRecycler : BaseUnityPlugin {

		public const string PluginGUID = PluginAuthor + "." + PluginName;
		public const string PluginAuthor = "quasikyo";
		public const string PluginName = "ReduceRecycler";
		public const string PluginVersion = "1.0.1";

		public void Awake() {
			Log.Init(Logger);
			Log.Info($"Performing setup for {nameof(ReduceRecycler)}");
			ConfigManager.Init();

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

			bool isNotInfinity = !float.IsInfinity(cooldownSeconds);
			bool isTeleporterFinished = TeleporterInteraction.instance.isCharged;
			bool isTeleporterRelevant = ConfigManager.EnableOnlyAfterTeleporter.Value;
			// discrete math, relevancy implies a need for a charged TP
			bool allowCooldown = !isTeleporterRelevant || isTeleporterFinished;
			bool doRemoveCooldown = isNotInfinity && allowCooldown;
			if (doRemoveCooldown) {
				equipment.characterBody.inventory.DeductActiveEquipmentCooldown(cooldownSeconds);
				Run.instance.SetRunStopwatch(stopwatchCurrentSeconds + cooldownSeconds);
			}
		}
	}
}
