﻿using Microsoft.Xna.Framework;
using Orus.Animations;
namespace Orus.Interfaces
{
    interface IMove
    {
        FrameAnimation MoveAnimation { get; set; }
        string MoveAnimationPath { get; set; }
        void Move(GameTime gameTime, bool directionIsRight);
    }
}