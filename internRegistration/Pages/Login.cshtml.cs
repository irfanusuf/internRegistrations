using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using BCrypt.Net;

namespace internRegistration.Pages
{
    public class LoginModel : PageModel
    {
        public LoginInfo loginInfo = new LoginInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }
        public void OnPost()
        {

            loginInfo.Email = Request.Form["Email"];
            loginInfo.Password = Request.Form["Password"];
            try
            {
                // Your SQL Server connection string
                string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT Email, Password FROM Users WHERE Email = @Email";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", loginInfo.Email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPasswordHash = reader["Password"].ToString();

                                // Verify the provided password against the stored hash
                                if (BCrypt.Net.BCrypt.Verify(loginInfo.Password, storedPasswordHash))
                                {
                                    // Successful login

                                    // Redirect to a secure page after successful login
                                    Response.Redirect("/List");
                                }
                            }
                        }
                    }
                }

                // Failed login
                errorMessage = "Invalid email or password.";

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

            }
        }

    }
}
