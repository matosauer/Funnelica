namespace Persistence.Identity;

public static class IdentityRoles
{
    public const string Administrator = "Administrator";
    public const string PowerUser = "PowerUser";

    public static readonly IReadOnlyList<string> All =
    [
        Administrator,
        PowerUser
    ];
}
