using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace SIIILab2.Models
{
    public class Road
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? PathFile { get; set; }
        public double CoorLatitude1 { get; set; }
        public double CoorLongitude1 { get; set; }
        public double CoorLatitude2 { get; set; }
        public double CoorLongitude2 { get; set; }

        [ForeignKey("Traffic")]
        public int trafficid { get; set; }
        public Traffic traffic { get; set; }

    }
}
