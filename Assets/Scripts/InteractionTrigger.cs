using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class InteractionTrigger : MonoBehaviour
    {
        [SerializeField] private float _actionDelay;
        [SerializeField] private UnityEvent _events;
        [SerializeField] private float _currentCount;
        private bool _countedThisFrame;

        private void OnEnable() => ResetCount();
        private void LateUpdate() => HandleCount();

        private void HandleCount()
        {
            if (!_countedThisFrame) ResetCount();

            _countedThisFrame = false;
        }

        public void Counting()
        {
            _currentCount -= Time.deltaTime;
            _countedThisFrame = true;
            if (_currentCount <= 0)
            {
                _events?.Invoke();
            }
        }

        private void ResetCount()
        {
            _currentCount = _actionDelay;
        }

    }
}
