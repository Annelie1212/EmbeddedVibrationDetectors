using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmDatabaseLibrary.Context;
using AlarmDatabaseLibrary.Models;
using VibrationDetectors.Models;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors.Services
{
    public class DbLogService
    {
        private readonly AlarmDbContext _context;

        public DbLogService(AlarmDbContext context)
        {
            _context = context;
        }
        public void CreateOne(DeviceLog deviceLog)
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
        public DeviceLog ReadOne(int deviceId)
        {
            var deviceLog = new DeviceLog();

            //A method that reads one log entry from VibrationDetectorStatusLog in the database based on deviceId.
            //Latest based on ActionLogDateTime.
            VibrationDetectorStatusLog? logEntry = _context.VibrationDetectorStatusLogs
                .Where(log => log.DeviceId == deviceId)
                .OrderByDescending(log => log.ActionLogDateTime)
                .FirstOrDefault();

            if (logEntry != null)
            {
                deviceLog.VibrationDetectorStatusLogId = logEntry.VibrationDetectorStatusLogId;
                deviceLog.UserId = logEntry.UserId;
                deviceLog.DeviceName = logEntry.DeviceName;
                deviceLog.DeviceId = logEntry.DeviceId;
                deviceLog.ActionLogDateTime = logEntry.ActionLogDateTime;
                deviceLog.DeviceAction = Enum.Parse<DeviceAction>(logEntry.DeviceAction);
                deviceLog.OldUserValue = logEntry.OldUserValue;
                deviceLog.NewUserValue = logEntry.NewUserValue;
                deviceLog.Location = logEntry.Location ?? "";
                deviceLog.AlarmArmed = logEntry.AlarmArmed;
                deviceLog.AlarmTriggered = logEntry.AlarmTriggered;
                deviceLog.VibrationLevel = logEntry.VibrationLevel;
                deviceLog.VibrationLevelThreshold = logEntry.VibrationLevelThreshold;
                deviceLog.LogMessage = logEntry.LogMessage;
                return deviceLog;
            }
            else
            {
               deviceLog.ErrorMessage = StatusAndErrorType.DeviceIdNotFound;

                return deviceLog;
            }
        }
    }
}
