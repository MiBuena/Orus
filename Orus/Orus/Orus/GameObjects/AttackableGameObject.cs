﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orus.Constants;
using Orus.Interfaces;
using Orus.GameObjects.Sprites;
using Orus.GameObjects.Sprites.Animations;
using System.Collections.Generic;
using Orus.Abilities;
using Orus.Core;

namespace Orus.GameObjects
{
    public class AttackingGameObject : AnimatedGameObject, IMove, ILife, IAttack, IDeath
    {
        private int maxHealth;
        private int health;
        private Sprite healthBar;
        private int armor;
        private double fireResistance;
        private double lightingResistance;
        private double arcaneResistance;
        private double iceResistance;
        private float moveSpeed;
        private FrameAnimation moveAnimation;
        private FrameAnimation attackAnimation;
        private FrameAnimation deathAnimation;
        private int attackDamage;
        private int attackRange;
        private float attackSpeed;
        private float timeUntilDamageSinceAttack;
        private bool isAttacking = false;
        private bool isUsingAbility = false;
        private float timeAttacking = 0.0f;
        private AttackingGameObject objectAttacked;
        private Ability abilityInUse;

        protected AttackingGameObject()
        {

        }

        protected AttackingGameObject(string name, Point2D position, Rectangle2D boundingBox, float moveSpeed,
             int health, int armor, double fireResistance, double lightingResistance, 
             double arcaneResistance, double iceResistance,
             int attackDamage, int attackRange, float attackSpeed, float timeUntilDamageSinceAttack)
             : base(name, position, boundingBox)
        {
            this.AttackDamage = attackDamage;
            this.AttackRange = attackRange;
            this.AttackSpeed = attackSpeed;
            this.TimeUntilDamageSinceAttack = timeUntilDamageSinceAttack;
            this.MaxHealth = health;
            this.Health = health;
            this.Armor = armor;
            this.FireResistance = fireResistance;
            this.LightingResistance = lightingResistance;
            this.ArcaneResistance = arcaneResistance;
            this.IceResistance = iceResistance;
            this.Bar = new Sprite("Sprites\\Bars\\Bar", this.Position);
            this.Bar.IsActive = true;
            this.MoveSpeed = moveSpeed;
        }

        public int Armor
        {
            get
            {
                return this.armor;
            }
            set
            {
                this.armor = value;
            }
        }

        public double ArmorAsPercentage
        {
            get
            {
                return (float)((this.Armor) * Constant.ConstantForArmorFormula) /
                            (1 + Constant.ConstantForArmorFormula * (this.Armor));
            }
        }

        public double FireResistance
        {
            get
            {
                return this.fireResistance;
            }
            set
            {
                this.fireResistance = value;
            }
        }

        public double LightingResistance
        {
            get
            {
                return this.lightingResistance;
            }
            set
            {
                this.lightingResistance = value;
            }
        }

        public double ArcaneResistance
        {
            get
            {
                return this.arcaneResistance;
            }
            set
            {
                this.arcaneResistance = value;
            }
        }

        public double IceResistance
        {
            get
            {
                return this.iceResistance;
            }
            set
            {
                this.iceResistance = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return this.maxHealth;
            }
            set
            {
                this.maxHealth = value;
            }
        }

        public int Health
        {
            get
            {
                return this.health;
            }
            set
            {
                if (value <= 0 && this.IddleAnimation != null)
                {
                    this.health = 0;
                    this.Die();
                }
                else
                {
                    this.health = value;
                }
            }
        }

        public Sprite Bar
        {
            get
            {
                return this.healthBar;
            }
            set
            {
                this.healthBar = value;
            }
        }

        public float MoveSpeed
        {
            get
            {
                return this.moveSpeed;
            }

            set
            {
                this.moveSpeed = value;
            }
        }

        public FrameAnimation MoveAnimation
        {
            get
            {
                return this.moveAnimation;
            }
            set
            {
                this.moveAnimation = value;
            }
        }

        public FrameAnimation AttackAnimation
        {
            get
            {
                return this.attackAnimation;
            }
            set
            {
                this.attackAnimation = value;
            }
        }

        public FrameAnimation DeathAnimation
        {
            get
            {
                return this.deathAnimation;
            }
            set
            {
                this.deathAnimation = value;
            }
        }

        public int AttackDamage
        {
            get
            {
                return this.attackDamage;
            }
            set
            {
                this.attackDamage = value;
            }
        }

        public int AttackRange
        {
            get
            {
                return this.attackRange;
            }
            set
            {
                this.attackRange = value;
            }
        }

        public float AttackSpeed
        {
            get
            {
                return this.attackSpeed;
            }
            set
            {
                this.attackSpeed = value;
            }
        }

        public float TimeUntilDamageSinceAttack
        {
            get
            {
                return this.timeUntilDamageSinceAttack;
            }
            set
            {
                this.timeUntilDamageSinceAttack = value;
            }
        }
        
        public bool IsAttacking
        {
            get
            {
                return this.isAttacking;
            }
            set
            {
                this.isAttacking = value;
            }
        }

        public bool IsUsingAbility
        {
            get
            {
                return this.isUsingAbility;
            }
            set
            {
                this.isUsingAbility = value;
            }
        }

        public float TimeAttacking
        {
            get
            {
                return this.timeAttacking;
            }
            set
            {
                this.timeAttacking = value;
            }
        }

        public AttackingGameObject ObjectAttacked
        {
            get
            {
                return this.objectAttacked;
            }
            set
            {
                this.objectAttacked = value;
            }
        }

        public Ability AbilityInUse
        {
            get
            {
                return this.abilityInUse;
            }
            set
            {
                this.abilityInUse = value;
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
                if (this.MoveAnimation != null)
                {
                    this.MoveAnimation.Position = value;
                }
                if (this.AttackAnimation != null)
                {
                    this.AttackAnimation.Position = value;
                }
                if (this.DeathAnimation != null)
                {
                    this.DeathAnimation.Position = value;
                }
                if (this.Bar != null)
                {
                    this.Bar.Position = value;
                }
                base.Position = value;
            }
        }

        public void Attack(List<AttackingGameObject> gameObjects)
        {
            this.AttackAnimation.IsActive = true;
            this.MoveAnimation.IsActive = false;
            this.IddleAnimation.IsActive = false;
            foreach (var gameObject in gameObjects)
            {
                if (this.CollidesForAttack(gameObject, !this.MoveAnimation.SpriteEffect.HasFlag(SpriteEffects.FlipHorizontally), this.AttackRange))
                {
                    this.ObjectAttacked = gameObject;
                    this.IsAttacking = true;
                    return;
                }
            }
        }

        public bool CollidesForAttack(ICollideable collider, bool isMovingRight, int additionalXOffset = 0)
        {
            if ((collider as AttackingGameObject) != null)
            {
                if ((collider as AttackingGameObject).Health == 0)
                {
                    return false;
                }
            }
            if (this == collider || !this.Collides(collider, additionalXOffset))
            {
                return false;
            }
            if (isMovingRight)
            {
                return this.Position.X < collider.Position.X;
            }
            else
            {
                return this.Position.X > collider.Position.X;
            }
        }

        public void Move(int distance, bool directionIsRight, bool collides)
        {
            this.MoveAnimation.IsActive = true;
            this.IddleAnimation.IsActive = false;
            MoveTo(distance, directionIsRight, collides);
        }

        public void MoveTo(int distance, bool directionIsRight, bool collides, bool flipImagesIfNecessary = true)
        {
            if (directionIsRight)
            {
                if (flipImagesIfNecessary)
                {
                    FlipImages(false);
                }
                if (!collides)
                {
                    this.Position = new Point2D(this.Position.X + ((distance) / Constant.Velocity) * this.MoveSpeed, this.Position.Y);
                }
            }
            else
            {
                if (flipImagesIfNecessary)
                {
                    FlipImages(true);
                }
                if (!collides)
                {
                    this.Position = new Point2D(this.Position.X - ((distance) / Constant.Velocity) * this.MoveSpeed, this.Position.Y);
                }
            }
        }

        public void StopMovement()
        {
            this.MoveAnimation.IsActive = false;
            this.IddleAnimation.IsActive = true;
        }

        public override void FlipImages(bool isFlipped)
        {
            base.FlipImages(isFlipped);
            if (this.MoveAnimation != null)
            {
                if (isFlipped)
                {
                    this.MoveAnimation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    this.MoveAnimation.SpriteEffect = SpriteEffects.None;
                }
            }
            if (this.AttackAnimation != null)
            {
                if (isFlipped)
                {
                    this.AttackAnimation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    this.AttackAnimation.SpriteEffect = SpriteEffects.None;
                }
            }
            if (this.DeathAnimation != null)
            {
                if (isFlipped)
                {
                    this.DeathAnimation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    this.DeathAnimation.SpriteEffect = SpriteEffects.None;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.MoveAnimation != null)
            {
                this.MoveAnimation.Animate(gameTime, this);
            }
            if (this.IsAttacking)
            {
                this.TimeAttacking += gameTime.ElapsedGameTime.Milliseconds;
                if(this.TimeAttacking > this.TimeUntilDamageSinceAttack)
                {
                    this.IsAttacking = false;
                    this.TimeAttacking = 0.0f;
                    if(this.CollidesForAttack(
                        this.ObjectAttacked, !this.MoveAnimation.SpriteEffect.HasFlag(SpriteEffects.FlipHorizontally),
                        this.AttackRange) && this.Health > 0)
                    {
                        OnHit();
                    }
                }
            }
            if (this.isUsingAbility)
            {
                this.AbilityInUse.Update(gameTime, this);
            }
            if (this.AttackAnimation != null)
            {
                this.AttackAnimation.Animate(gameTime, this);
            }
            if (this.DeathAnimation != null)
            {
                this.DeathAnimation.Animate(gameTime, this);
            }
        }

        protected virtual void OnHit()
        {
            this.ObjectAttacked.Health -= (int)(this.AttackDamage -
                            (this.AttackDamage * (this.ArmorAsPercentage / 100)));
        }

        public override void DrawAnimations(SpriteBatch spriteBatch)
        {
            base.DrawAnimations(spriteBatch);
            if (this.isUsingAbility)
            {
                this.AbilityInUse.Animation.Draw(spriteBatch);
            }
            if (this.Health > 0 && !OrusTheGame.Instance.GameInformation.GameMenu.CharacterSelectionInProgress)
            {
                this.DrawHealthBar(spriteBatch);
            }
            if(this.MoveAnimation != null)
            {
                this.MoveAnimation.Draw(spriteBatch);
            }
            if (this.AttackAnimation != null)
            {
                this.AttackAnimation.Draw(spriteBatch);
            }
            if (this.DeathAnimation != null)
            {
                this.DeathAnimation.Draw(spriteBatch);
            }
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            //The gray part 

            spriteBatch.Draw(this.Bar.Texture.Texture,
                new Vector2(this.Bar.Position.X + Constant.HealthBarOffsetX, this.Bar.Position.Y + Constant.HealthBarOffsetY),
                new Rectangle(0, 12, this.Bar.Texture.Texture.Width, 8), Color.Gray);

            //The red health
            double healthInPercentage = ((double)this.Health / this.MaxHealth) * 100;
            spriteBatch.Draw(this.Bar.Texture.Texture,
                new Rectangle((int)(this.Bar.Position.X + Constant.HealthBarOffsetX),
                (int)(this.Bar.Position.Y + Constant.HealthBarOffsetY),
                (int)(this.Bar.Texture.Texture.Width * (healthInPercentage / 100)), 8),
                 new Rectangle(0, 12, this.Bar.Texture.Texture.Width, 11), Color.Red);

            //The border
            spriteBatch.Draw(this.Bar.Texture.Texture,
               new Vector2(this.Bar.Position.X + Constant.HealthBarOffsetX, this.Bar.Position.Y + Constant.HealthBarOffsetY),
               new Rectangle(0, 0, this.Bar.Texture.Texture.Width, 11), Color.White);
        }

        public override void Die()
        {
            base.Die();
            this.AttackAnimation.IsActive = false;
            this.DeathAnimation.IsActive = true;
        }
    }
}
