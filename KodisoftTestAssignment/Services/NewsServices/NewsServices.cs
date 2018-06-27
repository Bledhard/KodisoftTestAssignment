using KodisoftTestAssignment.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        private readonly MainAppDbContext _dbContext;
        private readonly ILogger<NewsServices> _logger;
        private readonly IMemoryCache _cache;

        public NewsServices(
            MainAppDbContext dbContext, 
            ILogger<NewsServices> logger,
            IMemoryCache cache)
        {
            _dbContext = dbContext;
            _logger = logger;
            _cache = cache;
        }
    }
}
