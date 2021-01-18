using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios
{
	public class ProductsRequest
	{
		Request request = new Request();
		public async Task<string> GetProductos()
		{
			try
			{
				return await request.GetProducts();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public bool ABMProducts(Producto producto, string method)
		{

			return request.ABMProducts<Producto>(producto, method);
		}
	}
}
