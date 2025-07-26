using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.Common.Interfaces.RepositoryCategory;
public interface ICategoryRepo : 
    IQueryableRepository<Category>,
    IProjectableRepository<Category>,
    IWritableRepository<Category>
{
}
