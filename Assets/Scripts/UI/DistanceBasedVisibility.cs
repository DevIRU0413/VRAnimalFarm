using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts.Util;

namespace Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DistanceBasedVisibility : MonoBehaviour
    {
        [Header("대상")]
        [SerializeField] private Transform m_target; // 예: 플레이어 HMD

        [Header("거리 설정")]
        [SerializeField] private float m_visibleRange = 3f;

        [Header("Fade 설정")]
        [SerializeField] private float m_fadeInTime = 0.3f;
        [SerializeField] private float m_fadeOutTime = 0.3f;

        [Header("Look At 설정")]
        [SerializeField] private bool m_lookAtTarget = false;
        [SerializeField] private bool m_lookBackwards = true;

        [Header("Debug")]
        [SerializeField] private bool m_drawGizmo = true;

        private CanvasGroup m_canvasGroup;
        private Coroutine m_fadeCoroutine;
        private bool m_isVisible = false;

        private void Start()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            if (m_target == null)
                m_target = GameObject.FindWithTag("Player")?.transform;

            SetVisible(false, true); // 처음에는 안 보이게
        }

        private void Update()
        {
            if (m_target == null) return;

            float dist = Vector3.Distance(m_target.position, transform.position);
            bool shouldShow = dist <= m_visibleRange;

            if (shouldShow != m_isVisible)
            {
                m_isVisible = shouldShow;
                SetVisible(m_isVisible);
            }

            if (m_lookAtTarget)
            {
                transform.LookAt(m_target);
                if (m_lookBackwards)
                {
                    var rot = transform.rotation.eulerAngles;
                    rot.y += 180;
                    transform.rotation = Quaternion.Euler(0, rot.y, 0);
                }
            }
        }

        public void SetVisible(bool show, bool instant = false)
        {
            if (m_fadeCoroutine != null)
                StopCoroutine(m_fadeCoroutine);

            if (instant)
            {
                m_canvasGroup.alpha = show ? 1 : 0;
                SetChildrenActive(show);
            }
            else
            {
                m_fadeCoroutine = StartCoroutine(FadeRoutine(show));
            }
        }

        private IEnumerator FadeRoutine(bool fadeIn)
        {
            float duration = fadeIn ? m_fadeInTime : m_fadeOutTime;
            float start = m_canvasGroup.alpha;
            float end = fadeIn ? 1f : 0f;
            float time = 0f;

            if (fadeIn)
                SetChildrenActive(true);

            while (time < 1f)
            {
                time += Time.deltaTime / duration;
                m_canvasGroup.alpha = Mathf.Lerp(start, end, time);
                yield return null;
            }

            m_canvasGroup.alpha = end;

            if (!fadeIn)
                SetChildrenActive(false);

            m_fadeCoroutine = null;
        }

        private void SetChildrenActive(bool active)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(active);
        }

        private void OnDrawGizmos()
        {
            if (!m_drawGizmo) return;

            Color color = Color.red;
            color.a = 0.4f;
            Gizmos.color = color;
            GizmosEx.DrawGizmoDisk(transform, m_visibleRange);
        }
    }
}
