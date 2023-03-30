//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Pointer3D
{
    public class GraphicRaycastMethod : BaseRaycastMethod
    {
        [SerializeField]
        private Canvas m_Canvas;
        [SerializeField]
        private bool m_IgnoreReversedGraphics = true;

        public Canvas canvas { get { return m_Canvas; } set { m_Canvas = value; } }
        public bool ignoreReversedGraphics { get { return m_IgnoreReversedGraphics; } set { m_IgnoreReversedGraphics = value; } }
#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (m_Canvas == null)
            {
                m_Canvas = FindObjectOfType<Canvas>();
            }
        }
#endif
        public override int Raycast(Ray ray, float distance, List<RaycastResult> raycastResults)
        {
            return CanvasRaycastMethod.Raycast(canvas, ignoreReversedGraphics, ray, distance, raycaster, raycastResults);
        }
    }
}