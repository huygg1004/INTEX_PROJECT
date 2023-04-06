using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Protocols;

namespace Intex_app.Models
{
	public class Helpers
	{
		public static string GetRDSConnectionString()
		{
			return "Data Source=aaau0ezzx89som.ckkztcbsflkh.us-east-1.rds.amazonaws.com;Initial Catalog=ebdb;User ID=huy123456;Password=12345678;";
		}
	}
}