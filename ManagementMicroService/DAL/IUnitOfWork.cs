namespace ManagementMicroservice.DAL;

public interface IUnitOfWork
{
    Task CommitAsync();
    void Commit();
}
