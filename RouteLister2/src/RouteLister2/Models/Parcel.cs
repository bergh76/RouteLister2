namespace RouteLister2.Models
{
    public class Parcel
    {
        public int Id { get; set; }
        public string ParcelNumber { get; set; }
        public string Name { get;set;}
        public bool PickedStatus { get; set; }
        public string Distributor { get; set; }

        //public ParcelStatus ParcelStatus { get; set; }
        //public int ParcelStatusId { get; set; }

    }
}