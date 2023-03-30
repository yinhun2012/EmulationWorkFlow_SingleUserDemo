using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    public class VRPyramid : MonoBehaviour
    {
        public float speed = 50f;

        void Update()
        {
            transform.localEulerAngles += new Vector3(0, Time.deltaTime * speed, 0);
        }
    }
}