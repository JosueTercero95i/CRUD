using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Agenda
{
    public partial class Form1 : Form
    {
        private const string ArchivoContactos = "contactos.json";

        public Form1()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void GuardarContactos(List<Contacto> contactos)
        {
            var json = JsonConvert.SerializeObject(contactos, Formatting.Indented);
            File.WriteAllText(ArchivoContactos, json);
        }

        private List<Contacto> LeerContactos()
        {
            if (!File.Exists(ArchivoContactos))
                return new List<Contacto>();

            var json = File.ReadAllText(ArchivoContactos);
            return JsonConvert.DeserializeObject<List<Contacto>>(json) ?? new List<Contacto>();
        }

        private void CargarDatos()
        {
            var contactos = LeerContactos();

            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("ID", "ID");
                dataGridView1.Columns.Add("Nombre", "Nombre");
                dataGridView1.Columns.Add("Apellido", "Apellido");
                dataGridView1.Columns.Add("Telefono", "Teléfono");
                dataGridView1.Columns.Add("Correo", "Correo");
                dataGridView1.Columns.Add("Descripcion", "Descripción");
            }

            dataGridView1.Rows.Clear();

            foreach (var contacto in contactos)
            {
                dataGridView1.Rows.Add(contacto.ID, contacto.Nombre, contacto.Apellido, contacto.Telefono, contacto.Correo, contacto.Descripcion);
            }
        }



        private void CrearContacto()
        {
            var contactos = LeerContactos();  
            var nuevoContacto = new Contacto
            {
                ID = contactos.Count > 0 ? contactos.Max(c => c.ID) + 1 : 1,
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Apellido = txtApellido.Text,
                Correo = txtCorreo.Text,
                Descripcion = txtDescripcion.Text
            };

            contactos.Add(nuevoContacto); 
            GuardarContactos(contactos);  
            CargarDatos(); 
            MessageBox.Show("Contacto agregado con éxito.");
            LimpiarCampos();
        }




        private void EliminarContacto()
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
            var contactos = LeerContactos();
            var contacto = contactos.FirstOrDefault(c => c.ID == id);

            if (contacto != null)
            {
                contactos.Remove(contacto);
                GuardarContactos(contactos);
                CargarDatos();
                MessageBox.Show("Contacto eliminado con éxito.");
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDescripcion.Clear();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
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

            CrearContacto();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var contactoSeleccionado = dataGridView1.SelectedRows[0];

                if (contactoSeleccionado.Cells.Count >= 6)
                {
                    var id = (int)contactoSeleccionado.Cells[0].Value; 
                    var nombre = (string)contactoSeleccionado.Cells[1].Value;
                    var apellido = (string)contactoSeleccionado.Cells[2].Value;
                    var telefono = (string)contactoSeleccionado.Cells[3].Value;
                    var correo = (string)contactoSeleccionado.Cells[4].Value;
                    var descripcion = (string)contactoSeleccionado.Cells[5].Value;

                    Editar editarForm = new Editar(id, nombre, apellido, telefono, correo, descripcion);

                    editarForm.Show();

                    editarForm.FormClosed += (s, args) => CargarDatos();
                }
                else
                {
                    MessageBox.Show("La fila seleccionada no contiene los datos esperados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarContacto();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();

        }
    }
}
