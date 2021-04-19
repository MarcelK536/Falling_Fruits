using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Falling_Fruits
{
    class FruitEntity
    {
        public Vector3[] Position;
        public Model[] Model;
        public int[] random;



        public FruitEntity(Model[] m)
        {
            random = new int[7];
            Position = new Vector3[random.Length];
            var rando = new Random();
            for (int i = 0; i<random.Length; i++) {
                Position[i] = new Vector3(i, 7, 0);

                random[i] = rando.Next(0, m.Length);
            }
            this.Model = m;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i<Position.Length; i++) {
                Position[i].Y -= 0.01f;
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            for (int i = 0; i<random.Length; i++) { 
            foreach (ModelMesh modelMesh in Model[random[i]].Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.View = view;
                    effect.World = Matrix.CreateTranslation(Position[i]);
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
            }
        }
    }
}
