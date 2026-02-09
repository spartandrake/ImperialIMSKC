using System.Linq.Expressions;

namespace ImperialIMS.Repos
{
    public interface IRepo<T>
    {
        int Count { get; }
        bool HasChanges { get; }
        T Find(int? id);
        IEnumerable<T> GetRange(int skip, int take);
        IQueryable<T> Search(Expression<Func<T, bool>> filter = null, IOrderedQueryable<T> orderBy = null);
        int Add(T entity, bool persist = true);
        int AddRange(IEnumerable<T> entities, bool persist = true);
        int Update(T entity, bool persist = true);
        int UpdateRange(IEnumerable<T> entities, bool persist = true);
        int Delete(T entity, bool persist = true);
        int Delete(int id, long timestamp, bool persist = true);
        int DeleteRange(IEnumerable<T> entities, bool persist = true);
        int SaveChanges();
    }
}
