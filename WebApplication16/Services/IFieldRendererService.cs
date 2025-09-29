using WebApplication16.Models;

namespace WebApplication16.Services
{
    public interface IFieldRendererService
    {
        string RenderField(FormField field, string namePrefix = "");
    }
}
