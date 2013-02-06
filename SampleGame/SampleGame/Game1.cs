using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Drawing;  // DrawingHelper namespace

namespace SampleGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;                                      
        List<GameAgent> agentAIList = new List<GameAgent>();
        KeyboardState keyboardStateCurrent;
        KeyboardState keyboardStatePrevious;
        MouseState mouseStateCurrent;
        MouseState mouseStatePrevious;
        SpriteFont font1;                                   

        int windowWidth = 0;
        int windowHeight = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;   // height of screen
            graphics.PreferredBackBufferWidth = 800;    // width of screen
            graphics.IsFullScreen = false;           
            Content.RootDirectory = "Content";

            windowWidth = Window.ClientBounds.Width;
            windowHeight = Window.ClientBounds.Height;
        }

        protected override void Initialize()
        {
            // Initialize the input devices
            this.IsMouseVisible = true;
            keyboardStateCurrent = new KeyboardState();
            mouseStateCurrent = new MouseState();

            // Initialize DrawingHelper
            DrawingHelper.Initialize(GraphicsDevice);

            player = new Player();
            player.AnimationInterval = TimeSpan.FromMilliseconds(100);          // next frame every 100 miliseconds
            player.Position = new Vector2(windowWidth / 2, windowHeight / 2);   // setting position to center of screen
            player.RotationSpeed = 5.0f;                                        // rotate somewhat quick
            player.Speed = 4.0f;                                                // setting forward - backward speed
            player.InitializeSensors();                                         // initializes all sensors for the player object

            // ************ CREATING THE WALLS FOR THE ASSIGNMENT ********* //
            int defaultWalls = 2;

            for (int i = 0; i < defaultWalls; i++)
            {
                agentAIList.Add(new GameAgent()
                {
                    Type = (int)Enums.AgentType.Wall
                });
            }

            // ********** END CREATING THE WALLS FOR THE ASSIGNMENT ******* //

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // loading the player's image
            player.LoadContent(this.Content, "Images\\ship1", new Rectangle(0, 0, 38, 41), 8);

            // loading the font to display text on the screen
            font1 = Content.Load<SpriteFont>("fonts/Font1");

            // ************ LOADING THE WALLS FOR THE ASSIGNMENT ********* //

            Random rnd = new Random();

            for (int i = 0; i < agentAIList.Count; i++)
            {
                int randNumb = rnd.Next(1);
                agentAIList[i].LoadContent(this.Content, randNumb > 0 ? "Images\\wall" : "Images\\wall1", null, 1, randNumb > 0);
                agentAIList[i].Scale = (float)rnd.Next(100) / 50 + 1;
                agentAIList[i].Position = new Vector2(rnd.Next(windowWidth), rnd.Next(windowHeight));

                int targetIndex = -1;

                // making sure the walls aren't out of the zone, intersecting the player, or intersecting other walls
                while (targetIndex < i)
                {
                    Rectangle r = agentAIList[i].Bounds;
                    if (targetIndex < 0 && (r.Left < 0 || r.Top < 0 || r.Left + r.Width > windowWidth || r.Top + r.Height > windowHeight || r.Intersects(player.Bounds)))
                    {
                        agentAIList[i].Position = new Vector2(rnd.Next(windowWidth), rnd.Next(windowHeight));
                    }
                    else if (targetIndex >= 0 && agentAIList[i].Bounds.Intersects(agentAIList[targetIndex].Bounds))
                    {
                        agentAIList[i].Position = new Vector2(rnd.Next(windowWidth), rnd.Next(windowHeight));

                        targetIndex = 0;
                    }
                    else
                    {
                        targetIndex++;
                    }
                }
            }

            // ********* END LOADING THE WALLS FOR THE ASSIGNMENT ******** //
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if ((keyboardStateCurrent.IsKeyUp(Keys.Escape) && keyboardStatePrevious.IsKeyDown(Keys.Escape)) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update keyboard state
            keyboardStatePrevious = keyboardStateCurrent;
            keyboardStateCurrent = Keyboard.GetState();

            // Update mouse state
            mouseStatePrevious = mouseStateCurrent;
            mouseStateCurrent = Mouse.GetState();

            player.Update(gameTime, keyboardStateCurrent, keyboardStatePrevious, mouseStateCurrent, mouseStatePrevious, agentAIList, windowWidth, windowHeight);

            // Create new agent on mouse click
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton != ButtonState.Pressed)
            {
                // Create a new agent at mouse location
                GameAgent agent = new GameAgent();
                agent.LoadContent(this.Content, "Images\\agent");
                agent.Position = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                agent.Rotation = 0.0f;
                agentAIList.Add(agent);
                agent.Type = (int)Enums.AgentType.NPC;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);       // clears background to selected color

            spriteBatch.Begin();                        // begin drawing sprites

            // Draw each agent
            foreach (GameAgent agent in agentAIList)
            {
                agent.Draw(this.spriteBatch, font1);
            }

            // *********************** DRAWING TEXT ON THE SCREEN FOR ASSIGNMENT ******************** //

            Vector2 playerHeading = player.CalculateRotatedMovement(new Vector2(1, 0), player.Rotation);

            spriteBatch.DrawString(font1, "Player Pos: " + player.Position.X + ", " + player.Position.Y, new Vector2(20, 20), Color.LightGreen, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font1, "Player Heading: " + playerHeading.X + ", " + playerHeading.Y, new Vector2(20, 40), Color.DarkKhaki, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

            spriteBatch.DrawString(font1, "Sensor Keys", new Vector2(700, 500), Color.Red, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font1, "Rangefinders: P", new Vector2(680, 520), Color.Red, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font1, "Agent Sensors: O", new Vector2(670, 540), Color.Red, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font1, "Pie-Slice Sensors: I", new Vector2(655, 560), Color.Red, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

            // *********************** END DRAWING TEXT ON THE SCREEN FOR ASSIGNMENT ***************** //

            player.Draw(this.spriteBatch, font1);   // draws the player object on the screen


            // *********************** DRAWING SENSORS ON THE SCREEN FOR ASSIGNMENT ****************** //

            //TODO delete
            // Draw Adjacent Agent Sensors
            //DrawingHelper.DrawCircle(new Vector2(player.Position.X, player.Position.Y), 141, Color.Green, false);

            // *********************** END DRAWING SENSORS ON THE SCREEN FOR ASSIGNMENT ************** //

            spriteBatch.End();                          // stop drawing sprites

            base.Draw(gameTime);
        }
    }
}