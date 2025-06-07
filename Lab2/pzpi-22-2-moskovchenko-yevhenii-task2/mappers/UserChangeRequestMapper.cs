public class UserChangeRequestMapper
{
    public UserChangeRequestDto MapToDto(UserChangeRequest userChangeRequest)
    {
        return new UserChangeRequestDto
        {
            UserChangeRequestId = userChangeRequest.UserChangeRequestId,
            UserId = userChangeRequest.UserId,
            ChangeRequestId = userChangeRequest.ChangeRequestId
            // Інші властивості...
        };
    }

    public UserChangeRequest MapToEntity(UserChangeRequestDto userChangeRequestDto)
    {
        return new UserChangeRequest
        {
            UserChangeRequestId = userChangeRequestDto.UserChangeRequestId,
            UserId = userChangeRequestDto.UserId,
            ChangeRequestId = userChangeRequestDto.ChangeRequestId
            // Інші властивості...
        };
    }
}

public class UserChangeRequestDto
{
    public int UserChangeRequestId { get; set; }
    public int UserId { get; set; }
    public int ChangeRequestId { get; set; }
    // Інші властивості...
}
