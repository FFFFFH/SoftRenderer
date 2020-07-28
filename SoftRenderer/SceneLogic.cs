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

        private Vector camVelocity;

        private GameObject cube1;
        private GameObject cube2;
        public void Init()
        {
            Application.RenderType = RenderType.Texture;
            Application.Texture = Texture2D.LoadTexture("../../Textures/书记.jpg");
            Camera camera = Camera.main;
            camera.transform.position = new Vector(0, 0, -10);
            cube1 = new GameObject("cube1");
            var meshRenderer = cube1.AddComponent<MeshRenderer>();
            meshRenderer.mesh = CreateMesh();
            cube1.transform.rotationAngle = new Vector(10, 50, 45);
            cube1.transform.position = new Vector(0, 0, 0);
            //cube2 = new GameObject("cube2");
            //var meshRenderer2 = cube2.AddComponent<MeshRenderer>();
            //meshRenderer2.mesh = CreateMesh();
            //cube2.transform.rotationAngle = new Vector3(45, 45, 45);
            //cube2.transform.position = new Vector3(2, 0, 0);
            camVelocity = new Vector(0, 0, -0.1f);
        }

        public void Update()
        {
            RotateCube1();
            //MoveCamera();
        }

        private void RotateCube1()
        {
            Vector rotation = cube1.transform.rotationAngle;
            float r = rotation.y + 1;
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

        private Mesh CreateMesh()
        { 
            Vertex[] vertices = new Vertex[24];
            vertices[0] = new Vertex(new Vector(-1, -1, -1), Color.Red, 0, 0);
            vertices[1] = new Vertex(new Vector(1, -1, -1), Color.Cornsilk, 1, 0);
            vertices[2] = new Vertex(new Vector(1, 1, -1), Color.Peru, 1, 1);
            vertices[3] = new Vertex(new Vector(-1, 1, -1), Color.Green, 0, 1);


            vertices[4] = new Vertex(new Vector(1, -1, -1), Color.Brown, 0, 0);
            vertices[5] = new Vertex(new Vector(1, -1, 1), Color.Cyan, 1, 0);
            vertices[6] = new Vertex(new Vector(1, 1, 1), Color.Yellow, 1, 1);
            vertices[7] = new Vertex(new Vector(1, 1, -1), Color.Blue, 0, 1);

            vertices[8] = new Vertex(new Vector(1, -1, 1), Color.Brown, 0, 0);
            vertices[9] = new Vertex(new Vector(-1, -1, 1), Color.Cyan, 1, 0);
            vertices[10] = new Vertex(new Vector(-1, 1, 1), Color.Yellow, 1, 1);
            vertices[11] = new Vertex(new Vector(1, 1, 1), Color.Blue, 0, 1);

            vertices[12] = new Vertex(new Vector(-1, -1, 1), Color.Brown, 0, 0);
            vertices[13] = new Vertex(new Vector(-1, -1, -1), Color.Cyan, 1, 0);
            vertices[14] = new Vertex(new Vector(-1, 1, -1), Color.Yellow, 1, 1);
            vertices[15] = new Vertex(new Vector(-1, 1, 1), Color.Blue, 0, 1);

            vertices[16] = new Vertex(new Vector(-1, 1, -1), Color.Brown, 0, 0);
            vertices[17] = new Vertex(new Vector(1, 1, -1), Color.Cyan, 1, 0);
            vertices[18] = new Vertex(new Vector(1, 1, 1), Color.Yellow, 1, 1);
            vertices[19] = new Vertex(new Vector(-1, 1, 1), Color.Blue, 0, 1);

            vertices[20] = new Vertex(new Vector(-1, -1, -1), Color.Brown, 0, 0);
            vertices[21] = new Vertex(new Vector(-1, -1, 1), Color.Cyan, 1, 0);
            vertices[22] = new Vertex(new Vector(1, -1, 1), Color.Yellow, 1, 1);
            vertices[23] = new Vertex(new Vector(1, -1, -1), Color.Blue, 0, 1);


            Surface[] surfaces = new Surface[12];
            surfaces[0].A = 0;
            surfaces[0].B = 1;
            surfaces[0].C = 2;
            surfaces[1].A = 0;
            surfaces[1].B = 2;
            surfaces[1].C = 3;

            surfaces[2].A = 4;
            surfaces[2].B = 5;
            surfaces[2].C = 6;
            surfaces[3].A = 4;
            surfaces[3].B = 6;
            surfaces[3].C = 7;

            surfaces[4].A = 8;
            surfaces[4].B = 9;
            surfaces[4].C = 10;
            surfaces[5].A = 8;
            surfaces[5].B = 10;
            surfaces[5].C = 11;

            surfaces[6].A = 12;
            surfaces[6].B = 13;
            surfaces[6].C = 14;
            surfaces[7].A = 12;
            surfaces[7].B = 14;
            surfaces[7].C = 15;

            surfaces[8].A = 16;
            surfaces[8].B = 17;
            surfaces[8].C = 18;
            surfaces[9].A = 16;
            surfaces[9].B = 18;
            surfaces[9].C = 19;

            surfaces[10].A = 20;
            surfaces[10].B = 21;
            surfaces[10].C = 22;
            surfaces[11].A = 20;
            surfaces[11].B = 22;
            surfaces[11].C = 23;

            Mesh mesh = new Mesh(vertices, surfaces);
            return mesh;
        }

    }
}
