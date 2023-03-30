using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    public class JoyModelHand : AJoyModelBase
    {
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void OnTriggerDown(GameObject go)
        {
            base.OnTriggerDown(go);
            animator.SetFloat("play", 1f);
        }

        protected override void OnTriggerUp(GameObject go)
        {
            base.OnTriggerUp(go);
            animator.SetFloat("play", 0f);
        }
    }
}