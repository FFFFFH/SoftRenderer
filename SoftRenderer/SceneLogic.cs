using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class SceneLogic
    {
        private static SceneLogic m_Instance;

        public static SceneLogic Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new SceneLogic();
                }
                return m_Instance;
            }
        }
        private float rotateStep = 0;

        private float moveStep = 0;

        private Vector3 camVelocity;

        private GameObject cube1;
        private GameObject cube2;
        public void Init()
        {
            Application.RenderType = RenderType.GradientColor;
            Camera camera = Camera.main;
            camera.transform.position = new Vector3(0, 0, -10);
            cube1 = new GameObject("cube1");
            var meshRenderer = cube1.AddComponent<MeshRenderer>();
            meshRenderer.mesh = CreateMesh();
            cube1.transform.rotationAngle = new Vector3(45, 90, 30);
            cube1.transform.position = new Vector3(0, 0, 0);
            //cube2 = new GameObject("cube2");
            //var meshRenderer2 = cube2.AddComponent<MeshRenderer>();
            //meshRenderer2.mesh = CreateMesh();
            //cube2.transform.rotationAngle = new Vector3(45, 45, 45);
            //cube2.transform.position = new Vector3(2, 0, 0);

            camVelocity = new Vector3(0, 0, -0.1f);
        }

        public void Update()
        {
            RotateCube1();
            //MoveCamera();
        }

        private void RotateCube1()
        {
            Vector3 rotation = cube1.transform.rotationAngle;
            float r = rotation.y + 2;
            r %= 360;
            rotation.y = r;
            cube1.transform.rotationAngle = rotation;
        }

        private void MoveCamera()
        {
            var camPos = Camera.main.transform.position;

            if ((camPos.z < -20 && camVelocity.z < 0)
                || camPos.z > -10 && camVelocity.z > 0)
            {
                camVelocity.z = -camVelocity.z;
            }
            camPos.z += camVelocity.z;
            Camera.main.transform.position = camPos;
        }

        private Mesh CreateMesh1()
        {
            Vector3[] vects = new Vector3[6];
            vects[0] = new Vector3(-0.5f, -0.5f, 0);
            vects[1] = new Vector3(0.5f, -0.5f, 0);
            vects[2] = new Vector3(0.5f, 0.5f, 0);
            vects[3] = new Vector3(-0.5f, 0.5f, 0);

            vects[4] = new Vector3(0.5f, 0, 0.5f);
            vects[5] = new Vector3(0.5f, 1, 0.5f);

            Surface[] surfaces = new Surface[2];
            surfaces[0].A = 0;
            surfaces[0].B = 1;
            surfaces[0].C = 2;

            surfaces[1].A = 0;
            surfaces[1].B = 2;
            surfaces[1].C = 3;

            //surfaces[2].A = 1;
            //surfaces[2].B = 4;
            //surfaces[2].C = 5;

            //surfaces[3].A = 1;
            //surfaces[3].B = 5;
            //surfaces[3].C = 2;

            //return new Mesh(vects, surfaces);
            return null;
        }

        private Mesh CreateMesh2()
        {
            Vector3[] vects = new Vector3[6];
            vects[0] = new Vector3(0, 0, 0);
            vects[1] = new Vector3(1, 0, 0);
            vects[2] = new Vector3(0, 1, 0);
            Surface[] surfaces = new Surface[1];
            surfaces[0].A = 0;
            surfaces[0].B = 1;
            surfaces[0].C = 2;


            return null;
        }

        private Mesh CreateMesh()
        {
            Vertex[] vertices = new Vertex[8];
            vertices[0] = new Vertex(new Vector3(-1, -1, -1), Color.Red);
            vertices[1] = new Vertex(new Vector3(1, -1, -1), Color.Cornsilk);
            vertices[2] = new Vertex(new Vector3(1, 1, -1), Color.Peru);
            vertices[3] = new Vertex(new Vector3(-1, 1, -1), Color.Green);
            vertices[4] = new Vertex(new Vector3(1, -1, 1), Color.Brown);
            vertices[5] = new Vertex(new Vector3(1, 1, 1), Color.Cyan);
            vertices[6] = new Vertex(new Vector3(-1, -1, 1), Color.Yellow);
            vertices[7] = new Vertex(new Vector3(-1, 1, 1), Color.Blue);

            Surface[] surfaces = new Surface[12];
            surfaces[0].A = 0;
            surfaces[0].B = 1;
            surfaces[0].C = 2;
            surfaces[1].A = 0;
            surfaces[1].B = 2;
            surfaces[1].C = 3;

            surfaces[2].A = 1;
            surfaces[2].B = 4;
            surfaces[2].C = 5;
            surfaces[3].A = 1;
            surfaces[3].B = 5;
            surfaces[3].C = 2;

            surfaces[4].A = 4;
            surfaces[4].B = 6;
            surfaces[4].C = 7;
            surfaces[5].A = 4;
            surfaces[5].B = 7;
            surfaces[5].C = 5;

            surfaces[6].A = 6;
            surfaces[6].B = 0;
            surfaces[6].C = 3;
            surfaces[7].A = 6;
            surfaces[7].B = 3;
            surfaces[7].C = 7;

            surfaces[8].A = 3;
            surfaces[8].B = 2;
            surfaces[8].C = 5;
            surfaces[9].A = 3;
            surfaces[9].B = 5;
            surfaces[9].C = 7;

            surfaces[10].A = 1;
            surfaces[10].B = 0;
            surfaces[10].C = 4;
            surfaces[11].A = 0;
            surfaces[11].B = 6;
            surfaces[11].C = 4;

            Mesh mesh = new Mesh(vertices, surfaces);
            return mesh;
        }

    }
}
