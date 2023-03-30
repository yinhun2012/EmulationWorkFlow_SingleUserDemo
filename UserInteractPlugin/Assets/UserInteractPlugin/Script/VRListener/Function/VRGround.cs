using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    public class VRGround : MonoBehaviour
    {
        public Transform viveCameraRig;
        public Transform vrPymid;

        void Start()
        {
            ViveControl.Instance.AddGripDownListener(VRGripDownCallback);
            ViveControl.Instance.AddGripHoldListener(VRGripHoldCallback);
            ViveControl.Instance.AddGripUpListener(VRGripUpCallback);
        }

        private void VRGripDownCallback(HandRole hand)
        {
            vrPymid.position = new Vector3(-10000, -10000, -10000);
            vrPymid.gameObject.SetActive(true);
        }

        private void VRGripHoldCallback(HandRole hand)
        {
            RaycastHit hit;
            if (ViveControl.Instance.PointAtCastHit(out hit, hand))
            {
                if (hit.transform.tag == "ViveGround")
                {
                    vrPymid.position = hit.point + new Vector3(0, 0.2f, 0);
                }
            }
        }

        private void VRGripUpCallback(HandRole hand)
        {
            RaycastHit hit;
            if (ViveControl.Instance.PointAtCastHit(out hit, hand))
            {
                if (hit.transform.tag == "ViveGround")
                {
                    Vector3 pos = new Vector3(hit.point.x, viveCameraRig.position.y, hit.point.z);
                    viveCameraRig.position = pos;
                }
            }
            vrPymid.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ViveControl.Instance?.RemoveGripDownListener(VRGripDownCallback);
            ViveControl.Instance?.RemoveGripHoldListener(VRGripHoldCallback);
            ViveControl.Instance?.RemoveGripUpListener(VRGripUpCallback);
        }
    }
}