using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;

using System.Data.SQLite;
using Microsoft.Data.Sqlite;

using BCrypt.Net;

using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace travelapp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    //private readonly PasswordService _passwordService;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public bool loginFailed { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnPost()
    {
        
    }

    public async Task<IActionResult> OnPostLogin(string Username, string Password)
    {
        var passwordService = new PasswordService();
        string hashedPassword = passwordService.HashPassword(Password);

        if (IsValidUser(Username, Password))
        {
            /* MAKE A NEW PASSWORD
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            */

            HttpContext.Session.SetInt32("IsRamy", 1);

            return Redirect("/Home");
        } 

        loginFailed = true;

        return Page();
    }

    private bool IsValidUser(string enteredUsername, string enteredPassword)
    {
        var connection = new SqliteConnection(@"Data Source=C:\Users\1057247\Downloads\travelapp\Sqlite\Database.db");

        connection.Open();

        var sql = "SELECT * FROM User";
        var storedUsername = "";
        var storedHPassword = "";
        using var command = new SqliteCommand(sql, connection);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            storedUsername = reader.GetString(1);
            storedHPassword = reader.GetString(2);

            if (enteredUsername == storedUsername
                && BCrypt.Net.BCrypt.Verify(enteredPassword, storedHPassword))
            {

                reader.Close();
                connection.Close();
                return true;
            }

        }
        reader.Close();
        connection.Close();

        return false;
    }

    public class PasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

    public void OnGet()
    {
        HttpContext.Session.SetInt32("IsRamy", 0);
        
        Username = "";
        Password = "";
        loginFailed = false;
    }
}

