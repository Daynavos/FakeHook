using System;
using UnityEngine;

namespace _Scripts
{
    public class Grapple : MonoBehaviour
    {
        public Transform hookRoot;              // parent object containing rope + hook head (can be initially disabled)
        public Transform ropeTransform;         // child transform representing rope sprite (assumes sprite faces right)
        public Transform hookHeadTransform;
        public LayerMask hooks;
        public float hookSpeed = 40f;           // how quickly hook head moves toward target
        public float attachRadius = 0.2f;
        public event Action OnAttached;
        public event Action OnNotAttached;
        
        public bool IsAttached { get; private set; } = false;
        public Vector2 AttachPoint { get; private set; } = Vector2.zero;
        public bool IsFlying { get; private set; } = false;
        private Vector3 _target;
        
        void Update()
        {
            if (IsAttached)
            {
                Vector3 rootPos1 = hookRoot.position;
                Vector3 delta1 = hookHeadTransform.position - rootPos1;
                float distance1 = delta1.magnitude;
                ropeTransform.localScale = new Vector3(distance1, 0.5f, 1);
                hookHeadTransform.position = AttachPoint;
            }

            if (!IsFlying) return;
            Vector3 headPos = hookHeadTransform.position;
            // move head toward target
            Vector3 rawDir = (_target - headPos);
            Vector3 moveDir = rawDir.normalized;
            Vector3 newHeadPos = headPos + moveDir * (hookSpeed * Time.unscaledDeltaTime);
            hookHeadTransform.position = newHeadPos;
            
            float headAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            hookHeadTransform.rotation = Quaternion.Euler(0f, 0f, headAngle);
            
            Vector3 rootPos = hookRoot.position;
            Vector3 delta = newHeadPos - rootPos;
            float distance = delta.magnitude;

            
                // position rope at midpoint
            ropeTransform.position = rootPos + delta * 0.5f;

            // rotate rope to face head. Use Transform.right if sprite faces +X (right).

            ropeTransform.right = delta.normalized;
            
            
            ropeTransform.localScale = new Vector3(distance, 0.5f, 1);
            
            Collider2D hitHook = Physics2D.OverlapCircle(newHeadPos, attachRadius, hooks);
            if (hitHook != null)
            {
                // attached to a valid hook object
                IsFlying = false;
                IsAttached = true;
                AttachPoint = newHeadPos; // exact attach point
                OnAttached?.Invoke();
                return;
            }
            float distToTarget = Vector2.Distance(newHeadPos, _target);
            float reachedThresh = 0.1f;
            if (distToTarget <= reachedThresh)
            {
                IsFlying = false;
                IsAttached = false;
                hookRoot.localScale = Vector3.zero;
                OnNotAttached?.Invoke();
                return;
            }
            IsAttached = false;
        }

        void Start()
        {
            if (hookRoot != null)
                hookRoot.localScale = new Vector3(0, 0, 0);
            var sr = ropeTransform.GetComponent<SpriteRenderer>();
        }

        public void Throw(Vector2 target)
        {
            if (IsFlying|| IsAttached) return;
            // set target first
            _target = target;

            // position the head at the root start
            
            hookHeadTransform.position = hookRoot.position;
            
            // compute direction from root to target and rotate root/rope to face that direction immediately
            
            Vector3 fromRoot = (_target - hookRoot.position);
            
            float angle = Mathf.Atan2(fromRoot.y, fromRoot.x) * Mathf.Rad2Deg;
            hookRoot.rotation = Quaternion.Euler(0f, 0f, angle);
            
            hookRoot.localScale = Vector3.one;
            
            
            IsFlying = true;
        }

        public void Detach()
        {
            if (!IsAttached && !IsFlying) return;

            IsFlying = false;
            IsAttached = false;
            AttachPoint = Vector2.zero;
            hookRoot.localScale = Vector3.zero;
            OnNotAttached?.Invoke();
        }
    }
}