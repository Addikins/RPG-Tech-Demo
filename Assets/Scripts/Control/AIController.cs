using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float idleTime = 2f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedMultiplier = 0.2f;

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float aggroTime = Mathf.Infinity;
        int currentWaypointIndex = 0;

        LazyValue<Vector3> guardPosition;

        private void Awake()
        {
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (IsAggro(player) && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            UpdateTimers();
        }

        public void Aggro()
        {
            // Sets Timer
            aggroTime = 0;
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            aggroTime += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > idleTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedMultiplier);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggroMobEnemies();
        }

        private void AggroMobEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController enemyInstance = hit.collider.GetComponent<AIController>();
                if (enemyInstance == null) continue;

                enemyInstance.Aggro();
            }
        }

        private bool IsAggro(GameObject player)
        {
            if (aggroTime < suspicionTime) { return true; }
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity during editing
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, shoutDistance);
        }
    }
}