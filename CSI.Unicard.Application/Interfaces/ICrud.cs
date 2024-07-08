namespace CSI.Unicard.Application.Interfaces
{
    public interface ICrud<T>where T : class
    {
        Task<bool> Add(T entity);

        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAll();

        Task DeleteById(int id);
    }
}
