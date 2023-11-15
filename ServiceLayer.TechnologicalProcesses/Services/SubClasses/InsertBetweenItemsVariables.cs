using DB.Model.TechnologicalProcessInfo;

namespace ServiceLayer.TechnologicalProcesses.Services.SubClasses;

public class InsertBetweenItemsVariables
{
    private readonly int branch;
    public int OldNumber { get; private set; }
    public int NewNumber { get; private set; }
    public int NewPriority { get; private set; }
    public bool IsMainBranch { get => branch % 5 == 0; }
    public bool IsOldNumberBigger { get => OldNumber > NewNumber; }

    public InsertBetweenItemsVariables(TechnologicalProcessItem? firstItem, TechnologicalProcessItem secondItem)
    {
        branch = secondItem.Priority;
        OldNumber = secondItem.Number;
        NewNumber = firstItem != null ? (firstItem.Number < OldNumber ? firstItem.Number + 1 : firstItem.Number) : 1;
        NewPriority = GetNewPriority(firstItem, secondItem);
    }

    private int GetNewPriority(TechnologicalProcessItem? firstItem, TechnologicalProcessItem secondItem)
    {
        if (firstItem != null)
        {
            if (!IsMainBranch) return secondItem.Priority;
            
            if (firstItem.Priority < secondItem.Priority) 
                return firstItem.Priority + 5;
            return firstItem.Priority;
        }
        else
        {
            if (IsMainBranch)
                return 5;
        }
        return secondItem.Priority;
    }
}
