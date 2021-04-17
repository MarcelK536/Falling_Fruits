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
        public Vector3 Position;
        public Model Model;
        fruitTypes fruit;
        float fallSpeed;

        Matrix[] transformations;

        List<FruitEntity> fruitEntities = new List<FruitEntity>();

        public enum fruitTypes { ananas, apple, pear, cherry, melon}
        int fruitTypeAmount = Enum.GetNames(typeof(fruitTypes)).Length;
        public FruitEntity()
        {
            fruit = (fruitTypes) new Random().Next(fruitTypeAmount);
            Position = Vector3.Left * new Random().Next(10) + Vector3.Forward * new Random().Next(10);
            fallSpeed = 1f;
        }

        public void LoadModel(ContentManager content)
        {
            switch (fruit)
            {
                case fruitTypes.ananas:
                    Model = content.Load<Model>("Fruits\\ananas");
                    break;
                case fruitTypes.apple:
                    Model = content.Load<Model>("Fruits\\apfel");
                    break;
                case fruitTypes.pear:
                    Model = content.Load<Model>("Fruits\\birne");
                    break;
                case fruitTypes.cherry:
                    Model = content.Load<Model>("Fruits\\kirsche");
                    break;
                case fruitTypes.melon:
                    Model = content.Load<Model>("Fruits\\melone");
                    break;
                default:
                    Model = content.Load<Model>("Fruits\\melonenstueck");
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            Position -= Vector3.Down * fallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            NewFruit();
        }

        public void CollisionCheck()
        {
            //TODO
        }

        public void NewFruit()
        {
            while(fruitEntities.Count < 4)
            {
                fruitEntities.Add(new FruitEntity());
            }
        }

        public void Draw(Matrix projectionMatrix, Matrix viewMatrix)
        {
            foreach(FruitEntity fruit in fruitEntities)
            {
                DrawModel(Model, projectionMatrix, viewMatrix);
            }
        }
        protected void DrawModel(Model model, Matrix projectionMatrix, Matrix viewMatrix)
        {
            transformations = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transformations);
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.World = transformations[modelMesh.ParentBone.Index] * Matrix.CreateScale(10f);
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
        }
    }
}
