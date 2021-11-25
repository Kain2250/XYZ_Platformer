using System.Collections;
using UnityEngine;

namespace OneEyedJoe.Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();

    }
}
