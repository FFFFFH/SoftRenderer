using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class TransformUtility
    {
        public static Vector3 ComputeToScreenPos(Vector3 p)
        {
            return new Vector3(Screen.width / 2 + (p.x * Screen.width / 2),
                Screen.height / 2 - (p.y * Screen.height / 2), p.z);
        }
    }
}
