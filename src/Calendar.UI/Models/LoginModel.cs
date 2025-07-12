using System.ComponentModel.DataAnnotations;

namespace Calendar.UI.Models;

public class LoginModel
{
    public string UserNameOrEmail { get; set; }

    public string Password { get; set; }
}
