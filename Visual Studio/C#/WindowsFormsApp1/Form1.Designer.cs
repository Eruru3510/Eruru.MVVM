
namespace WindowsFormsApp1 {
	partial class Form1 {
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

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent () {
			this.ButtonAdd = new System.Windows.Forms.Button();
			this.FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.LabelRemark = new System.Windows.Forms.Label();
			this.LabelSchool = new System.Windows.Forms.Label();
			this.LabelAge = new System.Windows.Forms.Label();
			this.LabelName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ButtonAdd
			// 
			this.ButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonAdd.Location = new System.Drawing.Point(755, 450);
			this.ButtonAdd.Name = "ButtonAdd";
			this.ButtonAdd.Size = new System.Drawing.Size(75, 23);
			this.ButtonAdd.TabIndex = 0;
			this.ButtonAdd.Text = "添加";
			this.ButtonAdd.UseVisualStyleBackColor = true;
			// 
			// FlowLayoutPanel
			// 
			this.FlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.FlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FlowLayoutPanel.Location = new System.Drawing.Point(5, 45);
			this.FlowLayoutPanel.Name = "FlowLayoutPanel";
			this.FlowLayoutPanel.Size = new System.Drawing.Size(825, 400);
			this.FlowLayoutPanel.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(665, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(165, 40);
			this.label1.TabIndex = 9;
			this.label1.Text = "操作";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelRemark
			// 
			this.LabelRemark.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LabelRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelRemark.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelRemark.Location = new System.Drawing.Point(500, 5);
			this.LabelRemark.Name = "LabelRemark";
			this.LabelRemark.Size = new System.Drawing.Size(165, 40);
			this.LabelRemark.TabIndex = 8;
			this.LabelRemark.Text = "备注";
			this.LabelRemark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelSchool
			// 
			this.LabelSchool.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LabelSchool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelSchool.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelSchool.Location = new System.Drawing.Point(335, 5);
			this.LabelSchool.Name = "LabelSchool";
			this.LabelSchool.Size = new System.Drawing.Size(165, 40);
			this.LabelSchool.TabIndex = 7;
			this.LabelSchool.Text = "毕业学校";
			this.LabelSchool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelAge
			// 
			this.LabelAge.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LabelAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelAge.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelAge.Location = new System.Drawing.Point(170, 5);
			this.LabelAge.Name = "LabelAge";
			this.LabelAge.Size = new System.Drawing.Size(165, 40);
			this.LabelAge.TabIndex = 6;
			this.LabelAge.Text = "年龄";
			this.LabelAge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelName
			// 
			this.LabelName.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LabelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LabelName.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
			this.LabelName.Location = new System.Drawing.Point(5, 5);
			this.LabelName.Name = "LabelName";
			this.LabelName.Size = new System.Drawing.Size(165, 40);
			this.LabelName.TabIndex = 5;
			this.LabelName.Text = "用户名";
			this.LabelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(836, 477);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.LabelRemark);
			this.Controls.Add(this.LabelSchool);
			this.Controls.Add(this.LabelAge);
			this.Controls.Add(this.LabelName);
			this.Controls.Add(this.FlowLayoutPanel);
			this.Controls.Add(this.ButtonAdd);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button ButtonAdd;
		private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label LabelRemark;
		private System.Windows.Forms.Label LabelSchool;
		private System.Windows.Forms.Label LabelAge;
		private System.Windows.Forms.Label LabelName;
	}
}

