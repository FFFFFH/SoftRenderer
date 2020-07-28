﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public static class Screen
    {
        public static int width = 1024;
        public static int height = 512;
        public static Vector ComputeToScreenPos(Vector p)
        {
            return new Vector(Screen.width / 2 + (p.x * Screen.width / 2),
                Screen.height / 2 - (p.y * Screen.height / 2), p.z);
        }
    }
}
