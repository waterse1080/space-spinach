using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceSpinach {
    public class Hooks {
        internal static void Init() {

            // On character load
            On.RoR2.CharacterMaster.Awake += (orig, self) => {
                
                // Call Awake
                orig(self);
                
                // Attach stat tracking component to each master object
                self.gameObject.AddComponent<SpinachStats>();
                
                // Set item index number
                self.gameObject.GetComponent<SpinachStats>().setItemIndex(ItemCatalog.FindItemIndex("SpaceSpinach"));
            };

            // On recalculation of numerical stats
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {

                // Avoiding null reference calls for background birds and level hazards
                // Disabled for artifact boss
                if (self.inventory != null && self.name != "ArtifactShellBody" && self.name!= "ArtifactShellBody(Clone)") {

                    // Get stat tracker
                    SpinachStats stats = self.master.gameObject.GetComponent<SpinachStats>();

                    // Get the ammount of the item currently on the Character Body
                    int spinachCount = self.inventory.GetItemCount(stats.spaceSpinachItemIndex);

                    // If master stats have not been set, update them. Should happen once per master.
                    if (stats.defaultBaseHealth == 0.0f) {
                        stats.setBaseHealth(self.baseMaxHealth);
                        stats.setBaseDamage(self.baseDamage);
                        stats.setBaseSpeed(self.baseMoveSpeed);
                        stats.setBaseJump(self.baseJumpPower);
                        // Chat.AddMessage("These should be equal: " + stats.defaultBaseHealth + " == " + self.baseMaxHealth);
                    }

                    // Update HP
                    // Changed from 10 to 50% of base
                    self.baseMaxHealth = stats.defaultBaseHealth + (stats.defaultBaseHealth / 2) * spinachCount;
                    // Update Damage
                    // Changed from 5 to 25% of base
                    self.baseDamage = stats.defaultBaseDamage + (stats.defaultBaseDamage / 4) * spinachCount;
                    // Update Speed
                    // Changed from 1 to 33% of base
                    self.baseMoveSpeed = stats.defualtBaseSpeed + (stats.defualtBaseSpeed / 3) * spinachCount;
                    // Update Jump Height
                    // Canged from 2 to 66% of base
                    self.baseJumpPower = stats.defaultBaseJump + (stats.defaultBaseJump / 3) * 2 * spinachCount;
                }

                // Call Recalculate stats
                orig(self);
            };

            // On entity load or inventory change of entity
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) => {

                // Disabled for artifact boss
                if (self.name != "ArtifactShellBody" && self.name != "ArtifactShellBody(Clone)") {

                    // Get stat tracker
                    SpinachStats stats = self.master.gameObject.GetComponent<SpinachStats>();

                    // Get the ammount of the item currently on the Character Body
                    int spinachCount = self.inventory.GetItemCount(stats.spaceSpinachItemIndex);

                    // If master stats have not been set, update them. Should happen once per master.
                    if (stats.defaultBaseScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                        stats.setBaseScale(self.modelLocator.modelTransform.localScale);
                        stats.setCameraPos(self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos);
                    }

                    // Update the size of the entity based off of the # of the item held
                    Vector3 defaultBaseScale = stats.defaultBaseScale;
                    float x = defaultBaseScale.x;
                    float y = defaultBaseScale.y;
                    float z = defaultBaseScale.z;

                    // Disable scaling for REX
                    if (self.name != "TreebotBody(Clone)") {

                        // Changed from 50% to 250% of base scale per held item
                        self.modelLocator.modelTransform.localScale = new Vector3(x + spinachCount * x * 2.5f, y + spinachCount * y * 2.5f, z + spinachCount * z * 2.5f);

                        // Update camera location
                        if (self.isPlayerControlled) {
                            Vector3 basePos = stats.defaultCameraPos;
                            Vector3 spinachPos = new Vector3(0.0f, spinachCount * 1.8f * 5, -spinachCount * 1.8f * 5);
                            self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos = basePos + spinachPos;
                        }
                    } 
                }

                // Call OnInventoryChanged
                orig(self);
            };

            // On body destruction, including exiting to menus
            On.RoR2.CharacterBody.OnDestroy += (orig, self) => {

                // Makes sure that camera position is not messed up between games on the same character
                if (self.isPlayerControlled) {
                    self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos = self.master.gameObject.GetComponent<SpinachStats>().defaultCameraPos;
                }

                // Call OnDestroy
                orig(self);
            };
        }
    }
}
