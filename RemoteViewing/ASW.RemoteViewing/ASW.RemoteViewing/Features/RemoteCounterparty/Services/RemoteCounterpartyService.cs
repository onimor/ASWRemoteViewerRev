using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteCounterparty.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteCounterparty;
using ASW.Shared.Requests.RemoteCounterparty;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace ASW.RemoteViewing.Features.RemoteCounterparty.Services;

public class RemoteCounterpartyService : EntityRemoteBaseService<
PgDbContext,
RemoteCounterpartyEF,
RemoteCounterpartyDto,
AddRemoteCounterpartyRequest,
UpdateRemoteCounterpartyRequest>,
IRemoteCounterpartyService
{
    public RemoteCounterpartyService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }

    protected override Task<RemoteCounterpartyEF?> FindEntityToUpdateAsync(UpdateRemoteCounterpartyRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.CounterpartyId == request.Id && !x.IsRemoved);
    }

    protected override void MapToExistingEntity(RemoteCounterpartyEF counterparty, UpdateRemoteCounterpartyRequest request)
    {
        counterparty.Name = request.Name;
        counterparty.LegalAddress = request.LegalAddress;
        counterparty.ActualAddress = request.ActualAddress;
        counterparty.Contacts = request.Contacts;
        counterparty.INN = request.INN;
        counterparty.KPP = request.KPP;
        counterparty.OGRN = request.OGRN;
    }
    protected override RemoteCounterpartyEF MapFromCreateRequest(AddRemoteCounterpartyRequest request)
    {
        return new RemoteCounterpartyEF
        {
            CounterpartyId = request.Id,
            Name = request.Name,
            LegalAddress = request.LegalAddress,
            ActualAddress = request.ActualAddress,
            Contacts = request.Contacts,
            INN = request.INN,
            KPP = request.KPP,
            OGRN = request.OGRN,
        };
    }
    protected override RemoteCounterpartyEF MapFromUpdateRequest(UpdateRemoteCounterpartyRequest request)
    {
        return new RemoteCounterpartyEF
        {
            CounterpartyId = request.Id,
            Name = request.Name,
            LegalAddress = request.LegalAddress,
            ActualAddress = request.ActualAddress,
            Contacts = request.Contacts,
            INN = request.INN,
            KPP = request.KPP,
            OGRN = request.OGRN,
        };
    }
}
