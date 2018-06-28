using KodisoftTestAssignment.Interfaces;
using KodisoftTestAssignment.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILogger<NewsServices> _logger;
        private readonly IMemoryCache _cache;

        public NewsServices(
            INewsRepository newsRepository, 
            ILogger<NewsServices> logger,
            IMemoryCache cache)
        {
            _newsRepository = newsRepository;
            _logger = logger;
            _cache = cache;
        }
    }
}
