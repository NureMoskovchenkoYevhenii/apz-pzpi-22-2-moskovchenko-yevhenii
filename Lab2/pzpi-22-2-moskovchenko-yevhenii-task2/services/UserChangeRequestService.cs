using Microsoft.Extensions.Localization;

public class UserChangeRequestService
{
    private readonly IUserChangeRequestRepository _userChangeRequestRepository;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UserChangeRequestService(IUserChangeRequestRepository userChangeRequestRepository, IStringLocalizer<SharedResources> localizer)
    {
        _userChangeRequestRepository = userChangeRequestRepository;
        _localizer = localizer;
    }

    public async Task<UserChangeRequest> AddUserChangeRequestAsync(UserChangeRequest userChangeRequest)
    {
        return await _userChangeRequestRepository.AddAsync(userChangeRequest);
    }

    public async Task<IEnumerable<UserChangeRequest>> GetAllUserChangeRequestsAsync()
    {
        return await _userChangeRequestRepository.GetAllAsync();
    }

    public async Task<UserChangeRequest> GetUserChangeRequestByIdAsync(int userChangeRequestId)
    {
        return await _userChangeRequestRepository.GetByIdAsync(userChangeRequestId);
    }

    public async Task UpdateUserChangeRequestAsync(int userChangeRequestId, UserChangeRequest updatedUserChangeRequest)
    {
        var userChangeRequest = await _userChangeRequestRepository.GetByIdAsync(userChangeRequestId);
        if (userChangeRequest != null)
        {
            userChangeRequest.UserId = updatedUserChangeRequest.UserId;
            userChangeRequest.ChangeRequestId = updatedUserChangeRequest.ChangeRequestId;
            await _userChangeRequestRepository.UpdateAsync(userChangeRequest);
        }
    }

    public async Task DeleteUserChangeRequestAsync(int userChangeRequestId)
    {
        await _userChangeRequestRepository.DeleteAsync(userChangeRequestId);
    }
}