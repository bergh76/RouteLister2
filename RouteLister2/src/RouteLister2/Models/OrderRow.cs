namespace RouteLister2.Models
{
    public class OrderRow
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; }
        public string PackageId { get; set; }
        public Parcel Parcel { get; set; }
        public int ParcelId { get; set; }
        public OrderRowStatus OrderRowStatus {get;set;}
        public int OrderRowStatusId { get; set; }
    }
}