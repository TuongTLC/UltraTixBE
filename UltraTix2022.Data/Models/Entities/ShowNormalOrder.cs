namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowNormalOrder
    {
        public ShowNormalOrder()
        {
            ShowNormalOrderDetails = new HashSet<ShowNormalOrderDetail>();
        }

        public Guid Id { get; set; }
        public Guid ShowId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public string Description { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
        public virtual Show Show { get; set; } = null!;
        public virtual ICollection<ShowNormalOrderDetail> ShowNormalOrderDetails { get; set; }
    }
}
