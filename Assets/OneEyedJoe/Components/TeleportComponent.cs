using System.Collections;
using UnityEngine;

namespace OneEyedJoe.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;

        public void Teleport(GameObject target)
        {
            StartCoroutine(AnimateTeleport(target));
        }
        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var rb = target.GetComponent<Rigidbody2D>();

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return AnimationAlpha(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);

            target.SetActive(true);
            rb.simulated = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            yield return AnimationAlpha(sprite, 1);
        }

        private IEnumerator AnimationAlpha(SpriteRenderer sprite, float destAlpha)
        {
            var alphaTime = 0f;
            var spriteAlpha = sprite.color.a;
            while (alphaTime < _alphaTime)
            {
                alphaTime += Time.deltaTime;
                var progress = alphaTime / _alphaTime;
                var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;
            
                yield return null;
            }
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(
                    target.transform.position,
                    _destTransform.position, progress);
                
                yield return null;
            }
        }
    }
}
