using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq; 
using System.Windows.Forms; 

namespace Agenda
{
    public partial class Editar : Form
    {
        private int id;

       
        public Editar(int id, string nombre, string apellido, string telefono, string correo, string descripcion)
        {
            InitializeComponent(); 

            this.id = id; 

            txtNombre.Text = nombre;
            txtApellido.Text = apellido;
            txtTelefono.Text = telefono;
            txtCorreo.Text = correo;
            txtDescripcion.Text = descripcion;
        }


        private void btnAtras_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtApellido.Text) ||
                string.IsNullOrEmpty(txtTelefono.Text) ||
                string.IsNullOrEmpty(txtCorreo.Text) ||
                string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            var contactos = LeerContactos();

            var contacto = contactos.FirstOrDefault(c => c.ID == id);

            if (contacto != null)
            {
                contacto.Nombre = txtNombre.Text;
                contacto.Apellido = txtApellido.Text;
                contacto.Telefono = txtTelefono.Text;
                contacto.Correo = txtCorreo.Text;
                contacto.Descripcion = txtDescripcion.Text;

                GuardarContactos(contactos);

                this.Close();

                MessageBox.Show("Contacto actualizado con éxito.");
            }
        }

        private List<Contacto> LeerContactos()
        {
            if (!File.Exists("contactos.json"))
                return new List<Contacto>();

            var json = File.ReadAllText("contactos.json");

            return JsonConvert.DeserializeObject<List<Contacto>>(json) ?? new List<Contacto>();
        }

        private void GuardarContactos(List<Contacto> contactos)
        {
            var json = JsonConvert.SerializeObject(contactos, Formatting.Indented);

            File.WriteAllText("contactos.json", json);
        }
    }
}
