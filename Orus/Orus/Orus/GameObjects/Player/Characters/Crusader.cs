﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Orus.Abilities;
using Orus.Constants;
using Orus.Sprites.Animations;

namespace Orus.GameObjects.Player.Characters
{
    public class Crusader : Character
    {
        private DoubleStrike doubleStrike;

        public Crusader(Point2D position, ContentManager Content)
            : this(Constant.CrusaderDefaultName, position, Content)
        {

        }

        public Crusader(string name, Point2D position, ContentManager Content) 
            : base(name, position, 
                  new Rectangle((int)position.X + Constant.CrusaderWidth / 2, (int)position.Y,
                      Constant.CrusaderWidth , Constant.DefaultHeighForEverything), 
                  Constant.CrusaderDefaultMoveSpeed,
                  Constant.CrusaderDefaultHealth, Constant.CrusaderDefaultArmor, Constant.CrusaderDefaultFireResistance,
                  Constant.CrusaderDefaultLightingResistance, Constant.CrusaderDefaultArcaneResistance, Constant.CrusaderDefaultIceResistance,
                  Constant.CrusaderDefaultAttackDamage, Constant.CrusaderAttackRange, 
                  Constant.CrusaderAttackFrame * Constant.TimeForFrameInMilliSeconds * Constant.CrusaderDeathFramesNumber)
        {
            this.IddleAnimation = new FrameAnimation(
                 Content.Load<Texture2D>(Constant.CrusaderIddleAnimationPath),
                 Constant.CrusaderIddleFramesNumber,
                 this);
            this.IddleAnimation.IsActive = true;
            this.MoveAnimation = new FrameAnimation(
                 Content.Load<Texture2D>(Constant.CrusaderMoveAnimationPath),
                 Constant.CrusaderMoveFramesNumber,
                 this);
            this.AttackAnimation = new FrameAnimation(
                 Content.Load<Texture2D>(Constant.CrusaderAttackAnimationPath),
                 Constant.CrusaderAttackFramesNumber,
                 this);
            this.AttackAnimation.IsLoop = false;
            this.DeathAnimation = new FrameAnimation(
                 Content.Load<Texture2D>(Constant.CrusaderDeathAnimationPath),
                 Constant.CrusaderDeathFramesNumber,
                 this);
            this.DeathAnimation.IsLoop = false;
            this.DoubleStrike = new DoubleStrike(this.AttackDamage * 2, 
                Constant.CrusaderDoubleAttackAnimationPath, 
                Constant.CrusaderDoubleAttackFramesNumber);
        }

        public DoubleStrike DoubleStrike
        {
            get
            {
                return this.doubleStrike;
            }
            set
            {
                this.doubleStrike = value;
            }
        }

        public override Point2D Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                if (this.DoubleStrike != null)
                {
                    this.DoubleStrike.Animation.Position = value;
                }
                base.Position = value;
            }
        }

        public override void Animate(GameTime gameTime)
        {
            base.Animate(gameTime);
            this.DoubleStrike.Update(gameTime, this);
        }

        public override void DrawAnimations(SpriteBatch spriteBatch)
        {
            base.DrawAnimations(spriteBatch);
            this.DoubleStrike.Animation.Draw(spriteBatch);
        }

        public override void FlipImages(bool isFlipped)
        {
            base.FlipImages(isFlipped);
            if (this.DoubleStrike != null)
            {
                if (isFlipped)
                {
                    this.DoubleStrike.Animation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    this.DoubleStrike.Animation.SpriteEffect = SpriteEffects.None;
                }
            }
        }

        public override void Die()
        {
            base.Die();
            this.DoubleStrike.Animation.IsActive = false;
        }
    }
}
