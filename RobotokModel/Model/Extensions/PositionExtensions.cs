﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model.Extensions
{
    public static class PositionExtensions
    {
        public static Position PoistionInDirection(this Position position, Direction rotation)
        {
            var newPosition = new Position();
            switch (rotation)
            {
                case Direction.Left:
                    newPosition.X = position.X - 1;
                    newPosition.Y = position.Y;
                    break;
                case Direction.Up:
                    newPosition.X = position.X;
                    newPosition.Y = position.Y - 1;
                    break;
                case Direction.Right:
                    newPosition.X = position.X + 1;
                    newPosition.Y = position.Y;
                    break;
                case Direction.Down:
                    newPosition.X = position.X;
                    newPosition.Y = position.Y + 1;
                    break;
            }
            return newPosition;
        }
    }
}