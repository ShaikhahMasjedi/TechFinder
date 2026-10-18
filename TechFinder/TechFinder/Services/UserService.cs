using System.Linq;
using TechFinder.Models;
// 
public class UserService
{
    private readonly TechFinder_storeContext _context;

    public UserService(TechFinder_storeContext context)
    {
        _context = context;
    }

    public void ResetAllUsersLoginStatus()
    {
        var users = _context.Users.ToList();
        foreach (var user in users)
        {
            user.Login = 0;
        }
        _context.SaveChanges();
    }
}
