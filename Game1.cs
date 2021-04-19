﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Falling_Fruits
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        Player player;
        FruitEntity fruitEntity;

        List<GUI> mainMenu = new List<GUI>();
        List<GUI> creditsMenu = new List<GUI>();
        SpriteFont CreditsFont;

        Model banana;
        Model apple;
        Model pear;
        Model melon;

        Model fruit_plate;
        Model plate;
        Model terrain;

        Random random;

        List<string> fruitTypes = new List<string>()
        {
            "banana","apple","pear","melon"
        };
        List<FruitEntity> fruitList = new List<FruitEntity>();

        enum GameState
        {
            MainMenu,
            inGame,
            Credits
        }

        GameState gameState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            player = new Player(Vector3.Zero);


            mainMenu.Add(new GUI("play"));
            mainMenu.Add(new GUI("credits"));

            creditsMenu.Add(new GUI("back"));
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            //CameraSetup
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -50f);

            worldMatrix = Matrix.CreateTranslation(new Vector3(0f, 0f, 0f));
            viewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, 10f), new Vector3(0f, 0f, 0f), Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            CreditsFont = Content.Load<SpriteFont>("GUI\\CreditsFont");

            mainMenu.Find(x => x.AssetName == "play").LoadContent(Content.Load<Texture2D>("GUI\\Play"));
            mainMenu.Find(x => x.AssetName == "credits").LoadContent(Content.Load<Texture2D>("GUI\\Credits"));

            foreach (GUI g in mainMenu)
            {
                g.CenterElement(600, 800);
            }
            mainMenu.Find(x => x.AssetName == "credits").MoveElement(210, 0);

            creditsMenu.Find(x => x.AssetName == "back").LoadContent(Content.Load<Texture2D>("GUI\\Back"));
            foreach (GUI g in creditsMenu)
            {
                g.CenterElement(600, 800);
            }
            creditsMenu.Find(x => x.AssetName == "back").MoveElement(-85, -75);

            player.LoadContent(Content);

            banana = Content.Load<Model>("Fruits\\banane");
            apple = Content.Load<Model>("Fruits\\apfel");
            pear = Content.Load<Model>("Fruits\\birne");
            melon = Content.Load<Model>("Fruits\\melone");
            plate = Content.Load<Model>("Player\\teller");
            terrain = Content.Load<Model>("Player\\boden");
            fruit_plate = Content.Load<Model>("Fruits\\FruchtTeller");

            Model[] m = new Model[4];
            m[0] = banana;
            m[1] = apple;
            m[2] = pear;
            m[3] = melon;
            fruitEntity = new FruitEntity(m);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            worldMatrix = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds / 2);

            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.inGame:
                    UpdateInGame(gameTime);
                    break;
                case GameState.Credits:
                    UpdateCredits(gameTime);
                    break;
            };
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            foreach (GUI g in mainMenu)
            {
                if (g.Update())
                {
                    switch (g.AssetName)
                    {
                        case ("play"):
                            gameState = GameState.inGame;
                            break;
                        case ("credits"):
                            gameState = GameState.Credits;
                            break;
                        default:
                            break;
            
                    }
                }
            }
            
        }
        private void UpdateInGame(GameTime gameTime)
        {
            player.Update(gameTime);
            fruitEntity.Update(gameTime, player.GetPos());
        }
        private void UpdateCredits(GameTime gameTime)
        {
            foreach (GUI g in creditsMenu)
            {
                if (g.Update())
                {
                    switch (g.AssetName)
                    {
                        case ("back"):
                            gameState = GameState.MainMenu;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.inGame:
                    DrawInGame(gameTime);
                    break;
                case GameState.Credits:
                    DrawCredits(gameTime);
                    break;
            };
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            DrawModel(fruit_plate);
            _spriteBatch.Begin();
            foreach(GUI g in mainMenu)
            {
                g.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }

        private void DrawInGame(GameTime gameTime)
        {
            player.Draw(projectionMatrix);
            fruitEntity.Draw(gameTime, player.GetView(), projectionMatrix);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(CreditsFont, "Score:", new Vector2(10, 10), Color.Black);
            _spriteBatch.End();

        }

        private void DrawCredits(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach (GUI g in creditsMenu)
            {
                g.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(CreditsFont, "Credits:", new Vector2(100, 50), Color.Black);
            _spriteBatch.DrawString(CreditsFont, "Fruit Models are:", new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(CreditsFont, "\"Low Poly Fruits v2\" (https://skfb.ly/6D6QF) by EdwinRC \n is licensed under Creative Commons Attribution \n (http://creativecommons.org/licenses/by/4.0/).", new Vector2(25, 150), Color.Black);
            _spriteBatch.End();
        }

        protected void DrawModel(Model model)
        {
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
        }
    }
}
