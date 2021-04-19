using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Falling_Fruits
{
    class Player
    {
        Model pModel;
        Vector3 pPosition;
        Vector3 pRotation;
        float pSpeed;

        Vector3 camPos;
        Matrix viewMatrix;
        Matrix[] transformations;

        public Player(Vector3 pos)
        {
            camPos = new Vector3(pos.X, pos.Y + 10, pos.Z - 10);
            viewMatrix = Matrix.CreateLookAt(camPos, Vector3.Zero, Vector3.Up);
            pRotation = Vector3.Zero;
            pSpeed = 5f;
        }

        public void LoadContent(ContentManager content)
        {
            pModel = content.Load<Model>("Player\\teller");
        }

        public void Update(GameTime gameTime)
        {
            camPos.X = pRotation.X - 15f * (float)Math.Sin(pRotation.Y);
            camPos.Z = pRotation.Z - 5f * (float)Math.Cos(pRotation.Y);
            viewMatrix = Matrix.CreateLookAt(camPos, pPosition, Vector3.Up);

            KeyboardState kbState = Keyboard.GetState();

            if(kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
            {
                pPosition.X += (float)Math.Sin(pRotation.Y) * pSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                pPosition.Z += (float)Math.Cos(pRotation.Y) * pSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
            {
                pPosition.X -= (float)Math.Sin(pRotation.Y) * pSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                pPosition.Z -= (float)Math.Cos(pRotation.Y) * pSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
            {
                pRotation.Y += .05f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
            {
                pRotation.Y -= .05f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        public void Draw(Matrix projectionMatrix)
        {
            transformations = new Matrix[pModel.Bones.Count];
            pModel.CopyAbsoluteBoneTransformsTo(transformations);
            foreach (ModelMesh modelMesh in pModel.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.View = viewMatrix;
                    effect.World = transformations[modelMesh.ParentBone.Index] * Matrix.CreateScale(.01f) * Matrix.CreateRotationX(pRotation.X) * Matrix.CreateRotationY(pRotation.Y) * Matrix.CreateRotationZ(pRotation.Z) * Matrix.CreateTranslation(pPosition);
                    effect.Projection = projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
        }
    }
}
