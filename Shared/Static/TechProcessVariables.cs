namespace Shared.Static;

public static class TechProcessVariables
{
    public static int[] IssuedStatusesId = { 9, 10 };
    public static int[] DevelopingStatuses = { 2, 4, 5, 6, 7, 8, 11 };
    public static int[] StatusesWithoutIssueDuplicate = { 2, 4, 5, 6, 7, 8, 9, 11, 10 };

    public static int[] ForNotInWork = { 4 };
    public static int[] ForInWork = { 5, 6 };
    public static int[] ForPaused = { 4 };
    public static int[] ForOnApproval = { 7, 11 };
    public static int[] ForReturnForRework = { 4 };
    public static int[] ForForIssuance = { 9, 7 };
    public static int[] ForIssued = { 10, 7 };
    public static int[] ForCompleted = { 7, 8 };
}
