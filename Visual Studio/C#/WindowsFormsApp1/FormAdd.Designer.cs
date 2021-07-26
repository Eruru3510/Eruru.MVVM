
namespace WindowsFormsApp1 {
	partial class FormAdd {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent () {
			this.Label = new System.Windows.Forms.Label();
			this.TextBox = new System.Windows.Forms.TextBox();
			this.ButtonConfirm = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ComboBox = new System.Windows.Forms.ComboBox();
			this.FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Label
			// 
			this.Label.Location = new System.Drawing.Point(5, 5);
			this.Label.Name = "Label";
			this.Label.Size = new System.Drawing.Size(100, 23);
			this.Label.TabIndex = 0;
			this.Label.Text = "label1";
			this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TextBox
			// 
			this.TextBox.Location = new System.Drawing.Point(5, 30);
			this.TextBox.Name = "TextBox";
			this.TextBox.Size = new System.Drawing.Size(100, 21);
			this.TextBox.TabIndex = 1;
			// 
			// ButtonConfirm
			// 
			this.ButtonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonConfirm.Location = new System.Drawing.Point(528, 418);
			this.ButtonConfirm.Name = "ButtonConfirm";
			this.ButtonConfirm.Size = new System.Drawing.Size(75, 23);
			this.ButtonConfirm.TabIndex = 2;
			this.ButtonConfirm.Text = "确定";
			this.ButtonConfirm.UseVisualStyleBackColor = true;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.Location = new System.Drawing.Point(608, 418);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 3;
			this.ButtonCancel.Text = "取消";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			// 
			// ComboBox
			// 
			this.ComboBox.FormattingEnabled = true;
			this.ComboBox.Location = new System.Drawing.Point(110, 30);
			this.ComboBox.Name = "ComboBox";
			this.ComboBox.Size = new System.Drawing.Size(121, 20);
			this.ComboBox.TabIndex = 4;
			// 
			// FlowLayoutPanel
			// 
			this.FlowLayoutPanel.Location = new System.Drawing.Point(40, 75);
			this.FlowLayoutPanel.Name = "FlowLayoutPanel";
			this.FlowLayoutPanel.Size = new System.Drawing.Size(400, 250);
			this.FlowLayoutPanel.TabIndex = 5;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(425, 350);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// FormAdd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(688, 446);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.FlowLayoutPanel);
			this.Controls.Add(this.ComboBox);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonConfirm);
			this.Controls.Add(this.TextBox);
			this.Controls.Add(this.Label);
			this.Name = "FormAdd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Form_Add";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label Label;
		private System.Windows.Forms.TextBox TextBox;
		private System.Windows.Forms.Button ButtonConfirm;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.ComboBox ComboBox;
		private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel;
		private System.Windows.Forms.Button button1;
	}
}