﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotokModel.Model.Interfaces;

namespace RobotokModel.Model
{
    internal class Controller : IController
    {
        public SimulationData SimulationData { get ; set ; }

        public RobotOperation[] ClaculateOperations()
        {
            throw new NotImplementedException();
        }

        public RobotOperation[] NextStep()
        {
            throw new NotImplementedException();
        }
    }
}
