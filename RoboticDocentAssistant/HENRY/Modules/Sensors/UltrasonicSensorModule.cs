﻿using HENRY.ModuleSystem;
using System.Timers;

namespace HENRY.Modules.Sensors
{
    /// <summary>
    /// Secondary ultrasonic sensor processing.
    /// </summary>
    /// TO DO:
    /// - Find use for it. Whatever isn't processed by the controller, process here.
    class UltrasonicSensorModule : LengarioModuleAuxiliary
    {
        Timer t;
        ErrorLog ultra_log;

        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\..\Sounds\HENRY_Horn.wav");
        int dist2obstacle = Constants.MAX_DIST;
        private int reccomendedSpeed = 0;
        private string logline;

        public UltrasonicSensorModule()
        {
            ultra_log = new ErrorLog(this);
            
            for (int i = 0; i <= Constants.US_NUM; i++)
                SetPropertyValue("UltraS" + i.ToString(), 0);
            SetPropertyValue("ClosestObstacle", Constants.MAX_DIST);
            SetPropertyValue("ReccomendedUltrasonicSpeed", 0);

            //t = new Timer();
            //t.Interval = 330;
            //t.Elapsed += t_Elapsed;
            //t.Start();
        }

        public int Calculate()
        {
            // what do?
            dist2obstacle = Constants.MAX_DIST;
            logline = "";


            for (int i = 0; i < Constants.US_NUM; i++)
            {
                int current_sensor_dist = GetPropertyValue("UltraS" + (i + 1).ToString()).ToInt32();
                logline += current_sensor_dist.ToString() + ",";
                if (i == 0) // Takes the mast sensor and subtracts 40mm from the sensed distance to even it out with the rest
                {
                    current_sensor_dist = current_sensor_dist - Constants.MAST_TO_FRONT;
                }

                if (current_sensor_dist < dist2obstacle) // sets smallest distance
                {
                    dist2obstacle = current_sensor_dist;
                }
            }
            logline += dist2obstacle.ToString();
            SetPropertyValue("ClosestObstacle", dist2obstacle);
            ultra_log.WriteToLog(logline);
            //if (300 < dist2obstacle)
            //{
            //    reccomendedSpeed = 1;
            //}

            //else if (100 < dist2obstacle && dist2obstacle < 300)
            //{
            //    reccomendedSpeed = 0;
            //}

            //else if (dist2obstacle < 100)
            //{
            //    //player.Play();
            //    return -1;
            //}
            reccomendedSpeed = 1;
            SetPropertyValue("ReccomendedUltrasonicSpeed", reccomendedSpeed);

            return 0;
        }
        
        public void StartRecording()
        {
            ultra_log.OpenLog();
            ultra_log.WriteToLog("ultra1,ultra2,ultra3,ultra4,ultra5,closest_obstacle");
        }

        public void StopRecording()
        {
            ultra_log.CloseLog();
        }

        public override string GetModuleName()
        {
            return "UltrasonicSensorModule";
        }
    }
}
