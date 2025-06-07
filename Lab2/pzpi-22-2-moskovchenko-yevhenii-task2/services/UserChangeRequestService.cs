public class UserChangeRequestService
{
    private readonly IUserChangeRequestRepository _userChangeRequestRepository;

    public UserChangeRequestService(IUserChangeRequestRepository userChangeRequestRepository)
    {
        _userChangeRequestRepository = userChangeRequestRepository;
    }

    public void AddUserChangeRequest(UserChangeRequest userChangeRequest)
    {
        _userChangeRequestRepository.Add(userChangeRequest);
    }

    public IEnumerable<UserChangeRequest> GetAllUserChangeRequests()
    {
        return _userChangeRequestRepository.GetAll();
    }

    public UserChangeRequest GetUserChangeRequestById(int userChangeRequestId)
    {
        return _userChangeRequestRepository.GetById(userChangeRequestId);
    }

    public void UpdateUserChangeRequest(int userChangeRequestId, UserChangeRequest updatedUserChangeRequest)
    {
        var userChangeRequest = _userChangeRequestRepository.GetById(userChangeRequestId);
        if (userChangeRequest != null)
        {
            userChangeRequest.UserId = updatedUserChangeRequest.UserId;
            userChangeRequest.ChangeRequestId = updatedUserChangeRequest.ChangeRequestId;
            _userChangeRequestRepository.Update(userChangeRequest);
        }
    }

    public void DeleteUserChangeRequest(int userChangeRequestId)
    {
        _userChangeRequestRepository.Delete(userChangeRequestId);
    }
}
