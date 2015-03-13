using System;


namespace XPlaneGenConsole
{
    [Flags]
    public enum EngineFields
    {
        None,
        All
    }

    [Flags]
    public enum SystemFields
    {
        None,
        All
    }

    [Flags]
    public enum FlightFields
    {
        None,
        NormalAcceleration,
        LongitudinalAcceleration,
        LateralAcceleration,
        ADAHRUsed,
        AHRSSStatus,
        All
    }
}