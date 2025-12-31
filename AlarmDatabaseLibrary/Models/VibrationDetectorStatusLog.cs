using System;
using System.Collections.Generic;

namespace AlarmDatabaseLibrary.Models;

public partial class VibrationDetectorStatusLog
{
    public int VibrationDetectorStatusLogId { get; set; }

    public DateTime ActionLogDateTime { get; set; }

    public string DeviceAction { get; set; } = null!;

    public int OldUserValue { get; set; }

    public int NewUserValue { get; set; }

    public int UserId { get; set; }

    public int DeviceId { get; set; }

    public string DeviceName { get; set; } = null!;

    public string? Location { get; set; }

    public bool AlarmArmed { get; set; }

    public bool AlarmTriggered { get; set; }

    public int VibrationLevel { get; set; }

    public int VibrationLevelThreshold { get; set; }

    public string LogMessage { get; set; }
}
