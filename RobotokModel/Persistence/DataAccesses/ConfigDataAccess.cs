﻿using RobotokModel.Model;
using RobotokModel.Model.Extensions;
using RobotokModel.Persistence.Interfaces;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotokModel.Persistence.DataAccesses
{
    public class ConfigDataAccess : IDataAccess
    {
        #region Private

        private Uri baseUri;

        #endregion

        #region Constructor
        public ConfigDataAccess()
        {
            this.baseUri = new("C:\\");
        }

        #endregion

        #region Properties
        public SimulationData SimulationData { get; set; } = null!;

        #endregion

        #region Async
        public async Task LoadAsync(string path)
        {
            try
            {
                //TODO
            }
            catch
            {
                throw new JSonError();
            }

            await Task.Run(() =>
            {
                SimulationData = new SimulationData
                {
                    DistributionStrategy = Strategy.RoundRobin,
                    RevealedTaskCount = 1,
                    Map = new ITile[10, 10],
                    Goals =
                    [
                        new Goal
                        {
                            Position = new Position {X = 0,Y = 0},
                            Id = 0
                        },
                        new Goal
                        {
                            Position = new Position {X = 2,Y = 2},
                            Id = 1
                        },
                        new Goal
                        {
                            Position = new Position {X = 3,Y = 3},
                            Id = 2
                        }
                    ],
                    Robots =
                    [
                        new Robot
                        {
                            Id = 0,
                            Position = new Position { X = 0,Y = 4},
                            Rotation = Direction.Right
                        },
                        new Robot
                        {
                            Id = 1,
                            Position = new Position { X = 1,Y = 4},
                            Rotation = Direction.Right
                        },
                        new Robot
                        {
                            Id = 2,
                            Position = new Position { X = 2,Y = 4},
                            Rotation = Direction.Right
                        }
                    ]
                };
            });

            for (int i = 0; i < SimulationData.Map.GetLength(0); i++)
            {
                for (int j = 0; j < SimulationData.Map.GetLength(1); j++)
                {
                    SimulationData.Map[i, j] = EmptyTile.Instance;
                }
            }

            foreach (Robot robot in SimulationData.Robots)
            {
                SimulationData.Map.SetAtPosition(robot.Position, robot);
            }
        }
        #endregion

        #region SideMethods
        private void SetMap(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] mapData = File.ReadAllText(filePath).Split('\n');
            // map[0]: type octile nem tudjuk mit jelent, nem használjuk
            int height = int.Parse(mapData[1].Split(' ')[1]);
            int width = int.Parse(mapData[2].Split(' ')[1]);
            ITile[,] map = new ITile[width, height];
            for (int y = 0; y < height; y++)
            {
                string row = mapData[y+4];
                for (int x = 0; x < width; x++)
                {
                    if (row[x] == '.')
                    {
                        map[x, y] = EmptyTile.Instance;
                    }
                    else
                    {
                        map[x, y] = Block.Instance;
                    }
                }
            }
            SimulationData.Map = map;
        }
        private void SetRobots(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] robotData = File.ReadAllText(filePath).Split("\r\n");
            int robotCount = int.Parse(robotData[0]);
            for (int i = 1; i <= robotCount; i++)
            {
                int intPos = int.Parse(robotData[i]);
                

                int x = intPos % SimulationData.Map.GetLength(0);
                int y = intPos / SimulationData.Map.GetLength(0);
                if (x > 0) { x--; }
                if (y > 0) { y--; }

                Robot r = new Robot
                {
                    Id = i - 1,
                    Position = new Position { X = x, Y = y },
                    Rotation = Direction.Right
                };
                SimulationData.Robots.Add(r);
                SimulationData.Map.SetAtPosition(r.Position, r);
            }
        }
        private void SetGoals(string path)
        {
            string filePath = new Uri(baseUri, path).AbsolutePath;

            string[] goalData = File.ReadAllText(filePath).Split('\n');
            int goalCount = int.Parse(goalData[0]);
            for (int i = 1; i <= goalCount; i++)
            {
                int intPos = int.Parse(goalData[i]);
                int x = intPos % SimulationData.Map.GetLength(0);
                int y = intPos / SimulationData.Map.GetLength(0);
                if (x > 0) { x--; }
                if (y > 0) { y--; }

                Goal g = new Goal
                {
                    Id = i - 1,
                    Position = new Position { X = x, Y = y },
                };
                SimulationData.Goals.Add(g);
            }
        }
        #endregion

        #region Syncronous
        public void Load(string path)
        {
            try
            {
                var cucc = path;
                baseUri = new(path);

                string jsonString = File.ReadAllText(path);
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                options.Converters.Add(new JsonStringEnumConverter());
                Config? config = JsonSerializer.Deserialize<Config>(jsonString, options) ?? throw new JSonError("Serialization of config file was unsuccesful!");
                Strategy strategy;
                switch (config.TaskAssignmentStrategy)
                {
                    case "roundrobin":
                        strategy = Strategy.RoundRobin;
                        break;
                    default:
                        strategy = Strategy.RoundRobin;
                        break;
                }
                SimulationData = new SimulationData
                {
                    DistributionStrategy = strategy,
                    RevealedTaskCount = config.NumTasksReveal,
                    Map = null!,
                    Goals =[],
                    Robots = []
                };
                SetMap(config.MapFile);
                SetRobots(config.AgentFile);
                SetGoals(config.TaskFile);
            }
            catch (Exception ex) 
            {
                var exs = ex;
                throw new JSonError();
            }
        }
        #endregion

    }
}
