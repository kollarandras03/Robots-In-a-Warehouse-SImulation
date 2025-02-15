﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotokModel.Model
{
    internal interface IController
    {
        public SimulationData SimulationData { get; set; }

        public RobotOperation[] NextStep();
        public RobotOperation[] ClaculateOperations();
    }
}
