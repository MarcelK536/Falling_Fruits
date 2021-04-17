using Microsoft.Xna.Framework;
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

        List<GUI> mainMenu = new List<GUI>();

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

            mainMenu.Find(x => x.AssetName == "play").LoadContent(Content.Load<Texture2D>("GUI\\Play"));
            mainMenu.Find(x => x.AssetName == "credits").LoadContent(Content.Load<Texture2D>("GUI\\Credits"));

            foreach (GUI g in mainMenu)
            {
                g.CenterElement(600, 800);
            }
            mainMenu.Find(x => x.AssetName == "credits").MoveElement(150, 0);

            player.LoadContent(Content);

            banana = Content.Load<Model>("Fruits\\banane");
            apple = Content.Load<Model>("Fruits\\apfel");
            pear = Content.Load<Model>("Fruits\\birne");
            melon = Content.Load<Model>("Fruits\\melone");
            plate = Content.Load<Model>("Player\\teller");
            terrain = Content.Load<Model>("Player\\boden");
            fruit_plate = Content.Load<Model>("Fruits\\FruchtTeller");
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
        }
        private void UpdateCredits(GameTime gameTime)
        {

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
            DrawModel(melon);
            player.Draw(projectionMatrix);
        }

        private void DrawCredits(GameTime gameTime)
        {
            DrawModel(pear);
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
