using System.Reflection;
using BepInEx.Logging;
using R2API;
using RoR2;
using UnityEngine;

namespace SpaceSpinach {
    internal static class Assets {
        // Item Statics
        internal static GameObject SpaceSpinachPrefab;
        internal static Sprite SpaceSpinachIcon;

        internal static ItemDef SpaceSpinachItemDef;
        internal static ManualLogSource Logger;

        internal static void Init(ManualLogSource Logger) {
            // Chat.AddMessage("Loaded SpaceSpinach!");

            Assets.Logger = Logger;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpaceSpinach.spacespinachbundle")) {
                var bundle = AssetBundle.LoadFromStream(stream);

                SpaceSpinachPrefab = bundle.LoadAsset<GameObject>("Assets/Import/E-Can.prefab");
                SpaceSpinachIcon = bundle.LoadAsset<Sprite>("Assets/Import/CanIcon.png");

                SpaceSpinachAsLunarItem();
            }

        }

        // Define the item when it is created as a Lunar item
        private static void SpaceSpinachAsLunarItem() {

            // Create the item instance and populate it
            SpaceSpinachItemDef = ScriptableObject.CreateInstance<ItemDef>();
            var Spin = SpaceSpinachItemDef;
            Spin.name = "SpaceSpinach";
            Spin.tier = ItemTier.Lunar;
            Spin.pickupIconSprite = SpaceSpinachIcon;
            Spin.pickupModelPrefab = SpaceSpinachPrefab;
            Spin.nameToken = "Space Spinach";
            Spin.pickupToken = "Grow in size. Gain bonuses to health, damage, and mobility.";
            Spin.descriptionToken = "Grow in size. Gain bonuses to health, damage, speed, and jump height.";
            Spin.loreToken = "Said to be grown with a special bonemeal fertilizer on the rouge planet of Lumena, the exact nutritional contents remain a mystery as the back has been scratched off...";
            Spin.tags = new[] {
                    ItemTag.Damage,
                    ItemTag.Utility
            };

            // TODO: Add new item display rules on a per-survivor basis
            var itemDisplayRules = new ItemDisplayRule[1];
            itemDisplayRules[0].followerPrefab = SpaceSpinachPrefab;
            itemDisplayRules[0].childName = "Chest";
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f);
            itemDisplayRules[0].localAngles = new Vector3(0f, 90f, 0f);
            itemDisplayRules[0].localPos = new Vector3(-0.25f, -0.1f, 0f);

            var spaceSpinach = new CustomItem(SpaceSpinachItemDef, itemDisplayRules);

            // Add item, log if there is an error
            if (!ItemAPI.Add(spaceSpinach)) {
                SpaceSpinachItemDef = null;
                Logger.LogError("Unable to add Space Spinach");
            }

        }
    }
}