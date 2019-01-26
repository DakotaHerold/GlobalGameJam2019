using UnityEngine;
using System.Collections;

namespace Jam
{
    public class OrthoSmoothFollow : MonoBehaviour
    {

        public Transform target;
        public float smoothTime = 0.3f;

        private Vector3 velocity = Vector3.zero;

        void FixedUpdate()
        {
            Vector3 goalPos = target.position;
            goalPos.z = transform.position.z;
            Vector3 newTransform = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
            transform.position = new Vector3(newTransform.x, newTransform.y, newTransform.z);
        }
    }
}