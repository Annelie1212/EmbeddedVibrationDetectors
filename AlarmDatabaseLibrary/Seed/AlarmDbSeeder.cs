using AlarmDatabaseLibrary.Context;
using AlarmDatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDatabaseLibrary.Seed
{
    public class AlarmDbSeeder
    {
        private readonly AlarmDbContext _context;

        public AlarmDbSeeder(AlarmDbContext context)
        {
            _context = context;
        }

        public void Seed()
        { 
            if (_context.VibrationDetectorStatusLogs.Any())
                return;

            _context.VibrationDetectorStatusLogs.Add(new VibrationDetectorStatusLog
            {
                ActionLogDateTime = DateTime.UtcNow,
                DeviceAction = "arm",
                OldUserValue = 0,
                NewUserValue = 0,
                UserId = 99,
                DeviceId = 1,
                DeviceName = "Sensor01",
                Location = "Basement",
                AlarmArmed = false,
                AlarmTriggered = false,
                VibrationLevel = 0,
                VibrationLevelThreshold = 5,
                LogMessage = "Initial seed log entry"   
            });

            _context.SaveChanges();
        }

    }
}
