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
        public Vector3 pPosition;
        Vector3 pRotation;
        float pSpeed;
        public int score;

        Vector3 camPos;
        public Matrix viewMatrix;
        Matrix[] transformations;

        public Player(Vector3 pos)
        {
            camPos = new Vector3(pos.X, pos.Y + 7, pos.Z - 15);
            pRotation = Vector3.Zero;
            pSpeed = 0.5f;
            score = 0;
        }

        public void LoadContent(ContentManager content)
        {
            pModel = content.Load<Model>("Player\\teller");
        }

        public Matrix GetView()
        {
            return viewMatrix;
        }

        public Vector3 GetPos()
        {
            return pPosition;
        }

        public void Update(GameTime gameTime)
        {
            camPos.X = pPosition.X - 15f * (float)Math.Sin(pRotation.Y);
            camPos.Z = pPosition.Z - 15f * (float)Math.Cos(pRotation.Y);
            viewMatrix = Matrix.CreateLookAt(camPos, pPosition, Vector3.Up);

            KeyboardState kbState = Keyboard.GetState();

            if(kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
            {
                pPosition.X += (float)Math.Sin(pRotation.Y) * pSpeed;
                pPosition.Z += (float)Math.Cos(pRotation.Y) * pSpeed;
            }
            if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
            {
                pPosition.X -= (float)Math.Sin(pRotation.Y) * pSpeed;
                pPosition.Z -= (float)Math.Cos(pRotation.Y) * pSpeed;

            }
            if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
            {
                pRotation.Y += .05f;
            }
            if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
            {
                pRotation.Y -= .05f;
            }

            if (pPosition.X > 15)
            {
                pPosition.X = 15;
            }
            if (pPosition.X < -15)
            {
                pPosition.X = -15;
            }
            if (pPosition.Z > 15)
            {
                pPosition.Z = 15;
            }
            if (pPosition.Z < -15)
            {
                pPosition.Z = -15;
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
