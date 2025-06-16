using UnityEngine;

namespace Scripts.Utility
{
    public class LookAtTarget : MonoBehaviour
    {
        [Header("타겟 설정")]
        [SerializeField] private Transform m_target;
        [SerializeField] private string m_targetTag = "Player";

        [Header("옵션")]
        [SerializeField] private bool m_onlyYAxis = true;
        [SerializeField] private bool m_reverse = true;

        private void Start()
        {
            if (m_target == null && !string.IsNullOrEmpty(m_targetTag))
            {
                GameObject targetObj = GameObject.FindWithTag(m_targetTag);
                if (targetObj != null)
                    m_target = targetObj.transform;
            }
        }

        private void Update()
        {
            if (m_target == null) return;

            Vector3 direction = m_target.position - transform.position;

            if (m_onlyYAxis)
                direction.y = 0;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (m_reverse)
            {
                Vector3 angles = targetRotation.eulerAngles;
                angles.y += 180;
                targetRotation = Quaternion.Euler(0, angles.y, 0);
            }

            transform.rotation = targetRotation;
        }
    }
}
