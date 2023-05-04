using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication1.Pages.Clients
{
	public class CreateModel : PageModel
	{
		public CLientInfo clientInfo = new CLientInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
		}
		public void OnPost()
		{
			clientInfo.fullname = Request.Form["fullname"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.adresse = Request.Form["address"];

			if (clientInfo.fullname.Length == 0 || clientInfo.email.Length == 0 || 
				clientInfo.phone.Length==0 || clientInfo.adresse.Length==0)
			{
				errorMessage = "All the fields are required";
				return;
			}
			//save the new client into the database
			try
			{
				//connect to the database using the connection string
				String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
				//creat sql connection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					//open the connection
					connection.Open();
					//creat the sqlquery to read the data from our clients table 
					string sql = "Insert into clients"+
								"(fullname,email,phone,adresse) values"+
								"(@fullname, @email, @phone, @address);";
					//creat the sqlcommand wich allows us to execute our sqlquery
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@fullname", clientInfo.fullname);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.adresse);
						
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			clientInfo.fullname = "";
			clientInfo.email = "";
			clientInfo.phone = "";
			clientInfo.adresse = "";
			successMessage = "New Client Added Correctly";

			Response.Redirect("/Clients/Index");
		}
	}
}