namespace SiteMonitor.Data.Repositories
{
    public interface IUnitOfWork
    {
        void Complete();
    }
}
