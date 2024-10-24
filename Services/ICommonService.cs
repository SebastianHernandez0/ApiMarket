namespace ApiMarket.Services
{
    public interface ICommonService<T,TI,TU>
    {
        Task<IEnumerable<T>> Get();
        Task<T>GetById(int id);
        Task<T>Add(TI productoRequestDto);
        Task<T> Update(int id, TU productoRequestDto);
        Task Delete(int id);
    }
}
