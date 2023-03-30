using HTC.UnityPlugin.Pointer3D;
using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInteractPlugin
{
    public enum EPadQuadrantType
    {
        Center,
        Top,
        Left,
        Right,
        Bottom,
    }

    public class AVRJoystickBase : MonoBehaviour
    {
        public Transform uiRoot;
        public HandRole handRole;
        public Material lineMat;
        public PhysicsRaycastMethod physicCaster;
        public CanvasRaycastMethod canvasCaster;

        public GameObject joyModel;
        public GameObject handModel;

        public EJoystickModelType setJoyModelType = EJoystickModelType.Joystick;

        protected EJoystickModelType crtJoyModelType = EJoystickModelType.Joystick;

        //辅助线
        [Range(0.001f, 0.1f)]
        private float lineWidth = 0.001f;
        [Range(100f, 1000f)]
        public float lineDist = 1000f;

        //辅助球
        private GameObject dotSphere;
        private Material dotMat;

        protected GameObject lineModel;
        protected Animator animator;
        protected LineRenderer lineRender;

        protected bool isActive = true;
        public bool IsActive { get { return isActive; } }

        protected WebVRController controller;

        //圆盘pad touch滑动
        protected Vector2 crtPadCoord;
        protected Vector2 lastPadCoord;
        protected Vector2 deltaPadCoord;

        protected GameObject hitObject;

        protected static AVRJoystickBase instance = null;
        public static AVRJoystickBase Instance { get { return instance; } }

        //grab功能
        //抓取球
        public GameObject grabSphere;

        protected GameObject grabObject;

        protected bool isGrabing = false;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            lineModel = new GameObject("_line");
            lineModel.transform.SetParent(transform);
            lineModel.transform.localPosition = Vector3.zero;
            lineModel.transform.localEulerAngles = Vector3.zero;
            lineModel.transform.localScale = Vector3.one;
            lineRender = lineModel.AddComponent<LineRenderer>();
            lineRender.startWidth = lineWidth;
            lineRender.endWidth = lineWidth;
            lineRender.sharedMaterial = lineMat;
            lineRender.positionCount = 2;

            dotMat = new Material(Shader.Find("ViveVR/ViveDotShader"));
            dotMat.SetColor("_MainColor", new Color32(255, 50, 0, 128));
            dotMat.SetFloat("_MainOffset", 0.3f);
            dotMat.SetFloat("_MainStep", 0.8f);
            dotSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dotSphere.GetComponent<Renderer>().sharedMaterial = dotMat;
            Collider.Destroy(dotSphere.GetComponent<Collider>());
            dotSphere.transform.SetParent(transform);
            dotSphere.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            dotSphere.SetActive(false);
        }

        //默认线段
        private void FixedRayLine()
        {
            lineRender.sharedMaterial.SetColor("_MainColor", new Color32(255, 150, 0, 255));
            lineRender.sharedMaterial.SetInt("_Hit", 0);
            Vector3 point = transform.position + transform.forward * lineDist;
            lineRender.SetPosition(0, transform.position);
            lineRender.SetPosition(1, point);
        }
        //ui击中线段
        private void CanvasRayLine(Vector3 hitpoint)
        {
            lineRender.sharedMaterial.SetColor("_MainColor", new Color32(0, 255, 0, 255));
            lineRender.sharedMaterial.SetInt("_Hit", 1);
            Vector3 point = transform.position + transform.forward * lineDist;
            lineRender.SetPosition(0, transform.position);
            lineRender.SetPosition(1, hitpoint);
        }
        //world击中线段
        private void WorldRayLine(Vector3 hitpoint)
        {
            lineRender.sharedMaterial.SetColor("_MainColor", new Color32(255, 255, 0, 255));
            lineRender.sharedMaterial.SetInt("_Hit", 1);
            Vector3 point = transform.position + transform.forward * lineDist;
            lineRender.SetPosition(0, transform.position);
            lineRender.SetPosition(1, hitpoint);
        }

        //改变joystick model类型
        public virtual void ChangeJoyModelType(EJoystickModelType joytype)
        {
            if (crtJoyModelType != joytype)
            {
                switch (joytype)
                {
                    case EJoystickModelType.Joystick:
                        {
                            joyModel.SetActive(true);
                            handModel.SetActive(false);
                            lineModel.SetActive(true);
                        }
                        break;
                    case EJoystickModelType.Hand:
                        {
                            joyModel.SetActive(false);
                            handModel.SetActive(true);
                            lineModel.SetActive(false);
                        }
                        break;
                }
                crtJoyModelType = joytype;
            }
        }

        protected virtual void Start()
        {
            controller = gameObject.GetComponent<WebVRController>();

            ChangeJoyModelType(setJoyModelType);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (isActive)
            {
                grabObject = other.gameObject;
                OnCollideDown(other.gameObject);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (isActive)
            {
                OnCollideHold(other.gameObject);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (isActive)
            {
                OnCollideUp(other.gameObject);
            }
        }

        void Update()
        {
            if (!isActive)
            {
                return;
            }
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool ishit = Physics.Raycast(ray, out hit);
            if (GetPhysicCasterCount() > 0)
            {
                RaycastHit phit = GetPhysicCasterRaycastHit();
                CanvasRayLine(phit.point);
                if (!dotSphere.activeSelf)
                {
                    dotSphere.SetActive(true);
                }
                dotSphere.transform.position = phit.point;
            }
            else if (GetCanvasCasterCount() > 0)
            {
                RaycastResult presult = GetCanvasCasterRaycastResult();
                CanvasRayLine(presult.worldPosition);
                OnDotHold(presult.worldPosition);
            }
            else
            {
                FixedRayLine();
                if (GetCanvasCasterCount() <= 0)
                {
                    OnDotHold(new Vector3(-10000, -10000, -10000));
                }
                if (GetPhysicCasterCount() <= 0)
                {
                    if (dotSphere.activeSelf)
                    {
                        dotSphere.SetActive(false);
                    }
                }
            }
            //world击中射线
            if (ishit)
            {
                if (!hit.transform.CompareTag(ViveControl.HTC_VIVE_GROUND_TAG))
                {
                    WorldRayLine(hit.point);
                    if (!dotSphere.activeSelf)
                    {
                        dotSphere.SetActive(true);
                    }
                    dotSphere.transform.position = hit.point;
                }
            }
            //steamVR
            //左右辅助按钮
            if (ViveInput.GetPressDown(handRole, ControllerButton.Grip))
            {
                OnGripDown();
            }
            if (ViveInput.GetPress(handRole, ControllerButton.Grip))
            {
                OnGripHold();
            }
            if (ViveInput.GetPressUp(handRole, ControllerButton.Grip))
            {
                OnGripUp();
            }
            //menu按钮操作
            if (ViveInput.GetPressDown(handRole, ControllerButton.Menu))
            {
                OnMenuDown();
            }
            if (ViveInput.GetPress(handRole, ControllerButton.Menu))
            {
                OnMenuHold();
            }
            if (ViveInput.GetPressUp(handRole, ControllerButton.Menu))
            {
                OnMenuUp();
            }
            //pad操作
            //pad中心（0,0）左-1 下-1 右1 上1
            //计算deltaangles
            crtPadCoord = ViveInput.GetPadTouchAxis(handRole);
            if (ViveInput.GetPressDown(handRole, ControllerButton.Pad))
            {
                lastPadCoord = ViveInput.GetPadTouchAxis(handRole);
                OnPadDown(crtPadCoord);
                EPadQuadrantType quadtype = EPadQuadrantType.Top;
                //计算coord所处象限
                if (crtPadCoord.x > -0.5f
                   && crtPadCoord.x < 0.5f
                   && crtPadCoord.y > -0.5f
                   && crtPadCoord.y < 0.5f)
                {
                    quadtype = EPadQuadrantType.Center;
                }
                else if (crtPadCoord.x < -0.5f
                    && crtPadCoord.y > -0.5f
                    && crtPadCoord.y < 0.5f)
                {
                    quadtype = EPadQuadrantType.Left;
                }
                else if (crtPadCoord.x > 0.5f
                   && crtPadCoord.y > -0.5f
                   && crtPadCoord.y < 0.5f)
                {
                    quadtype = EPadQuadrantType.Right;
                }
                else if (crtPadCoord.y > 0.5f
                  && crtPadCoord.x > -0.5f
                  && crtPadCoord.x < 0.5f)
                {
                    quadtype = EPadQuadrantType.Top;
                }
                else if (crtPadCoord.y < -0.5f
                 && crtPadCoord.x > -0.5f
                 && crtPadCoord.x < 0.5f)
                {
                    quadtype = EPadQuadrantType.Bottom;
                }
                OnPadQuadrantDown(quadtype);
            }
            if (ViveInput.GetPressUp(handRole, ControllerButton.Pad))
            {
                OnPadUp(crtPadCoord);
            }
            if (ViveInput.GetPadTouchAxis(handRole) != Vector2.zero)
            {
                if (lastPadCoord != crtPadCoord)
                {
                    deltaPadCoord = lastPadCoord - crtPadCoord;
                    lastPadCoord = crtPadCoord;
                }
                if (ViveInput.GetPress(handRole, ControllerButton.Pad))
                {
                    OnPadHold(crtPadCoord);
                    OnPadHoldDelta(deltaPadCoord);
                }
                else
                {
                    OnPadTouch(crtPadCoord);
                    OnPadTouchDelta(deltaPadCoord);
                }
            }
            //trigger操作
            if (ViveInput.GetPressDown(handRole, ControllerButton.Trigger))
            {
                //trigger
                if (ishit)
                {
                    hitObject = hit.transform.gameObject;
                }
                else
                {
                    hitObject = null;
                }
                OnTriggerDown(hitObject);
                //grab
                if (grabObject != null)
                {
                    isGrabing = true;
                    grabSphere.transform.position = grabObject.transform.position;
                    grabSphere.transform.eulerAngles = grabObject.transform.eulerAngles;
                    OnGrabDown(grabObject, transform.position, transform.eulerAngles);
                }
            }
            if (ViveInput.GetPress(handRole, ControllerButton.Trigger))
            {
                OnTriggerHold(hitObject);
                //grab
                if (isGrabing)
                {
                    OnGrabHold(grabObject, grabSphere.transform.position, grabSphere.transform.eulerAngles, transform.position, transform.eulerAngles);
                }
            }
            if (ViveInput.GetPressUp(handRole, ControllerButton.Trigger))
            {
                OnTriggerUp(hitObject);
                //grab
                if (isGrabing)
                {
                    OnGrabUp(grabObject);
                    isGrabing = false;
                }
            }
            //mozla web vr
            /*if (controller.GetButtonDown("Trigger") || controller.GetButtonDown("Grip"))
            {
                OnGripDown();
                if (ishit)
                {
                    OnObjectDown(hit.transform.gameObject);
                }
            }
            if (controller.GetButton("Trigger") || controller.GetButton("Grip"))
            {
                OnGripHold();
            }
            if (controller.GetButtonUp("Trigger") || controller.GetButtonUp("Grip"))
            {
                OnGripUp();
            }*/
        }

        #region statement
        public virtual RaycastHit GetPhysicCasterRaycastHit()
        {
            return physicCaster.HitRaycasts[0];
        }

        public virtual int GetPhysicCasterCount()
        {
            return physicCaster.HitCount;
        }

        public virtual RaycastResult GetCanvasCasterRaycastResult()
        {
            return canvasCaster.HitResults[0];
        }

        public virtual int GetCanvasCasterCount()
        {
            return canvasCaster.HitCount;
        }

        public virtual bool GetTriggerDown()
        {
            if (ViveInput.GetPressDown(handRole, ControllerButton.Trigger))
            {
                return true;
            }
            return false;
        }

        public virtual bool GetTrigger()
        {
            if (ViveInput.GetPress(handRole, ControllerButton.Trigger))
            {
                return true;
            }
            return false;
        }

        public virtual bool GetTriggerUp()
        {
            if (ViveInput.GetPressUp(handRole, ControllerButton.Trigger))
            {
                return true;
            }
            return false;
        }

        public virtual Ray GetJoystickRay()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            return ray;
        }

        public virtual bool PointAtTarget(out GameObject go)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit rayhit;
            if (RaycastCtrl.Instance.RayCheck(ray, out rayhit))
            {
                go = rayhit.transform.gameObject;
                return true;
            }
            go = null;
            return false;
        }

        public virtual bool PointAtCastHit(out RaycastHit rayhit)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (RaycastCtrl.Instance.RayCheck(ray, out rayhit))
            {
                return true;
            }
            return false;
        }

        public virtual bool PointAtCastHit(LayerMask layer, out RaycastHit rayhit)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (RaycastCtrl.Instance.RayCheck(ray, layer, out rayhit))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region  listener
        /// <summary>
        /// ui圆点坐标更新
        /// </summary>
        /// <param name="wpos"></param>
        protected virtual void OnDotHold(Vector3 wpos)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnDotHold", handRole));
        }
        /// <summary>
        /// 手柄侧面左右grip按钮按下
        /// </summary>
        protected virtual void OnGripDown()
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnGripDown", handRole));
        }
        /// <summary>
        /// 手柄侧面左右grip按钮按住
        /// </summary>
        protected virtual void OnGripHold()
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnGripHold", handRole));
        }
        /// <summary>
        /// 手柄侧面左右grip按钮抬起
        /// </summary>
        protected virtual void OnGripUp()
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnGripUp", handRole));
        }
        /// <summary>
        /// 手柄正面上部menu小按钮按下
        /// </summary>
        protected virtual void OnMenuDown()
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnMenuDown", handRole));
        }
        /// <summary>
        /// 手柄正面上部menu小按钮按住
        /// </summary>
        protected virtual void OnMenuHold()
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnMenuHold", handRole));
        }
        /// <summary>
        /// 手柄正面上部menu小按钮抬起
        /// </summary>
        protected virtual void OnMenuUp()
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} OnMenuUp", handRole));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad按下
        /// </summary>
        /// <param name="axis"></param>
        protected virtual void OnPadDown(Vector2 axis)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} point:{1} OnPadDown", handRole, point));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad象限按下
        /// </summary>
        /// <param name="pqtype"></param>
        protected virtual void OnPadQuadrantDown(EPadQuadrantType pqtype)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} point:{1} OnPadQuadrantDown", handRole, point));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad抬起
        /// </summary>
        /// <param name="axis"></param>
        protected virtual void OnPadUp(Vector2 axis)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} point:{1} OnPadUp", handRole, point));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad触摸
        /// </summary>
        /// <param name="axis"></param>
        protected virtual void OnPadTouch(Vector2 axis)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} point:{1} OnMenuTouch", handRole, point));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad触摸DeltaAngle
        /// </summary>
        /// <param name="delta"></param>
        protected virtual void OnPadTouchDelta(Vector2 delta)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} point:{1} OnPadTouchDelta", handRole, point));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad按住
        /// </summary>
        /// <param name="axis"></param>
        protected virtual void OnPadHold(Vector2 axis)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} point:{1} OnPadHold", handRole, point));
        }
        /// <summary>
        /// 手柄正面中间圆盘pad按住DeltaAngle
        /// </summary>
        /// <param name="delta"></param>
        protected virtual void OnPadHoldDelta(Vector2 delta)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} delta:{1} OnPadHoldDelta", handRole, delta));
        }
        /// <summary>
        /// 手柄底部trigger按钮按下
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnTriggerDown(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnTriggerDown", handRole, go.name));
        }
        /// <summary>
        /// 手柄底部trigger按钮按住
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnTriggerHold(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnTriggerHold", handRole, go.name));
        }
        /// <summary>
        /// 手柄底部trigger按钮抬起
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnTriggerUp(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnTriggerUp", handRole, go.name));
        }
        /// <summary>
        /// 手柄collider触碰按下
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnCollideDown(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnTouchDown", handRole, go.name));
        }
        /// <summary>
        /// 手柄collider触碰按住
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnCollideHold(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnTouchHold", handRole, go.name));
        }
        /// <summary>
        /// 手柄collider触碰抬起
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnCollideUp(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnTouchUp", handRole, go.name));
        }
        /// <summary>
        /// 触碰按下trigger抓取
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnGrabDown(GameObject go, Vector3 jpos, Vector3 jang)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnGrabDown", handRole, go.name));
        }
        /// <summary>
        /// 触碰按下trigger抓取运动
        /// </summary>
        /// <param name="go"></param>
        /// <param name="wpos"></param>
        /// <param name="wang"></param>
        protected virtual void OnGrabHold(GameObject go, Vector3 wpos, Vector3 wang, Vector3 jpos, Vector3 jang)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnGrabHold", handRole, go.name));
        }
        /// <summary>
        /// 触碰抬起trigger放掉
        /// </summary>
        /// <param name="go"></param>
        protected virtual void OnGrabUp(GameObject go)
        {
            //LogModule.GetInstance().NormLog(string.Format("Hand:{0} go:{1} OnGrabUp", handRole, go.name));
        }
        #endregion
    }
}