using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UserInteractPlugin
{
    public enum EMouseType
    {
        Left = 0,
        Right = 1,
        Center = 2
    }

    public class UserInteract : MonoSingleton<UserInteract>
    {
        private System.Array mouseTypes;

        private EMouseType doubleMouseType;
        private HandRole doubleHandRole;
        private bool isMouseOrTriggerDoubleDown = false;
        private const float mouseOrTriggerDoubleThreshold = 0.3f;
        private float mouseOrTriggerDownElapse = 0f;
        private float mouseOrTriggerDownRealTime = 0f;

        #region ///delegate

        public delegate void PCMouseDownDelegate(EMouseType mtype);
        public delegate void PCMouseHoldDelegate(EMouseType mtype);
        public delegate void PCMouseUpDelegate(EMouseType mtype);
        public delegate void PCMouseDownOnGameobjectDelegate(EMouseType mtype, GameObject go);
        public delegate void PCMouseHoldOnGameobjectDelegate(EMouseType mtype, GameObject go);
        public delegate void PCMouseUpOnGameobjectDelegate(EMouseType mtype, GameObject go);
        public delegate void PCMouseDoubleDownDelegate(EMouseType mtype);
        public delegate void PCMouseDoubleDownOnGameobjectDelegate(EMouseType mtype, GameObject go);

        private PCMouseDownDelegate pcMouseDownListener;
        private PCMouseHoldDelegate pcMouseHoldListener;
        private PCMouseUpDelegate pcMouseUpListener;
        private PCMouseDownOnGameobjectDelegate pcMouseDownOnGoListener;
        private PCMouseHoldOnGameobjectDelegate pcMouseHoldOnGoListener;
        private PCMouseUpOnGameobjectDelegate pcMouseUpOnGoListener;
        private PCMouseDoubleDownDelegate pcMouseDoubleDownListener;
        private PCMouseDoubleDownOnGameobjectDelegate pcMouseDoubleDownOnGoListener;

        #endregion

        void Start()
        {
            mouseTypes = System.Enum.GetValues(typeof(EMouseType));
        }

        private void Update()
        {
            isMouseOrTriggerDoubleDown = false;
            if (PCIsMouseDown(doubleMouseType) || VRIsTriggerDown(doubleHandRole))
            {
                mouseOrTriggerDownElapse = Time.realtimeSinceStartup - mouseOrTriggerDownRealTime;
                if (mouseOrTriggerDownElapse <= mouseOrTriggerDoubleThreshold)
                {
                    isMouseOrTriggerDoubleDown = true;
                }
                mouseOrTriggerDownRealTime = Time.realtimeSinceStartup;
            }
            //PC交互
            foreach (EMouseType mtype in mouseTypes)
            {
                if (PCIsMouseDown(mtype))
                {
                    PCBroadcastMouseDownListener(mtype);
                    GameObject go;
                    if (PCGetMousePointGameobject(out go))
                    {
                        PCBroadcastMouseDownOnGameobjectListener(mtype, go);
                    }
                }
                if (PCIsMouseHold(mtype))
                {
                    PCBroadcastMouseHoldListener(mtype);
                    GameObject go;
                    if (PCGetMousePointGameobject(out go))
                    {
                        PCBroadcastMouseHoldOnGameobjectListener(mtype, go);
                    }
                }
                if (PCIsMouseUp(mtype))
                {
                    PCBroadcastMouseUpListener(mtype);
                    GameObject go;
                    if (PCGetMousePointGameobject(out go))
                    {
                        PCBroadcastMouseUpOnGameobjectListener(mtype, go);
                    }
                }
            }


        }

        #region ///注册交互
        //mouse down
        public void PCAddMouseDownListener(PCMouseDownDelegate listener)
        {
            pcMouseDownListener += listener;
        }

        public void PCRemoveMouseDownListener(PCMouseDownDelegate listener)
        {
            pcMouseDownListener -= listener;
        }

        public void PCBroadcastMouseDownListener(EMouseType mtype)
        {
            pcMouseDownListener?.Invoke(mtype);
        }
        //mouse hold
        public void PCAddMouseHoldListener(PCMouseHoldDelegate listener)
        {
            pcMouseHoldListener += listener;
        }

        public void PCRemoveMouseHoldListener(PCMouseHoldDelegate listener)
        {
            pcMouseHoldListener -= listener;
        }

        public void PCBroadcastMouseHoldListener(EMouseType mtype)
        {
            pcMouseHoldListener?.Invoke(mtype);
        }
        //mouse up
        public void PCAddMouseUpListener(PCMouseUpDelegate listener)
        {
            pcMouseUpListener += listener;
        }

        public void PCRemoveMouseUpListener(PCMouseUpDelegate listener)
        {
            pcMouseUpListener -= listener;
        }

        public void PCBroadcastMouseUpListener(EMouseType mtype)
        {
            pcMouseUpListener?.Invoke(mtype);
        }
        //mouse down on gameobject
        public void PCAddMouseDownOnGameobjectListener(PCMouseDownOnGameobjectDelegate listener)
        {
            pcMouseDownOnGoListener += listener;
        }

        public void PCRemoveMouseDownOnGameobjectListener(PCMouseDownOnGameobjectDelegate listener)
        {
            pcMouseDownOnGoListener -= listener;
        }

        public void PCBroadcastMouseDownOnGameobjectListener(EMouseType mtype, GameObject go)
        {
            pcMouseDownOnGoListener?.Invoke(mtype, go);
        }
        //mouse hold on gameobject
        public void PCAddMouseHoldOnGameobjectListener(PCMouseHoldOnGameobjectDelegate listener)
        {
            pcMouseHoldOnGoListener += listener;
        }

        public void PCRemoveMouseHoldOnGameobjectListener(PCMouseHoldOnGameobjectDelegate listener)
        {
            pcMouseHoldOnGoListener -= listener;
        }

        public void PCBroadcastMouseHoldOnGameobjectListener(EMouseType mtype, GameObject go)
        {
            pcMouseHoldOnGoListener?.Invoke(mtype, go);
        }
        //mouse up on gameobject
        public void PCAddMouseUpOnGameobjectListener(PCMouseUpOnGameobjectDelegate listener)
        {
            pcMouseUpOnGoListener += listener;
        }

        public void PCRemoveMouseUpOnGameobjectListener(PCMouseUpOnGameobjectDelegate listener)
        {
            pcMouseUpOnGoListener -= listener;
        }

        public void PCBroadcastMouseUpOnGameobjectListener(EMouseType mtype, GameObject go)
        {
            pcMouseUpOnGoListener?.Invoke(mtype, go);
        }
        //mouse double down
        public void PCAddMouseDoubleDownListener(PCMouseDoubleDownDelegate listener)
        {
            pcMouseDoubleDownListener += listener;
        }

        public void PCRemoveMouseDoubleDownListener(PCMouseDoubleDownDelegate listener)
        {
            pcMouseDoubleDownListener -= listener;
        }

        public void PCBroadcastMouseDoubleDownListener(EMouseType mtype)
        {
            pcMouseDoubleDownListener?.Invoke(mtype);
        }
        //mouse double down on gameobject
        public void PCAddMouseDoubleDownOnGameobjectListener(PCMouseDoubleDownOnGameobjectDelegate listener)
        {
            pcMouseDoubleDownOnGoListener += listener;
        }

        public void PCRemoveMouseDoubleDownListener(PCMouseDoubleDownOnGameobjectDelegate listener)
        {
            pcMouseDoubleDownOnGoListener -= listener;
        }

        public void PCBroadcastMouseDoubleDownOnGameobjectListener(EMouseType mtype, GameObject go)
        {
            pcMouseDoubleDownOnGoListener?.Invoke(mtype, go);
        }
        #endregion

        #region ///PC交互
        /// <summary>
        /// 是否点击在uiObject上
        /// </summary>
        /// <returns></returns>
        public bool PCIsPointOverUiObject()
        {
            if (EventSystem.current != null)
            {
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
                if (EventSystem.current.IsPointerOverGameObject())
#elif UNITY_ANDROID || UNITY_IOS
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 按下鼠标 
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseDown(EMouseType mtype)
        {
            if (Input.GetMouseButtonDown((int)mtype))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 按住鼠标
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseHold(EMouseType mtype)
        {
            if (Input.GetMouseButton((int)mtype))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 抬起鼠标
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseUp(EMouseType mtype)
        {
            if (Input.GetMouseButtonUp((int)mtype))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在物体上鼠标按下
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseDownOnGameobject(GameObject go, EMouseType mtype)
        {
            if (PCIsMouseDown(mtype))
            {
                GameObject hitgo;
                if (PCGetMousePointGameobject(out hitgo))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 在物体上鼠标按住
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseHoldOnGameobject(GameObject go, EMouseType mtype)
        {
            if (PCIsMouseHold(mtype))
            {
                GameObject hitgo;
                if (PCGetMousePointGameobject(out hitgo))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 在物体上鼠标抬起
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseUpOnGameobject(GameObject go, EMouseType mtype)
        {
            if (PCIsMouseUp(mtype))
            {
                GameObject hitgo;
                if (PCGetMousePointGameobject(out hitgo))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 按下鼠标两次
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseDoubleDown(EMouseType mtype)
        {
            doubleMouseType = mtype;
            return isMouseOrTriggerDoubleDown;
        }
        /// <summary>
        /// 在物体上按下鼠标两次
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool PCIsMouseDoubleDownOnGameobject(GameObject go, EMouseType mtype)
        {
            doubleMouseType = mtype;
            if (isMouseOrTriggerDoubleDown)
            {
                GameObject hitgo;
                if (PCGetMousePointGameobject(out hitgo))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region ///VR交互
        /// <summary>
        /// 是否点击在uiObject上
        /// </summary>
        /// <returns></returns>
        public bool VRIsPointOverUiObject()
        {
            if (ViveControl.Instance != null)
            {
                if (ViveControl.Instance.GetCanvasCasterCount(HandRole.LeftHand) > 0)
                {
                    return true;
                }
                if (ViveControl.Instance.GetCanvasCasterCount(HandRole.RightHand) > 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// VR按下扳机 
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsTriggerDown(HandRole hand)
        {
            if (ViveControl.Instance.GetTriggerDown(hand))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// VR按住扳机 
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsTriggerHold(HandRole hand)
        {
            if (ViveControl.Instance.GetTrigger(hand))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// VR抬起扳机
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public bool VRIsTriggerUp(HandRole hand)
        {
            if (ViveControl.Instance.GetTriggerUp(hand))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在物体上按下扳机
        /// </summary>
        /// <param name="go"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsTriggerDownOnGameobject(GameObject go, HandRole hand)
        {
            if (VRIsTriggerDown(hand))
            {
                GameObject hitgo;
                if (VRGetTriggerPointGameobject(out hitgo, hand))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 在物体上按住扳机
        /// </summary>
        /// <param name="go"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsTriggerHoldOnGameobject(GameObject go, HandRole hand)
        {
            if (VRIsTriggerHold(hand))
            {
                GameObject hitgo;
                if (VRGetTriggerPointGameobject(out hitgo, hand))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 在物体上抬起扳机
        /// </summary>
        /// <param name="go"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsTriggerUpOnGameobject(GameObject go, HandRole hand)
        {
            if (VRIsTriggerUp(hand))
            {
                GameObject hitgo;
                if (VRGetTriggerPointGameobject(out hitgo, hand))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 按下扳机两次
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsMouseDoubleDown(HandRole hand)
        {
            doubleHandRole = hand;
            return isMouseOrTriggerDoubleDown;
        }
        /// <summary>
        /// 在物体上按下扳机两次
        /// </summary>
        /// <param name="go"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRIsTriggerDoubleDownOnGameobject(GameObject go, HandRole hand)
        {
            doubleHandRole = hand;
            if (isMouseOrTriggerDoubleDown)
            {
                GameObject hitgo;
                if (VRGetTriggerPointGameobject(out hitgo, hand))
                {
                    if (hitgo == go)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region 
        /// <summary>
        /// 获取鼠标指向射线
        /// </summary>
        /// <param name="mousepos"></param>
        /// <returns></returns>
        public Ray PCGetMousePointRay(Vector3 mousepos)
        {
            Ray ray = new Ray();
            ray = CameraCtrl.Instance.GetScreenPointToRay(mousepos);
            return ray;
        }
        /// <summary>
        /// 获取鼠标指向物体
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool PCGetMousePointGameobject(out GameObject go)
        {
            if (CameraCtrl.Instance.PointAtTarget(out go))
            {
                return true;
            }
            go = null;
            return false;
        }
        /// <summary>
        /// 获取鼠标射线raycasthit
        /// </summary>
        /// <param name="rayhit"></param>
        /// <returns></returns>
        public bool PCGetMousePointRaycastHit(out RaycastHit rayhit)
        {
            if (CameraCtrl.Instance.PointAtCastHit(out rayhit))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取鼠标射线raycasthit
        /// </summary>
        /// <param name="rayhit"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool PCGetMousePointRaycastHit(LayerMask layer, out RaycastHit rayhit)
        {
            if (CameraCtrl.Instance.PointAtCastHit(layer, out rayhit))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取手柄指向射线
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public Ray VRGetJoystickPointRay(HandRole hand)
        {
            Ray ray = new Ray();
            ray = ViveControl.Instance.GetJoystickRay(hand);
            return ray;
        }
        /// <summary>
        /// 获取扳机指向物体
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool VRGetTriggerPointGameobject(out GameObject go, HandRole hand)
        {
            if (ViveControl.Instance.PointAtTarget(out go, hand))
            {
                return true;
            }
            go = null;
            return false;
        }
        /// <summary>
        /// 获取扳机指向raycastHit
        /// </summary>
        /// <param name="rayhit"></param>
        /// <returns></returns>
        public bool VRGetTriggerPointRaycastHit(out RaycastHit rayhit, HandRole hand)
        {
            if (ViveControl.Instance.PointAtCastHit(out rayhit, hand))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取扳机指向raycastHit
        /// </summary>
        /// <param name="rayhit"></param>
        /// <param name="layer"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public bool VRGetTriggerPointRaycastHit(LayerMask layer, out RaycastHit rayhit, HandRole hand)
        {
            if (ViveControl.Instance.PointAtCastHit(layer, out rayhit, hand))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}