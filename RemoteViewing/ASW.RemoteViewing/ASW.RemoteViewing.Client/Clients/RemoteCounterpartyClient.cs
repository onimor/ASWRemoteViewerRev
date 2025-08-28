using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteCounterparty;
using ASW.Shared.Requests.RemoteCounterparty;

namespace ASW.RemoteViewing.Client.Clients
{
    public class RemoteCounterpartyClient(SafeHttpClient httpClient)
    {
        private readonly SafeHttpClient _httpClient = httpClient;
        private const string BaseRoute = "/api/v1/ReferenceBooks/Counterparties"; 

        public async Task<List<RemoteCounterpartyDto>?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetAsync<List<RemoteCounterpartyDto>>(BaseRoute, cancellationToken);
        }

        public async Task<RemoteCounterpartyDto?> GetByIdAsync(Guid counterpartyId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetAsync<RemoteCounterpartyDto>($"{BaseRoute}/{counterpartyId}", cancellationToken);
        }

        public async Task<RemoteCounterpartyDto?> CreateAsync(AddRemoteCounterpartyRequest request, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsync<AddRemoteCounterpartyRequest, RemoteCounterpartyDto>(BaseRoute, request, cancellationToken);
        }

        public async Task UpdateAsync(UpdateRemoteCounterpartyRequest request, CancellationToken cancellationToken = default)
        {
            await _httpClient.PutAsync(BaseRoute, request, cancellationToken);
        }

        public async Task DeleteAsync(Guid counterpartyId, CancellationToken cancellationToken = default)
        {
            await _httpClient.DeleteAsync($"{BaseRoute}/{counterpartyId}", cancellationToken);
        }
    }
}
