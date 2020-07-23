using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Camera : Component
    {
        private static Camera m_Main;
        public static Camera main
        {
           get
            {
                if (m_Main == null)
                {
                    GameObject go = new GameObject("MainCamera");
                    m_Main = go.AddComponent<Camera>();
                }
                return m_Main;
           }
        }


        public Matrix GetWorldToView()
        {
            Vector3 cameraPos = transform.position;

            //TODO: 由摄像机的朝向计算
            Vector3 targetPos = new Vector3(0, 0, 0);

            //TODO: 由摄像机的朝向计算
            Vector3 upDir = new Vector3(0, 1, 0);

            return Matrix.LookAtLH(cameraPos,
                targetPos, upDir);
        }

        public Matrix GetPerspectiveMatrix()
        {
            return Matrix.PerspectiveFov(fov,aspect, nearPlane, farPlene);
        }

        public Camera()
        {
            aspect = Screen.width / Screen.height;
        }

        public float fov = 45f;
        public float aspect = 2;
        public float nearPlane = 2;
        public float farPlene = 20;

    }
}
