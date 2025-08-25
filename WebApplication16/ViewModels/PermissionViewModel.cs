namespace WebApplication16.ViewModels
{
    public class PermissionViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleClaimViewModel> RoleClaims { get; set; }
    }

    public class RoleClaimViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}