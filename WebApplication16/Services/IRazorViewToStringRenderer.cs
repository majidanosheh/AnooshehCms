using System.Threading.Tasks;

namespace WebApplication16.Services
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}

