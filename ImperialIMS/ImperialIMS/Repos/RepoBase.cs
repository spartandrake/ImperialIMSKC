using ImperialIMS.Data;
using ImperialIMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace ImperialIMS.Repos
{
    public abstract class RepoBase<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {

        protected readonly ApplicationDbContext Db;
        protected Microsoft.EntityFrameworkCore.DbSet<T> Table;
        public ApplicationDbContext Context => Db;

        protected RepoBase(IConfiguration config)
        {
            DbContextOptionsBuilder<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));

            Db = new ApplicationDbContext(options.Options, config);
            Table = Db.Set<T>();
        }

        protected RepoBase(DbContextOptions<ApplicationDbContext> options, IConfiguration config)
        {
            Db = new ApplicationDbContext(options, config);
            Table = Db.Set<T>();
        }

        public bool HasChanges => Db.ChangeTracker.HasChanges();

        public int Count => Table.Count();

        public virtual T Find(int? id)
        {
            return Table.Find(id);
        }

        /// <summary>
        /// A way to search/sort and select only certain properties from the collection T
        /// Based on the Get override in the generic repository at https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        /// </summary>
        /// <param name="filter">The linq query to filter results on</param>
        /// <param name="orderBy">The field to sort by</param>
        /// <returns></returns>
        public IQueryable<T> Search(Expression<Func<T, bool>> filter = null, IOrderedQueryable<T> orderBy = null)
        {
            IQueryable<T> query = Table.Where(x => x.Id > 0);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return query.OrderByDescending(q => orderBy);
            }

            return query;
        }

        internal static IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        public virtual IEnumerable<T> GetRange(int skip, int take)
            => RepoBase<T>.GetRange(Table, skip, take);

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        internal T GetEntryFromChangeTracker(int? id)
        {
            return Db.ChangeTracker.Entries<T>()
                .Select((EntityEntry e) => (T)e.Entity)
                    .FirstOrDefault(x => x.Id == id);
        }

        public virtual int Delete(int id, long timestamp, bool persist = true)
        {
            var entry = GetEntryFromChangeTracker(id);
            if (entry != null)
            {
                if (timestamp != null && entry.TimeStamp.Equals(timestamp))
                {
                    return Delete(entry, persist);
                }
                throw new Exception("Unable to delete due to concurrency violation.");
            }
            Db.Entry(new T { Id = id, TimeStamp = timestamp }).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                //Should log
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                //Should log 
                Console.WriteLine(ex);
                throw;
            }
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }
            Db.Dispose();
            _disposed = true;
        }
    }
}
