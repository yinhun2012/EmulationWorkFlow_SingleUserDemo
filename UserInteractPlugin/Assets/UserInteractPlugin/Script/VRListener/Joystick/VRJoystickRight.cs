using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    public class VRJoystickRight : AVRJoystickBase
    {
        protected override void OnDotHold(Vector3 wpos)
        {
            base.OnDotHold(wpos);
            ViveControl.Instance.BroadcastDotHoldListener(handRole, wpos);
        }

        protected override void OnGripDown()
        {
            base.OnGripDown();
            ViveControl.Instance.BroadcastGripDown(handRole);
        }

        protected override void OnGripHold()
        {
            base.OnGripHold();
            ViveControl.Instance.BroadcastGripHold(handRole);
        }

        protected override void OnGripUp()
        {
            base.OnGripUp();
            ViveControl.Instance.BroadcastGripUp(handRole);
        }

        protected override void OnMenuDown()
        {
            base.OnMenuDown();
            ViveControl.Instance.BroadcastMenuDown(handRole);
        }

        protected override void OnMenuHold()
        {
            base.OnMenuHold();
            ViveControl.Instance.BroadcastMenuHold(handRole);
        }

        protected override void OnMenuUp()
        {
            base.OnMenuUp();
            ViveControl.Instance.BroadcastMenuUp(handRole);
        }

        protected override void OnPadDown(Vector2 axis)
        {
            base.OnPadDown(axis);
            ViveControl.Instance.BroadcastPadDown(handRole, axis);
        }

        protected override void OnPadQuadrantDown(EPadQuadrantType pqtype)
        {
            base.OnPadQuadrantDown(pqtype);
            ViveControl.Instance.BroadcastPadQuadrantDown(handRole, pqtype);
        }

        protected override void OnPadUp(Vector2 axis)
        {
            base.OnPadUp(axis);
            ViveControl.Instance.BroadcastPadUp(handRole, axis);
        }

        protected override void OnPadTouch(Vector2 axis)
        {
            base.OnPadTouch(axis);
            ViveControl.Instance.BroadcastPadTouch(handRole, axis);
        }

        protected override void OnPadTouchDelta(Vector2 delta)
        {
            base.OnPadTouchDelta(delta);
            ViveControl.Instance.BroadcastPadTouchDelta(handRole, delta);
        }

        protected override void OnPadHold(Vector2 axis)
        {
            base.OnPadHold(axis);
            ViveControl.Instance.BroadcastPadHold(handRole, axis);
        }

        protected override void OnPadHoldDelta(Vector2 delta)
        {
            base.OnPadHoldDelta(delta);
            ViveControl.Instance.BroadcastPadHoldDelta(handRole, delta);
        }

        protected override void OnTriggerDown(GameObject go)
        {
            base.OnTriggerDown(go);
            ViveControl.Instance.BroadcastTriggerDown(handRole, go);
        }

        protected override void OnTriggerHold(GameObject go)
        {
            base.OnTriggerHold(go);
            ViveControl.Instance.BroadcastTriggerHold(handRole, go);
        }

        protected override void OnTriggerUp(GameObject go)
        {
            base.OnTriggerUp(go);
            ViveControl.Instance.BroadcastTriggerUp(handRole, go);
        }

        protected override void OnCollideDown(GameObject go)
        {
            base.OnCollideDown(go);
            ViveControl.Instance.BroadcastCollideDown(handRole, go);
        }

        protected override void OnCollideHold(GameObject go)
        {
            base.OnCollideHold(go);
            ViveControl.Instance.BroadcastCollideHold(handRole, go);
        }

        protected override void OnCollideUp(GameObject go)
        {
            base.OnCollideUp(go);
            ViveControl.Instance.BroadcastCollideUp(handRole, go);
        }

        protected override void OnGrabDown(GameObject go, Vector3 jpos, Vector3 jang)
        {
            base.OnGrabDown(go, jpos, jang);
            ViveControl.Instance.BroadcastGrabDown(handRole, go, jpos, jang);
        }

        protected override void OnGrabHold(GameObject go, Vector3 wpos, Vector3 wang, Vector3 jpos, Vector3 jang)
        {
            base.OnGrabHold(go, wpos, wang, jpos, jang);
            ViveControl.Instance.BroadcastGrabHold(handRole, go, wpos, wang, jpos, jang);
        }

        protected override void OnGrabUp(GameObject go)
        {
            base.OnGrabUp(go);
            ViveControl.Instance.BroadcastGrabUp(handRole, go);
        }
    }
}