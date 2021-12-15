using ProWay.OrientacaoObjetos.Models.ContaBancaria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proway.OrientacaoObjetos.Views.Contas
{
    public partial class FormContaPoupanca : Form
    {
        public FormContaPoupanca()
        {
            InitializeComponent();
            Listar(); // irá fazer consulta no banco e apresentar no grid
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text.Trim();
            double saldo = Convert.ToDouble(textBoxSaldo.Text.Trim());
            string codigo = textBoxCodigo.Text.Trim();

            // Instancia um objeto chamado contaPoupanca da ContaPoupanca
            var contaPoupanca = new ContaPoupanca(nome, saldo);

            if(codigo == string.Empty)
            {
                CadastrarNoBancoDeDados(contaPoupanca);
            }
            else
            {
                contaPoupanca.Codigo = Convert.ToInt32(codigo);
                EditarNoBancoDeDados(contaPoupanca);
            }
            textBoxCodigo.Clear();
            textBoxNome.Clear();
            textBoxSaldo.Clear();

            Listar();
        }

        private void EditarNoBancoDeDados(ContaPoupanca contaPoupanca)
        {
            SqlConnection sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\moc\source\repos\ProWay.OrientacaoObjetos.Models\Proway.OrientacaoObjetos.Views\BancoDados\Database.mdf;Integrated Security=True";
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "UPDATE ContasPoupanca SET Nome = @NOME, saldo = @SALDO WHERE codigo = @CODIGO";
                sqlCommand.Parameters.AddWithValue("@NOME",
                    contaPoupanca.Nome);
                sqlCommand.Parameters.AddWithValue("@SALDO",
                    contaPoupanca.Saldo);
                sqlCommand.Parameters.AddWithValue("@CODIGO",
                    contaPoupanca.Codigo);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Registo atualizado com sucesso.");
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Erro ao realizar atualização do registro no banco de dados.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível atualizar o seu registro.");
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void CadastrarNoBancoDeDados(ContaPoupanca contaPoupanca)
        {
            var caminho = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\moc\source\repos\ProWay.OrientacaoObjetos.Models\Proway.OrientacaoObjetos.Views\BancoDados\Database.mdf;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(caminho);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            var sql = "INSERT INTO ContasPoupanca (nome, saldo) " +
                    "VALUES (@NOME, @SALDO)";

            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.AddWithValue("@NOME", contaPoupanca.Nome);
            sqlCommand.Parameters.AddWithValue("@SALDO", contaPoupanca.Saldo);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            MessageBox.Show("Cadastrado com sucesso.");
        }

        private void Listar()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\moc\source\repos\ProWay.OrientacaoObjetos.Models\Proway.OrientacaoObjetos.Views\BancoDados\Database.mdf;Integrated Security=True";
            sqlConnection.Open();

            SqlCommand comando = new SqlCommand();
            comando.Connection = sqlConnection;
            comando.CommandText = "SELECT codigo, nome, saldo FROM ContasPoupanca";

            DataTable dataTable = new DataTable();
            dataTable.Load(comando.ExecuteReader());

            dataGridView.Rows.Clear();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];
                int codigo = Convert.ToInt32(dataRow[0]);
                string nome = dataRow[1].ToString();
                double saldo = Convert.ToDouble(dataRow[2]);
                dataGridView.Rows.Add(codigo, nome, saldo);

            }
        }
        private void buttonEditar_Click(object sender, EventArgs e)
        {
            int quantidadeLinhasSelecionadas = dataGridView.SelectedRows.Count;

            if(quantidadeLinhasSelecionadas <= 0)
            {
                MessageBox.Show("Selecione um registro");
                return;
            }

            int codigo = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\moc\source\repos\ProWay.OrientacaoObjetos.Models\Proway.OrientacaoObjetos.Views\BancoDados\Database.mdf;Integrated Security=True";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "SELECT * FROM ContasPoupanca WHERE codigo = @CODIGO";
            sqlCommand.Parameters.AddWithValue("@CODIGO", codigo);

            DataTable dataTable = new DataTable();
            dataTable.Load(sqlCommand.ExecuteReader());

            if(dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Registro não encontrado no banco de dados");
                return;
            }

            DataRow datarow = dataTable.Rows[0];
            int codigoBanco = Convert.ToInt32(datarow[0]);
            string nome = datarow[1].ToString();
            double saldo = Convert.ToDouble(datarow[2]);

            ContaPoupanca contaPoupanca = new ContaPoupanca(nome, saldo);
            contaPoupanca.Codigo = codigoBanco;
            textBoxCodigo.Text = contaPoupanca.Codigo.ToString();
            textBoxNome.Text = contaPoupanca.Nome;
            textBoxSaldo.Text = contaPoupanca.Saldo.ToString();

        }

        private void buttonApagar_Click(object sender, EventArgs e)
        {
            int quantidadeLinhasSelecionadas = dataGridView.SelectedRows.Count;

            if(quantidadeLinhasSelecionadas <= 0)
            {
                MessageBox.Show("Selecione no mínimo um registro para apagar");
            }

            int codigo = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);

            DialogResult dialogResult = MessageBox.Show(
                "Deseja realmente apagar?",
                "Aviso",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);
            if(dialogResult == DialogResult.Yes)
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\moc\source\repos\ProWay.OrientacaoObjetos.Models\Proway.OrientacaoObjetos.Views\BancoDados\Database.mdf;Integrated Security=True";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "DELETE FROM ContasPoupanca WHERE codigo = @CODIGO";
                sqlCommand.Parameters.AddWithValue("@CODIGO", codigo);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                Listar();
            }
        }

        
    }
}
