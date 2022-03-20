using OneEyedJoe.Creatures;
using OneEyedJoe.Creatures.Hero;
using UnityEngine;

namespace OneEyedJoe.Components.Collectables
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}
