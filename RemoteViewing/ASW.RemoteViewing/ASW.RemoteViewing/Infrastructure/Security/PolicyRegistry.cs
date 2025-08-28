using ASW.RemoteViewing.Features.Authorization;
using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Security;

namespace ASW.RemoteViewing.Infrastructure.Security;

public static class PolicyRegistry
{
    public static IEnumerable<PolicyConfig> GetAll()
    {
        var policies = new List<PolicyConfig>();
        policies.AddRange(RemoteWeighingPolicies());
        policies.AddRange(UserPolicies());
        policies.AddRange(PlaceUserPolicies());
        policies.AddRange(IntegrationUserPolicies());
        policies.AddRange(ExporterPolicies());
        policies.AddRange(RemoteAxesDistPolicies());
        policies.AddRange(RemoteAxesVelPolicies());
        policies.AddRange(RemoteAxesWeightPolicies());
        policies.AddRange(RemoteCameraWeightPolicies());
        policies.AddRange(RemoteCarPolicies());
        policies.AddRange(RemoteCounterpartyPolicies());
        policies.AddRange(RemoteDriverPolicies());
        policies.AddRange(RemoteEmptyWeighingPolicies());
        policies.AddRange(RemoteGoodPolicies());
        policies.AddRange(RemotePhotosPolicies());
        policies.AddRange(RemotePostPolicies());
        policies.AddRange(RemotePostUsersPolicies());
        policies.AddRange(RemoteTrailerPolicies());
        policies.AddRange(RemoteUserPolicies());
        policies.AddRange(RemoteWarehouseMovementPolicies());
        policies.AddRange(RemoteWeighingAdditionalFieldPolicies());
        return policies;
    }

    private static IEnumerable<PolicyConfig> RemoteWeighingPolicies() =>
    [
        new(Policies.RemoteWeighing.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteWeighing.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteWeighing.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteWeighing.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> UserPolicies() =>
    [
        new(Policies.User.CanView, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.User.CanAdd, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.User.CanEdit, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.User.CanDelete, [AuthSchemes.UserJwt], [Roles.Admin], true)
    ];
    private static IEnumerable<PolicyConfig> IntegrationUserPolicies() =>
    [
        new(Policies.IntegrationUser.CanView, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.IntegrationUser.CanAdd, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.IntegrationUser.CanEdit, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.IntegrationUser.CanDelete, [AuthSchemes.UserJwt], [Roles.Admin], true)
    ]; 
    private static IEnumerable<PolicyConfig> PlaceUserPolicies() =>
    [
        new(Policies.PlaceUser.CanView, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.PlaceUser.CanAdd, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.PlaceUser.CanEdit, [AuthSchemes.UserJwt], [Roles.Admin], true),
        new(Policies.PlaceUser.CanDelete, [AuthSchemes.UserJwt], [Roles.Admin], true)
    ];
    private static IEnumerable<PolicyConfig> ExporterPolicies() =>
    [
        new(Policies.Exporter.CanExport,
           [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
           [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
           true),
    ];
    private static IEnumerable<PolicyConfig> RemoteAxesDistPolicies() =>
    [
        new(Policies.RemoteAxesDist.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteAxesDist.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteAxesDist.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteAxesDist.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteAxesVelPolicies() =>
    [
        new(Policies.RemoteAxesVel.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteAxesVel.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteAxesVel.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteAxesVel.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteAxesWeightPolicies() =>
    [
        new(Policies.RemoteAxesWeight.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteAxesWeight.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteAxesWeight.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteAxesWeight.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteCameraWeightPolicies() =>
    [
        new(Policies.RemoteCamera.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteCamera.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteCamera.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteCamera.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteCarPolicies() =>
    [
        new(Policies.RemoteCar.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteCar.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteCar.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteCar.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteCounterpartyPolicies() =>
    [
        new(Policies.RemoteCounterparty.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteCounterparty.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteCounterparty.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteCounterparty.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteDriverPolicies() =>
    [
        new(Policies.RemoteDriver.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteDriver.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteDriver.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteDriver.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteEmptyWeighingPolicies() =>
    [
        new(Policies.RemoteEmptyWeighing.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteEmptyWeighing.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteEmptyWeighing.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteEmptyWeighing.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteGoodPolicies() =>
    [
        new(Policies.RemoteGood.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteGood.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteGood.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteGood.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemotePhotosPolicies() =>
    [
        new(Policies.RemotePhotos.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemotePhotos.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemotePhotos.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemotePhotos.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemotePostPolicies() =>
    [
        new(Policies.RemotePost.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemotePost.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemotePost.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemotePost.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemotePostUsersPolicies() =>
    [
        new(Policies.RemotePostUsers.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemotePostUsers.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemotePostUsers.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemotePostUsers.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteTrailerPolicies() =>
    [
        new(Policies.RemoteTrailer.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteTrailer.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteTrailer.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteTrailer.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteUserPolicies() =>
    [
        new(Policies.RemoteUser.CanView, [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt], [Roles.Admin, Roles.PlaceClient], true),
        new(Policies.RemoteUser.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteUser.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteUser.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteWarehouseMovementPolicies() =>
    [
        new(Policies.RemoteWarehouseMovement.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteWarehouseMovement.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteWarehouseMovement.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteWarehouseMovement.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
    private static IEnumerable<PolicyConfig> RemoteWeighingAdditionalFieldPolicies() =>
    [
        new(Policies.RemoteWeighingAdditionalField.CanView,
            [AuthSchemes.UserJwt, AuthSchemes.PlaceUserJwt, AuthSchemes.IntegrationUser],
            [Roles.Admin, Roles.Viewing, Roles.Integration, Roles.PlaceClient],
            true),
        new(Policies.RemoteWeighingAdditionalField.CanAdd, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteWeighingAdditionalField.CanEdit, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true),
        new(Policies.RemoteWeighingAdditionalField.CanDelete, [AuthSchemes.PlaceUserJwt], [Roles.PlaceClient], true)
    ];
}
