using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace otel.models.EF {
    public class AppDb : DbContext {
        public AppDb(DbContextOptions<AppDb> options) : base(options) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<TickerModel> TickerModels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }

    public class TickerModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Precision(25, 9)]
        public decimal Price { get; set; }
        [Precision(25, 9)]
        public decimal AdjustPrice { get; set; }
        [Precision(25, 9)]
        public decimal Size { get; set; }
        [Precision(25, 9)]
        public decimal RemainSize { get; set; }
        public int TickerTemplateModelId { get; set; }
    }
}
