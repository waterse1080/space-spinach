using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceSpinach {
    public class Hooks {
        internal static void Init() {
            int spinachCount;

            // On character load
            On.RoR2.CharacterMaster.Awake += (orig, self) => {
                // Call Awake
                orig(self);
                // Attach stat tracking component to each master object
                self.gameObject.AddComponent<SpinachStats>();
            };

            // Recalculation of numerical stats
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                // Avoiding null reference calls for background birds and level hazards 
                if (self.inventory != null) {
                    
                    // Get SpaceSpinach # if the object has an inventory
                    spinachCount = self.inventory.GetItemCount(Assets.SpaceSpinachItemIndex);
                    
                    // If master stats have not been set, update them. Should happen once per master.
                    if (self.master.gameObject.GetComponent<SpinachStats>().defaultBaseHealth == 0.0f) {
                        self.master.gameObject.GetComponent<SpinachStats>().setBaseHealth(self.baseMaxHealth);
                        self.master.gameObject.GetComponent<SpinachStats>().setBaseDamage(self.baseDamage);
                        self.master.gameObject.GetComponent<SpinachStats>().setBaseSpeed(self.baseMoveSpeed);
                        self.master.gameObject.GetComponent<SpinachStats>().setBaseJump(self.baseJumpPower);
                    }

                    // Update HP
                    self.baseMaxHealth = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseHealth + 10 * spinachCount;
                    // Update Damage
                    self.baseDamage = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseDamage + 5 * spinachCount;
                    // Update Speed
                    self.baseMoveSpeed = self.master.gameObject.GetComponent<SpinachStats>().defualtBaseSpeed + spinachCount;
                    // Update Jump Height
                    self.baseJumpPower = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseJump + 2 * spinachCount;
                }                

                // Call Recalculate stats
                orig(self);
            };

            // On entity load or inventory change of entity
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) => {
                
                // Get the ammount of the item currently on the Character Body
                spinachCount = self.inventory.GetItemCount(Assets.SpaceSpinachItemIndex);
                
                // If master stats have not been set, update them. Should happen once per master.
                if (self.master.gameObject.GetComponent<SpinachStats>().defaultBaseScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                    self.master.gameObject.GetComponent<SpinachStats>().setBaseScale(self.modelLocator.modelTransform.localScale);
                    self.master.gameObject.GetComponent<SpinachStats>().setCameraPos(self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos);
                }

                // Update the size of the entity based off of the # of the item held
                float x = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseScale.x;
                float y = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseScale.y;
                float z = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseScale.z;
                
                // +50% of base scale per held item
                self.modelLocator.modelTransform.localScale = new UnityEngine.Vector3(x + spinachCount * x / 2, y + spinachCount * y / 2, z + spinachCount * z / 2);

                // Update camera location
                // TODO: return to default on exit via menu
                Vector3 basePos = self.master.gameObject.GetComponent<SpinachStats>().defaultCameraPos;
                Vector3 spinachPos = new Vector3(0.0f, spinachCount * 1.8f, -spinachCount * 1.8f);
                self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos = basePos + spinachPos;

                // Call OnInventoryChanged
                orig(self);
            };
        }
    }
}
