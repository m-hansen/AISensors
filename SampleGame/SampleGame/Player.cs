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
using Drawing;

namespace SampleGame
{
    public class Player : GameAgent
    {
        public float Speed;  // forward - backward speed
        public List<Sensor> Sensors = new List<Sensor>();

        public Player()
        {
            Initialize();
        }

        public Vector2 CalculateRotatedMovement(Vector2 point, float rotation)
        {
            return Vector2.Transform(point, Matrix.CreateRotationZ(rotation));
        }

        protected virtual void Initialize()
        {
            // ********** CREATING SENSOR LIST FOR ASSIGNMENT ************* //

            Sensors.Add(new Sensor(this)
            {
                Type = (int)Enums.SensorType.RangeFinder,
                Rotation1 = (float)Math.PI / 3,
                Key = Keys.P,
                MaxDistance = 100
            });

            /*sensorList.Add(new Sensor(this)
            {
                Type = (int)Enums.SensorType.RangeFinder,
                Rotation1 = 0,
                Key = Keys.P,
                MaxDistance = 100
            });

            sensorList.Add(new Sensor(this)
            {
                Type = (int)Enums.SensorType.RangeFinder,
                Rotation1 = -1 * (float)Math.PI / 3,
                Key = Keys.P,
                MaxDistance = 100
            });

            sensorList.Add(new Sensor(this)
            {
                Type = (int)Enums.SensorType.AgentSensor,
                Radius = 100,
                Key = Keys.O,
                MaxDistance = 100
            });

            sensorList.Add(new Sensor(this) // - 60 to 60 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Rotation1 = -1 * (float)Math.PI / 3,
                Rotation2 = (float)Math.PI / 3,
                MaxDistance = 100
            });

            sensorList.Add(new Sensor(this) // 60 to 120 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Rotation1 = (float)Math.PI / 3,
                Rotation2 = 2 * (float)Math.PI / 3,
                MaxDistance = 100
            });

            sensorList.Add(new Sensor(this) // 120 to 240 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Rotation1 = 2 * (float)Math.PI / 3,
                Rotation2 = 4 * (float)Math.PI / 3,
                MaxDistance = 100
            });

            sensorList.Add(new Sensor(this) // 240 to 300 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Rotation1 = 4 * (float)Math.PI / 3,
                Rotation2 = 5 * (float)Math.PI / 3,
                MaxDistance = 100
            });*/

            // ********** END CREATING SENSOR LIST FOR ASSIGNMENT ********* //

            //base.Initialize();
        }

        public void Update(GameTime gameTime, KeyboardState keyboardStateCurrent, KeyboardState keyboardStatePrevious, 
            MouseState mouseStateCurrent, MouseState mouseStatePrevious, List<GameAgent> agentAIList,
            int windowWidth, int windowHeight)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // rotation
            if (keyboardStateCurrent.IsKeyDown(Keys.Left) || keyboardStateCurrent.IsKeyDown(Keys.A))
                Rotation -= (elapsedTime * RotationSpeed) % MathHelper.TwoPi;
            if (keyboardStateCurrent.IsKeyDown(Keys.Right) || keyboardStateCurrent.IsKeyDown(Keys.D))
                Rotation += (elapsedTime * RotationSpeed) % MathHelper.TwoPi;

            // movement
            if (keyboardStateCurrent.IsKeyDown(Keys.Up) || keyboardStateCurrent.IsKeyDown(Keys.W))
            {
                Vector2 nextPos = CalculateRotatedMovement(new Vector2(0, -1), Rotation) * Speed + Position;

                if (IsValidMove(nextPos, agentAIList, windowWidth, windowHeight))
                    Position = nextPos;
            }
            if (keyboardStateCurrent.IsKeyDown(Keys.Down) || keyboardStateCurrent.IsKeyDown(Keys.S))
            {
                Vector2 nextPos = CalculateRotatedMovement(new Vector2(0, 1), Rotation) * Speed + Position;

                if (IsValidMove(nextPos, agentAIList, windowWidth, windowHeight))
                    Position = nextPos;
            }

            

            base.Update(gameTime);
        }

        private bool IsValidMove(Vector2 nextPos, List<GameAgent> agentAIList, int windowWidth, int windowHeight)
        {
            Rectangle rect = new Rectangle
            (
                (int)(nextPos.X - Origin.X * Scale),
                (int)(nextPos.Y - Origin.Y * Scale),
                FrameWidth,
                FrameHeight
            );

            bool collision = false;

            foreach (GameAgent agent in agentAIList)
            {
                if (collision = agent.Bounds.Intersects(rect))
                    break;
            }

            return (!collision && rect.Left > 0 && rect.Left + rect.Width < windowWidth && rect.Top > 0 && rect.Top + rect.Height < windowHeight);
        }

        // Render the sprite to the screen
        public virtual void Draw(SpriteBatch sprites, KeyboardState keyboardStateCurrent)
        {
            
        }
    }
}
