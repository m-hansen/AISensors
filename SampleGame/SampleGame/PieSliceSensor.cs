using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Drawing;

// TODO - return values
// activation levels

// should an agent be able to trigger two portions at the same time?
// would it be better to check for outer bounds of sensor by pixel and inner bounds by origin to prevent this?

namespace SampleGame
{
    public class PieSliceSensor : Sensor
    {
        public int MaxDistance;
        public float Rotation1;
        public float Rotation2;
        public string DisplayText;
        public int Index;
        private bool isTriggered;
        private Vector2 endPoint1;
        private Vector2 endPoint2;
        
        public override void Update(KeyboardState keyboard, List<GameAgent> agentAIList, Vector2 playerPos, float playerRot)
        {
            // reinitializing the sensor to not triggered (no agent withing proximity)
            isTriggered = false;

            // the sensor is active if the key is currently being pushed down
            Active = keyboard.IsKeyDown(Key);

            // end point of the one side of the pie slice (beginning point is the player position)
            endPoint1 = CalculateRotatedMovement(new Vector2(0, -1), playerRot + Rotation1) * MaxDistance + playerPos;

            // end point of the other side of the pie slice (beginning point is the player position)
            endPoint2 = CalculateRotatedMovement(new Vector2(0, -1), playerRot + Rotation2) * MaxDistance + playerPos;

            // pie slice sensors only work for npcs
            List<GameAgent> npcs = agentAIList.Where(a => a.Type == (int)Enums.AgentType.NPC).ToList();

            foreach (GameAgent agent in npcs)
            {
                // if the agent is within the pie slice sensor
                if (isTriggered = IsInAgentSensorRange(agent, playerPos, endPoint1, endPoint2))
                    break;
            }
        }

        private bool IsInAgentSensorRange(GameAgent agent, Vector2 playerPos, Vector2 endPoint1, Vector2 endPoint2)
        {
            Rectangle agentBounds = agent.Bounds;

            // getting the 4 points of the agent
            Vector2 bottomLeft = new Vector2(agentBounds.Left, agentBounds.Top);
            Vector2 bottomRight = new Vector2(agentBounds.Left + agentBounds.Width, agentBounds.Top);
            Vector2 topLeft = new Vector2(agentBounds.Left, agentBounds.Top + agentBounds.Height);
            Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);

            return
                IsPointInTriangle(bottomLeft, playerPos, endPoint1, endPoint2) ||
                IsPointInTriangle(bottomRight, playerPos, endPoint1, endPoint2) ||
                IsPointInTriangle(topLeft, playerPos, endPoint1, endPoint2) ||
                IsPointInTriangle(topRight, playerPos, endPoint1, endPoint2);
        }

        private bool IsPointInTriangle(Vector2 targetPoint, Vector2 a, Vector2 b, Vector2 c)
        {
            // the pie slice sensor creates a triangle, if an agent is within that triangle
            // then the sensor is triggered. 
            // NOTE: Since cross products only work for 3D vectors, converting all the vectors to 3D
            Vector3 _targetPoint = new Vector3(targetPoint, 0);
            Vector3 _a = new Vector3(a, 0);
            Vector3 _b = new Vector3(b, 0);
            Vector3 _c = new Vector3(c, 0);

            return (IsSameSide(_targetPoint, _a, _b, _c) &&
                IsSameSide(_targetPoint, _b, _a, _c) &&
                IsSameSide(_targetPoint, _c, _a, _b));
        }

        private bool IsSameSide(Vector3 point1, Vector3 point2, Vector3 a, Vector3 b)
        {
            Vector3 p1 = Vector3.Cross(b - a, point1 - a);
            Vector3 p2 = Vector3.Cross(b - a, point2 - a);

            return Vector3.Dot(p1, p2) >= 0;
        }

        public override void Draw(SpriteBatch sprites, Vector2 startPoint, SpriteFont font1)
        {
            if (Active)
            {
                if (isTriggered)
                {
                    DrawingHelper.DrawFastLine(startPoint, endPoint1, Color.Yellow);
                    DrawingHelper.DrawFastLine(startPoint, endPoint2, Color.Yellow);
                    DrawingHelper.DrawFastLine(endPoint1, endPoint2, Color.Red);

                    sprites.DrawString(font1, "Pie-Slice Sensor " + DisplayText + ": Triggered", new Vector2(20, 460 + 20 * Index), Color.LightGreen, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
                else
                {
                    DrawingHelper.DrawFastLine(startPoint, endPoint1, Color.Yellow);
                    DrawingHelper.DrawFastLine(startPoint, endPoint2, Color.Yellow);
                }
            }
        }
    }
}
