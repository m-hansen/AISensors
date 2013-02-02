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

// TODO add return values
// relative angles
// distance

namespace SampleGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AdjacentAgentSensor : Sensor
    {
        private bool isInRange;                     // if an agent is 
        private Vector2 distance = new Vector2();   // distance between agent and player

        public override void Update(KeyboardState keyboard, List<GameAgent> agentAIList, Vector2 playerPos, float playerRot)
        {
            // the sensor is active if the key is currently being pushed down
            Active = keyboard.IsKeyDown(Key);

            // check if agent is within the radius
            foreach (GameAgent agent in agentAIList)
            {
                // get the X/Y distance between player and agent
                distance.X = playerPos.X - agent.Position.X;
                distance.Y = playerPos.Y - agent.Position.Y;

                // TODO - efficency check?
                // check if an agent is within range
                isInRange = (Math.Pow(distance.X, 2) + Math.Pow(distance.Y, 2) <= Math.Pow(Radius, 2)) ? true : false;
            }
        }

        public override void Draw(SpriteBatch sprites, Vector2 center, SpriteFont font1)
        {
            if (Active)
            {
                DrawingHelper.DrawCircle(new Vector2(center.X, center.Y), Radius, (isInRange ? Color.Red : Color.Green), false);

                //sprites.DrawString(font1, "Range Finder " + DirectionText + ": " + Math.Round(intersectingPoint.X, 2) + ", " + Math.Round(intersectingPoint.Y, 2) + ", " + Math.Round(intersectionDistance, 2), new Vector2(20, 400 + 20 * Index), Color.LightGreen, 0.0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
            }
        }

    }
}
