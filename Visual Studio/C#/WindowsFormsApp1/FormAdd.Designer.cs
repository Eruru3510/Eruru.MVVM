
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
			this.ButtonConfirm = new System.Windows.Forms.Button ();
			this.ButtonCancel = new System.Windows.Forms.Button ();
			this.LabelName = new System.Windows.Forms.Label ();
			this.TextBoxName = new System.Windows.Forms.TextBox ();
			this.TextBoxAge = new System.Windows.Forms.TextBox ();
			this.label1 = new System.Windows.Forms.Label ();
			this.label2 = new System.Windows.Forms.Label ();
			this.TextBoxRemark = new System.Windows.Forms.TextBox ();
			this.label3 = new System.Windows.Forms.Label ();
			this.ComboBoxSchool = new System.Windows.Forms.ComboBox ();
			this.LabelError = new System.Windows.Forms.Label ();
			this.SuspendLayout ();
			// 
			// ButtonConfirm
			// 
			this.ButtonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonConfirm.Location = new System.Drawing.Point (50, 130);
			this.ButtonConfirm.Name = "ButtonConfirm";
			this.ButtonConfirm.Size = new System.Drawing.Size (75, 23);
			this.ButtonConfirm.TabIndex = 2;
			this.ButtonConfirm.Text = "确定";
			this.ButtonConfirm.UseVisualStyleBackColor = true;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.Location = new System.Drawing.Point (130, 130);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size (75, 23);
			this.ButtonCancel.TabIndex = 3;
			this.ButtonCancel.Text = "取消";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			// 
			// LabelName
			// 
			this.LabelName.Location = new System.Drawing.Point (5, 5);
			this.LabelName.Name = "LabelName";
			this.LabelName.Size = new System.Drawing.Size (70, 20);
			this.LabelName.TabIndex = 4;
			this.LabelName.Text = "用户名：";
			this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TextBoxName
			// 
			this.TextBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxName.Location = new System.Drawing.Point (55, 5);
			this.TextBoxName.Name = "TextBoxName";
			this.TextBoxName.Size = new System.Drawing.Size (150, 21);
			this.TextBoxName.TabIndex = 5;
			// 
			// TextBoxAge
			// 
			this.TextBoxAge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxAge.Location = new System.Drawing.Point (55, 30);
			this.TextBoxAge.Name = "TextBoxAge";
			this.TextBoxAge.Size = new System.Drawing.Size (150, 21);
			this.TextBoxAge.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point (5, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (70, 20);
			this.label1.TabIndex = 6;
			this.label1.Text = "年  龄：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point (5, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (70, 20);
			this.label2.TabIndex = 8;
			this.label2.Text = "学  校：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TextBoxRemark
			// 
			this.TextBoxRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.TextBoxRemark.Location = new System.Drawing.Point (55, 80);
			this.TextBoxRemark.Name = "TextBoxRemark";
			this.TextBoxRemark.Size = new System.Drawing.Size (150, 21);
			this.TextBoxRemark.TabIndex = 11;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point (5, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (70, 20);
			this.label3.TabIndex = 10;
			this.label3.Text = "备  注：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ComboBoxSchool
			// 
			this.ComboBoxSchool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.ComboBoxSchool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ComboBoxSchool.FormattingEnabled = true;
			this.ComboBoxSchool.Location = new System.Drawing.Point (55, 55);
			this.ComboBoxSchool.Name = "ComboBoxSchool";
			this.ComboBoxSchool.Size = new System.Drawing.Size (150, 20);
			this.ComboBoxSchool.TabIndex = 12;
			// 
			// LabelError
			// 
			this.LabelError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.LabelError.Location = new System.Drawing.Point (5, 105);
			this.LabelError.Name = "LabelError";
			this.LabelError.Size = new System.Drawing.Size (200, 20);
			this.LabelError.TabIndex = 13;
			this.LabelError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormAdd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (211, 161);
			this.Controls.Add (this.LabelError);
			this.Controls.Add (this.ComboBoxSchool);
			this.Controls.Add (this.TextBoxRemark);
			this.Controls.Add (this.label3);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.TextBoxAge);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.TextBoxName);
			this.Controls.Add (this.LabelName);
			this.Controls.Add (this.ButtonCancel);
			this.Controls.Add (this.ButtonConfirm);
			this.Name = "FormAdd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Form_Add";
			this.ResumeLayout (false);
			this.PerformLayout ();

		}

		#endregion
		private System.Windows.Forms.Button ButtonConfirm;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Label LabelName;
		private System.Windows.Forms.TextBox TextBoxName;
		private System.Windows.Forms.TextBox TextBoxAge;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TextBoxRemark;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox ComboBoxSchool;
		private System.Windows.Forms.Label LabelError;
	}
}