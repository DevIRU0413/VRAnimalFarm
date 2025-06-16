using UnityEngine;

namespace Scripts.UI
{
    public class PopupOpener : MonoBehaviour
    {
        public bool m_isStartOpenPopup = false;
        public GameObject popupPrefab;

        protected Canvas m_canvas;

        protected virtual void Start()
        {
            m_canvas = GetComponentInParent<Canvas>();

            if (m_canvas != null)
                FindMainCamera();

            if (m_isStartOpenPopup)
                OpenPopup();
        }

        private void FindMainCamera()
        {
            m_canvas.worldCamera = Camera.main;
        }

        public virtual void OpenPopup()
        {
            var popup = Instantiate(popupPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.SetParent(m_canvas.transform, false);
            popup.GetComponent<Popup>().Open();
        }
    }
}
