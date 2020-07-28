using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    //目前此类的父类是世界结点，可以拓展为自定义父节点
    public class Transform
    {
        private Vector m_Position;

        //世界坐标
        public Vector position
        {
            set
            {
                if (m_Position == value) return;
                m_Position = value;
                UpdateMatrix();
            }
            get => m_Position.Copy();
        }

        private Vector m_RotationAngle;

        //世界坐标下的旋转
        public Vector rotationAngle
        {
            set
            {
                if (m_RotationAngle == value) return;
                m_RotationAngle = value;
                UpdateMatrix();
            }
            get => m_RotationAngle.Copy();
        }

        private Vector m_LocalScale;

        public Vector localScale
        {
            set
            {
                if (m_LocalScale == value) return;
                m_LocalScale = value;
                UpdateMatrix();
            }
            get => m_LocalScale.Copy();
        }

        public Matrix translateMatrix;

        public Matrix rotateMatrix;

        public Matrix scaleMatrix;

        public Matrix object2world;

        public Matrix world2View;

        public Matrix viewToClip;

        public Transform()
        {
            Init();
        }

        private void Init()
        {
            m_Position = Vector.Zero;
            m_RotationAngle = Vector.Zero;
            m_LocalScale = Vector.One;
            scaleMatrix = Matrix.Identity;
            translateMatrix = Matrix.Identity;
            rotateMatrix = Matrix.Identity;
            object2world = Matrix.Identity;
            viewToClip = Matrix.Identity;
            world2View = Matrix.Identity;
        }

        private void UpdateMatrix()
        {
            translateMatrix = Matrix.Translation(m_Position);
            rotateMatrix = Matrix.RotationAngle(m_RotationAngle);
            scaleMatrix = Matrix.Scale(m_LocalScale);
            object2world = scaleMatrix * rotateMatrix * translateMatrix;
        }

        public Vector ApplyTransfer(Vector point)
        {
            //return point.ApplyTransfer(object2world).ApplyTransfer(world2View).ApplyTransfer(viewToClip, true);

            Vector v1 = point.ApplyTransfer(object2world);
            Vector v2 = v1.ApplyTransfer(world2View);
            Vector v3 = v2.ApplyTransfer(viewToClip);
            return v3;
        }

        public Vector ApplyObj2World(Vector point)
        {
            return point.ApplyTransfer(object2world);
        }

        public Vector ApplyWorldToView(Vector point)
        {
            return point.ApplyTransfer(world2View);
        }

        public Vector ApplyViewToClip(Vector point)
        {
            return viewToClip.ApplyTransfer(point);
        }
    }
}
