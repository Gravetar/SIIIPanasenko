using System.ComponentModel.DataAnnotations.Schema;

namespace SIIILab2.Models
{
    public class RequestRoad
    {
        public int Id { get; set; }
        public string? Costumer { get; set; }
        public DateTime Reques_date { get; set; }
        public string? Status {  get; set; }
        public string? Result { get; set; }

        [ForeignKey("Road")]
        public int roadid { get; set; }
        public Road road { get; set; }
    }
}
