using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerWallets = new HashSet<CustomerWallet>();
            PostFollowers = new HashSet<PostFollower>();
            ShowOrders = new HashSet<ShowOrder>();
            ShowReviews = new HashSet<ShowReview>();
            ShowTransactionHisotries = new HashSet<ShowTransactionHisotry>();
        }

        public Guid Id { get; set; }
        public string? AvatarImgUrl { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual AppUser IdNavigation { get; set; } = null!;
        public virtual ICollection<CustomerWallet> CustomerWallets { get; set; }
        public virtual ICollection<PostFollower> PostFollowers { get; set; }
        public virtual ICollection<ShowOrder> ShowOrders { get; set; }
        public virtual ICollection<ShowReview> ShowReviews { get; set; }
        public virtual ICollection<ShowTransactionHisotry> ShowTransactionHisotries { get; set; }
    }
}
