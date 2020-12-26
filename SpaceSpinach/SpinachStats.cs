using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;

namespace SpaceSpinach {
    // This is where the default game stats, size scale, and camera position are stored for each game object that has space spinach
    public class SpinachStats : MonoBehaviour {
        public Vector3 defaultBaseScale;
        public Vector3 defaultCameraPos;
        public float defaultBaseHealth;
        public float defaultBaseDamage;
        public float defualtBaseSpeed;
        public float defaultBaseJump;

        public void setBaseScale(Vector3 newScale) {
            defaultBaseScale = newScale;
        }

        public void setCameraPos(Vector3 newPos) {
            defaultCameraPos = newPos;
        }

        public void setBaseHealth(float newHealth) {
            defaultBaseHealth = newHealth;
        }

        public void setBaseDamage(float newDamage) {
            defaultBaseDamage = newDamage;
        }

        public void setBaseSpeed(float newSpeed) {
            defualtBaseSpeed = newSpeed;
        }

        public void setBaseJump(float newJump) {
            defaultBaseJump = newJump;
        }
    }
}
