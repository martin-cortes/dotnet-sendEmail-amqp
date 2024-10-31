namespace Application.Interfaces.Infrastructure
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<string> InsertDocumentAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(string id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> UpdateAsync(string id, TEntity entity);

        Task<bool> DeleteAsync(string id);
    }
}