using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Falling_Fruits
{
    class FruitEntity
    {
        public Vector3 Position;
        public Model Model;

        public FruitEntity(Model m)
        {
            this.Model = m;
        }
    }
}
