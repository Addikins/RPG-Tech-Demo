using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 10f;
        [SerializeField] bool isHoming = false;
        [SerializeField] bool passThroughEnemy = false;
        [SerializeField] bool passThroughPlayer = false;

        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 5f;
        [SerializeField] GameObject[] destroyOnImpact = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onProjectileLaunch;
        [SerializeField] UnityEvent onProjectileLand;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            onProjectileLaunch.Invoke();
        }

        private void Update()
        {
            if (target == null) { return; }
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage += damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>())
            {
            if (other.GetComponent<Health>().IsDead()) { return; }
            }

            if (other.tag != "Player" && other.tag != "Enemy")
            {
                DestroyProjectile();
                return;
            }
            if (other.tag == "Player" && passThroughPlayer) { return; }
            if (other.tag == "Enemy" && passThroughEnemy) { return; }

            if (other != target.GetComponent<Collider>() || other.GetComponent<TerrainCollider>())
            {
                DestroyProjectile();
                return;
            }


            onProjectileLand.Invoke();
            target.TakeDamage(instigator, damage);
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            DestroyProjectile();
        }

        private void DestroyProjectile()
        {
            foreach (GameObject toDestroy in destroyOnImpact)
            {
                speed = 0;
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}