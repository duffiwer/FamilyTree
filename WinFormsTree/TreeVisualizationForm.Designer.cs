namespace WinFormsTree
{
    partial class TreeVisualizationForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  TreeView для отображения семейного дерева.
        /// </summary>
        private System.Windows.Forms.TreeView treeView;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();

            this.treeView.Location = new System.Drawing.Point(13, 13);  
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(760, 400);
            this.treeView.TabIndex = 0; 

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);  
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450); 
            this.Controls.Add(this.treeView);
            this.Name = "TreeVisualizationForm";  
            this.Text = "Дерево семьи";  
            this.Load += new System.EventHandler(this.TreeVisualizationForm_Load);  
            this.ResumeLayout(false);
        }

        #endregion
    }
}
