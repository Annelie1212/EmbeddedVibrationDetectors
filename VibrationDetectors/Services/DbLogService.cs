using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmDatabaseLibrary.Context;
using AlarmDatabaseLibrary.Models;
using VibrationDetectors.Models;

namespace VibrationDetectors.Services
{
    public class DbLogService
    {
        private readonly AlarmDbContext _context;

        public DbLogService(AlarmDbContext context)
        {
            _context = context;
        }
        public void SeedOne(DeviceLog deviceLog)
        {

            //A method that sends all data from the DTO deviceLog to VibrationDetectorStatusLog in the database.
            

            _context.VibrationDetectorStatusLogs.Add(new VibrationDetectorStatusLog
            {
                ActionLogDateTime = deviceLog.ActionLogDateTime,
                DeviceAction = deviceLog.DeviceAction.ToString(),
                OldUserValue = deviceLog.OldUserValue,
                NewUserValue = deviceLog.NewUserValue,
                UserId = deviceLog.UserId,
                DeviceId = deviceLog.DeviceId,
                DeviceName = deviceLog.DeviceName,
                Location = deviceLog.Location,
                AlarmArmed = deviceLog.AlarmArmed,
                AlarmTriggered = deviceLog.AlarmTriggered,
                VibrationLevel = deviceLog.VibrationLevel,
                VibrationLevelThreshold = deviceLog.VibrationLevelThreshold,
                LogMessage = deviceLog.LogMessage
            });

            _context.SaveChanges();
        }
    }
}
