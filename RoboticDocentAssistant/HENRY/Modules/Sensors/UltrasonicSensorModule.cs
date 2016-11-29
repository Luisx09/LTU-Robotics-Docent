﻿using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Sensors
{
    class UltrasonicSensorModule : GenericSensorModule
    {
        Timer t;
        
        const int UltraSNum = 8;

        public UltrasonicSensorModule()
        {
            for (int i = 0; i <= UltraSNum; i++)
                SetPropertyValue("UltraS" + i.ToString(), 0.0);

            SetPropertyValue("UltraSNum", UltraSNum);

            t = new Timer();
            t.Interval = 330;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            // I think we have the same problem as in infrared
            // with multiple obstacles 
            // (See comment in InfraredSensorModule)
        }
        
        public override string GetModuleName()
        {
            return "UltrasonicSensorModule";
        }
    }
}
