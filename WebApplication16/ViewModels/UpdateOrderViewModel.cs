using System.Collections.Generic;

namespace WebApplication16.ViewModels
{
    public class UpdateOrderViewModel
    {
        public int FormId { get; set; }
        public List<int> FieldIds { get; set; } = new List<int>();
    }
}
