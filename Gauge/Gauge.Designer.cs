
namespace DeckSharp
{
    partial class Gauge
    {
        /// <summary> 
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Refresher = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Refresher
            // 
            this.Refresher.Enabled = true;
            this.Refresher.Interval = 500;
            this.Refresher.Tick += new System.EventHandler(this.Refresher_Tick);
            // 
            // Gauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Gauge";
            this.Size = new System.Drawing.Size(252, 241);
            this.Load += new System.EventHandler(this.Gauge_Load);
            this.SizeChanged += new System.EventHandler(this.Gauge_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Gauge_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Refresher;
    }
}
