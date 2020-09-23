using System.Reflection;
using R2API;
using RoR2;
using UnityEngine;

namespace SpaceSpinach {
    public class SpinachStats : MonoBehaviour {
        public Vector3 defaultBaseScale;
        public float defaultBaseHealth;
        public float defaultBaseDamage;
        public float defualtBaseSpeed;

        public void setBaseScale(Vector3 newScale) {
            defaultBaseScale = newScale;
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
    }
}
