namespace BizLayer.Builders;

public abstract class BaseBuilder<TEntity>
    where TEntity : class
{
    public abstract TEntity Build();
    public abstract Task<TEntity> BuildAsync();
}