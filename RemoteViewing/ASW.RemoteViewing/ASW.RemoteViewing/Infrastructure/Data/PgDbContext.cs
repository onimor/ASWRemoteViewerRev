using ASW.RemoteViewing.Features.IntegrationUser.Entities;
using ASW.RemoteViewing.Features.PlaceUser.Entities;
using ASW.RemoteViewing.Features.RemoteActionLog.Entities;
using ASW.RemoteViewing.Features.RemoteAdditionalField.Entities;
using ASW.RemoteViewing.Features.RemoteAxesDist.Entities;
using ASW.RemoteViewing.Features.RemoteAxesVel.Entities;
using ASW.RemoteViewing.Features.RemoteAxesWeight.Entities;
using ASW.RemoteViewing.Features.RemoteCamera.Entities;
using ASW.RemoteViewing.Features.RemoteCar.Entities;
using ASW.RemoteViewing.Features.RemoteCounterparty.Entities;
using ASW.RemoteViewing.Features.RemoteDriver.Entities;
using ASW.RemoteViewing.Features.RemoteEmptyWeighing.Entities;
using ASW.RemoteViewing.Features.RemoteGood.Entities;
using ASW.RemoteViewing.Features.RemotePhotos.Entities;
using ASW.RemoteViewing.Features.RemotePost.Entities;
using ASW.RemoteViewing.Features.RemotePostUsers.Entities;
using ASW.RemoteViewing.Features.RemoteTrailer.Entities;
using ASW.RemoteViewing.Features.RemoteWarehouseMovement.Entities;
using ASW.RemoteViewing.Features.RemoteWeighing.Entities;
using ASW.RemoteViewing.Features.RemoteWeighingAdditionalField.Entities;
using ASW.RemoteViewing.Features.User.Entities;
using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Security;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace ASW.RemoteViewing.Infrastructure.Data;

public class PgDbContext : DbContext
{
    public PgDbContext(DbContextOptions<PgDbContext> options) : base(options) { } 
    #region конструктор 
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var builder = new NpgsqlDbContextOptionsBuilder(options);
            builder.SetPostgresVersion(new Version(9, 4));
        }
    }
    #endregion

    #region Public properties
    public virtual DbSet<UserEF> Users { get; set; }
    public virtual DbSet<IntegrationUserEF> IntegrationUsers { get; set; }
    public virtual DbSet<PlaceUserEF> PlaceUsers { get; set; }
    public virtual DbSet<RemoteActionLogEF> RemoteActionLogs { get; set; }
    public virtual DbSet<RemotePostEF> RemotePosts { get; set; }
    public virtual DbSet<RemotePostUserEF> RemotePostUsers { get; set; }
    public virtual DbSet<RemoteCameraEF> RemoteCameras { get; set; }
    public virtual DbSet<RemoteCarEF> RemoteCars { get; set; }
    public virtual DbSet<RemoteCounterpartyEF> RemoteCounterparties { get; set; }
    public virtual DbSet<RemoteDriverEF> RemoteDrivers { get; set; }
    public virtual DbSet<RemoteGoodEF> RemoteGoods { get; set; }
    public virtual DbSet<RemoteTrailerEF> RemoteTrailers { get; set; }
    public virtual DbSet<RemoteAdditionalFieldEF> RemoteAdditionalFields { get; set; }
    public virtual DbSet<RemoteWeighingAdditionalFieldEF> RemoteWeighingAdditionalFields { get; set; }
    public virtual DbSet<RemoteWeighingEF> RemoteWeightings { get; set; }
    public virtual DbSet<RemotePhotoTaraEF> RemotePhotosTara { get; set; }
    public virtual DbSet<RemotePhotoBruttoEF> RemotePhotosBrutto { get; set; }
    public virtual DbSet<RemoteEmptyWeighingPhotoEF> RemoteEmptyWeighingPhotos { get; set; }
    public virtual DbSet<RemoteEmptyWeighingEF> RemoteEmptyWeightings { get; set; }
    public virtual DbSet<RemoteWarehouseMovementsEF> RemoteWarehouseMovements { get; set; }
    public virtual DbSet<RemoteAxesWeightEF> RemoteAxesWeights { get; set; }
    public virtual DbSet<RemoteAxesVelEF> RemoteAxesVels { get; set; }
    public virtual DbSet<RemoteAxesDistEF> RemoteAxesDists { get; set; }
    #endregion

    #region Overridden method
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEF>().HasData(GetUsers());
        base.OnModelCreating(modelBuilder);
    }
    #endregion


    #region Private method 
    private List<UserEF> GetUsers()
    {
        var user = new UserEF { Id = Guid.NewGuid(), Login = "admin", FullFIO = "admin", ReductionFIO = "admin", Role = Roles.Admin, IsRemoved = false };
        user.Password = PassHelper<UserEF>.GetHash(user, "123"); 
        return [user];
    }
    #endregion
}

