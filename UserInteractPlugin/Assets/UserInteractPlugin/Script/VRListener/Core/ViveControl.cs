using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    /// <summary>
    /// vive管理器
    /// </summary>
    public class ViveControl : MonoSingleton<ViveControl>
    {
        public const string HTC_VIVE_GROUND_TAG = "ViveGround";

        public Transform VRRegion;

        public VRJoystickLeft joystickLeft;
        public VRJoystickRight joystickRight;

        public AVRJoystickBase GetJoystickBase(HandRole hand)
        {
            switch (hand)
            {
                case HandRole.LeftHand:
                    return joystickLeft;
                case HandRole.RightHand:
                    return joystickRight;
            }
#if UNITY_EDITOR
            Debug.LogErrorFormat("ViveControl GetJoystickBase Error hand = {0}", hand);
#endif
            return null;
        }

        #region ///event
        public delegate void JoystickDotHold(HandRole hand, Vector3 wpos);
        public delegate void JoystickGripDown(HandRole hand);
        public delegate void JoystickGripHold(HandRole hand);
        public delegate void JoystickGripUp(HandRole hand);
        public delegate void JoystickMenuDown(HandRole hand);
        public delegate void JoystickMenuHold(HandRole hand);
        public delegate void JoystickMenuUp(HandRole hand);
        public delegate void JoystickPadDown(HandRole hand, Vector2 axis);
        public delegate void JoystickPadQuadrantDown(HandRole hand, EPadQuadrantType pqtype);
        public delegate void JoystickPadUp(HandRole hand, Vector2 axis);
        public delegate void JoystickPadTouch(HandRole hand, Vector2 axis);
        public delegate void JoystickPadTouchDelta(HandRole hand, Vector2 delta);
        public delegate void JoystickPadHold(HandRole hand, Vector2 axis);
        public delegate void JoystickPadHoldDelta(HandRole hand, Vector2 delta);
        public delegate void JoystickTriggerDown(HandRole hand, GameObject go);
        public delegate void JoystickTriggerHold(HandRole hand, GameObject go);
        public delegate void JoystickTriggerUp(HandRole hand, GameObject go);
        public delegate void JoystickCollideDown(HandRole hand, GameObject go);
        public delegate void JoystickCollideHold(HandRole hand, GameObject go);
        public delegate void JoystickCollideUp(HandRole hand, GameObject go);
        public delegate void JoystickGrabDown(HandRole hand, GameObject go, Vector3 jpos, Vector3 jang);
        public delegate void JoystickGrabHold(HandRole hand, GameObject go, Vector3 wpos, Vector3 wang, Vector3 jpos, Vector3 jang);
        public delegate void JoystickGrabUp(HandRole hand, GameObject go);

        private JoystickDotHold joystickDotHoldCallback;
        private JoystickGripDown joystickGripDownCallback;
        private JoystickGripHold joystickGripHoldCallback;
        private JoystickGripUp joystickGripUpCallback;
        private JoystickMenuDown joystickMenuDownCallback;
        private JoystickMenuHold joystickMenuHoldCallback;
        private JoystickMenuUp joystickMenuUpCallback;
        private JoystickPadDown joystickPadDownCallback;
        private JoystickPadQuadrantDown joystickPadQuadrantDownCallback;
        private JoystickPadUp joystickPadUpCallback;
        private JoystickPadTouch joystickPadTouchCallback;
        private JoystickPadTouchDelta joystickPadTouchDeltaCallback;
        private JoystickPadHold joystickPadHoldCallback;
        private JoystickPadHoldDelta joystickPadHoldDeltaCallback;
        private JoystickTriggerDown joystickTriggerDownCallback;
        private JoystickTriggerHold joystickTriggerHoldCallback;
        private JoystickTriggerUp joystickTriggerUpCallback;
        private JoystickCollideDown joystickCollideDownCallback;
        private JoystickCollideHold joystickCollideHoldCallback;
        private JoystickCollideUp joystickCollideUpCallback;
        private JoystickGrabDown joystickGrabDown;
        private JoystickGrabHold joystickGrabHold;
        private JoystickGrabUp joystickGrabUp;
        #endregion

        #region ///listener

        #region dot
        public void AddDotHoldListener(JoystickDotHold listener)
        {
            joystickDotHoldCallback += listener;
        }

        public void RemoveDotHoldListener(JoystickDotHold listener)
        {
            joystickDotHoldCallback -= listener;
        }

        public void BroadcastDotHoldListener(HandRole hand, Vector3 wpos)
        {
            joystickDotHoldCallback?.Invoke(hand, wpos);
        }
        #endregion

        #region grip
        public void AddGripDownListener(JoystickGripDown listener)
        {
            joystickGripDownCallback += listener;
        }

        public void RemoveGripDownListener(JoystickGripDown listener)
        {
            joystickGripDownCallback -= listener;
        }

        public void BroadcastGripDown(HandRole hand)
        {
            joystickGripDownCallback?.Invoke(hand);
        }

        public void AddGripHoldListener(JoystickGripHold listener)
        {
            joystickGripHoldCallback += listener;
        }

        public void RemoveGripHoldListener(JoystickGripHold listener)
        {
            joystickGripHoldCallback -= listener;
        }

        public void BroadcastGripHold(HandRole hand)
        {
            joystickGripHoldCallback?.Invoke(hand);
        }

        public void AddGripUpListener(JoystickGripUp listener)
        {
            joystickGripUpCallback += listener;
        }

        public void RemoveGripUpListener(JoystickGripUp listener)
        {
            joystickGripUpCallback -= listener;
        }

        public void BroadcastGripUp(HandRole hand)
        {
            joystickGripUpCallback?.Invoke(hand);
        }
        #endregion

        #region menu
        public void AddMenuDownListener(JoystickMenuDown listener)
        {
            joystickMenuDownCallback += listener;
        }

        public void RemoveMenuDownListener(JoystickMenuDown listener)
        {
            joystickMenuDownCallback -= listener;
        }

        public void BroadcastMenuDown(HandRole hand)
        {
            joystickMenuDownCallback?.Invoke(hand);
        }

        public void AddMenuHoldListener(JoystickMenuHold listener)
        {
            joystickMenuHoldCallback += listener;
        }

        public void RemoveMenuHoldListener(JoystickGripHold listener)
        {
            joystickGripHoldCallback -= listener;
        }

        public void BroadcastMenuHold(HandRole hand)
        {
            joystickMenuHoldCallback?.Invoke(hand);
        }

        public void AddMenuUpListener(JoystickMenuUp listener)
        {
            joystickMenuUpCallback += listener;
        }

        public void RemoveMenuUpListener(JoystickMenuUp listener)
        {
            joystickMenuUpCallback -= listener;
        }

        public void BroadcastMenuUp(HandRole hand)
        {
            joystickMenuUpCallback?.Invoke(hand);
        }

        #endregion

        #region pad
        //down
        public void AddPadDownListener(JoystickPadDown listener)
        {
            joystickPadDownCallback += listener;
        }

        public void RemovePadDownListener(JoystickPadDown listener)
        {
            joystickPadDownCallback -= listener;
        }

        public void BroadcastPadDown(HandRole hand, Vector2 point)
        {
            joystickPadDownCallback?.Invoke(hand, point);
        }
        //down quadrant
        public void AddPadQuadrantDownListener(JoystickPadQuadrantDown listener)
        {
            joystickPadQuadrantDownCallback += listener;
        }

        public void RemovePadQuadrantDownListener(JoystickPadQuadrantDown listener)
        {
            joystickPadQuadrantDownCallback -= listener;
        }

        public void BroadcastPadQuadrantDown(HandRole hand, EPadQuadrantType pqtype)
        {
            joystickPadQuadrantDownCallback?.Invoke(hand, pqtype);
        }
        //up
        public void AddPadUpListener(JoystickPadUp listener)
        {
            joystickPadUpCallback += listener;
        }

        public void RemovePadUpListener(JoystickPadUp listener)
        {
            joystickPadUpCallback -= listener;
        }

        public void BroadcastPadUp(HandRole hand, Vector2 point)
        {
            joystickPadUpCallback?.Invoke(hand, point);
        }
        //touch
        public void AddPadTouchListener(JoystickPadTouch listener)
        {
            joystickPadTouchCallback += listener;
        }

        public void RemovePadTouchListener(JoystickPadTouch listener)
        {
            joystickPadTouchCallback -= listener;
        }

        public void BroadcastPadTouch(HandRole hand, Vector2 point)
        {
            joystickPadTouchCallback?.Invoke(hand, point);
        }
        //touch delta
        public void AddPadTouchDeltaListener(JoystickPadTouchDelta listener)
        {
            joystickPadTouchDeltaCallback += listener;
        }

        public void RemovePadTouchDeltaListener(JoystickPadTouchDelta listener)
        {
            joystickPadTouchDeltaCallback -= listener;
        }

        public void BroadcastPadTouchDelta(HandRole hand, Vector2 point)
        {
            joystickPadTouchDeltaCallback?.Invoke(hand, point);
        }
        //hold
        public void AddPadHoldListener(JoystickPadHold listener)
        {
            joystickPadHoldCallback += listener;
        }

        public void RemovePadHoldListener(JoystickPadHold listener)
        {
            joystickPadHoldCallback -= listener;
        }

        public void BroadcastPadHold(HandRole hand, Vector2 point)
        {
            joystickPadHoldCallback?.Invoke(hand, point);
        }
        //hold delta
        public void AddPadHoldDeltaListener(JoystickPadHoldDelta listener)
        {
            joystickPadHoldDeltaCallback += listener;
        }

        public void RemovePadHoldDeltaListener(JoystickPadHoldDelta listener)
        {
            joystickPadHoldDeltaCallback -= listener;
        }

        public void BroadcastPadHoldDelta(HandRole hand, Vector2 point)
        {
            joystickPadHoldDeltaCallback?.Invoke(hand, point);
        }
        #endregion

        #region trigger
        public void AddTriggerDownListener(JoystickTriggerDown listener)
        {
            joystickTriggerDownCallback += listener;
        }

        public void RemoveTriggerDownListener(JoystickTriggerDown listener)
        {
            joystickTriggerDownCallback -= listener;
        }

        public void BroadcastTriggerDown(HandRole hand, GameObject go)
        {
            joystickTriggerDownCallback?.Invoke(hand, go);
        }

        public void AddTriggerHoldListener(JoystickTriggerHold listener)
        {
            joystickTriggerHoldCallback += listener;
        }

        public void RemoveTriggerHoldListener(JoystickTriggerHold listener)
        {
            joystickTriggerHoldCallback -= listener;
        }

        public void BroadcastTriggerHold(HandRole hand, GameObject go)
        {
            joystickTriggerHoldCallback?.Invoke(hand, go);
        }

        public void AddTriggerUpListener(JoystickTriggerUp listener)
        {
            joystickTriggerUpCallback += listener;
        }

        public void RemoveTriggerUpListener(JoystickTriggerUp listener)
        {
            joystickTriggerUpCallback -= listener;
        }

        public void BroadcastTriggerUp(HandRole hand, GameObject go)
        {
            joystickTriggerUpCallback?.Invoke(hand, go);
        }
        #endregion

        #region collide
        public void AddCollideDownListener(JoystickCollideDown listener)
        {
            joystickCollideDownCallback += listener;
        }

        public void RemoveCollideDownListener(JoystickCollideDown listener)
        {
            joystickCollideDownCallback -= listener;
        }

        public void BroadcastCollideDown(HandRole hand, GameObject go)
        {
            joystickCollideDownCallback?.Invoke(hand, go);
        }

        public void AddCollideHoldListener(JoystickCollideHold listener)
        {
            joystickCollideHoldCallback += listener;
        }

        public void RemoveCollideHoldListener(JoystickCollideHold listener)
        {
            joystickCollideHoldCallback -= listener;
        }

        public void BroadcastCollideHold(HandRole hand, GameObject go)
        {
            joystickCollideHoldCallback?.Invoke(hand, go);
        }

        public void AddCollideUpListener(JoystickCollideUp listener)
        {
            joystickCollideUpCallback += listener;
        }

        public void RemoveCollideUpListener(JoystickCollideUp listener)
        {
            joystickCollideUpCallback -= listener;
        }

        public void BroadcastCollideUp(HandRole hand, GameObject go)
        {
            joystickCollideUpCallback?.Invoke(hand, go);
        }
        #endregion

        #region grab

        public void AddGrabDownListener(JoystickGrabDown listener)
        {
            joystickGrabDown += listener;
        }

        public void RemoveGrabDownListener(JoystickGrabDown listener)
        {
            joystickGrabDown -= listener;
        }

        public void BroadcastGrabDown(HandRole hand, GameObject go, Vector3 jpos, Vector3 jang)
        {
            joystickGrabDown?.Invoke(hand, go, jpos, jang);
        }

        public void AddGrabHoldListener(JoystickGrabHold listener)
        {
            joystickGrabHold += listener;
        }

        public void RemoveGrabHoldListener(JoystickGrabHold listener)
        {
            joystickGrabHold -= listener;
        }

        public void BroadcastGrabHold(HandRole hand, GameObject go, Vector3 wpos, Vector3 wang, Vector3 jpos, Vector3 jang)
        {
            joystickGrabHold?.Invoke(hand, go, wpos, wang, jpos, jang);
        }

        public void AddGrabUpListener(JoystickGrabUp listener)
        {
            joystickGrabUp += listener;
        }

        public void RemoveGrabUpListener(JoystickGrabUp listener)
        {
            joystickGrabUp -= listener;
        }

        public void BroadcastGrabUp(HandRole hand, GameObject go)
        {
            joystickGrabUp?.Invoke(hand, go);
        }

        #endregion

        #endregion

        #region ///statement
        public int GetPhysicCasterCount(HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                return joystickLeft.GetPhysicCasterCount();
            }
            else if (hand == HandRole.RightHand)
            {
                return joystickRight.GetPhysicCasterCount();
            }
            return -1;
        }

        public int GetCanvasCasterCount(HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                return joystickLeft.GetCanvasCasterCount();
            }
            else if (hand == HandRole.RightHand)
            {
                return joystickRight.GetCanvasCasterCount();
            }
            return -1;
        }

        public bool GetTriggerDown(HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                return joystickLeft.GetTriggerDown();
            }
            else if (hand == HandRole.RightHand)
            {
                return joystickRight.GetTriggerDown();
            }
            return false;
        }

        public bool GetTrigger(HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                return joystickLeft.GetTrigger();
            }
            else if (hand == HandRole.RightHand)
            {
                return joystickRight.GetTrigger();
            }
            return false;
        }

        public bool GetTriggerUp(HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                return joystickLeft.GetTriggerUp();
            }
            else if (hand == HandRole.RightHand)
            {
                return joystickRight.GetTriggerUp();
            }
            return false;
        }

        public Ray GetJoystickRay(HandRole hand)
        {
            Ray ray = new Ray();
            if (hand == HandRole.LeftHand)
            {
                ray = joystickLeft.GetJoystickRay();
            }
            else if (hand == HandRole.RightHand)
            {
                ray = joystickRight.GetJoystickRay();
            }
            return ray;
        }

        public bool PointAtTarget(out GameObject go, HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                if (joystickLeft.PointAtTarget(out go))
                {
                    return true;
                }
            }
            else if (hand == HandRole.RightHand)
            {
                if (joystickRight.PointAtTarget(out go))
                {
                    return true;
                }
            }
            go = null;
            return false;
        }

        public bool PointAtCastHit(out RaycastHit rayhit, HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                if (joystickLeft.PointAtCastHit(out rayhit))
                {
                    return true;
                }
            }
            else if (hand == HandRole.RightHand)
            {
                if (joystickRight.PointAtCastHit(out rayhit))
                {
                    return true;
                }
            }
            rayhit = new RaycastHit();
            return false;
        }

        public bool PointAtCastHit(LayerMask layer, out RaycastHit rayhit, HandRole hand)
        {
            if (hand == HandRole.LeftHand)
            {
                if (joystickLeft.PointAtCastHit(layer, out rayhit))
                {
                    return true;
                }
            }
            else if (hand == HandRole.RightHand)
            {
                if (joystickRight.PointAtCastHit(layer, out rayhit))
                {
                    return true;
                }
            }
            rayhit = new RaycastHit();
            return false;
        }
        #endregion
    }
}