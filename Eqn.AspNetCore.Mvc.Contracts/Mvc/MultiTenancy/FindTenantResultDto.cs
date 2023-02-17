namespace Eqn.AspNetCore.Mvc.Contracts.Mvc.MultiTenancy;

[Serializable]
public class FindTenantResultDto
{
    public bool Success { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }
}
