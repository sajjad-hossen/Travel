namespace TravelBD.Domain.Enums;

public enum LocationType
{
    District,
    Upazila,
    TransitHub,
    TouristSpot
}

public enum TransportMode
{
    Bus,
    Train,
    Flight,
    Launch,
    Cng,
    ChanderGari,
    Ferry,
    Boat
}

public enum RouteTier
{
    AcBus,
    NonAcBus,
    SleeperBus,
    ShovonChair,
    Snigdha,
    EconomyAir,
    ReservedVehicle,
    LocalShared
}

public enum BudgetLevel
{
    Budget,
    MidRange,
    Luxury,
    Resort,
    EcoCottage
}
