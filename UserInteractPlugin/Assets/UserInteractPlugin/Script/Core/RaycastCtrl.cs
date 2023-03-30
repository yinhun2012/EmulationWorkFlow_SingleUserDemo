using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
    public class RaycastCtrl : Singleton<RaycastCtrl>
    {
        private const float RAYCAST_MAX_DISTANCE = 1000f;
        private GameObject[] tagGos;
        private Dictionary<string, GameObject> gosDict = new Dictionary<string, GameObject>();

        /// <summary>
        /// 射线检测
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="layer"></param>
        /// <param name="rayhit"></param>
        /// <returns></returns>
        public bool RayCheck(Vector3 from, Vector3 dir, out RaycastHit rayhit)
        {
#if UNITY_EDITOR
            Debug.DrawRay(from, dir, Color.white);
#endif
            if (Physics.Raycast(from, dir, out rayhit))
            {
#if UNITY_EDITOR
                Debug.DrawLine(from, rayhit.point, Color.red);
#endif
                return true;
            }
            return false;
        }

        public bool RayCheck(Ray ray, out RaycastHit rayhit)
        {
#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
#endif
            if (Physics.Raycast(ray, out rayhit))
            {
#if UNITY_EDITOR
                Debug.DrawLine(ray.origin, rayhit.point, Color.red);
#endif
                return true;
            }
            return false;
        }

        public bool RayCheck(Ray ray, LayerMask layer, out RaycastHit rayhit)
        {
#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
#endif
            if (Physics.Raycast(ray, out rayhit, RAYCAST_MAX_DISTANCE, 1 << layer))
            {
#if UNITY_EDITOR
                Debug.DrawLine(ray.origin, rayhit.point, Color.red);
#endif
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取场景中标注tag物体
        /// </summary>
        /// <param name="goname"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public GameObject GetObject(string goname, string tag)
        {
            if (gosDict.ContainsKey(goname))
            {
                return gosDict[goname];
            }
            if (tagGos == null)
            {
                tagGos = GameObject.FindGameObjectsWithTag(tag);
            }
            for (int i = 0; i < tagGos.Length; i++)
            {
                GameObject go = tagGos[i];
                if (go.name == goname)
                {
                    gosDict.Add(goname, go);
                    return go;
                }
            }
#if UNITY_EDITOR
            Debug.LogErrorFormat("RaycastCtrl GetObject Error goname = {0}", goname);
#endif
            return null;
        }
    }
}