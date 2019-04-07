namespace Gebirgszug_Simulation
{
    partial class N_Eingabe
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.N_Eingabe_Feld = new System.Windows.Forms.NumericUpDown();
            this.Übernehmen_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.N_Eingabe_Feld)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bitte geben sie ihr gewünschte Länge N ein";
            // 
            // N_Eingabe_Feld
            // 
            this.N_Eingabe_Feld.Location = new System.Drawing.Point(12, 25);
            this.N_Eingabe_Feld.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.N_Eingabe_Feld.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.N_Eingabe_Feld.Name = "N_Eingabe_Feld";
            this.N_Eingabe_Feld.Size = new System.Drawing.Size(210, 20);
            this.N_Eingabe_Feld.TabIndex = 1;
            this.N_Eingabe_Feld.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Übernehmen_Button
            // 
            this.Übernehmen_Button.Location = new System.Drawing.Point(12, 51);
            this.Übernehmen_Button.Name = "Übernehmen_Button";
            this.Übernehmen_Button.Size = new System.Drawing.Size(210, 25);
            this.Übernehmen_Button.TabIndex = 2;
            this.Übernehmen_Button.Text = "Übernehmen";
            this.Übernehmen_Button.UseVisualStyleBackColor = true;
            this.Übernehmen_Button.Click += new System.EventHandler(this.Übernehmen_Button_Click);
            // 
            // N_Eingabe
            // 
            this.AcceptButton = this.Übernehmen_Button;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(232, 86);
            this.ControlBox = false;
            this.Controls.Add(this.Übernehmen_Button);
            this.Controls.Add(this.N_Eingabe_Feld);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "N_Eingabe";
            this.Text = "N_Eingabe";
            ((System.ComponentModel.ISupportInitialize)(this.N_Eingabe_Feld)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown N_Eingabe_Feld;
        private System.Windows.Forms.Button Übernehmen_Button;
    }
}