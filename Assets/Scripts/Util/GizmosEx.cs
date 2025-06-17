using UnityEngine;

namespace Scripts.Util
{
    public static class GizmosEx
    {
        private const float GIZMO_DISK_THICKNESS = 0.01f;
        public static void DrawGizmoDisk(this Transform t, float radius)
        {
            Matrix4x4 oldMatrix = Gizmos.matrix;
            // Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.5f); //this is gray, could be anything
            Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, new Vector3(1, GIZMO_DISK_THICKNESS, 1));
            Gizmos.DrawSphere(Vector3.zero, radius);
            Gizmos.matrix = oldMatrix;
        }
    }
}
