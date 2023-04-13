using EquipoQ22.Datos;
using EquipoQ22.Domino;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//COMPLETAR --> Curso:  1w4    Legajo:    113443     Apellido y Nombre: Fonseca Maria Victoria 




namespace EquipoQ22
{
    public partial class FrmAlta : Form
    {
        private Equipo nueva;
        private DBHelper gestor;

        public FrmAlta()
        {
            InitializeComponent();
            nueva = new Equipo();
            gestor = new DBHelper();
        }
        private void FrmAlta_Load(object sender, EventArgs e)
        {
            CargarCombo();
          
            
        }

        private void CargarCombo()
        {
            cboPersona.DataSource = gestor.ConsultarDB("SP_CONSULTAR_PERSONAS");
            cboPersona.ValueMember = "id_persona";
            cboPersona.DisplayMember = "nombre_completo";
        }

   
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboPersona.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar una persona", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboPosicion.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar una posicion", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

         
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["jugador"].Value.ToString().Equals(cboPersona.Text))
                {
                    MessageBox.Show("Este jugador ya se encuentra", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            DataRowView item = (DataRowView)cboPersona.SelectedItem;

            int idPersona = Convert.ToInt32(item.Row.ItemArray[0]); 
            string nombreCompleto = item.Row.ItemArray[1].ToString();
            int clase = Convert.ToInt32(item.Row.ItemArray[2]); ;
            string posi = item.Row.ItemArray[1].ToString();

            int cam = Convert.ToInt32(nudCamiseta.Text);

            Persona P = new Persona(idPersona, nombreCompleto, clase);
            Jugador detalle = new Jugador(P, cam, posi);

            nueva.AgregarDetalle(detalle);
            dgvDetalles.Rows.Add(new object[] { idPersona, nombreCompleto, cam, clase });
            lblTotal.Text = Convert.ToString(nueva.CalcularTotal());
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtPais.Text == "")
            {
                MessageBox.Show("Debe especificar un pais", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPais.Focus();
                return;
            }
            if (txtDT.Text == "")
            {
                MessageBox.Show("Debe especificar un dt.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDT.Focus();
                return;
            }
            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Ha olvidado? algun jugador?.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboPersona.Focus();
                return;
            }

           

            LimpiarCampos();
            GuardarEquipo();
        }

        private void GuardarEquipo()
        {
            nueva.pais = txtPais.Text;
            nueva.director = txtDT.Text;

            if (gestor.Confirmar(nueva))
            {
                MessageBox.Show("Equipo registrado con exito.", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("ERROR. No se pudo registrar el equipo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void LimpiarCampos()
        {
            cboPersona.SelectedIndex = -1;
            cboPosicion.SelectedIndex = -1;
            nudCamiseta.Value = 1;
            txtDT.Clear();
            txtPais.Clear();
         
        }

      
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 4)
            {
                nueva.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                lblTotal.Text = Convert.ToString(nueva.CalcularTotal());
            }
        }
    }
}
