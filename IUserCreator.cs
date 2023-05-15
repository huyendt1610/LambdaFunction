namespace LambdaDemo
{
    public interface IUserCreator
    {
        Task<bool> CreateUser(User user);
    }
}