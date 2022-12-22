using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class UltraTixDBContext : DbContext
    {
        public UltraTixDBContext()
        {
        }

        public UltraTixDBContext(DbContextOptions<UltraTixDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppTransaction> AppTransactions { get; set; } = null!;
        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<ArtistFollower> ArtistFollowers { get; set; } = null!;
        public virtual DbSet<ArtistRequest> ArtistRequests { get; set; } = null!;
        public virtual DbSet<ArtistWallet> ArtistWallets { get; set; } = null!;
        public virtual DbSet<ArtistWithdrawTransaction> ArtistWithdrawTransactions { get; set; } = null!;
        public virtual DbSet<Campaign> Campaigns { get; set; } = null!;
        public virtual DbSet<CampaignDetail> CampaignDetails { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerPurchaseTransaction> CustomerPurchaseTransactions { get; set; } = null!;
        public virtual DbSet<CustomerWallet> CustomerWallets { get; set; } = null!;
        public virtual DbSet<CustomerWithdrawTransaction> CustomerWithdrawTransactions { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<FeedbackType> FeedbackTypes { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<MoMoResponse> MoMoResponses { get; set; } = null!;
        public virtual DbSet<Organizer> Organizers { get; set; } = null!;
        public virtual DbSet<OrganizerWallet> OrganizerWallets { get; set; } = null!;
        public virtual DbSet<OrganizerWithdrawTransaction> OrganizerWithdrawTransactions { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostComment> PostComments { get; set; } = null!;
        public virtual DbSet<PostFollower> PostFollowers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SaleStage> SaleStages { get; set; } = null!;
        public virtual DbSet<SaleStageDetail> SaleStageDetails { get; set; } = null!;
        public virtual DbSet<Show> Shows { get; set; } = null!;
        public virtual DbSet<ShowCategory> ShowCategories { get; set; } = null!;
        public virtual DbSet<ShowOrder> ShowOrders { get; set; } = null!;
        public virtual DbSet<ShowOrderDetail> ShowOrderDetails { get; set; } = null!;
        public virtual DbSet<ShowRequest> ShowRequests { get; set; } = null!;
        public virtual DbSet<ShowReview> ShowReviews { get; set; } = null!;
        public virtual DbSet<ShowStaff> ShowStaffs { get; set; } = null!;
        public virtual DbSet<ShowTicket> ShowTickets { get; set; } = null!;
        public virtual DbSet<ShowTransactionHisotry> ShowTransactionHisotries { get; set; } = null!;
        public virtual DbSet<ShowType> ShowTypes { get; set; } = null!;
        public virtual DbSet<StaffShowDetail> StaffShowDetails { get; set; } = null!;
        public virtual DbSet<SysAdminWithdrawTransaction> SysAdminWithdrawTransactions { get; set; } = null!;
        public virtual DbSet<SystemAdmin> SystemAdmins { get; set; } = null!;
        public virtual DbSet<SystemAdminWallet> SystemAdminWallets { get; set; } = null!;
        public virtual DbSet<SystemToArtistWalletTransaction> SystemToArtistWalletTransactions { get; set; } = null!;
        public virtual DbSet<SystemToOrganizerTransaction> SystemToOrganizerTransactions { get; set; } = null!;
        public virtual DbSet<TicketType> TicketTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTransaction>(entity =>
            {
                entity.ToTable("AppTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("PaymentID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.AppTransactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__AppTransa__Payme__5D95E53A");
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUser");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'Nguyễn Văn A')");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AppUser__RoleID__5E8A0973");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artist");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.AvatarImgUrl)
                    .HasMaxLength(500)
                    .HasColumnName("AvatarImgURL");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Artist)
                    .HasForeignKey<Artist>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Artist__ID__5F7E2DAC");
            });

            modelBuilder.Entity<ArtistFollower>(entity =>
            {
                entity.ToTable("ArtistFollower");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.FollowerId).HasColumnName("FollowerID");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistFollowers)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArtistFol__Artis__5B438874");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.ArtistFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArtistFol__Follo__5C37ACAD");
            });

            modelBuilder.Entity<ArtistRequest>(entity =>
            {
                entity.ToTable("ArtistRequest");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.IdissueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDIssueDate");

                entity.Property(e => e.Idlocation)
                    .HasMaxLength(200)
                    .HasColumnName("IDLocation");

                entity.Property(e => e.Idnumber)
                    .HasMaxLength(50)
                    .HasColumnName("IDNumber");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Pending')");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ArtistRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("ArtistRequest_AppUser_ID_fk");
            });

            modelBuilder.Entity<ArtistWallet>(entity =>
            {
                entity.ToTable("ArtistWallet");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(500);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OwnerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletType).HasMaxLength(50);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.ArtistWallets)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArtistWal__Owner__607251E5");
            });

            modelBuilder.Entity<ArtistWithdrawTransaction>(entity =>
            {
                entity.ToTable("ArtistWithdrawTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArtistWalletId)
                    .HasColumnName("ArtistWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.Property(e => e.WalletArtistReceiverId)
                    .HasColumnName("WalletArtistReceiverID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletReceiverDetail).HasMaxLength(500);

                entity.HasOne(d => d.ArtistWallet)
                    .WithMany(p => p.ArtistWithdrawTransactions)
                    .HasForeignKey(d => d.ArtistWalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArtistWit__Artis__6166761E");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ArtistWithdrawTransaction)
                    .HasForeignKey<ArtistWithdrawTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArtistWithdr__ID__625A9A57");
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("ArtistID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BookingLink).HasMaxLength(1000);

                entity.Property(e => e.ShowId)
                    .HasColumnName("ShowID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Campaign__Artist__634EBE90");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Campaign__ShowID__6442E2C9");
            });

            modelBuilder.Entity<CampaignDetail>(entity =>
            {
                entity.ToTable("CampaignDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CampaignDescription).HasMaxLength(500);

                entity.Property(e => e.CampaignEndDate).HasColumnType("datetime");

                entity.Property(e => e.CampaignId)
                    .HasColumnName("CampaignID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CampaignName).HasMaxLength(50);

                entity.Property(e => e.CampaignStartDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerDiscount).HasDefaultValueSql("((0))");

                entity.Property(e => e.SaleStageDetailId).HasColumnName("SaleStageDetailID");

                entity.Property(e => e.TicketBookingPageLink).HasMaxLength(500);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignDetails)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CampaignD__Campa__65370702");

                entity.HasOne(d => d.SaleStageDetail)
                    .WithMany(p => p.CampaignDetails)
                    .HasForeignKey(d => d.SaleStageDetailId)
                    .HasConstraintName("FK__CampaignD__SaleS__13BCEBC1");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.AvatarImgUrl)
                    .HasMaxLength(500)
                    .HasColumnName("AvatarImgURL");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Customer__ID__681373AD");
            });

            modelBuilder.Entity<CustomerPurchaseTransaction>(entity =>
            {
                entity.ToTable("CustomerPurchaseTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CustomerWalletId)
                    .HasColumnName("CustomerWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ShowOrderId).HasColumnName("ShowOrderID");

                entity.Property(e => e.SystemWalletId)
                    .HasColumnName("SystemWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.CustomerPurchaseTransaction)
                    .HasForeignKey<CustomerPurchaseTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerPurc__ID__7073AF84");

                entity.HasOne(d => d.ShowOrder)
                    .WithMany(p => p.CustomerPurchaseTransactions)
                    .HasForeignKey(d => d.ShowOrderId)
                    .HasConstraintName("FK__CustomerP__ShowO__038683F8");
            });

            modelBuilder.Entity<CustomerWallet>(entity =>
            {
                entity.ToTable("CustomerWallet");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(500);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OwnerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletType).HasMaxLength(50);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.CustomerWallets)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerW__Owner__6AEFE058");
            });

            modelBuilder.Entity<CustomerWithdrawTransaction>(entity =>
            {
                entity.ToTable("CustomerWithdrawTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CustomerWalletId)
                    .HasColumnName("CustomerWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.Property(e => e.WalletCustomerReceiverId)
                    .HasColumnName("WalletCustomerReceiverID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletReceiverDetail).HasMaxLength(500);

                entity.HasOne(d => d.CustomerWallet)
                    .WithMany(p => p.CustomerWithdrawTransactions)
                    .HasForeignKey(d => d.CustomerWalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerW__Custo__6BE40491");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.CustomerWithdrawTransaction)
                    .HasForeignKey<CustomerWithdrawTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerWith__ID__6CD828CA");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.FeedbackTypeId).HasColumnName("FeedbackTypeID");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ProblemBrief).HasMaxLength(200);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.FeedbackType)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.FeedbackTypeId)
                    .HasConstraintName("Feedback_FeedbackType_null_fk");
            });

            modelBuilder.Entity<FeedbackType>(entity =>
            {
                entity.ToTable("FeedbackType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TypeDiscription).HasMaxLength(200);

                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.LocationDescription).HasMaxLength(500);

                entity.Property(e => e.ShowId)
                    .HasColumnName("ShowID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Location__ShowID__6DCC4D03");
            });

            modelBuilder.Entity<MoMoResponse>(entity =>
            {
                entity.ToTable("MoMoResponse");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasMaxLength(200);

                entity.Property(e => e.Message).HasMaxLength(200);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(200)
                    .HasColumnName("OrderID");

                entity.Property(e => e.OrderInfo).HasMaxLength(200);

                entity.Property(e => e.OrderType).HasMaxLength(200);

                entity.Property(e => e.PartnerCode).HasMaxLength(200);

                entity.Property(e => e.PayType).HasMaxLength(200);

                entity.Property(e => e.RequestId)
                    .HasMaxLength(200)
                    .HasColumnName("RequestID");

                entity.Property(e => e.ResponseTime).HasMaxLength(200);

                entity.Property(e => e.ResultCode).HasMaxLength(200);

                entity.Property(e => e.Signature).HasMaxLength(200);

                entity.Property(e => e.TransId)
                    .HasMaxLength(200)
                    .HasColumnName("TransID");
            });

            modelBuilder.Entity<Organizer>(entity =>
            {
                entity.ToTable("Organizer");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.AvatarImgUrl)
                    .HasMaxLength(500)
                    .HasColumnName("AvatarImgURL");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.TaxLocation).HasMaxLength(200);

                entity.Property(e => e.TaxNumber).HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Organizer)
                    .HasForeignKey<Organizer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Organizer__ID__6EC0713C");
            });

            modelBuilder.Entity<OrganizerWallet>(entity =>
            {
                entity.ToTable("OrganizerWallet");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(500);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OwnerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletType).HasMaxLength(50);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.OrganizerWallets)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Organizer__Owner__6FB49575");
            });

            modelBuilder.Entity<OrganizerWithdrawTransaction>(entity =>
            {
                entity.ToTable("OrganizerWithdrawTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.OrganizerWalletId)
                    .HasColumnName("OrganizerWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.Property(e => e.WalletOrganizerReceiverId)
                    .HasColumnName("WalletOrganizerReceiverID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletReceiverDetail).HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.OrganizerWithdrawTransaction)
                    .HasForeignKey<OrganizerWithdrawTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrganizerWit__ID__719CDDE7");

                entity.HasOne(d => d.OrganizerWallet)
                    .WithMany(p => p.OrganizerWithdrawTransactions)
                    .HasForeignKey(d => d.OrganizerWalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Organizer__Organ__70A8B9AE");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PaymentDescription).HasMaxLength(500);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LikeCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.PostImageUrl)
                    .HasMaxLength(500)
                    .HasColumnName("PostImageURL");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Public')");

                entity.Property(e => e.TicketBookingPageLink).HasMaxLength(500);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Post_Artist_null_fk");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.ToTable("PostComment");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CommentTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CommentorId).HasColumnName("CommentorID");

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.HasOne(d => d.Commentor)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.CommentorId)
                    .HasConstraintName("PostComment_AppUser_null_fk");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("PostComment_Post_null_fk");
            });

            modelBuilder.Entity<PostFollower>(entity =>
            {
                entity.ToTable("PostFollower");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PostId)
                    .HasColumnName("PostID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.PostFollowers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostFollo__Custo__74794A92");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostFollowers)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostFollo__PostI__756D6ECB");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.RoleDescription).HasMaxLength(500);
            });

            modelBuilder.Entity<SaleStage>(entity =>
            {
                entity.ToTable("SaleStage");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SaleStageDescription).HasMaxLength(500);

                entity.Property(e => e.SaleStageEndDate).HasColumnType("datetime");

                entity.Property(e => e.SaleStageOrder).HasMaxLength(50);

                entity.Property(e => e.SaleStageStartDate).HasColumnType("datetime");

                entity.Property(e => e.ShowId)
                    .HasColumnName("ShowID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.SaleStages)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SaleStage__ShowI__76619304");
            });

            modelBuilder.Entity<SaleStageDetail>(entity =>
            {
                entity.ToTable("SaleStageDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SaleStageId).HasColumnName("SaleStageID");

                entity.Property(e => e.TicketTypeId).HasColumnName("TicketTypeID");

                entity.Property(e => e.TicketTypeNormalUnitSold).HasDefaultValueSql("((0))");

                entity.Property(e => e.TicketTypeViaLinkUnitSold).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.SaleStage)
                    .WithMany(p => p.SaleStageDetails)
                    .HasForeignKey(d => d.SaleStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SaleStage__SaleS__0EF836A4");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.SaleStageDetails)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SaleStage__Ticke__0FEC5ADD");
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.ToTable("Show");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DescriptionImageUrl).HasMaxLength(200);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(200)
                    .HasColumnName("ImgURL");

                entity.Property(e => e.IsTransferRevenueToOrganizer).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShowEndDate).HasColumnType("datetime");

                entity.Property(e => e.ShowName).HasMaxLength(200);

                entity.Property(e => e.ShowOrganizerId)
                    .HasColumnName("ShowOrganizerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ShowStartDate).HasColumnType("datetime");

                entity.Property(e => e.ShowTypeId)
                    .HasColumnName("ShowTypeID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Step).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Shows)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Show__CategoryID__589C25F3");

                entity.HasOne(d => d.ShowOrganizer)
                    .WithMany(p => p.Shows)
                    .HasForeignKey(d => d.ShowOrganizerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Show__ShowOrgani__7755B73D");

                entity.HasOne(d => d.ShowType)
                    .WithMany(p => p.Shows)
                    .HasForeignKey(d => d.ShowTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Show__ShowTypeID__7849DB76");
            });

            modelBuilder.Entity<ShowCategory>(entity =>
            {
                entity.ToTable("ShowCategory");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.ShowTypeId).HasColumnName("ShowTypeID");

                entity.HasOne(d => d.ShowType)
                    .WithMany(p => p.ShowCategories)
                    .HasForeignKey(d => d.ShowTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowCateg__ShowT__54CB950F");
            });

            modelBuilder.Entity<ShowOrder>(entity =>
            {
                entity.ToTable("ShowOrder");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsUsed).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDescription).HasMaxLength(500);

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.ShowOrders)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK__ShowOrder__Campa__00AA174D");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ShowOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowOrder__Custo__7A3223E8");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.ShowOrders)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowOrder__ShowI__318258D2");
            });

            modelBuilder.Entity<ShowOrderDetail>(entity =>
            {
                entity.ToTable("ShowOrderDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CampaignDetailId).HasColumnName("CampaignDetailID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.SaleStageDetailId).HasColumnName("SaleStageDetailID");

                entity.Property(e => e.ShowOrderId).HasColumnName("ShowOrderID");

                entity.HasOne(d => d.CampaignDetail)
                    .WithMany(p => p.ShowOrderDetails)
                    .HasForeignKey(d => d.CampaignDetailId)
                    .HasConstraintName("FK__ShowOrder__Campa__74444068");

                entity.HasOne(d => d.SaleStageDetail)
                    .WithMany(p => p.ShowOrderDetails)
                    .HasForeignKey(d => d.SaleStageDetailId)
                    .HasConstraintName("FK__ShowOrder__SaleS__12C8C788");

                entity.HasOne(d => d.ShowOrder)
                    .WithMany(p => p.ShowOrderDetails)
                    .HasForeignKey(d => d.ShowOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShowOrderDetail_ShowOrder");
            });

            modelBuilder.Entity<ShowRequest>(entity =>
            {
                entity.ToTable("ShowRequest");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Message).HasMaxLength(500);

                entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.Property(e => e.ShowStaffId).HasColumnName("ShowStaffID");

                entity.Property(e => e.State).HasMaxLength(50);

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.ShowRequests)
                    .HasForeignKey(d => d.OrganizerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowReque__Organ__22401542");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.ShowRequests)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowReque__ShowI__214BF109");

                entity.HasOne(d => d.ShowStaff)
                    .WithMany(p => p.ShowRequests)
                    .HasForeignKey(d => d.ShowStaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowReque__ShowS__2057CCD0");
            });

            modelBuilder.Entity<ShowReview>(entity =>
            {
                entity.ToTable("ShowReview");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateTimeReview).HasColumnType("datetime");

                entity.Property(e => e.Rate).HasDefaultValueSql("((0.0))");

                entity.Property(e => e.ReviewMessage).HasMaxLength(1000);

                entity.Property(e => e.ReviewerId)
                    .HasColumnName("ReviewerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ReviewerName).HasMaxLength(500);

                entity.Property(e => e.ShowId)
                    .HasColumnName("ShowID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.ShowReviews)
                    .HasForeignKey(d => d.ReviewerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowRevie__Revie__2C88998B");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.ShowReviews)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowRevie__ShowI__2AA05119");
            });

            modelBuilder.Entity<ShowStaff>(entity =>
            {
                entity.ToTable("ShowStaff");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.AvatarImgUrl)
                    .HasMaxLength(500)
                    .HasColumnName("AvatarImgURL");

                entity.Property(e => e.OrganizerId)
                    .HasColumnName("OrganizerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ShowStaff)
                    .HasForeignKey<ShowStaff>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowStaff__ID__7C1A6C5A");

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.ShowStaffs)
                    .HasForeignKey(d => d.OrganizerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowStaff__Organ__7B264821");
            });

            modelBuilder.Entity<ShowTicket>(entity =>
            {
                entity.ToTable("ShowTicket");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CampaignDetailId).HasColumnName("CampaignDetailID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.SaleStageId).HasColumnName("SaleStageID");

                entity.Property(e => e.ShowOrderDetailId).HasColumnName("ShowOrderDetailID");

                entity.Property(e => e.TicketTypeId).HasColumnName("TicketTypeID");

                entity.HasOne(d => d.CampaignDetail)
                    .WithMany(p => p.ShowTickets)
                    .HasForeignKey(d => d.CampaignDetailId)
                    .HasConstraintName("FK__ShowTicke__Campa__7AF13DF7");

                entity.HasOne(d => d.SaleStage)
                    .WithMany(p => p.ShowTickets)
                    .HasForeignKey(d => d.SaleStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowTicke__SaleS__7BE56230");

                entity.HasOne(d => d.ShowOrderDetail)
                    .WithMany(p => p.ShowTickets)
                    .HasForeignKey(d => d.ShowOrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowTicke__ShowO__7CD98669");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.ShowTickets)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowTicke__Ticke__7DCDAAA2");
            });

            modelBuilder.Entity<ShowTransactionHisotry>(entity =>
            {
                entity.ToTable("ShowTransactionHisotry");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArtistCommission).HasDefaultValueSql("((0))");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Revenue).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.Property(e => e.ShowName).HasMaxLength(200);

                entity.Property(e => e.ShowOrderId).HasColumnName("ShowOrderID");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.ShowTransactionHisotries)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK__ShowTrans__Campa__1A69E950");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ShowTransactionHisotries)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowTrans__Custo__1975C517");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.ShowTransactionHisotries)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowTrans__ShowI__178D7CA5");

                entity.HasOne(d => d.ShowOrder)
                    .WithMany(p => p.ShowTransactionHisotries)
                    .HasForeignKey(d => d.ShowOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowTrans__ShowO__1881A0DE");
            });

            modelBuilder.Entity<ShowType>(entity =>
            {
                entity.ToTable("ShowType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ShowTypeDescription).HasMaxLength(500);

                entity.Property(e => e.ShowTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<StaffShowDetail>(entity =>
            {
                entity.ToTable("StaffShowDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ShowId)
                    .HasColumnName("ShowID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ShowStaffId)
                    .HasColumnName("ShowStaffID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.StaffShowDetails)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StaffShow__ShowI__7D0E9093");

                entity.HasOne(d => d.ShowStaff)
                    .WithMany(p => p.StaffShowDetails)
                    .HasForeignKey(d => d.ShowStaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StaffShow__ShowS__7E02B4CC");
            });

            modelBuilder.Entity<SysAdminWithdrawTransaction>(entity =>
            {
                entity.ToTable("SysAdminWithdrawTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SystemAdminWalletId)
                    .HasColumnName("SystemAdminWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.Property(e => e.WalletReceiverDetail).HasMaxLength(500);

                entity.Property(e => e.WalletSysAdminReceiverId)
                    .HasColumnName("WalletSysAdminReceiverID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.SysAdminWithdrawTransaction)
                    .HasForeignKey<SysAdminWithdrawTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SysAdminWith__ID__7FEAFD3E");

                entity.HasOne(d => d.SystemAdminWallet)
                    .WithMany(p => p.SysAdminWithdrawTransactions)
                    .HasForeignKey(d => d.SystemAdminWalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SysAdminW__Syste__7EF6D905");
            });

            modelBuilder.Entity<SystemAdmin>(entity =>
            {
                entity.ToTable("SystemAdmin");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.SystemAdmin)
                    .HasForeignKey<SystemAdmin>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SystemAdmin__ID__00DF2177");
            });

            modelBuilder.Entity<SystemAdminWallet>(entity =>
            {
                entity.ToTable("SystemAdminWallet");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(500);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OwnerID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.WalletType).HasMaxLength(50);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.SystemAdminWallets)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SystemAdm__Owner__01D345B0");
            });

            modelBuilder.Entity<SystemToArtistWalletTransaction>(entity =>
            {
                entity.ToTable("SystemToArtistWalletTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArtistWalletId)
                    .HasColumnName("ArtistWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SystemWalletId)
                    .HasColumnName("SystemWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.SystemToArtistWalletTransaction)
                    .HasForeignKey<SystemToArtistWalletTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SystemToArti__ID__02C769E9");
            });

            modelBuilder.Entity<SystemToOrganizerTransaction>(entity =>
            {
                entity.ToTable("SystemToOrganizerTransaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.OrganizerWalletId)
                    .HasColumnName("OrganizerWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SystemWalletId)
                    .HasColumnName("SystemWalletID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDescription).HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.SystemToOrganizerTransaction)
                    .HasForeignKey<SystemToOrganizerTransaction>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SystemToOrga__ID__03BB8E22");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.ToTable("TicketType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ShowId)
                    .HasColumnName("ShowID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TicketTypeDescription).HasMaxLength(50);

                entity.Property(e => e.TicketTypeName).HasMaxLength(50);

                entity.Property(e => e.TicketTypeNormalUnitSold).HasDefaultValueSql("((0))");

                entity.Property(e => e.TicketTypeViaLinkUnitSold).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.TicketTypes)
                    .HasForeignKey(d => d.ShowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketTyp__ShowI__078C1F06");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
