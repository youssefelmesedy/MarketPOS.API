using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.RepositoryInterfaces.RepositoryCategory;
public interface ICategoryRepo : 
    IQueryableRepository<Category>,
    IProjectableRepository<Category>,
    IWritableRepository<Category>
{
}
