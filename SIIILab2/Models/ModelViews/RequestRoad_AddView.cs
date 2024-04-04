using System.ComponentModel.DataAnnotations.Schema;

namespace SIIILab2.Models.ModelViews
{
    public class RequestRoad_AddView
    {
        public string? costumer { get; set; }
        public string? status { get; set; }
        public string? result { get; set; }
        public int roadid { get; set; }
    }
}
