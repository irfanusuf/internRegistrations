using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using BCrypt.Net;     

namespace internRegistration.Pages
{


    public class SignupModel : PageModel
    {

    


        public UserInfo userInfo = new UserInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

            
        public void OnPost()
        {
            // taking data from form 
            userInfo.Firstname = Request.Form["Firstname"];
            userInfo.Lastname = Request.Form["Lastname"];
            userInfo.Email = Request.Form["Email"];
            userInfo.Password = Request.Form["Password"];


            if (userInfo.Firstname.Length == 0 || userInfo.Lastname.Length == 0 || userInfo.Email.Length == 0 || userInfo.Password.Length == 0)
            {
                errorMessage = "All feilds are required";
                return;
            }



            try
            {

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userInfo.Password);
                //connection string for connecting with data base 

                String connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True";

                //creating a  sql connection by paasing the connection string 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();    // opening a connection 

                    String sql = "INSERT INTO Users" +
                        "(Firstname, Lastname, Email, Password) VALUES " +
                        "(@Firstname, @Lastname, @Email, @Password)";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // passing the data from userinfo into sql query

                        command.Parameters.AddWithValue("@Firstname", userInfo.Firstname);
                        command.Parameters.AddWithValue("@Lastname", userInfo.Lastname);
                        command.Parameters.AddWithValue("@Email", userInfo.Email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);



                        // execute the sql query 
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }

            userInfo.Firstname = "";
            userInfo.Lastname = "";
            userInfo.Email = "";
            userInfo.Password = "";
            Response.Redirect("/Login");

        }
    }
}
