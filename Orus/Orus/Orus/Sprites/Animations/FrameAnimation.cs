﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orus.Constants;
using Orus.GameObjects;
using Orus.GameObjects.Enemies;
using Orus.GameObjects.Player.Characters;

namespace Orus.Sprites.Animations
{
    public class FrameAnimation : Sprite
    {
        private float time = 0f;

        public float Time { get { return time; } set { time = value; } }

        public FrameAnimation(Texture2D Texture, int frames, AnimatedGameObject animatedGameObject)
            : base(Texture, frames, animatedGameObject)
        {

        }

        public FrameAnimation(Texture2D Texture, int frames)
            : base(Texture, frames)
        {

        }

        public void Animate(GameTime gameTime)
        {
            this.Time += gameTime.ElapsedGameTime.Milliseconds;
            if (this.Time > Constant.TimeForFrameInMilliSeconds * this.Rectangles.Length)
            {
                time = 0f;
                this.FrameIndex++;
                if(this.FrameIndex == this.Rectangles.Length)
                {
                    this.FrameIndex = 0;
                }
            }
        }

        public void Animate(GameTime gameTime, AnimatedGameObject animatedObject)
        {
            if(!this.IsActive)
            {
                return;
            }
            this.Time += gameTime.ElapsedGameTime.Milliseconds;
            if (this.Time > (Constant.TimeForFrameInMilliSeconds * this.Rectangles.Length) / animatedObject.AnimationSpeed)
            {
                this.Time = 0f;
                this.FrameIndex++;
                if (this.FrameIndex == this.Rectangles.Length)
                {
                    this.FrameIndex = 0;
                    if(!this.IsLoop)
                    {
                        this.IsActive = false;
                        if ((animatedObject as AttackableGameObject) != null)
                        {
                            if ((animatedObject as AttackableGameObject).Health > 0)
                            {
                                animatedObject.IddleAnimation.IsActive = true;
                            }
                        }
                    }
                }
            }
        }
    }
}