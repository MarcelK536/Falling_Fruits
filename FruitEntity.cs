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

        public bool[] gefangen;


        int w = 26;

        Random rando = new Random();


        public FruitEntity(Model[] m)
        {
 
            random = new int[26];
            Position = new Vector3[random.Length];
            gefangen = new bool[random.Length];

            for (int i = 0; i<random.Length; i++) {
                Position[i] = new Vector3(i-(w/2), rando.Next(7, 28), 0);
                gefangen[i] = false;
                random[i] = rando.Next(0, m.Length);
            }
            this.Model = m;
        }

        public void Update(GameTime gameTime, Vector3 playerPos)
        {
            for (int i = 0; i<Position.Length; i++) {
                
                if(Vector3.Distance(new Vector3(playerPos.X, playerPos.Y -2, playerPos.Z), Position[i])< 3)
                {
                    gefangen[i] = true;
                }

                if (Position[i].Y > -13 && !gefangen[i])
                {
                    Position[i].Y -= 0.07f;
                }
                else if(!gefangen[i])
                {
                    Position[i] = new Vector3(i - (w / 2), rando.Next(7, 28), 0);
                }
                else
                {
                    //Position[i] = Vector3.Zero;
                }
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

        public int getCaught()
        {
            int anzahlGefangen = 0;
            for(int i =0; i< gefangen.Length; i++)
            {
                if(gefangen[i])
                    anzahlGefangen++;
            }
            return anzahlGefangen;
        }
    }
}
