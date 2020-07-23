using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    //这里相当于CPU端，准备数据然后送到GPU渲染出来
    public class GameLogic
    {
        private static GameLogic m_Instance;

        public static GameLogic Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GameLogic();
                }
                return m_Instance;
            }
        }
        private float temp = 0;

        private GameObject cube1;
        private GameObject cube2;
        public void Init()
        {
            Camera camera = Camera.main;
            camera.transform.position = new Vector3(0, 0, -10);
            cube1 = new GameObject("cube");
            var meshRenderer = cube1.AddComponent<MeshRenderer>();
            meshRenderer.mesh = CreateMesh();
            cube1.transform.rotationAngle = new Vector3(0, 45, 0);
            cube2 = new GameObject("cube");
//            var meshRenderer2 = cube2.AddComponent<MeshRenderer>();
//            meshRenderer2.mesh = CreateMesh1();
//            cube2.transform.rotationAngle = new Vector3(45, 45, 45);
//            cube2.transform.position = new Vector3(5, 0, 0);
        }

        public void Update()
        {
            Vector3 rotation = cube1.transform.rotationAngle;
            temp += 1;
            temp %= 180;
            rotation.y = temp;
            Debug.Log("rotation " + rotation);
            cube1.transform.rotationAngle = rotation;
            //            cube2.transform.rotationAngle = rotation;
        }

        public MeshRenderer CreateMeshRenderer()
        {
            Mesh mesh = CreateMesh2();
            MeshRenderer meshRenderer = new MeshRenderer();
            return meshRenderer;
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

            return new Mesh(vects, surfaces);
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


            return new Mesh(vects, surfaces);
        }

        private Mesh CreateMesh()
        {
            Vector3[] vects = new Vector3[8];
            vects[0] = new Vector3(-1, -1, -1);
            vects[1] = new Vector3(1, -1, -1);
            vects[2] = new Vector3(1, 1, -1);
            vects[3] = new Vector3(-1, 1, -1);
            vects[4] = new Vector3(1, -1, 1);
            vects[5] = new Vector3(1, 1, 1);
            vects[6] = new Vector3(-1, -1, 1);
            vects[7] = new Vector3(-1, 1, 1);

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


            Mesh mesh = new Mesh(vects, surfaces);
            return mesh;
        }

    }
}
