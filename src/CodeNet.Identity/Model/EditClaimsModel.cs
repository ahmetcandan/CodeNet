using System.Security.Claims;

namespace CodeNet.Identity.Settings;

public class EditClaimsModel
{
    public string Type { get; set; }
    public string Value { get; set; }

    public Claim GetClaim()
    {
        return new(Type, Value);
    }
}
