using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
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
        List<GUI> winLoseMenu = new List<GUI>();
        SpriteFont CreditsFont;
        double gameTimeRemaining = 61;
        int amountOfFruit = 30;

        Model fruit_plate;
        Model terrain;
        Model plate;


        Model pineapple;
        Model apple;
        Model pear;
        Model banana;
        Model melon;
        Model cherry;

        List<Model> fruitModels = new List<Model>();

        Random random = new Random();

        List<FruitEntity> fruitList;
  
        enum GameState
        {
            MainMenu,
            inGame,
            Credits,
            YouWin,
            YouLose
        }

        GameState gameState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            player = new Player(Vector3.Zero);
            fruitList = new List <FruitEntity>();


            mainMenu.Add(new GUI("play"));
            mainMenu.Add(new GUI("credits"));

            creditsMenu.Add(new GUI("back"));

            winLoseMenu.Add(new GUI("play"));
            winLoseMenu.Add(new GUI("credits"));
            winLoseMenu.Add(new GUI("back"));
        }

        protected override void Initialize()
        {
            //CameraSetup
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -50f);

            worldMatrix = Matrix.CreateTranslation(new Vector3(0f, 0f, 0f));
            viewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, 10f), new Vector3(0f, 0f, 0f), Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            base.Initialize();
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
            creditsMenu.Find(x => x.AssetName == "back").MoveElement(-85,-75);

            winLoseMenu.Find(x => x.AssetName == "play").LoadContent(Content.Load<Texture2D>("GUI\\Play"));
            winLoseMenu.Find(x => x.AssetName == "credits").LoadContent(Content.Load<Texture2D>("GUI\\Credits"));
            winLoseMenu.Find(x => x.AssetName == "back").LoadContent(Content.Load<Texture2D>("GUI\\Back"));
            foreach(GUI g in winLoseMenu)
            {
                g.CenterElement(600, 800);
            }
            winLoseMenu.Find(x => x.AssetName == "play").MoveElement(-110, 0);
            winLoseMenu.Find(x => x.AssetName == "credits").MoveElement(100, 0);
            winLoseMenu.Find(x => x.AssetName == "back").MoveElement(310, 0);


            player.LoadContent(Content);

            fruit_plate = Content.Load<Model>("Fruits\\FruchtTeller");
            plate = Content.Load<Model>("Player\\teller");

            pineapple = Content.Load<Model>("Fruits\\ananas");
            apple = Content.Load<Model>("Fruits\\apfel");
            pear = Content.Load<Model>("Fruits\\banane");
            banana = Content.Load<Model>("Fruits\\birne");
            melon = Content.Load<Model>("Fruits\\melone");
            cherry = Content.Load<Model>("Fruits\\kirsche");

            fruitModels.Add(pineapple);
            fruitModels.Add(apple);
            fruitModels.Add(pear);
            fruitModels.Add(banana);
            fruitModels.Add(melon);
            fruitModels.Add(cherry);

            CreditsFont = Content.Load<SpriteFont>("CreditsFont");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

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
                case GameState.YouLose:
                    UpdateYouLose(gameTime);
                    break;
                case GameState.YouWin:
                    UpdateYouWin(gameTime);
                    break;
            };
        }



        private void UpdateMainMenu(GameTime gameTime)
        {
            worldMatrix = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds / 2);
            foreach (GUI g in mainMenu)
            {
                if (g.Update(gameTime))
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
            List<FruitEntity> collected = new List<FruitEntity>();

            if (fruitList.Count < 5)
            {
                int randomFruit = random.Next(fruitModels.Count);
                fruitList.Add(new FruitEntity(fruitModels[randomFruit]));
            }
            foreach(FruitEntity fruit in fruitList)
            {
                fruit.Update(gameTime);
                if (fruit.CollisionCheck(player, fruit))
                {
                    collected.Add(fruit);
                }
            }
            fruitList = fruitList.Except(collected).ToList();
            player.Update(gameTime);

            gameTimeRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateCredits(GameTime gameTime)
        {
            foreach (GUI g in creditsMenu)
            {
                if (g.Update(gameTime))
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
        private void UpdateYouLose(GameTime gameTime)
        {
            foreach (GUI g in winLoseMenu)
            {
                if (g.Update(gameTime))
                {
                    switch (g.AssetName)
                    {
                        case ("back"):
                            gameState = GameState.MainMenu;
                            break;
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

        private void UpdateYouWin(GameTime gameTime)
        {
            player.score = 0;
            gameTimeRemaining = 61;
            foreach (GUI g in winLoseMenu)
            {
                if (g.Update(gameTime))
                {
                    switch (g.AssetName)
                    {
                        case ("back"):
                            gameState = GameState.MainMenu;
                            break;
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
                case GameState.YouLose:
                    DrawYouLose(gameTime);
                    break;
                case GameState.YouWin:
                    DrawYouWin(gameTime);
                    break;
            };
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            DrawModel(fruit_plate);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(CreditsFont, "Falling Fruits", new Vector2(303, 13), Color.Gray);
            _spriteBatch.DrawString(CreditsFont, "Falling Fruits", new Vector2(300, 10), Color.Black);
            foreach (GUI g in mainMenu)
            {
                g.Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }

        private void DrawInGame(GameTime gameTime)
        {           
            player.Draw(projectionMatrix);
            foreach(FruitEntity fruit in fruitList)
            {
                fruit.Draw(fruitList,projectionMatrix, player.GetView());
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(CreditsFont, "Score: " + player.score + " of " + amountOfFruit, new Vector2(10, 10), Color.Black);
            _spriteBatch.DrawString(CreditsFont, "Score: " + player.score + " of " + amountOfFruit, new Vector2(11, 11), Color.White);
            _spriteBatch.DrawString(CreditsFont, "Verbleibene Zeit: " + (int) gameTimeRemaining, new Vector2(300, 10), Color.Black);
            if(gameTimeRemaining < 10)
            {
                _spriteBatch.DrawString(CreditsFont, "Verbleibene Zeit: " + (int)gameTimeRemaining, new Vector2(301, 11), Color.Red);
            }
            else {
                _spriteBatch.DrawString(CreditsFont, "Verbleibene Zeit: " + (int)gameTimeRemaining, new Vector2(301, 11), Color.White);
            }
            _spriteBatch.End();

            if(gameTimeRemaining <= 0 && !(player.score >= amountOfFruit)) 
            {
                gameState = GameState.YouLose;
            }
            if(player.score >= amountOfFruit)
            {
                gameState = GameState.YouWin;
            }
            DrawModelPlayerView(Content.Load<Model>("Player\\Boden"));
        }

        private void DrawCredits(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach(GUI g in creditsMenu)
            {
                g.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(CreditsFont, "Credits:", new Vector2(100, 50), Color.Black);
            _spriteBatch.DrawString(CreditsFont, "Fruit Models are:", new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(CreditsFont, "\"Low Poly Fruits v2\" (https://skfb.ly/6D6QF) by EdwinRC \n is licensed under Creative Commons Attribution \n (http://creativecommons.org/licenses/by/4.0/).", new Vector2(25, 150), Color.Black);
            _spriteBatch.End();
        }
        private void DrawYouLose(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach(GUI g in winLoseMenu)
            {
                g.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(CreditsFont, "Du hast verloren.", new Vector2(300, 300), Color.Black);
            _spriteBatch.End();

            DrawModel(plate);
        }

        private void DrawYouWin(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach (GUI g in winLoseMenu)
            {
                g.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(CreditsFont, "Du hast gewonnen!", new Vector2(280, 20), Color.Black);
            _spriteBatch.DrawString(CreditsFont, "Du hast gewonnen!", new Vector2(300, 40), Color.Red);
            _spriteBatch.DrawString(CreditsFont, "Du hast gewonnen!", new Vector2(320, 60), Color.Blue);
            _spriteBatch.DrawString(CreditsFont, "Du hast gewonnen!", new Vector2(340, 80), Color.Green);
            _spriteBatch.End();

            DrawModel(fruit_plate);
        }

        protected void DrawModel(Model model)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
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

        protected void DrawModelPlayerView(Model model)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.View = player.GetView();
                    effect.World = worldMatrix * Matrix.CreateScale(new Vector3(20,1,20)) * Matrix.CreateRotationY(-0.37f);
                    effect.Projection = projectionMatrix * Matrix.CreateTranslation(Vector3.Up * 0.25f);
                    effect.EnableDefaultLighting();
                }
                modelMesh.Draw();
            }
        }
    }
}
