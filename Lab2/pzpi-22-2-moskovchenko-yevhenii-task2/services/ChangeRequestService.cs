public class ChangeRequestService
{
    private readonly IChangeRequestRepository _changeRequestRepository;
    private readonly IUserChangeRequestRepository _userChangeRequestRepository;

    public ChangeRequestService(IChangeRequestRepository changeRequestRepository, IUserChangeRequestRepository userChangeRequestRepository)
    {
        _changeRequestRepository = changeRequestRepository;
        _userChangeRequestRepository = userChangeRequestRepository;
    }

    public void AddChangeRequest(ChangeRequest changeRequest, int userId)
    {
        _changeRequestRepository.Add(changeRequest);

        var userChangeRequest = new UserChangeRequest
        {
            UserId = userId,
            ChangeRequestId = changeRequest.ChangeRequestId
        };

        _userChangeRequestRepository.Add(userChangeRequest);
    }


    public IEnumerable<ChangeRequest> GetAllChangeRequests()
    {
        return _changeRequestRepository.GetAll();
    }

    public ChangeRequest GetChangeRequestById(int changeRequestId)
    {
        return _changeRequestRepository.GetById(changeRequestId);
    }

    public void UpdateChangeRequest(int changeRequestId, ChangeRequest updatedChangeRequest)
    {
        var changeRequest = _changeRequestRepository.GetById(changeRequestId);
        if (changeRequest != null)
        {
            changeRequest.RequestDate = DateTime.SpecifyKind(updatedChangeRequest.RequestDate, DateTimeKind.Utc);
            changeRequest.Status = updatedChangeRequest.Status;
            changeRequest.DayTypeId = updatedChangeRequest.DayTypeId;
            changeRequest.StartDate = DateTime.SpecifyKind(updatedChangeRequest.StartDate, DateTimeKind.Utc);
            changeRequest.EndDate = DateTime.SpecifyKind(updatedChangeRequest.EndDate, DateTimeKind.Utc);
            _changeRequestRepository.Update(changeRequest);
        }
    }


    public void DeleteChangeRequest(int changeRequestId)
    {
        _changeRequestRepository.Delete(changeRequestId);
    }
}
