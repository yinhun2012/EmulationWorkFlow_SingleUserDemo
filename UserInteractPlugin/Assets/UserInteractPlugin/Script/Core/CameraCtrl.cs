using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UserInteractPlugin
{
    public class CameraCtrl : MonoSingleton<CameraCtrl>
    {
        public Camera mainCamera;

        private Collider collid;

        protected override void Awake()
        {
            base.Awake();

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            collid = GetComponent<Collider>();
        }

        void Start()
        {

        }

        /// <summary>
        /// 获取screen point to ray
        /// </summary>
        /// <param name="mpos"></param>
        /// <returns></returns>
        public Ray GetScreenPointToRay(Vector3 mpos)
        {
            Ray ray = mainCamera.ScreenPointToRay(mpos);
            return ray;
        }
        /// <summary>
        /// 点击目标
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool PointAtTarget(out GameObject go)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (RaycastCtrl.Instance.RayCheck(ray, out rayhit))
            {
                go = rayhit.transform.gameObject;
                return true;
            }
            go = null;
            return false;
        }
        /// <summary>
        /// 观察目标
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool LookAtTarget(out GameObject go)
        {
            Vector3 from = transform.position;
            Vector3 dir = transform.forward;
            RaycastHit rayhit;
            if (RaycastCtrl.Instance.RayCheck(from, dir, out rayhit))
            {
                go = rayhit.transform.gameObject;
                return true;
            }
            go = null;
            return false;
        }
        /// <summary>
        /// 点击casthit
        /// </summary>
        /// <returns></returns>
        public bool PointAtCastHit(out RaycastHit rayhit)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (RaycastCtrl.Instance.RayCheck(ray, out rayhit))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 点击casthit
        /// </summary>
        /// <param name="rayhit"></param>
        /// <returns></returns>
        public bool PointAtCastHit(LayerMask layer, out RaycastHit rayhit)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (RaycastCtrl.Instance.RayCheck(ray, layer, out rayhit))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否在视域范围
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool IsInsideCamera(GameObject go)
        {
            return IsInsideCamera(go.transform);
        }
        /// <summary>
        /// 是否在视域范围
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool IsInsideCamera(Transform p)
        {
            float fov = mainCamera.fieldOfView;
            float hzfov = fov * mainCamera.aspect;
            float hhzfov = hzfov / 2f;
            Vector3 fwd = mainCamera.transform.forward;
            Vector3 p2c = p.position - mainCamera.transform.position;
            float ang = Vector3.Angle(p2c, fwd);
            if (ang <= hhzfov)
            {
                return true;
            }
            return false;
        }
    }
}