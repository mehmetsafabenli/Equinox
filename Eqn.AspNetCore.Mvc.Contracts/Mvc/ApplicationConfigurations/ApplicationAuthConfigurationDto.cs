namespace Eqn.AspNetCore.Mvc.Contracts.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationAuthConfigurationDto
{
    public Dictionary<string, bool> GrantedPolicies { get; set; }

    public ApplicationAuthConfigurationDto()
    {
        GrantedPolicies = new Dictionary<string, bool>();
    }
}
