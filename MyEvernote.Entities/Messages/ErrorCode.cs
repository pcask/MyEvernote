
namespace MyEvernote.Entities.Messages
{
    public enum ErrorCode
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,

        UsernameOrPassWrong = 151,
        UserIsNotActive = 152,
        ActivateIdDoesNotExists = 153,
        UserNotFound = 154,
        UserPassWrong = 155,
        UserPassAndRePassDontMatch = 156,
        ProfileCouldNotUpdated = 157,
        UserCouldNotRemove = 158,
        UserCouldNotInserted = 159,
        UserCouldNotUpdated = 160
    }
}
