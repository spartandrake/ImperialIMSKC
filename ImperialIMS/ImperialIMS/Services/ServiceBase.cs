using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ServiceBase<T>
    {
        private IRepo<T> _repo { get; set; }
        private IConfiguration _configuration { get; set; }
        private ILogger<T> _logger { get; set; }
        private T _value { get; set; }
        private List<T> _values { get; set; }
        public ServiceBase(IRepo<T> repo, IConfiguration configuration, ILogger<T> logger)
        {
            _repo = repo;
            _configuration = configuration;
            _logger = logger;
        }
        public T Get(int Id)
        {
            try
            {
                _value = _repo.Search(x => x.Id == Id)
                .FirstOrDefault() ?? new T();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting Object with {Id}. " + ex.Message, Id);
            }
            return _value;
        }
        public List<T> GetAll()
        {
            try
            {
                _values.Clear();
                _value.AddRange(_repo.Search(x => !x.IsDeleted));
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting all of the objects.");
            }
            return _values;
        }
        public List<T> GetForUser(string ApplicationUserId)
        {
            try
            {
                _values.Clear();
                _values.AddRange(_repo.Search(x => x.ApplicationUserId == ApplicationUserId && !x.IsDeleted)
                .Include(x => x.ApplicationUser));
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting Objects with ApplicationUserId of {Id}. " + ex.Message, ApplicationUserId);
            }
            return _values;
        }
        public List<T> GetRecycleBin()
        {
            try
            {
                _values.Clear();
                _discussionThreads.AddRange(_repo.Search(x => x.IsDeleted).Include(x => x.ApplicationUser));
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting Objects set to be deleted. " + ex.Message);
            }
            return _values;
        }
        public void Add(T value)
        {
            try
            {
                _repo.Add(value);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding Value with Id:{Id} . " + ex.Message, value.Id);
            }
        }
        public void Update(T value)
        {
            try
            {
                _repo.Update(value);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating value with Id {Id}. " + ex.Message, value.Id);
            }
        }
        public async Task Remove(int id)  //Used for hard deletes
        {
            T value = Get(id);
            try
            {
                _repo.Delete(value);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error removing Value with Id {Id}. " + ex.Message, value.Id);
            }
        }
        public async Task Delete(int id) //Used for soft deletes
        {
            T value = Get(id);
            value.IsDeleted = true;
            try
            {
                _repo.Update(value);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting value with Id {Id}. " + ex.Message, value.Id);
            }
        }
        public async Task UnDelete(int id)//Used to restore a soft delete
        {
            T value = Get(id);
            value.IsDeleted = false;
            try
            {
                _repo.Update(value);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error undeleting value with Id {Id}. " + ex.Message, value.Id);
            }
        }
    }
}
