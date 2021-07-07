using R2API;
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
                /// self.gameObject.GetComponent<SpinachStats>().setItemIndex(ItemCatalog.FindItemIndex("SpaceSpinach"));
            };
            

            // Recalculation of numerical stats
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                // Avoiding null reference calls for background birds and level hazards
                // Artifact boss no longer dies on spawn
                if (self.inventory != null && self.name != "ArtifactShellBody" && self.name!= "ArtifactShellBody(Clone)") {

                    // Get SpaceSpinach # if the object has an inventory
                    // TODO: get item index once
                    ItemIndex spaceSpinachItemIndex = ItemCatalog.FindItemIndex("SpaceSpinach");
                    spinachCount = self.inventory.GetItemCount(spaceSpinachItemIndex);

                    // If master stats have not been set, update them. Should happen once per master.
                    // TODO: Replace all of these GetComponent calls with an initial pointer decleration of SpinachStats
                    SpinachStats stats = self.master.gameObject.GetComponent<SpinachStats>();
                    if (stats.defaultBaseHealth == 0.0f) {
                        stats.setBaseHealth(self.baseMaxHealth);
                        stats.setBaseDamage(self.baseDamage);
                        stats.setBaseSpeed(self.baseMoveSpeed);
                        stats.setBaseJump(self.baseJumpPower);
                        // Chat.AddMessage("These should be equal: " + stats.defaultBaseHealth + " == " + self.baseMaxHealth);
                    }

                    // Update HP
                    self.baseMaxHealth = stats.defaultBaseHealth + 10 * spinachCount;
                    // Update Damage
                    self.baseDamage = stats.defaultBaseDamage + 5 * spinachCount;
                    // Update Speed
                    self.baseMoveSpeed = stats.defualtBaseSpeed + spinachCount;
                    // Update Jump Height
                    self.baseJumpPower = stats.defaultBaseJump + 2 * spinachCount;
                    /**
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
                    **/
                }

                // Call Recalculate stats
                orig(self);
            };

            // On entity load or inventory change of entity
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) => {
                // Disabled for artifact boss
                if (self.name != "ArtifactShellBody" && self.name != "ArtifactShellBody(Clone)") {
                    // Get the ammount of the item currently on the Character Body
                    ItemIndex spaceSpinachItemIndex = ItemCatalog.FindItemIndex("SpaceSpinach");
                    spinachCount = self.inventory.GetItemCount(spaceSpinachItemIndex);

                    // If master stats have not been set, update them. Should happen once per master.
                    // TODO: Replace GetComponent calls with single pointer decleration
                    if (self.master.gameObject.GetComponent<SpinachStats>().defaultBaseScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                        self.master.gameObject.GetComponent<SpinachStats>().setBaseScale(self.modelLocator.modelTransform.localScale);
                        self.master.gameObject.GetComponent<SpinachStats>().setCameraPos(self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos);
                    }

                    // Update the size of the entity based off of the # of the item held
                    Vector3 defaultBaseScale = self.master.gameObject.GetComponent<SpinachStats>().defaultBaseScale;
                    float x = defaultBaseScale.x;
                    float y = defaultBaseScale.y;
                    float z = defaultBaseScale.z;

                    // +50% of base scale per held item
                    self.modelLocator.modelTransform.localScale = new Vector3(x + spinachCount * x / 2, y + spinachCount * y / 2, z + spinachCount * z / 2);

                    // Update camera location
                    // TODO: return to default on exit via menu
                    Vector3 basePos = self.master.gameObject.GetComponent<SpinachStats>().defaultCameraPos;
                    Vector3 spinachPos = new Vector3(0.0f, spinachCount * 1.8f, -spinachCount * 1.8f);
                    self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos = basePos + spinachPos;
                }
                // Call OnInventoryChanged
                orig(self);
            };

            // Makes sure that camera position is not messed up between games on the same character
            // TODO: Get rid of erronious errors in the log on non-player deaths / stage clear
            On.RoR2.CharacterBody.OnDestroy += (orig, self) => {
                self.GetComponent<CameraTargetParams>().cameraParams.standardLocalCameraPos = self.master.gameObject.GetComponent<SpinachStats>().defaultCameraPos;

                orig(self);
            };
        }
    }
}
