using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SampleGame
{
    public class Player : GameAgent
    {
        public float Speed;  // forward - backward speed

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
    }
}
