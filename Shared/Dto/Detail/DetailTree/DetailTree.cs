namespace Shared.Dto.Detail.DetailTree;

public class DetailTree
{
    public int Id { get; set; }
    public string PositionNumber { get; set; }
    public string SerialNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Unit { get; set; }
    public float Count { get; set; } = 1;
    public DetailTree Back { get; set; }
    public DetailTree? Next { get; set; }
    public List<DetailTree> Items { get; set; }
    public int Counter = 0;

    public DetailTree(int id)
    {
        Id = id;
        Back = null!;
        Items = new();
        Next = null!;
    }

    public DetailTree(int id, List<DetailTree> items)
    {
        Id = id;
        Back = null!;
        Items = new();
        foreach (var item in items)
            Items.Add(item);
        Next = null!;
    }

    public DetailTree(DetailTree back, DetailTree next, List<DetailTree> items)
    {
        Id = next.Id;
        SerialNumber = next.SerialNumber;
        Title = next.Title;
        PositionNumber = next.PositionNumber;
        Unit = next.Unit;
        Back = back;
        Items = new();
        foreach (var item in items)
            Items.Add(item);
    }

    public DetailTree(DetailTree back, DetailTree next, List<DetailTree> items, float count)
    {
        Id = next.Id;
        SerialNumber = next.SerialNumber;
        Title = next.Title;
        Unit = next.Unit;
        PositionNumber = next.PositionNumber;
        Count = count;
        Back = back;
        Items = new();
        foreach (var item in items)
            Items.Add(item);
    }

    public float GetMultiplyCount(float count) => Count * count;

    public void GeneratePositionNumbers()
    {
        var i = 1;
        foreach (var item in Items)
        {
            item.PositionNumber = Back == null ? $"{i}" :$"{PositionNumber}.{i}";
            i++;
        }
    }

    public void GetNext()
    {
        if (Counter >= Items.Count)
        {
            Next = null;
            return;
        }
        Next = Items[Counter];
        Counter++;
    }
}
