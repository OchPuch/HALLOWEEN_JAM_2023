using UnityEngine;

namespace Pistol
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float speed = 20f;
        public GameObject hitEffect;
        void Start()
        {
            rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var killable = other.gameObject.GetComponent<IKillable>();
            if (killable != null)
            {
                killable.Kill();
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Things"))
            {
                var thing = other.GetComponent<InteractableThing>();
                if (thing != null && !thing.onlySoul)
                {
                    thing.Interact();
                    DestroyBullet();
                }
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
            {
                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
