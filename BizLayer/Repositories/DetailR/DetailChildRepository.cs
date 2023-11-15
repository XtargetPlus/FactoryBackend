using DB.Model.DetailInfo;
using Microsoft.EntityFrameworkCore;

namespace BizLayer.Repositories.DetailR;

public class DetailChildRepository
{
    private readonly int _childDetailId;
    private readonly int _fatherDetailId;
    private readonly DbSet<DetailsChild> _repository;

    public DetailChildRepository(int childDetailId, int fatherDetailId, DbContext context)
    {
        _childDetailId = childDetailId;
        _fatherDetailId = fatherDetailId;
        _repository = context.Set<DetailsChild>();
    }

    public async Task<bool> IsFatherDetailsSeniorAsync()
    {
        var currentFatherId = await _repository.Where(dc => dc.ChildId == _fatherDetailId).Select(dc => dc.FatherId).FirstOrDefaultAsync();

        while (currentFatherId > 0)
        {
            if (currentFatherId == _childDetailId)
                return true;
            currentFatherId = await _repository.Where(dc => dc.ChildId == currentFatherId).Select(dc => dc.FatherId).FirstOrDefaultAsync();
        }

        return false;
    }

    public async Task<bool> IsChildDetailInLineupAsync() => await _repository.AnyAsync(dc => dc.FatherId == _fatherDetailId && dc.ChildId == _childDetailId);
}