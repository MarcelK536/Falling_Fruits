using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Falling_Fruits
{
    class FruitEntity
    {
        public Vector3 fposition;
        public Model fmodel;
        float fallSpeed;

        Matrix[] transformations;

        public enum fType { ananas, apple, pear, cherry, melon}

        public FruitEntity(Model genModel)
        {
            fposition = Vector3.Left * new Random().Next(-10, 10) + Vector3.Forward * new Random().Next(-10, 10) + Vector3.Up * new Random().Next(8);
            fmodel = genModel;
            fallSpeed = 0.2f;         
        }

        public void Update(GameTime gameTime)
        {
            if(fposition.Y > 0)
            fposition += Vector3.Down * fallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool CollisionCheck(Player player, FruitEntity fruit)
        {
            if (Vector3.Distance(player.pPosition, fruit.fposition) < 3)
            {
               player.score++;
               return true;
            }
            return false;
        }


        public void Draw(List<FruitEntity> fruitList, Matrix projectionMatrix, Matrix playerView)
        {
            foreach(FruitEntity fruit in fruitList)
            {
                DrawModel(fruit.fmodel, projectionMatrix, playerView,fruit.fposition);
            }
        }

        protected void DrawModel(Model model, Matrix projectionMatrix, Matrix playerView, Vector3 fruitPosition)
        {
            transformations = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transformations);
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.View = playerView;
                    effect.World = Matrix.CreateTranslation(fruitPosition);
                    effect.Projection = projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
        }
    }
}
