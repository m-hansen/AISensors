using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Drawing;

namespace SampleGame
{
    class Sensor
    {
        public int Type;            // which type of sensor (see Enums class)
        public bool IsVisible;      // whether the sensor is visible
        public Vector2 Direction1;  // used for Rangefinders & Pie-Slice
        public float Rotation1;     // used for Rangefinders & Pie-Slice
        public float Rotation2;     // used for Pie-Slice
        public float Radius;        // used for Agent Sensors
        public Keys Key;            // key to be pushed to activate sensor
        public int MaxDistance;     // how far the sensor will go

        private Player player;

        public Sensor(Player p)
        {
            player = p;
        }

        public void Update(KeyboardState keyboard, List<GameAgent> agentAIList)
        {
            // TODO check collision with wall if rangefiner
            



            if (IsVisible = keyboard.IsKeyDown(Key))
            {
                switch (Type)
                {
                    case (int)Enums.SensorType.RangeFinder:


                        break;

                    case (int)Enums.SensorType.AgentSensor:


                        break;

                    case (int)Enums.SensorType.PieSliceSensor:

                        break;
                }
            }
        }

        // Draw the sensors
        public virtual void Draw(KeyboardState keyboard/*SpriteBatch sprites*/)
        {
            //int rfInterval = 15;          // size of the angle between each sensor
            //int rfAngle = 30;             // the angle between the far sensors
            //int rfLength = -35;           // length of each sensor
            DrawingHelper.Begin(PrimitiveType.LineList);
            /*for (int i = 0; i <= player.Rotation; i += rfInterval) // this is the "range" of the sensors
            {
                DrawingHelper.DrawLine(
                    new Vector2(player.Position.X, player.Position.Y),
                    player.CalculateRotatedMovement(new Vector2(i - rfInterval, rfLength), player.Rotation) * player.Speed + player.Position,
                    Color.Red);
            }*/

            if (IsVisible = keyboard.IsKeyDown(Key))
            {
                switch (Type)
                {
                    case (int)Enums.SensorType.RangeFinder:
                        DrawingHelper.DrawLine(
                            new Vector2(player.Position.X, player.Position.Y),
                            player.CalculateRotatedMovement(new Vector2(360, MaxDistance), player.Rotation) * player.Speed + player.Position,
                            Color.Red);
                        break;

                    case (int)Enums.SensorType.AgentSensor:


                        break;

                    case (int)Enums.SensorType.PieSliceSensor:

                        break;
                }
            }
            
            DrawingHelper.DrawLine(new Vector2(0, 0), new Vector2(100, 100), Color.White);

            /*DrawingHelper.DrawLine(
                   new Vector2(player.Position.X, player.Position.Y),
                   player.CalculateRotatedMovement(new Vector2(i - rfInterval, MaxDistance), player.Rotation) * player.Speed + player.Position,
                   Color.Red);

            DrawingHelper.DrawLine(
                   new Vector2(player.Position.X, player.Position.Y),
                   player.CalculateRotatedMovement(new Vector2(i - rfInterval, rfLength), player.Rotation) * player.Speed + player.Position,
                   Color.Red);*/
            DrawingHelper.End();
        }
    }
}
