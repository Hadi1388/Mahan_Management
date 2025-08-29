using installments_Payment.DataAccessLayer.Entities.Installments;
using installments_Payment.DataAccessLayer.Entities.Inventories;
using installments_Payment.DataAccessLayer.Entities.Requests;
using installments_Payment.DataAccessLayer.Entities.Treatments;
using installments_Payment.DataAccessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.DataAccessLayer.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentType> TreatmentTypes { get; set; }
        public DbSet<RequestGuaranteeFile> RequestGuaranteeFiles { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<InstallmentPenalty> InstallmentPenalties { get; set; }
        public DbSet<TreatmentProcess> TreatmentProcesses { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Commodity)
                .WithMany()
                .HasForeignKey(oi => oi.CommodityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
