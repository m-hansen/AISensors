using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SampleGame
{
    class Sensor
    {
        public int Type;            // which type of sensor (see Enums class)
        public bool Active;         // whether the sensor is active
        public Vector2 Direction1;  // used for Rangefinders & Pie-Slice
        public float Rotation1;     // used for Rangefinders & Pie-Slice
        public float Rotation2;     // used for Pie-Slice
        public float Radius;        // used for Agent Sensors
        public Keys Key;            // key to be pushed to activate sensor
        public int MaxDistance;     // how far the sensor will go

        public void Update(KeyboardState keyboard, List<GameAgent> agentAIList)
        {
            if (Active = keyboard.IsKeyDown(Key))
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
    }
}
