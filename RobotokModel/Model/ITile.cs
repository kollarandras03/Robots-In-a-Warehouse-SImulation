using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    public interface ITile
    {
        public bool IsPassable { get; }
    }

    /// <summary>
    /// Represents an empty tile.
    /// <para/>
    /// Singleton: use <c>Instance</c>
    /// </summary>
    public sealed class EmptyTile : ITile
    {
        public bool IsPassable => true;

        private static readonly EmptyTile _instance = new();

        static EmptyTile() { }
        private EmptyTile() { }
        public static EmptyTile Instance => _instance;
    }

    /// <summary>
    /// Represents a tile that can't be taken by any robot
    /// <para/>
    /// Singleton: use <c>Instance</c>
    /// </summary>
    public sealed class Block : ITile
    {
        public bool IsPassable => false;

        private static readonly Block _instance = new();

        static Block() { }
        private Block() { }
        public static Block Instance => _instance;
    }

    public class Robot : ITile
    {
        private static int id = 0;

        public static readonly List<Robot> Robots = [];
        public static void EndTurn()
        {
            Robots.ForEach(f => f.MovedThisTurn = false);
            
        }
        public bool IsPassable => false;
        public Direction Rotation { get; set; }
        public Goal CurrentGoal { get; set; }
        public RobotOperation NextOperation { get; set; }
        public bool MovedThisTurn { get; set; } = false;
        public Position Position { get; set; }
        public int Id { get; }
        public Robot()
        {
            Id = id;
            Rotation = Direction.Left;
            id++;
            Robots.Add(this);
        }
    }
}
