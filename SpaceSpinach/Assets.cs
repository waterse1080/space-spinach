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

                SpaceSpinachAsGreenTierItem();

                // AddLanguageTokens();
            }

        }

        private static void SpaceSpinachAsGreenTierItem() {
            // Define the item when it is created as a green tier item

            SpaceSpinachItemDef = ScriptableObject.CreateInstance<ItemDef>();
            var Spin = SpaceSpinachItemDef;
            Spin.name = "SpaceSpinach"; // Internal name, no spaces or special characters
            Spin.tier = ItemTier.Tier2;
            Spin.pickupIconSprite = SpaceSpinachIcon;
            Spin.pickupModelPrefab = SpaceSpinachPrefab;
            Spin.nameToken = "Space Spinach"; // Stylised Name
            Spin.pickupToken = "Grow in size. Gain small bonuses to health, damage, and speed!";
            Spin.descriptionToken = "Grow in size. Gain small bonuses to health, damage, speed, and jump height!";
            Spin.loreToken = "Said to be grown with a special bonemeal fertilizer, the exact nutritional contents remain a mystery as the back has been scratched off...";
            Spin.tags = new[] {
                    ItemTag.Damage,
                    ItemTag.Utility
            };

            var itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = SpaceSpinachPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model, was 0.15f for bone
            itemDisplayRules[0].localAngles = new Vector3(0f, 90f, 0f); // rotate the model, was 0, 180, 0 for bone
            itemDisplayRules[0].localPos = new Vector3(-0.25f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest, was -0.35, -0.1, 0 for bone

            var spaceSpinach = new CustomItem(SpaceSpinachItemDef, itemDisplayRules);

            if (!ItemAPI.Add(spaceSpinach)) { // Add space spinach to the items, find index using ItemCatalog.FindItemIndex("SpaceSpinach");
                SpaceSpinachItemDef = null;
                Logger.LogError("Unable to add Space Spinach");
            }

        }

        // Causes issues
       /* private static void AddLanguageTokens() {
            LanguageAPI.Add("SPINACH_NAME", "Space Spinach");
            LanguageAPI.Add("SPINACH_PICKUP", "Grow in size. Gain small bonuses to health, damage, and speed!");
            LanguageAPI.Add("SPINACH_DESC", "Grow in size. Gain small bonuses to health, damage, speed, and jump height!");
            LanguageAPI.Add("SPINACH_LORE", "Said to be grown with a special bonemeal fertilizer, the exact nutritional contents remain a mystery as the back has been scratched off...");
        }*/
    }
}