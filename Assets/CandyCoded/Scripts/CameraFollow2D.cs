using System.Collections;
using UnityEngine;

namespace CandyCoded
{

    [System.Serializable]
    public struct CameraConstraints2D
    {
        [Header("Freeze Original Position")]
        public bool freezePositionX;
        public bool freezePositionY;
        [Header("Maintain Origin Offset")]
        public bool maintainOffsetX;
        public bool maintainOffsetY;
        [Header("Restrict Viewport to Transform")]
        public Transform boundsTransform;
        [Header("(or)")]
        [Header("Restrict Viewport to Manually Defined Bounds")]
        public Bounds bounds;
    }

    public class CameraFollow2D : MonoBehaviour
    {

        public bool tracking = true;

        public Transform mainTarget;

        public float dampRate = 0.3f;

        public CameraConstraints2D constraints;

        private Transform cameraTransform;
        private float cameraOrthographicSize;

        private Vector3 cameraPositionOffset = Vector2.zero;

        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {

            cameraTransform = Camera.main.transform;
            cameraOrthographicSize = Camera.main.orthographicSize;

            if (mainTarget == null)
            {

                mainTarget = gameObject.transform;

            }

            cameraPositionOffset = new Vector2(
                cameraTransform.position.x - mainTarget.transform.position.x,
                cameraTransform.position.y - mainTarget.transform.position.y
            );

        }

        private void LateUpdate()
        {

            if (tracking && mainTarget)
            {

                Vector3 newPosition = mainTarget.transform.position;

                if (constraints.maintainOffsetX) newPosition.x += cameraPositionOffset.x;
                if (constraints.maintainOffsetY) newPosition.y += cameraPositionOffset.y;

                if (constraints.boundsTransform)
                {

                    constraints.bounds = CandyCoded.Calculation.ParentBounds(constraints.boundsTransform);

                }

                float cameraExtentHorizontal = cameraOrthographicSize * Screen.width / Screen.height;

                if (constraints.bounds.size.magnitude != 0)
                {

                    newPosition.x = Mathf.Clamp(newPosition.x, constraints.bounds.min.x + cameraExtentHorizontal, constraints.bounds.max.x - cameraExtentHorizontal);
                    newPosition.y = Mathf.Clamp(newPosition.y, constraints.bounds.min.y + cameraOrthographicSize, constraints.bounds.max.y - cameraOrthographicSize);

                }

                if (constraints.freezePositionX) newPosition.x = cameraTransform.position.x;
                if (constraints.freezePositionY) newPosition.y = cameraTransform.position.y;

                newPosition.z = cameraTransform.position.z;

                cameraTransform.position = Vector3.SmoothDamp(
                    cameraTransform.position,
                    newPosition,
                    ref velocity,
                    dampRate
                );

            }

        }

    }

}
