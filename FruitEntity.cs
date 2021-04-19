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
        fType fruitType;
        float fallSpeed;

        Matrix[] transformations;

        List<FruitEntity> fruitEntities = new List<FruitEntity>();

        public enum fType { ananas, apple, pear, cherry, melon}
        int fruitTypeAmount = Enum.GetNames(typeof(fType)).Length;
        public FruitEntity(Model genModel)
        {
            fruitType = getFruit(genModel);
            fposition = Vector3.Left * new Random().Next(-10, 10) + Vector3.Forward * new Random().Next(-10,10);
            fmodel = genModel;
            fallSpeed = 1f;         
        }


        public fType getFruit (Model model) 
        {
            return fType.ananas;
        }

        public void Update(GameTime gameTime)
        {
            fposition -= Vector3.Down * fallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void CollisionCheck()
        {
            //TODO
        }


        public void Draw(Matrix projectionMatrix, Matrix viewMatrix, ContentManager content)
        {
            foreach(FruitEntity fruit in fruitEntities)
            {
                DrawModel(fruit.fmodel, projectionMatrix, viewMatrix);
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
                    effect.World = transformations[modelMesh.ParentBone.Index] * Matrix.CreateScale(100f) * Matrix.CreateTranslation(fposition) ;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
        }
    }
}
