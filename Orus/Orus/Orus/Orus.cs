using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orus.Animations;
using Orus.Constants;
using Orus.GameObjects.Enemies;
using Orus.GameObjects.Enemies.NormalEnemies;
using Orus.GameObjects.Player.Characters;
using Orus.Menu;
using System.Collections.Generic;

namespace Orus
{
    public class Orus : Game 
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Character character;
        private List<Enemy> enemies;

        private GraphicsDeviceManager Graphics { get { return this.graphics; } set { this.graphics = value; } }
        private SpriteBatch SpriteBatch { get { return this.spriteBatch; } set { this.spriteBatch = value; } }
        private Character Character { get { return this.character; } set { this.character = value; } }
        private List<Enemy> Enemies { get { return this.enemies; } set { this.enemies = value; } }

        public Orus()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = Constant.WindowWidth;
            graphics.PreferredBackBufferHeight = Constant.WindowHeight;
            graphics.ApplyChanges();
            GameMenu.Load(this.Content);

            Enemies = new List<Enemy>();
            Enemies.Add(new Zombie(new Vector2(400, 300), Content));
            Character = new Crusader(new Vector2(Constant.StartingPlayerXPosition, Constant.StartingPlayerYPosition), Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            UpdateInput(gameTime);
            if (GameMenu.IsMenuActive)
            {
                GameMenu.Update();
            }
            else
            {
                Character.Animate(gameTime);
                foreach (var enemy in Enemies)
                {
                    enemy.Animate(gameTime);
                }
            }
            base.Update(gameTime);
        }

        private void UpdateInput(GameTime gameTime)
        {
            if (Character.AttackAnimation.IsActive)
            {
                return;
            }
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                MoveCharacter(gameTime, true);
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                MoveCharacter(gameTime, false);
            }
            else
            {
                Character.StopMovement();
            }
            if (mouseState.LeftButton == ButtonState.Pressed && !Character.MoveAnimation.IsActive)
            {
                Character.Attack();
            }
        }

        private void MoveCharacter(GameTime gameTime, bool moveRight)
        {
            bool collides = false;
            foreach (var collider in this.Enemies)
            {
                if (collider.Collides(Character, moveRight))
                {
                    collides = true;
                }
            }
            if (!collides)
            {
                Character.Move(gameTime, moveRight, collides);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if(GameMenu.IsMenuActive)
            {
                GameMenu.Draw(spriteBatch);
            }
            else
            {
                Character.DrawAnimations(SpriteBatch);
                foreach (var enemy in Enemies)
                {
                    enemy.DrawAnimations(SpriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}
