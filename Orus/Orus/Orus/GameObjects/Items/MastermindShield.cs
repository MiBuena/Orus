﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Orus.Sprites;

namespace Orus.GameObjects.Items
{
    class MastermindShield : Item
    {
        public MastermindShield(string name, Point2D position, ContentManager content) : base(name, position, content)
        {
            this.ItemPicture = new Sprite(content.Load<Texture2D>("Sprites\\Items\\Mastermind_Shield"), position);
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.ItemPicture.Texture.Width, this.ItemPicture.Texture.Height);

        }

        public override void DrawOnTheGameMenu(SpriteBatch spriteBatch, Point2D cameraPoint)
        {
            if (this.IsCollectedByCharacter)
            {
                this.ItemPicture.Position = new Point2D(cameraPoint.X + 4 * this.ItemPicture.Texture.Width, cameraPoint.Y);
                this.ItemPicture.IsActive = true;
                this.BoundingBox = new Rectangle((int)this.ItemPicture.Position.X, (int)this.ItemPicture.Position.Y,
                    this.ItemPicture.Texture.Width, this.ItemPicture.Texture.Height);
                this.ItemPicture.Draw(spriteBatch);
            }
        }
        public override Rectangle BoundingBox { get; set; }
    }
}
