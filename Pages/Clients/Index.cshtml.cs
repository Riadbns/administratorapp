using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication1.Pages.Clients
{
	public class IndexModel : PageModel
	{
		public List<CLientInfo> listClients = new List<CLientInfo>();
		public void OnGet()
		{
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
					string sql = "select * from clients";
					//creat the sqlcommand wich allows us to execute our sqlquery
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								CLientInfo info = new CLientInfo();
								info.id = "" + reader.GetInt32(0);
								info.fullname = reader.GetString(1);
								info.email = reader.GetString(2);
								info.phone = reader.GetString(3);
								info.adresse = reader.GetString(4);
								info.created_at = reader.GetDateTime(5).ToString();

								listClients.Add(info);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
			}
		}
	}
	public class CLientInfo
	{
		public String id;
		public String fullname;
		public String email;
		public String phone;
		public String adresse;
		public String created_at;
	}
}