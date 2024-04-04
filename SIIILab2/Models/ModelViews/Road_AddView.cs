using System.ComponentModel.DataAnnotations.Schema;

namespace SIIILab2.Models.ModelViews
{
    public class Road_AddView
    {
        public string? address { get; set; }
        public string? pathFile { get; set; }
        public double coorLatitude1 { get; set; }
        public double coorLongitude1 { get; set; }
        public double coorLatitude2 { get; set; }
        public double coorLongitude2 { get; set; }
        public int trafficid { get; set; }
    }
}
