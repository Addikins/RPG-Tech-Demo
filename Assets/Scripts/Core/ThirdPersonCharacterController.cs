using System;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{

    public class ThirdPersonCharacterController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float gravity = -10f;
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float runSpeed = 6f;
        [SerializeField] float rollSpeed = 5f;
        [SerializeField] float jumpHeight = 2f;
        [SerializeField] float maxRollTime = 1f;
        [SerializeField] float rollDelay = .25f;
        [SerializeField] float fallDelay = .2f;
        [SerializeField] float bonusSpeedMultiplier = 1.5f;
        [SerializeField] bool hasSpeedBonus = true;
        [SerializeField] float speedSmoothTime = 0.15f;
        [SerializeField] float turnSmoothTime = 0.2f;
        [Header("Animations")]
        [SerializeField] float walkingAnimationSpeed = .5f;
        [SerializeField] float runningAnimationSpeed = 1f;
        [SerializeField] float rollingAnimationSpeed = 1f;
        [SerializeField] float maxAnimationSpeed = 1.5f;
        [SerializeField] int idleAnimationsCount = 47;
        [Header("Combat")]
        [SerializeField] float attackCooldown = .5f;
        [SerializeField] float attackTime = .5f;
        [Header("Raycasts & Cursors")]
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float weaponCursorRange = 5f;
        [SerializeField] float raycastRadius = 1f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        private enum State
        {
            Normal,
            Rolling,
            Falling,
            Attacking,
        }

        private State state;
        private float timeSpentRolling;
        private float timeSinceLastIdleAnimation;

        private float currentRunSpeed;
        private float maxSpeed;
        private float defaultRunAnimation;
        private float distanceToGround;
        private float velocityY;
        private float timeOffGround;
        private float timeAttacking;
        private float timeSinceLastAttack;
        private float timeSinceLastInput;
        private float idleInterval = 3f;
        private Vector3 velocity;

        float turnSmoothVelocity;
        float speedSmoothVelocity;
        float currentSpeed;
        Transform cameraT;
        Animator animator;
        CharacterController controller;
        Health health;


        private void Awake()
        {
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            state = State.Normal;
            defaultRunAnimation = runningAnimationSpeed;
            currentSpeed = walkSpeed;
            maxSpeed = runSpeed * bonusSpeedMultiplier;
        }

        void Start()
        {
            cameraT = Camera.main.transform;
        }

        void Update()
        {
            if (InteractWithUI()) { return; }
            if (health.IsDead())
            {
                SetCursor(CursorType.Dead);
                return;
            }
            if (InteractWithComponent()) { return; }

            //if (InteractWithMovement()) { return; }
            SetCursor(CursorType.None);
        }

        private void FixedUpdate()
        {
            CheckState();
        }

        private void CheckState()
        {
            switch (state)
            {
                case State.Normal:
                    if (!Input.anyKeyDown) { timeSinceLastInput += Time.deltaTime; } else { timeSinceLastInput = 0; }

                    CheckGround();
                    Movement();
                    Jump();
                    Roll();
                    CheckAttack();
                    break;
                case State.Rolling:
                    Rolling();
                    break;
                case State.Falling:
                    Falling();
                    CheckGround();
                    break;
                case State.Attacking:
                    Attacking();
                    break;
            }
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        //public void SetMoveIndicator(Vector3 target)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Instantiate(movementIndicator, (target + (Vector3.up / 8)), Quaternion.Euler(-90, 0, 0));
        //    }
        //}

        //public bool RaycastNavMesh(out Vector3 target)
        //{
        //    target = new Vector3();
        //    RaycastHit hit;
        //    bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        //    if (!hasHit) { return false; }

        //    NavMeshHit navMeshHit;
        //    bool hasCastToNavMesh = NavMesh.SamplePosition(
        //        hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

        //    if (!hasCastToNavMesh) { return false; }

        //    target = navMeshHit.position;

        //    return true;
        //}

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void Movement()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                hasSpeedBonus = !hasSpeedBonus;
            }

            CheckBonusSpeed();

            ToggleIdleAnimation();

            bool running = Input.GetKey(KeyCode.LeftShift);
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;

            if (inputDir != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            }
            float targetSpeed = ((running) ? currentRunSpeed : walkSpeed) * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

            HandleMovement();

            if (controller.isGrounded)
            {
                velocityY = 0;
            }

            float animationSpeed = ((running) ? runningAnimationSpeed : walkingAnimationSpeed) * inputDir.magnitude;
            animator.SetFloat("forwardSpeed", animationSpeed, speedSmoothTime, Time.deltaTime);
        }

        private void HandleMovement()
        {
            velocityY += gravity * Time.deltaTime;
            velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);
        }

        private void Roll()
        {
            if (Input.GetMouseButtonDown(2))
            {
                currentSpeed = Mathf.Max(rollSpeed, currentSpeed);
                animator.SetTrigger("roll");
                state = State.Rolling;
                timeSpentRolling = 0f;
            }
        }

        private void Rolling()
        {
            float delay;
            //Differenciates between run-to-roll vs stand-to-roll
            if (animator.GetFloat("forwardSpeed") > 1) { delay = 0; } else { delay = rollDelay; }

            timeSpentRolling += Time.deltaTime;
            if (timeSpentRolling > delay && timeSpentRolling < maxRollTime)
            {
                HandleMovement();
            }

            if (timeSpentRolling >= maxRollTime)
            {
                state = State.Normal;
            }
        }

        private void Falling()
        {
            HandleMovement();
        }

        private void CheckGround()
        {
            if (controller.isGrounded)
            {
                animator.SetBool("isGrounded", controller.isGrounded);
                state = State.Normal;
                timeOffGround = 0;
                return;
            }
            timeOffGround += Time.deltaTime;

            if (timeOffGround > fallDelay)
            {
                animator.SetBool("isGrounded", controller.isGrounded);
                state = State.Falling;
            }
        }

        private void CheckBonusSpeed()
        {
            if (hasSpeedBonus)
            {
                currentRunSpeed = maxSpeed;
                runningAnimationSpeed = maxAnimationSpeed;
                return;
            }
            currentRunSpeed = runSpeed;
            runningAnimationSpeed = defaultRunAnimation;
        }

        private void CheckAttack()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && timeSinceLastAttack > attackCooldown)
            {
                animator.SetTrigger("attack");
                animator.SetFloat("attackMotion", UnityEngine.Random.Range(0, 4));
                state = State.Attacking;
                timeSinceLastAttack = 0f;
                return;
            }

        }

        private void Attacking()
        {
            if (timeAttacking > attackTime)
            {
                timeAttacking = 0;
                state = State.Normal;
                print("Stopping Attack");
            }
            timeAttacking += Time.deltaTime;
        }

        private void Jump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("jump");
                float jumpVelocity = (float)Math.Sqrt(-2 * gravity * jumpHeight);
                velocityY = jumpVelocity;
                state = State.Falling;
            }
        }

        private void ToggleIdleAnimation()
        {
            timeSinceLastIdleAnimation += Time.deltaTime;

            if (timeSinceLastInput >= idleInterval && timeSinceLastIdleAnimation >= idleInterval)
            {
                animator.SetTrigger("idle");
                timeSinceLastIdleAnimation = 0;
            }
        }

        public float GetWeaponCursorRange()
        {
            return weaponCursorRange;
        }

        public void SetIdleInterveral(float interval)
        {
            idleInterval = interval;
        }

    }
}