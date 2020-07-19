using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Camera
    {
        public Transform transform;
        private static Camera m_Main;
        public static Camera main
        {
           get
            {
                if (m_Main == null)
                {
                    m_Main = new Camera();
                }
                return m_Main;
           }
        }
        public Camera()
        {
            transform = new Transform();
            aspect = Screen.width / Screen.height;
        }

        public float fov = 45f;
        public float aspect = 2;
        public float nearPlane = 1;
        public float farPlene = 20;

    }
}
