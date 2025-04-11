using BaseLibrary.Classes.Result;

namespace BaseLibrary.Interfaces;

public interface IService<T>
{
    public Task<Result<IEnumerable<T>?>> Get(CancellationToken cancellationToken);
    public Task<Result<T?>> GetById(Guid id, CancellationToken cancellationToken);
    public Task<Result<T?>> DeleteById(Guid id, CancellationToken cancellationToken);
}