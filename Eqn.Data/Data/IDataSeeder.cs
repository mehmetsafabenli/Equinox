namespace Eqn.Data.Data;

public interface IDataSeeder
{
    Task SeedAsync(DataSeedContext context);
}
