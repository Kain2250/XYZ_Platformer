using System.Collections;
using UnityEngine;

namespace OneEyedJoe.Creatures.Mobs.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();

    }
}
