#region Copyright GPLv3
//
//  This file is part of ILNumerics.Net. 
//
//  ILNumerics.Net supports numeric application development for .NET 
//  Copyright (C) 2007, H. Kutschbach, http://ilnumerics.net 
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//  Non-free licenses are also available. Contact info@ilnumerics.net 
//  for details.
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace ILNumerics.Drawing {
    /// <summary>
    /// Clipping data - this class is for internal use only!
    /// </summary>
    public class ILClippingData {

        #region eventing 
        /// <summary>
        /// fires if the data range have changed
        /// </summary>
        public event ILClippingDataChangedEvent Changed; 
        protected void OnChange() {
            if (m_eventingActive && Changed != null) {
                Changed ( this, new ClippingChangedEventArgs(this)); 
                m_isDirty = false; 
            }
        }
        #endregion

        #region attributes 
        private bool m_eventingActive = true;
        internal float m_xMin = float.MaxValue; 
        internal float m_yMin = float.MaxValue; 
        internal float m_zMin = float.MaxValue; 
        internal float m_xMax = float.MinValue; 
        internal float m_yMax = float.MinValue; 
        internal float m_zMax = float.MinValue; 
        internal bool m_isDirty; 
        #endregion

        #region properties 
        /// <summary>
        /// minimum value for x axis
        /// </summary>
        public float XMin {
            get {
                return m_xMin; 
            }
            set {
                if (m_xMin != value) {
                    m_xMin = value;
                    OnChange();
                }
            }
        }
        /// <summary>
        /// minimum value for y axis
        /// </summary>
        public float YMin {
            get {
                return m_yMin; 
            }
            set {
                if (m_yMin != value) {
                    m_yMin = value;
                    OnChange();
                }
            }
        }
        /// <summary>
        /// minimum value for z axis
        /// </summary>
        public float ZMin {
            get {
                return m_zMin; 
            }
            set {
                if (m_zMin != value) {
                    m_zMin = value;
                    OnChange(); 
                }
            }
        }
        /// <summary>
        /// maximum value for x axis
        /// </summary>
        public float XMax {
            get {
                return m_xMax; 
            }
            set {
                if (m_xMax != value) {
                    m_xMax = value;
                    OnChange(); 
                }
            }
        }
        /// <summary>
        /// maximum value for y axis
        /// </summary>
        public float YMax {
            get {
                return m_yMax; 
            }
            set {
                if (m_yMax != value) {
                    m_yMax = value;
                    OnChange(); 
                }
            }
        }
        /// <summary>
        /// maximum value for z axis
        /// </summary>
        public float ZMax {
            get {
                return m_zMax; 
            }
            set {
                if (m_zMax != value) {
                    m_zMax = value;
                    OnChange(); 
                }
            }
        }
        /// <summary>
        /// minimum (coordinate)
        /// </summary>
        public ILPoint3Df Min {
            get {
                return new ILPoint3Df(XMin,YMin,ZMin);
            }
        }
        /// <summary>
        /// maximum (coordinate)
        /// </summary>
        public ILPoint3Df Max {
            get {
                return new ILPoint3Df(XMax,YMax,ZMax); 
            }
        }
        /// <summary>
        /// get center of this clipping range
        /// </summary>
        public ILPoint3Df CenterF {
            get { 
                ILPoint3Df ret; 
                ret.X = (XMax + XMin) / 2.0f; 
                ret.Y = (YMax + YMin) / 2.0f; 
                ret.Z = (ZMax + ZMin) / 2.0f; 
                return ret; 
            }
        }
        /// <summary>
        /// get width (x-direction) of this clipping range
        /// </summary>
        public float WidthF {
            get {
                return (XMax - XMin);
            }
        }
        /// <summary>
        /// get height (y-direction) of this clipping range
        /// </summary>
        public float HeightF {
            get {
                return (YMax - YMin);
            }
        }
        /// <summary>
        /// get depth (z-direction) of this clipping range
        /// </summary>
        public float DepthF {
            get {
                return (ZMax - ZMin);
            }
        }
        /// <summary>
        /// marks the limits as altered, without having fired a changed event yet
        /// </summary>
        public bool IsDirty {
            get {
                return m_isDirty; 
            }
        }
        #endregion

        #region public interface
        /// <summary>
        /// suspend the firing of events until EventingResume has been called
        /// </summary>
        public void EventingSuspend() {
            m_eventingActive = false;
            m_isDirty = false; 
        }
        /// <summary>
        /// Resume previously suspended eventing. Start sending events again.
        /// </summary>
        public void EventingResume() {
            m_eventingActive = true;
            if (m_isDirty && Changed != null) {
                OnChange();  
            }
        }
        /// <summary>
        /// update ranges for this object with union of both ranges. 
        /// </summary>
        /// <param name="clipData">clipping ranges to create union with</param>
        public void Update (ILClippingData clipData) {
            if (clipData.XMin < XMin) { m_isDirty = true; m_xMin = clipData.XMin; }
            if (clipData.YMin < YMin) { m_isDirty = true; m_yMin = clipData.YMin; }
            if (clipData.ZMin < ZMin) { m_isDirty = true; m_zMin = clipData.ZMin; }
            if (clipData.XMax > XMax) { m_isDirty = true; m_xMax = clipData.XMax; }
            if (clipData.YMax > YMax) { m_isDirty = true; m_yMax = clipData.YMax; }
            if (clipData.ZMax > ZMax) { m_isDirty = true; m_zMax = clipData.ZMax; }
            if (m_isDirty && m_eventingActive && Changed != null) 
                OnChange(); 
            }
        /// <summary>
        /// update ranges for this object with point coords for specific axes
        /// </summary>
        /// <param name="point">point with coords to update ranges with</param>
        /// <param name="updateBitFlags">bitflag combination to specify axis to be recognized: 1,2,4 -> x,y,z</param>
        public void Update (ILPoint3Df point, int updateBitFlags) {
            if ((updateBitFlags & 1) != 0) {
                if (point.X < XMin) { m_isDirty = true; m_xMin = point.X; }
                if (point.X > XMax) { m_isDirty = true; m_xMax = point.X; }
            }
            if ((updateBitFlags & 2) != 0) {
                if (point.Y < YMin) { m_isDirty = true; m_yMin = point.Y; }
                if (point.Y > YMax) { m_isDirty = true; m_yMax = point.Y; }
            }
            if ((updateBitFlags & 4) != 0) {
                if (point.Z < ZMin) { m_isDirty = true; m_zMin = point.Z; }
                if (point.Z > ZMax) { m_isDirty = true; m_zMax = point.Z; }
            }
            if (m_isDirty && m_eventingActive && Changed != null) 
                OnChange(); 
        }
        /// <summary>
        /// update clipping data for this object with union of this and rectangle specified
        /// </summary>
        /// <param name="luCorner">left upper corner</param>
        /// <param name="rbCorner">right lower corner</param>
        public void Update (ILPoint3Df luCorner, ILPoint3Df rbCorner) {
            bool oldeventState = m_eventingActive; 
            m_eventingActive = false; 
            Update(luCorner,7);
            m_eventingActive = oldeventState; 
            Update(rbCorner,7);
        }
        public void Update (ILPoint3Df center, float zoomFactor) {
            if (zoomFactor == 1.0f && center == CenterF) return; 
            float s = WidthF * zoomFactor / 2; 
            m_xMin = center.X - s; 
            m_xMax = center.X + s; 
            s = HeightF * zoomFactor / 2; 
            m_yMin = center.Y - s; 
            m_yMax = center.Y + s; 
            s = DepthF * zoomFactor / 2; 
            m_zMin = center.Z - s; 
            m_zMax = center.Z + s;
            m_isDirty = true; 
            if (m_eventingActive && Changed != null) 
                OnChange();  
        }
        /// <summary>
        /// Set clipping limits to volume inside the box specified 
        /// </summary>
        /// <param name="lunCorner">left-upper-near corner of the volume box</param>
        /// <param name="rbfCorner">right-bottom-far corner of the volume box</param>
        public void Set(ILPoint3Df lunCorner, ILPoint3Df rbfCorner) {
            m_xMin = Math.Min(lunCorner.X,rbfCorner.X); 
            m_xMax = Math.Max(lunCorner.X,rbfCorner.X); 
            m_yMin = Math.Min(lunCorner.Y,rbfCorner.Y); 
            m_yMax = Math.Max(lunCorner.Y,rbfCorner.Y); 
            m_zMin = Math.Min(lunCorner.Z,rbfCorner.Z); 
            m_zMax = Math.Max(lunCorner.Z,rbfCorner.Z); 
            m_isDirty = true; 
            if (m_eventingActive && Changed != null) 
                OnChange(); 
        }
        /// <summary>
        /// reset this clipping range to initial (all empty)
        /// </summary>
        public void Reset() {
            m_xMin = float.MaxValue; 
            m_yMin = float.MaxValue; 
            m_zMin = float.MaxValue; 
            m_xMax = float.MinValue; 
            m_yMax = float.MinValue;  
            m_zMax = float.MinValue; 
            m_isDirty = true; 
            if (m_eventingActive && Changed != null) 
                OnChange();  

        }
        /// <summary>
        /// copy this from other clipping data
        /// </summary>
        /// <param name="m_clippingData"></param>
        internal void CopyFrom(ILClippingData m_clippingData) {
            m_xMin = m_clippingData.XMin;
            m_yMin = m_clippingData.YMin;
            m_zMin = m_clippingData.ZMin;
            m_xMax = m_clippingData.XMax;
            m_yMax = m_clippingData.YMax;
            m_zMax = m_clippingData.ZMax;
            m_isDirty = true; 
            if (m_eventingActive && Changed != null) 
                OnChange();  
        }
        /// <summary>
        /// stretch clipping region to unit cube [0 1][0 1][0 1] 
        /// </summary>
        /// <returns></returns>
        public ILPoint3Df ScaleToUnitCube () {
            ILPoint3Df ret = new ILPoint3Df(); 
            float a = WidthF; 
            if (a != 0.0f)  ret.X = 1 / a;
            else            ret.X = 0.0f; 

            a = HeightF; 
            if (a != 0.0f)  ret.Y = 1 / a;
            else            ret.Y = 0.0f; 

            a = DepthF; 
            if (a != 0.0f)  ret.Z = 1 / a;
            else            ret.Z = 0.0f; 
            return ret; 
        }
        /// <summary>
        /// offset centering scaled unit cube to zero: [-0.5 0.5][-0.5 0.5][-0.5 0.5]
        /// </summary>
        /// <returns></returns>
        public ILPoint3Df CenterToUnitCube() {
            ILPoint3Df a = ScaleToUnitCube(); 
            ILPoint3Df b = new ILPoint3Df(); 
            b.X = -0.5f - a.X * XMin; 
            b.Y = -0.5f - a.Y * YMin; 
            if (ZMin < ZMax)
                b.Z = (-0.5f - a.Z * ZMin); 
            else b.Z = 0.0f; 
            return b; 
        }
        /// <summary>
        /// Clone this instance and return clone
        /// </summary>
        /// <returns>clone</returns>
        public ILClippingData Clone() {
            ILClippingData ret = new ILClippingData(); 
            ret.Update(this); 
            return ret;
        }
        /// <summary>
        /// Map coordinat from unit cube space [-0.5..0.5] into the space limited by this clipping data
        /// </summary>
        /// <param name="x">x coordinate (unit cube space: -0.5 ... 0.5)</param>
        /// <param name="y">y coordinate (unit cube space: -0.5 ... 0.5)</param>
        /// <param name="z">z coordinate (unit cube space: -0.5 ... 0.5)</param>
        /// <returns></returns>
        public ILPoint3Df Map(float x, float y, float z) {
            ILPoint3Df ret; 
            ret.X = (x + 0.5f) * (m_xMax-m_xMin) + m_xMin; 
            ret.Y = (y + 0.5f) * (m_yMax-m_yMin) + m_yMin; 
            ret.Z = (z + 0.5f) * (m_zMax-m_zMin) + m_zMin; 
            return ret; 
        }
        /// <summary>
        /// Ensure that this clipping data has valid length for all dimensions  [deprecated]
        /// </summary>
        /// <remarks>If the length of the clipping cube is infinity for any dimension, 
        /// that dimension is set to a range of -0.5...0.5.
        /// <para>This function is to be called by custom graphs which create their 
        /// size <b>relative</b> to the size of the clipping container. Those 
        /// graphs will need a valid container size and may call this function in the 
        /// constructor.</para></remarks>
        /// <returns>true if the length in any dimension had to be corrected (set to 1.0), false otherwise.</returns>
        private bool EnsureValidSize() {
            bool change = false; 
            if (float.IsInfinity(WidthF)) {
                change = true;
                m_xMin = -0.5f;
                m_xMax = 0.5f;
            }
            if (float.IsInfinity(HeightF)) {
                change = true;
                m_yMin = -0.5f;
                m_yMax = 0.5f; 
            }
            if (float.IsInfinity(DepthF)) {
                change = true;
                m_zMin = -0.5f;
                m_zMax = 0.5f;
            }
            if (change && m_eventingActive && Changed != null) {
                OnChange(); 
            }
            return change; 
        }
        #endregion

        #region operator overloads 
        public static bool operator == (ILClippingData limit1, ILClippingData limit2) {
            return (limit1.m_xMax == limit2.m_xMax && 
                    limit1.m_yMax == limit2.m_yMax && 
                    limit1.m_zMax == limit2.m_zMax && 
                    limit1.m_xMin == limit2.m_xMin && 
                    limit1.m_yMin == limit2.m_yMin && 
                    limit1.m_zMin == limit2.m_zMin); 
        }
        public static bool operator != (ILClippingData limit1, ILClippingData limit2) {
            return (limit1.m_xMax != limit2.m_xMax || 
                    limit1.m_yMax != limit2.m_yMax ||
                    limit1.m_zMax != limit2.m_zMax || 
                    limit1.m_xMin != limit2.m_xMin || 
                    limit1.m_yMin != limit2.m_yMin || 
                    limit1.m_zMin != limit2.m_zMin); 
        }

        #endregion

    }
}
