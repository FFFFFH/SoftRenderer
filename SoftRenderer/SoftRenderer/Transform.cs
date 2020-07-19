using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    //目前此类的父类是世界结点，
    //后面考虑加入自定义父节点
    public class Transform
    {
        private Vector3 m_Position;

        //世界坐标
        public Vector3 position
        {
            set
            {
                if (m_Position == value) return;
                m_Position = value;
                UpdateMatrix();
            }
            get => m_Position;
        }

        private Vector3 m_RotationAngle;

        //世界坐标下的旋转
        public Vector3 rotationAngle
        {
            set
            {
                if (m_RotationAngle == value) return;
                m_RotationAngle = value;
                UpdateMatrix();
            }
            get => m_RotationAngle;
        }

        private Vector3 m_LocalScale;

        public Vector3 localScale
        {
            set
            {
                if (m_LocalScale == value) return;
                m_LocalScale = value;
                UpdateMatrix();
            }
            get => m_LocalScale;
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
            m_Position = Vector3.Zero;
            m_RotationAngle = Vector3.Zero;
            m_LocalScale = Vector3.One;
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

        public Vector3 ApplyTransfer(Vector3 point)
        {
            Vector3 v1 = point.ApplyTransfer(object2world);
            Vector3 v2 = v1.ApplyTransfer(world2View);
            Vector3 v3 = v2.ApplyTransfer(viewToClip,true);
            return v3;
        }

    }
}
