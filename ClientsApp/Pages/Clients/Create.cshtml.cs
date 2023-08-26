using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.address = Request.Form["address"];
            clientInfo.phone = Request.Form["phone"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=RedmoBookPro14;Initial Catalog=mystore;Integrated Security=True;TrustServerCertificate=True";
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                String sql = "INSERT INTO clients " +
                    "(name, email, phone, address) VALUES " +
                    "(@name, @email, @phone, @address)";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", clientInfo.name);
                command.Parameters.AddWithValue("@phone", clientInfo.phone);
                command.Parameters.AddWithValue("@address", clientInfo.address);
                command.Parameters.AddWithValue("@email", clientInfo.email);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.address = "";
            clientInfo.phone = "";
            successMessage = "New client added correctly";

            Response.Redirect("/Clients/Index");
        }
    }
}
