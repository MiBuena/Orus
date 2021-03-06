﻿namespace Orus.Abilities
{
    using GameObjects.Player.Characters;
    using System.Collections.Generic;
    using GameObjects;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Interfaces;
    using Core;

    public class Slash : AttackAbility
    {
        private const int cooldown = 20;
        private const float timeForAttack = 0.8f;

        public Slash()
        {

        }

        public Slash(int damage, string pathForAnimation, int framesForAnimation)
            : base(damage, cooldown, timeForAttack, pathForAnimation, framesForAnimation)
        {
            this.Animation.IsLoop = true;
        }

        protected override void UpdateAffectedTargets(AttackingGameObject thisObject)
        {
            foreach (var enemy in OrusTheGame.Instance.GameInformation.Levels[OrusTheGame.Instance.GameInformation.CurrentLevelIndex].Enemies)
            {
                if (thisObject.CollidesForAttack(enemy,
                    !thisObject.IddleAnimation.SpriteEffect.HasFlag(SpriteEffects.FlipHorizontally)))
                {
                    this.AffectedTargets.Add(enemy);
                }
            }
        }

        public override void Update(GameTime gameTime, AttackingGameObject objectUsingAbility)
        {
            base.Update(gameTime, objectUsingAbility);
            var possibleColliders = new List<ICollideable>();
            foreach (var enemy in OrusTheGame.Instance.GameInformation.Levels[OrusTheGame.Instance.GameInformation.CurrentLevelIndex].Enemies)
            {
                possibleColliders.Add(enemy);
            }
            possibleColliders.Add(OrusTheGame.Instance.GameInformation.Character);
            bool collides = false;
            foreach (var collider in possibleColliders)
            {
                foreach (var alreadyAffected in this.AffectedTargets)
                {
                    if (objectUsingAbility.Collides(alreadyAffected))
                    {
                        collides = true;
                    }
                }
                if (objectUsingAbility.CollidesForAttack(collider,
                    !objectUsingAbility.IddleAnimation.SpriteEffect.HasFlag(SpriteEffects.FlipHorizontally)))
                {
                    var enemy = collider as AttackingGameObject;
                    if(enemy != null)
                    {
                        this.AffectedTargets.Add(enemy);
                    }
                    collides = true;
                }
                if (collides)
                {
                    this.Animation.SpriteEffect = objectUsingAbility.IddleAnimation.SpriteEffect;
                    objectUsingAbility.MoveTo(3,
                    !objectUsingAbility.IddleAnimation.SpriteEffect.HasFlag(SpriteEffects.FlipHorizontally), false, false);
                }
            }
            if(!collides)
            {
                this.Animation.IsActive = false;
                objectUsingAbility.IddleAnimation.IsActive = true;
            }
        }

        public override void Action(Character character)
        {
            this.UpdateAffectedTargets(character);
            base.Action(character);
        }
    }
}
