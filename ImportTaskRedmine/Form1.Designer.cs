namespace Aptum.ImportTaskRedmine
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonAbrirExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxApiAccessKey = new System.Windows.Forms.TextBox();
            this.toolTipApiAccessKey = new System.Windows.Forms.ToolTip(this.components);
            this.buttonAtualizarProjetos = new System.Windows.Forms.Button();
            this.textBoxRedmineHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonProcessar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxProjetos = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxAtualizarProjetoExistente = new System.Windows.Forms.CheckBox();
            this.timerSelecaoProjeto = new System.Windows.Forms.Timer(this.components);
            this.timerCargaArquivo = new System.Windows.Forms.Timer(this.components);
            this.timerCadastroRedmine = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonAbrirExcel
            // 
            this.buttonAbrirExcel.Enabled = false;
            this.buttonAbrirExcel.Location = new System.Drawing.Point(205, 155);
            this.buttonAbrirExcel.Name = "buttonAbrirExcel";
            this.buttonAbrirExcel.Size = new System.Drawing.Size(303, 23);
            this.buttonAbrirExcel.TabIndex = 8;
            this.buttonAbrirExcel.Text = "1- Carregar Tarefas do Arquivo Excel";
            this.buttonAbrirExcel.UseVisualStyleBackColor = true;
            this.buttonAbrirExcel.Click += new System.EventHandler(this.buttonAbrirExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "API Access Key:";
            // 
            // textBoxApiAccessKey
            // 
            this.textBoxApiAccessKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxApiAccessKey.Location = new System.Drawing.Point(205, 41);
            this.textBoxApiAccessKey.Name = "textBoxApiAccessKey";
            this.textBoxApiAccessKey.Size = new System.Drawing.Size(303, 20);
            this.textBoxApiAccessKey.TabIndex = 2;
            this.textBoxApiAccessKey.Text = "47c1aa0a7ac8ae6b686b4b0b66e3a96aef3ab750";
            this.toolTipApiAccessKey.SetToolTip(this.textBoxApiAccessKey, "Você pode encontrar seu \"API Key\" na página da sua conta.\r\nBasta acessar o link \"" +
        "My account\" no canto superior direito da sua tela.");
            // 
            // toolTipApiAccessKey
            // 
            this.toolTipApiAccessKey.BackColor = System.Drawing.Color.Gold;
            this.toolTipApiAccessKey.IsBalloon = true;
            this.toolTipApiAccessKey.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // buttonAtualizarProjetos
            // 
            this.buttonAtualizarProjetos.Image = global::Aptum.ImportTaskRedmine.Properties.Resources.refresh_icon;
            this.buttonAtualizarProjetos.Location = new System.Drawing.Point(206, 98);
            this.buttonAtualizarProjetos.Name = "buttonAtualizarProjetos";
            this.buttonAtualizarProjetos.Size = new System.Drawing.Size(31, 27);
            this.buttonAtualizarProjetos.TabIndex = 6;
            this.toolTipApiAccessKey.SetToolTip(this.buttonAtualizarProjetos, "Clique para carregar/atualizar os projetos Redmine.");
            this.buttonAtualizarProjetos.UseVisualStyleBackColor = true;
            this.buttonAtualizarProjetos.Click += new System.EventHandler(this.buttonAtualizarProjetos_Click);
            // 
            // textBoxRedmineHost
            // 
            this.textBoxRedmineHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRedmineHost.Location = new System.Drawing.Point(205, 70);
            this.textBoxRedmineHost.Name = "textBoxRedmineHost";
            this.textBoxRedmineHost.Size = new System.Drawing.Size(303, 20);
            this.textBoxRedmineHost.TabIndex = 4;
            this.textBoxRedmineHost.Text = "http://192.168.254.118:3000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Endereço Redmine:";
            // 
            // buttonProcessar
            // 
            this.buttonProcessar.Enabled = false;
            this.buttonProcessar.Location = new System.Drawing.Point(205, 188);
            this.buttonProcessar.Name = "buttonProcessar";
            this.buttonProcessar.Size = new System.Drawing.Size(303, 23);
            this.buttonProcessar.TabIndex = 9;
            this.buttonProcessar.Text = "2- Cadastrar/Atualizar Tarefas no Redmine";
            this.buttonProcessar.UseVisualStyleBackColor = true;
            this.buttonProcessar.Click += new System.EventHandler(this.buttonProcessar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(103, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Projeto Redmine:";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.BackColor = System.Drawing.Color.Black;
            this.textBoxStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxStatus.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatus.ForeColor = System.Drawing.Color.White;
            this.textBoxStatus.Location = new System.Drawing.Point(105, 240);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(403, 123);
            this.textBoxStatus.TabIndex = 10;
            this.textBoxStatus.Text = "Aguardando inicio.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(105, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Status do Processamento:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.comboBoxProjetos);
            this.panel1.Location = new System.Drawing.Point(242, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 26);
            this.panel1.TabIndex = 12;
            // 
            // comboBoxProjetos
            // 
            this.comboBoxProjetos.Enabled = false;
            this.comboBoxProjetos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxProjetos.FormattingEnabled = true;
            this.comboBoxProjetos.Location = new System.Drawing.Point(3, 2);
            this.comboBoxProjetos.Name = "comboBoxProjetos";
            this.comboBoxProjetos.Size = new System.Drawing.Size(258, 21);
            this.comboBoxProjetos.TabIndex = 8;
            this.comboBoxProjetos.Text = "Selecione um projeto...";
            this.comboBoxProjetos.SelectedIndexChanged += new System.EventHandler(this.comboBoxProjetos_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 463);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(616, 437);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Informações";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(582, 208);
            this.label5.TabIndex = 1;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(15, 233);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(585, 195);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxAtualizarProjetoExistente);
            this.tabPage2.Controls.Add(this.textBoxRedmineHost);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.buttonAbrirExcel);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.textBoxApiAccessKey);
            this.tabPage2.Controls.Add(this.textBoxStatus);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.buttonProcessar);
            this.tabPage2.Controls.Add(this.buttonAtualizarProjetos);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(616, 437);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Importação";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxAtualizarProjetoExistente
            // 
            this.checkBoxAtualizarProjetoExistente.AutoSize = true;
            this.checkBoxAtualizarProjetoExistente.Location = new System.Drawing.Point(206, 132);
            this.checkBoxAtualizarProjetoExistente.Name = "checkBoxAtualizarProjetoExistente";
            this.checkBoxAtualizarProjetoExistente.Size = new System.Drawing.Size(148, 17);
            this.checkBoxAtualizarProjetoExistente.TabIndex = 14;
            this.checkBoxAtualizarProjetoExistente.Text = "Atualizar Projeto Existente";
            this.checkBoxAtualizarProjetoExistente.UseVisualStyleBackColor = true;
            // 
            // timerSelecaoProjeto
            // 
            this.timerSelecaoProjeto.Tick += new System.EventHandler(this.timerSelecaoProjeto_Tick);
            // 
            // timerCargaArquivo
            // 
            this.timerCargaArquivo.Tick += new System.EventHandler(this.timerCargaArquivo_Tick);
            // 
            // timerCadastroRedmine
            // 
            this.timerCadastroRedmine.Tick += new System.EventHandler(this.timerCadastroRedmine_Tick);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 482);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "FormPrincipal";
            this.Text = "ImportTaskRedmine - Versão 2.0.1";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonAbrirExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApiAccessKey;
        private System.Windows.Forms.ToolTip toolTipApiAccessKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonProcessar;
        private System.Windows.Forms.TextBox textBoxRedmineHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonAtualizarProjetos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxProjetos;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timerSelecaoProjeto;
        private System.Windows.Forms.Timer timerCargaArquivo;
        private System.Windows.Forms.Timer timerCadastroRedmine;
        private System.Windows.Forms.CheckBox checkBoxAtualizarProjetoExistente;
    }
}

