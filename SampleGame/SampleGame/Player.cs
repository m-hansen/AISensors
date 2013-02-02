using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Drawing;

namespace SampleGame
{
    public class Player : GameAgent
    {
        public float Speed;  // forward - backward speed
        private List<Sensor> sensorList = new List<Sensor>();

        public void InitializeSensors()
        {
            sensorList.Add(new RangeFinder()
            {
                Type = (int)Enums.SensorType.RangeFinder,
                Rotation = (float)Math.PI / 6,
                Key = Keys.P,
                MaxDistance = 150,
                Index = 0,
                DirectionText = "Right"
            });

            sensorList.Add(new RangeFinder()
            {
                Type = (int)Enums.SensorType.RangeFinder,
                Rotation = 0,
                Key = Keys.P,
                MaxDistance = 150,
                Index = 1,
                DirectionText = "Middle"
            });

            sensorList.Add(new RangeFinder()
            {
                Type = (int)Enums.SensorType.RangeFinder,
                Rotation = -1 * (float)Math.PI / 6,
                Key = Keys.P,
                MaxDistance = 150,
                Index = 2,
                DirectionText = "Left"
            });

            sensorList.Add(new AdjacentAgentSensor()
            {
                Type = (int)Enums.SensorType.AgentSensor,
                Radius = 150,
                Key = Keys.O,
                //MaxDistance = 100 // TODO - del
            });

            sensorList.Add(new PieSliceSensor() // - 60 to 60 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Key = Keys.I,
                Rotation1 = -1 * (float)Math.PI / 3,
                Rotation2 = (float)Math.PI / 3,
                MaxDistance = 150,
                DisplayText = "(1,0) - Straight Ahead",
                Index = 0
            });

            sensorList.Add(new PieSliceSensor() // 60 to 120 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Key = Keys.I,
                Rotation1 = (float)Math.PI / 3,
                Rotation2 = 2 * (float)Math.PI / 3,
                MaxDistance = 150,
                DisplayText = "(0,1) - Right",
                Index = 1
            });

            sensorList.Add(new PieSliceSensor() // 120 to 240 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Key = Keys.I,
                Rotation1 = 2 * (float)Math.PI / 3,
                Rotation2 = 4 * (float)Math.PI / 3,
                MaxDistance = 150,
                DisplayText = "(-1,0) - Backwards",
                Index = 2
            });

            sensorList.Add(new PieSliceSensor() // 240 to 300 degrees
            {
                Type = (int)Enums.SensorType.PieSliceSensor,
                Key = Keys.I,
                Rotation1 = 4 * (float)Math.PI / 3,
                Rotation2 = 5 * (float)Math.PI / 3,
                MaxDistance = 150,
                DisplayText = "(0,-1) - Left",
                Index = 3
            });


            //TODO - delete
            //sensorList.Add(new Sensor() // - 60 to 60 degrees
            //{
            //    Type = (int)Enums.SensorType.PieSliceSensor,
            //    Rotation1 = -1 * (float)Math.PI / 3,
            //    Rotation2 = (float)Math.PI / 3,
            //    //MaxDistance = 100
            //});

            //sensorList.Add(new Sensor() // 60 to 120 degrees
            //{
            //    Type = (int)Enums.SensorType.PieSliceSensor,
            //    Rotation1 = (float)Math.PI / 3,
            //    Rotation2 = 2 * (float)Math.PI / 3,
            //    //MaxDistance = 100
            //});

            //sensorList.Add(new Sensor() // 120 to 240 degrees
            //{
            //    Type = (int)Enums.SensorType.PieSliceSensor,
            //    Rotation1 = 2 * (float)Math.PI / 3,
            //    Rotation2 = 4 * (float)Math.PI / 3,
            //    //MaxDistance = 100
            //});

            //sensorList.Add(new Sensor() // 240 to 300 degrees
            //{
            //    Type = (int)Enums.SensorType.PieSliceSensor,
            //    Rotation1 = 4 * (float)Math.PI / 3,
            //    Rotation2 = 5 * (float)Math.PI / 3,
            //    //MaxDistance = 100
            //});
        }

        public Vector2 CalculateRotatedMovement(Vector2 point, float rotation)
        {
            return Vector2.Transform(point, Matrix.CreateRotationZ(rotation));
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

            foreach (Sensor sensor in sensorList)
            {
                sensor.Update(keyboardStateCurrent, agentAIList, this.Position, this.Rotation);
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

        public override void Draw(SpriteBatch sprites, SpriteFont font1)
        {
            //DrawingHelper.Begin(PrimitiveType.LineList);

            foreach (Sensor sensor in sensorList)
            {
                sensor.Draw(sprites, this.Position, font1);
            }

            //DrawingHelper.End();

            base.Draw(sprites, font1);
        }
    }
}
