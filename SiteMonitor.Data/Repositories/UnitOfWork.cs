using SiteMonitor.Data.Contexts;

namespace SiteMonitor.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
