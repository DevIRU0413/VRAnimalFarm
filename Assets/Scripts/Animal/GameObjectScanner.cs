using Unity.VisualScripting;

using UnityEngine;

namespace Scripts.Animal
{
    public class GameObjectScanner : MonoBehaviour
    {
        [Header("탐지 설정")]
        [SerializeField] private float m_checkRadius = 3f;
        [SerializeField] private LayerMask m_findLayer;

        [Header("디버그")]
        [SerializeField] private bool drawGizmo = true;

        public Collider[] ClosestFoods { get; private set; }
        public Collider ClosestObject { get; private set; }

        public bool ScanForLayerObject()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, m_checkRadius, m_findLayer);

            float closestDist = float.MaxValue;
            ClosestObject = null;

            foreach (var hit in hits)
            {
                if (hit != null)
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        ClosestObject = hit;
                    }
                }
            }

            ClosestFoods = hits;
            return ClosestObject != null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmo) return;
            Color color = Color.yellow;
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, m_checkRadius);
        }
    }
}
