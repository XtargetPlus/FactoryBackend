using DB.Model.StorageInfo.Graph;

namespace BizLayer.Repositories.GraphR.Variables;

public class GraphDetailTreeNode
{
    public OperationGraphDetail Current { get ; set; }
    
    private List<OperationGraphDetail> _items;
    private int _capacity = 0;

    public GraphDetailTreeNode(OperationGraphDetail current)
    {
        Current = current;
    }

    public void SetItems(List<OperationGraphDetail> items) => _items = items;

    public OperationGraphDetail? GetNext()
    {
        if (_capacity == _items.Count) return null;
        
        var next = _items[_capacity];
        _capacity++;
        return next;
    }
}