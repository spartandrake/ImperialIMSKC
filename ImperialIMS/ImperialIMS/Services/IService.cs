using System.Linq.Expressions;

namespace ImperialIMS.Services
{
    public interface IService<T>
    {
        T Get(int Id);
        List<T> GetAll();
        List<T> GetRecycleBin();
        public void Add(T value);
        public void Update(T value);
        public Task Delete(int Id);
        public Task Remove(int Id);
        public Task UnDelete(int Id);

    }
}
