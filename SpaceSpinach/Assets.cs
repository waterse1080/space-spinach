﻿using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;

namespace SpaceSpinach {
    internal static class Assets {
        // Item Statics
        internal static GameObject SpaceSpinachPrefab;
        internal static ItemIndex SpaceSpinachItemIndex;
        // Item private paths and prefix
        private const string ModPrefix = "@SpaceSpinach:";
        private const string PrefabPath = ModPrefix + "Assets/Import/skull/skull.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/skull_icon/SkullIcon.png";

        internal static void Init() {
            Chat.AddMessage("Loaded SpaceSpinach!");
            // First registering your AssetBundle into the ResourcesAPI with a modPrefix that'll also be used for your prefab and icon paths
            // note that the string parameter of this GetManifestResourceStream call will change depending on
            // your namespace and file name
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpaceSpinach.bonemeal")) {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                SpaceSpinachPrefab = bundle.LoadAsset<GameObject>("Assets/Import/skull/skull.prefab");
            }

            SpaceSpinachAsWhiteTierItem();
            
        }

        private static void SpaceSpinachAsWhiteTierItem() {
            // Define the item when it is created as a white tier item
            var spaceSpinachItemDef = new ItemDef {
                name = "SpaceSpinach", // Internal name, no spaces or special characters
                tier = ItemTier.Tier1,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Space Spinach", // Stylised Name
                pickupToken = "Contains extra calcium so you can grow big and strong!",
                descriptionToken = "Contains extra calcium so you can grow big and strong!",
                loreToken = "B O N E S",
                tags = new[] {
                    // ItemTag.Damage,
                    ItemTag.Utility
                }
            };

            var itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = SpaceSpinachPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var spaceSpinach = new R2API.CustomItem(spaceSpinachItemDef, itemDisplayRules);

            SpaceSpinachItemIndex = ItemAPI.Add(spaceSpinach); // ItemAPI sends back the ItemIndex of your item
        }
    }
}