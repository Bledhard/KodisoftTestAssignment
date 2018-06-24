using KodisoftTestAssignment.Models;

namespace KodisoftTestAssignment.Services
{
    public partial class NewsServices
    {
        private readonly MainAppDbContext _context;

        public NewsServices(MainAppDbContext context)
        {
            _context = context;
        }
    }
}
