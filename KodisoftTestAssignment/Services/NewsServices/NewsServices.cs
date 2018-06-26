using KodisoftTestAssignment.Models;
using Microsoft.Extensions.Logging;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        private readonly MainAppDbContext _dbContext;
        private readonly ILogger<NewsServices> _logger;

        public NewsServices(
            MainAppDbContext dbContext, 
            ILogger<NewsServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}
