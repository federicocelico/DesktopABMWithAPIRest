using Negocios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Farmacity
{
    public partial class wfPrincipal : Form
    {
        ProductsRequest productsRequest = new ProductsRequest();
        List<Producto> list = new List<Producto>();
        bool edita = false;
        public wfPrincipal()
        {
            InitializeComponent();
        }

        private void wfPrincipal_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private async void CargarProductos()
        {
            list = await GetProductsAsync();
            dgvProductos.DataSource = list;
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private async Task<List<Producto>> GetProductsAsync()
        {
            string response = await productsRequest.GetProductos();
            List<Producto> list = JsonConvert.DeserializeObject<List<Producto>>(response);
            return list;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto();

            if(txtDescripcion.Text != string.Empty)
            {
                producto.Descripcion = txtDescripcion.Text;
                producto.Precio = Convert.ToDouble(txtPrecio.Text);
                producto.Stock = Convert.ToInt32(txtStock.Text);
                producto.Activo = Convert.ToInt32(txtActivo.Text);
            }
          

            if (edita)
            {
                producto.idArticulo = Convert.ToInt32(txtId.Text);

                if (productsRequest.ABMProducts(producto, "PUT"))
                {
                    MessageBox.Show("El producto fue modificado con Exito.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("El producto  no se pudo modificar.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                edita = false;
            }
            else
            {
               

                if (productsRequest.ABMProducts(producto, "POST"))
                {
                    MessageBox.Show("El producto fue dado de alta con Exito.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("El producto  no se pudo dar de alta.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           

        }

        private void dgvProductos_DoubleClick(object sender, EventArgs e)
        {
            edita = true;
            txtId.Text = dgvProductos.SelectedRows[0].Cells[0].Value.ToString();
            txtDescripcion.Text = dgvProductos.SelectedRows[0].Cells[1].Value.ToString();
            txtPrecio.Text = dgvProductos.SelectedRows[0].Cells[2].Value.ToString();
            txtStock.Text = dgvProductos.SelectedRows[0].Cells[3].Value.ToString();
            txtActivo.Text = dgvProductos.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void Limpiar()
        {
            txtId.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtActivo.Clear();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto();

            producto.idArticulo = Convert.ToInt32(txtId.Text);

            if (productsRequest.ABMProducts(producto, "DELETE"))
            {
                MessageBox.Show("El producto fue Eliminado con Exito.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
                CargarProductos();
            }
            else
            {
                MessageBox.Show("El producto  no se pudo Eliminar.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
            edita = false;
        }
        public void Filtrar(string criterio)
        {
          
            var query = from l in list where l.Descripcion.ToLower().StartsWith(criterio.ToLower()) select l;
      
            dgvProductos.DataSource = query.ToList();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Filtrar(txtBuscar.Text.Trim());
        }
    }
}
