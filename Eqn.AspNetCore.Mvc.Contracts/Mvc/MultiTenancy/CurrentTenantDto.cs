namespace Eqn.AspNetCore.Mvc.Contracts.Mvc.MultiTenancy;

public class CurrentTenantDto
{
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public bool IsAvailable { get; set; }
}
