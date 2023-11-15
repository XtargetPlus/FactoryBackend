namespace Shared.Enums;

public enum GraphStatus
{
    InWork = 12,
    Active = 13,
    Paused = 14,
    Completed = 15,
    Canceled = 16,
}

public enum GraphAccessTypeForFilters
{
    All,
    ReadAndEdit,
    Readonly
}

public enum GraphOpenType
{
    WithRepeats,
    WithoutRepeats
}

public enum GraphDetailVisibility
{
    All,
    InWork
}

public enum GraphOwnershipType
{
    All,
    Owner,
    AccessProvided
}

public enum NewUserAccess
{
    None,
    ReadAndEdit,
    Readonly
}

public enum GraphProductAvailability
{
    All,
    Have,
    HaveNot
}

public enum DeleteFromGraphsGroupType
{
    Full,
    ToNewGroup,
    ToSingle
}