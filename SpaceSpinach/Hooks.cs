using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceSpinach {
    public class Hooks {
        internal static void Init() {
            int spinachCount;

            // On character load
            On.RoR2.CharacterBody.Awake += (orig, self) => {
                // Call Start
                orig(self);
                // Attach the stat-tracking component to each object
                self.gameObject.AddComponent<SpinachStats>();
                // Set the stats
                self.gameObject.GetComponent<SpinachStats>().setBaseScale(self.modelLocator.modelTransform.localScale);
                self.gameObject.GetComponent<SpinachStats>().setCameraPos(self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos);
                self.gameObject.GetComponent<SpinachStats>().setBaseHealth(self.baseMaxHealth);
                self.gameObject.GetComponent<SpinachStats>().setBaseDamage(self.baseDamage);
                self.gameObject.GetComponent<SpinachStats>().setBaseSpeed(self.baseMoveSpeed);
                self.gameObject.GetComponent<SpinachStats>().setBaseJump(self.baseJumpPower);

                // Test output
                /** Chat.AddMessage("ObjectName: " + self.name);
                Chat.AddMessage("Scale: " + self.gameObject.GetComponent<SpinachStats>().defaultBaseScale);
                Chat.AddMessage("Health: " + self.gameObject.GetComponent<SpinachStats>().defaultBaseHealth);
                Chat.AddMessage("Damage: " + self.gameObject.GetComponent<SpinachStats>().defaultBaseDamage);
                Chat.AddMessage("Speed: " + self.gameObject.GetComponent<SpinachStats>().defualtBaseSpeed); **/                
            };

            // Recalculation of numerical stats
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                // Get SpaceSpinach #
                spinachCount = self.inventory.GetItemCount(Assets.SpaceSpinachItemIndex);

                // Update HP
                self.baseMaxHealth = self.gameObject.GetComponent<SpinachStats>().defaultBaseHealth + 10 * spinachCount;
                // Update Damage
                self.baseDamage = self.gameObject.GetComponent<SpinachStats>().defaultBaseDamage + 5 * spinachCount;
                // Update Speed
                self.baseMoveSpeed = self.gameObject.GetComponent<SpinachStats>().defualtBaseSpeed + spinachCount;
                // Update Jump Height
                self.baseJumpPower = self.gameObject.GetComponent<SpinachStats>().defaultBaseJump + 2 * spinachCount;

                // Call Recalculate stats
                orig(self);
            };

            // On entity load or inventory change of entity
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) => {
                // Get the ammount of the item currently on the Character Body
                spinachCount = self.inventory.GetItemCount(Assets.SpaceSpinachItemIndex);
                
                // Update the size of the entity based off of the # of the item held
                float x = self.gameObject.GetComponent<SpinachStats>().defaultBaseScale.x;
                float y = self.gameObject.GetComponent<SpinachStats>().defaultBaseScale.y;
                float z = self.gameObject.GetComponent<SpinachStats>().defaultBaseScale.z;
                self.modelLocator.modelTransform.localScale = new UnityEngine.Vector3(x + spinachCount * x / 2, y + spinachCount * y / 2, z + spinachCount * z / 2);

                // Update camera location
                Vector3 basePos = self.gameObject.GetComponent<SpinachStats>().defaultCameraPos;
                Vector3 spinachPos = new Vector3(0.0f, spinachCount * 1.8f, -spinachCount * 1.8f);
                self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos = basePos + spinachPos;

                // Call OnInventoryChanged
                orig(self);  
            };
        }
    }
}
