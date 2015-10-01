using Aptum.ImportTaskRedmine.Entities;
using Aptum.ImportTaskRedmine.Services;
using Excel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Redmine.Net.Api.Types;

namespace Aptum.ImportTaskRedmine
{
    public partial class FormPrincipal : Form
    {
        public string mensagensDeStatus;
        public bool CargaValidacaoFeitaComSucesso = false;
        public bool CargaValidacaoConcluida = false;
        public bool CadastroConcluido = false;
        public bool ErroConexaoRedmine = false;
        public bool ErroCadastroRedmine = false;
        public List<Project> listaDeProjetos;
        RedmineServices redmineService;
        List<ExcelIssue> IssueList;

        public FormPrincipal()
        {
            InitializeComponent();
            redmineService = new RedmineServices(textBoxRedmineHost.Text, textBoxApiAccessKey.Text);
            mensagensDeStatus = "";
        }

        private bool CamposObrigatoriosPreenchidos()
        {
            if (textBoxApiAccessKey.Text == "" || textBoxRedmineHost.Text == "" || comboBoxProjetos.SelectedItem == null)
            {
                MessageBox.Show(null, "Todos os três campos são de preenchimento obrigatório",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
                return true;
        }

        private bool ApiKeyEnderecoPreenchidos()
        {
            if (textBoxApiAccessKey.Text == "" || textBoxRedmineHost.Text == "")
            {
                MessageBox.Show(null, "Api Key e Endereço Redmine são de preenchimento obrigatório",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
                return true;
        }

        private void buttonAbrirExcel_Click(object sender, EventArgs e)
        {
            CargaValidacaoConcluida = false;
            if (CamposObrigatoriosPreenchidos())
            {
                openFileDialog1.Filter = "Excel (.xlsx)|*.xlsx";
                openFileDialog1.FilterIndex = 1;
                DialogResult dialogResult = openFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Project projeto = (Project)comboBoxProjetos.SelectedItem;
                    timerCargaArquivo.Enabled = true;

                    //inicia o processo como uma thread
                    Thread backgroundThread = new Thread(
                        new ThreadStart(() =>
                        {
                            CargaValidacaoFeitaComSucesso = CarregarValidarExcel(projeto);
                            CargaValidacaoConcluida = true;
                        }
                    ));
                    backgroundThread.Start();
                }
            }
        }

        private void HabilitarCadastro()
        {
            timerCargaArquivo.Enabled = false;
            const string MSG_SUCESSO = "O carregamento e validação concluídos com sucesso!";
            buttonProcessar.Enabled = true;
            buttonProcessar.Focus();
            MessageBox.Show(null, MSG_SUCESSO, "Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool CarregarValidarExcel(Project projeto)
        {
            const string MSG_CORRIJA_ERROS = "CORRIJA OS PROBLEMAS NO ARQUIVO OU NO REDMINE";
            
            //limpa a lista para uma eventual escolha de novo arquivo
            if (IssueList != null)
                IssueList.Clear();

            this.mensagensDeStatus = "Iniciando carregamento do arquivo..." + Environment.NewLine;
            IssueList = CarregarExcel(projeto);

            if (IssueList != null)
            {
                mensagensDeStatus += "Carregamento concluído com sucesso!" + Environment.NewLine;
                mensagensDeStatus += "Iniciando validação dos usuários..." + Environment.NewLine;
                var ListaDeUsuariosNaoEncontrados = redmineService.ValidarListaDeUsuarios(IssueList).Where(x => x.AssigneeId == 0);
                if (ListaDeUsuariosNaoEncontrados.Count() > 0)
                {
                    mensagensDeStatus = "Um ou mais usuários não foram encontrados:" + Environment.NewLine;
                    foreach (var excelIssue in ListaDeUsuariosNaoEncontrados)
                        mensagensDeStatus += " - " + excelIssue.AssigneeName + Environment.NewLine;
                    mensagensDeStatus += MSG_CORRIJA_ERROS;
                    return false;
                }
                else
                {
                    mensagensDeStatus += "Validação dos usuários concluída com sucesso!" + Environment.NewLine;
                    mensagensDeStatus += "Iniciando validação dos trackers..." + Environment.NewLine;
                    var ListTrackersNaoEncontrados = redmineService.ValidarListaDeTrackers(IssueList).Where(x => x.TrackerId == 0);
                    if (ListTrackersNaoEncontrados.Count() > 0)
                    {
                        mensagensDeStatus = "Um ou mais trackers não foram encontrados:" + Environment.NewLine;
                        foreach (var excelIssue in ListTrackersNaoEncontrados)
                            mensagensDeStatus += " - " + excelIssue.Tracker + Environment.NewLine;
                        mensagensDeStatus += MSG_CORRIJA_ERROS;
                        return false;
                    }
                    else
                    {
                        if (checkBoxAtualizarProjetoExistente.Checked)
                        {
                            mensagensDeStatus += "Validação dos trackers concluída com sucesso!" + Environment.NewLine;
                            mensagensDeStatus += "Iniciando validação das issues existentes..." + Environment.NewLine;
                            //var ListIssuesNaoEncontrados = redmineService.ValidarIdIssue(IssueList);
                            var ListIssuesNaoEncontrados = redmineService.ValidarIdIssues(IssueList);
                            if (ListIssuesNaoEncontrados.Count() > 0)
                            {
                                mensagensDeStatus = "Um ou mais issues não foram encontradas:" + Environment.NewLine;
                                foreach (var excelIssue in ListIssuesNaoEncontrados)
                                    mensagensDeStatus += " - " + excelIssue.IdRedmine + Environment.NewLine;
                                mensagensDeStatus += MSG_CORRIJA_ERROS;
                                return false;
                            }
                            else
                            {
                                mensagensDeStatus += "Todas validações concluídas com sucesso!" + Environment.NewLine;
                                return true;
                            }
                        }
                        else
                        {
                            mensagensDeStatus += "Todas validações concluídas com sucesso!" + Environment.NewLine;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private List<ExcelIssue> CarregarExcel(Project projeto)
        {
            List<ExcelIssue> listExcelIssues = null;

            System.IO.Stream fileStream;
            try
            {
                fileStream = openFileDialog1.OpenFile();
                listExcelIssues = ExcelIssue.CarregarIssuesExcel(fileStream, projeto.Id);

                if (listExcelIssues == null)
                    MessageBox.Show(null, "O arquivo Excel está fora do layout padrão ou contém dados incompatíveis.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (IOException ioex)
            {
                MessageBox.Show(null, "O arquivo Excel está sendo usado por outro processo." + Environment.NewLine
                    + ioex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine("1° Catch Erro: " + ioex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("2° Catch Erro: " + ex.Message);
            }

            return listExcelIssues;
        }

        private void buttonAtualizarProjetos_Click(object sender, EventArgs e)
        {
            const string MSG_ERRO = "Não foi possível se conectar ao Redmine!";
            ErroConexaoRedmine = false;

            if (ApiKeyEnderecoPreenchidos())
            {
                listaDeProjetos = null;
                mensagensDeStatus = "Buscando informações dos projetos no Redmine..." + Environment.NewLine;
                comboBoxProjetos.Enabled = false;
                timerSelecaoProjeto.Enabled = true;

                //inicia o processo como uma thread
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        listaDeProjetos = redmineService.ListarProjetos();
                        if(listaDeProjetos == null)
                        {
                            ErroConexaoRedmine = true;
                            mensagensDeStatus += MSG_ERRO + Environment.NewLine;
                            MessageBox.Show(null, MSG_ERRO, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                ));
                backgroundThread.Start();
            }
        }

        private void HabilitarSelecaoDeProjetos()
        {
            timerSelecaoProjeto.Enabled = false;
            comboBoxProjetos.Items.AddRange(listaDeProjetos.ToArray());
            comboBoxProjetos.Enabled = true;
            comboBoxProjetos.Focus();
            textBoxStatus.Text += "Informações dos projetos carregadas com sucesso!";
        }

        private void comboBoxProjetos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProjetos.SelectedItem != null)
            {
                buttonAbrirExcel.Enabled = true;
                buttonAbrirExcel.Focus();
            }
        }

        private void buttonProcessar_Click(object sender, EventArgs e)
        {
            if (CamposObrigatoriosPreenchidos())
            {
                DialogResult result = MessageBox.Show(null, "Tem certeza que deseja continuar?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result.Equals(DialogResult.Yes))
                {
                    CadastroConcluido = false;
                    timerCadastroRedmine.Enabled = true;

                    //inicia o processo como uma thread
                    Thread backgroundThread = new Thread(
                        new ThreadStart(() =>
                        {
                            CadastrarIssuesNoRedmine(checkBoxAtualizarProjetoExistente.Checked);     
                        }
                    ));
                    backgroundThread.Start();
                }
            }
        }

        private bool CadastrarIssuesNoRedmine(bool atualizarProjetoExistente)
        {
            const string MSG_SUCESSO = "Todos cadastros concluídos com sucesso!";
            const string MSG_ERRO_TRACKER = "Atualização abortada! Existe um tracker não associado ao projeto.";
            const string MSG_ERRO = "Atualização abortada! Um erro inesperado aconteceu: ";
            mensagensDeStatus = "Iniciando cadastro das tarefas no Redmine..." + Environment.NewLine;
            ErroCadastroRedmine = false;

            foreach (var issue in IssueList)
            {
                //redmineService.CadastrarIssue(issue, atualizarProjetoExistente);
                //mensagensDeStatus += "Tarefa '" + issue.IdRedmine + "' atualizada com sucesso!" + Environment.NewLine;

                try
                {
                    redmineService.CadastrarIssue(issue, atualizarProjetoExistente);
                    mensagensDeStatus += "Tarefa '" + issue.IdRedmine + "' atualizada com sucesso!" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Equals("Tracker Not Found"))
                    {
                        mensagensDeStatus += MSG_ERRO_TRACKER;
                        MessageBox.Show(null, MSG_ERRO_TRACKER, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ErroCadastroRedmine = true;
                        return false;
                    }
                    else
                    {
                        mensagensDeStatus += MSG_ERRO;
                        MessageBox.Show(null, MSG_ERRO + ex.Message, "Erro no Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ErroCadastroRedmine = true;
                        return false;
                    }
                }
            }
            mensagensDeStatus += MSG_SUCESSO;
            CadastroConcluido = true;
            MessageBox.Show(null, MSG_SUCESSO, "Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        private void timerSelecaoProjeto_Tick(object sender, EventArgs e)
        {
            ExibirStatusDaExecucao();

            if (!ErroConexaoRedmine && listaDeProjetos!= null)
            {
                HabilitarSelecaoDeProjetos();
                Cursor.Current = Cursors.Default;
            }
            else if (ErroConexaoRedmine)
            {
                timerSelecaoProjeto.Enabled = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void timerCargaArquivo_Tick(object sender, EventArgs e)
        {
            ExibirStatusDaExecucao();

            if (CargaValidacaoConcluida)
            {
                if (CargaValidacaoFeitaComSucesso)
                    HabilitarCadastro();

                timerCargaArquivo.Enabled = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void timerCadastroRedmine_Tick(object sender, EventArgs e)
        {
            ExibirStatusDaExecucao();

            if (!ErroCadastroRedmine && CadastroConcluido)
            {
                timerCadastroRedmine.Enabled = false;
                Cursor.Current = Cursors.Default;
            }
            else if (ErroCadastroRedmine)
            {
                timerSelecaoProjeto.Enabled = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void ExibirStatusDaExecucao()
        {
            Cursor.Current = Cursors.WaitCursor;
            textBoxStatus.Text = mensagensDeStatus;
            textBoxStatus.SelectionStart = textBoxStatus.TextLength;
            textBoxStatus.ScrollToCaret();
        }
    }
}
