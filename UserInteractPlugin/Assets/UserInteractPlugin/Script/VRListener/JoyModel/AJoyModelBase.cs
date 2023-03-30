using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    public enum EJoystickModelType
    {
        Joystick = 0,
        Hand = 1,
    }

    public class AJoyModelBase : MonoBehaviour
    {
        public EJoystickModelType modelType;

        protected AVRJoystickBase joystickBase;

        protected virtual void Awake()
        {
            joystickBase = GetComponentInParent<AVRJoystickBase>();
        }

        protected virtual void OnEnable()
        {
            ViveControl.Instance.AddTriggerDownListener(TriggleDownCallback);
            ViveControl.Instance.AddTriggerHoldListener(TriggleHoldCallback);
            ViveControl.Instance.AddTriggerUpListener(TriggleUpCallback);
        }

        private void TriggleDownCallback(HandRole hand, GameObject go)
        {
            if (hand.Equals(joystickBase.handRole))
            {
                OnTriggerDown(go);
            }
        }

        protected virtual void OnTriggerDown(GameObject go)
        {

        }

        private void TriggleHoldCallback(HandRole hand, GameObject go)
        {
            if (hand.Equals(joystickBase.handRole))
            {
                OnTriggerHold(go);
            }
        }

        protected virtual void OnTriggerHold(GameObject go)
        {

        }

        private void TriggleUpCallback(HandRole hand, GameObject go)
        {
            if (hand.Equals(joystickBase.handRole))
            {
                OnTriggerUp(go);
            }
        }

        protected virtual void OnTriggerUp(GameObject go)
        {

        }

        protected virtual void OnDisable()
        {
            ViveControl.Instance?.RemoveTriggerDownListener(TriggleDownCallback);
            ViveControl.Instance?.RemoveTriggerHoldListener(TriggleHoldCallback);
            ViveControl.Instance?.RemoveTriggerUpListener(TriggleUpCallback);
        }
    }
}