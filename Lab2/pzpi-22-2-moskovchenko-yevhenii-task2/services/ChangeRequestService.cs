using Microsoft.Extensions.Localization;

public class ChangeRequestService
{
    private readonly IChangeRequestRepository _changeRequestRepository;
    private readonly IUserChangeRequestRepository _userChangeRequestRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ChangeRequestService(
        IChangeRequestRepository changeRequestRepository,
        IUserChangeRequestRepository userChangeRequestRepository,
        IStringLocalizer<SharedResources> localizer)
    {
        _changeRequestRepository = changeRequestRepository;
        _userChangeRequestRepository = userChangeRequestRepository;
        _localizer = localizer;
    }

    public async Task<ChangeRequest> AddChangeRequestAsync(ChangeRequest changeRequest, int userId)
    {
        var createdRequest = await _changeRequestRepository.AddAsync(changeRequest);

        var userChangeRequest = new UserChangeRequest
        {
            UserId = userId,
            ChangeRequestId = createdRequest.ChangeRequestId
        };

        await _userChangeRequestRepository.AddAsync(userChangeRequest);
        return createdRequest;
    }

    public async Task<IEnumerable<ChangeRequest>> GetAllChangeRequestsAsync()
    {
        return await _changeRequestRepository.GetAllAsync();
    }

    public async Task<ChangeRequest> GetChangeRequestByIdAsync(int changeRequestId)
    {
        return await _changeRequestRepository.GetByIdAsync(changeRequestId);
    }

    public async Task UpdateChangeRequestAsync(int changeRequestId, ChangeRequest updatedChangeRequest)
    {
        var changeRequest = await _changeRequestRepository.GetByIdAsync(changeRequestId);
        if (changeRequest != null)
        {
            changeRequest.Status = updatedChangeRequest.Status;
            // ... інші поля для оновлення ...
            await _changeRequestRepository.UpdateAsync(changeRequest);
        }
    }

    public async Task DeleteChangeRequestAsync(int changeRequestId)
    {
        await _changeRequestRepository.DeleteAsync(changeRequestId);
    }
}