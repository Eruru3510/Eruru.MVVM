
namespace WindowsFormsApp1 {
	partial class UserControlForm1Item {
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose (bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent () {
			this.LabelName = new System.Windows.Forms.Label();
			this.LabelAge = new System.Windows.Forms.Label();
			this.LabelSchool = new System.Windows.Forms.Label();
			this.LabelRemark = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ButtonEdit = new System.Windows.Forms.Button();
			this.ButtonDelete = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// LabelName
			// 
			this.LabelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelName.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelName.Location = new System.Drawing.Point(0, 0);
			this.LabelName.Name = "LabelName";
			this.LabelName.Size = new System.Drawing.Size(165, 40);
			this.LabelName.TabIndex = 0;
			this.LabelName.Text = "label1";
			this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelAge
			// 
			this.LabelAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelAge.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelAge.Location = new System.Drawing.Point(165, 0);
			this.LabelAge.Name = "LabelAge";
			this.LabelAge.Size = new System.Drawing.Size(165, 40);
			this.LabelAge.TabIndex = 1;
			this.LabelAge.Text = "label1";
			this.LabelAge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelSchool
			// 
			this.LabelSchool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelSchool.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelSchool.Location = new System.Drawing.Point(330, 0);
			this.LabelSchool.Name = "LabelSchool";
			this.LabelSchool.Size = new System.Drawing.Size(165, 40);
			this.LabelSchool.TabIndex = 2;
			this.LabelSchool.Text = "label1";
			this.LabelSchool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelRemark
			// 
			this.LabelRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelRemark.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelRemark.Location = new System.Drawing.Point(495, 0);
			this.LabelRemark.Name = "LabelRemark";
			this.LabelRemark.Size = new System.Drawing.Size(165, 40);
			this.LabelRemark.TabIndex = 3;
			this.LabelRemark.Text = "label1";
			this.LabelRemark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(660, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(165, 40);
			this.label1.TabIndex = 4;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ButtonEdit
			// 
			this.ButtonEdit.Location = new System.Drawing.Point(665, 10);
			this.ButtonEdit.Name = "ButtonEdit";
			this.ButtonEdit.Size = new System.Drawing.Size(75, 23);
			this.ButtonEdit.TabIndex = 5;
			this.ButtonEdit.Text = "编辑";
			this.ButtonEdit.UseVisualStyleBackColor = true;
			// 
			// ButtonDelete
			// 
			this.ButtonDelete.Location = new System.Drawing.Point(745, 10);
			this.ButtonDelete.Name = "ButtonDelete";
			this.ButtonDelete.Size = new System.Drawing.Size(75, 23);
			this.ButtonDelete.TabIndex = 6;
			this.ButtonDelete.Text = "删除";
			this.ButtonDelete.UseVisualStyleBackColor = true;
			// 
			// UserControlForm1Item
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ButtonDelete);
			this.Controls.Add(this.ButtonEdit);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.LabelRemark);
			this.Controls.Add(this.LabelSchool);
			this.Controls.Add(this.LabelAge);
			this.Controls.Add(this.LabelName);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "UserControlForm1Item";
			this.Size = new System.Drawing.Size(825, 40);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label LabelName;
		private System.Windows.Forms.Label LabelAge;
		private System.Windows.Forms.Label LabelSchool;
		private System.Windows.Forms.Label LabelRemark;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ButtonEdit;
		private System.Windows.Forms.Button ButtonDelete;
	}
}
