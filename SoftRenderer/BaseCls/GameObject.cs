using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public enum Layer
    {
        Default = 0
    }

    public class GameObject
    {
        public string m_Name;

        public Layer layer { get; set; }

        public bool active
        {
            get;
            set;
        }

        public Transform transform = new Transform();

        private List<Component> components = new List<Component>();

        public GameObject(string name = "GameObject")
        {
            m_Name = name;
        }

        public T AddComponent<T>() where T : Component,new()
        {
            if (GetComponent<T>()!=null)
            {
                Debug.Log("请勿重复添加组件 ！");
                return null;
            }
            var com = new T();
            com.enable = true;
            com.transform = transform;
            com.gameObject = this;
            components.Add(com);
            return com;
        }
         
        public T AddComponent<T>(T com) where T : Component
        {
            if (GetComponent<T>() != null)
            {
                Debug.Log("请勿重复添加组件 ！");
                return null;
            }
            com.transform = transform;
            com.gameObject = this;
            components.Add(com);
            return com;
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T)
                {
                    return (T)components[i];
                }
            }
            return null;
        }

    }
}
