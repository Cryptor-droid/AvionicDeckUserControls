using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeckSharp
{
    public partial class Gauge : UserControl
    {
        #region Properties
        private string name = "Gauge";
        private double Value = 0.0;
        private double Min = 0.0;
        private double Max = 100.0;
        private double SlitInt = 1;
        private double ValInte = 1;
        private Font SF = SystemFonts.DefaultFont;
        private Font VF = SystemFonts.DefaultFont;

        [Description(""), Category("Gauge")]
        public Color GaugeNameColor { get; set; }
        [Description(""), Category("Gauge")]
        public Color ScreenColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Color ScreenValueColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Color ScreenSlitColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Color PointerNeedleColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Color PointerCenterColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Color DigitalScreenColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Color DigitalValueColor{ get; set; }
        [Description(""), Category("Gauge")]
        public Font ScreenFont { get => SF; set { SF = value; this.Refresh(); } }
        [Description(""), Category("Gauge")]
        public Font ValueFont { get => VF; set { VF = value; this.Refresh(); } }

        [Description("The name displayed on the gauge"), Category("Gauge")]
        public string Gauge_Name
        {
            get => name;
            set { name = value; this.Refresh(); }
        }
        [Description("Gauge Value"), Category("Gauge")]
        public double Gauge_Value
        {
            get
            {
                return Value;
            }
            set 
            {
                var val = value;
                if (val > Max) val = Max;
                if (val < Min) val = Min;
                Value = val;
                this.Refresh();
            }
        }
        [Description("Gauge Maximum Value"), Category("Gauge")]
        public double Gauge_Max
        {
            get => Max;
            set  { Max = value; this.Refresh(); }
        }
        [Description("Screen slit interval"), Category("Gauge")]
        public double Gauge_Slit_Interval
        {
            get => SlitInt;
            set { SlitInt = value; this.Refresh(); }
        }
        [Description("Screen slit interval"), Category("Gauge")]
        public double Gauge_Value_Interval
        {
            get => ValInte;
            set { ValInte = value; this.Refresh(); }
        }
        [Description("Gauge Minimum Value"), Category("Gauge")]
        public double Gauge_Min
        {
            get => Min;
            set { Min = value; this.Refresh(); }
        }

        private float SFratio;
        private float VFratio;
        private float GNratio;
        #endregion
        public Gauge()
        {
            InitializeComponent();
        }

        private void Gauge_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            Refresher.Enabled = true;
            int gaugeEdge = Width > Height ? Height : Width;
            int SFpx = (int)((SF.Size / 72) * 96);
            int VFpx = (int)((VF.Size / 72) * 96);
            int GNpx = (int)((this.Font.Size / 72) * 96);

            SFratio = (float)SFpx / (float)gaugeEdge;
            VFratio = (float)VFpx / (float)gaugeEdge;
            GNratio = (float)GNpx / (float)gaugeEdge;
        }
        static Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
        private void Gauge_Paint(object sender, PaintEventArgs e)
        {
            int gaugeEdge = Width > Height ? Height : Width;
            gaugeEdge = gaugeEdge < 5 ? 5 : gaugeEdge;
            Size GaugeSize = new Size(gaugeEdge, gaugeEdge);
            Point GaugeOffset = new Point((Width > Height ? Width - Height : 0) / 2, (Width > Height ? 0 : Height - Width) / 2);
            Bitmap bm = new Bitmap(gaugeEdge, gaugeEdge);
            Graphics gr = System.Drawing.Graphics.FromImage(bm);
            Point center = new Point(gaugeEdge / 2, gaugeEdge / 2);

            // Drawing border of gauge
            gr.FillEllipse(new SolidBrush(ScreenColor), new Rectangle(0, 0, gaugeEdge - 1, gaugeEdge - 1));
            gr.DrawEllipse(new Pen(this.ForeColor), new Rectangle(0, 0, gaugeEdge - 1, gaugeEdge - 1));

            // Drawing Gauge limit //
            Point GaugeLimit1 = RotatePoint(new Point(GaugeSize.Width / 2, 0), center, 135);
            Point GaugeLimit2 = RotatePoint(new Point(GaugeSize.Width / 2, 0), center, 225);
            gr.FillPie(new SolidBrush(DigitalScreenColor), new Rectangle(0, 0, gaugeEdge - 1, gaugeEdge - 1), 135, -90);
            gr.DrawLine(new Pen(this.ForeColor), center, GaugeLimit1);
            gr.DrawLine(new Pen(this.ForeColor), center, GaugeLimit2);

            // Drawing name of the gauge and the value //
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Far;
            format.Alignment = StringAlignment.Center;
            gr.DrawString(name, new Font(this.Font.FontFamily, GNratio * gaugeEdge, FontStyle.Regular, GraphicsUnit.Pixel), new SolidBrush(GaugeNameColor), new RectangleF(center.X-(gaugeEdge/4),center.Y,gaugeEdge/2, (float)(gaugeEdge / 2.2)), format);
            gr.DrawString(Value.ToString(), new Font(VF.FontFamily, VFratio * gaugeEdge, FontStyle.Regular,GraphicsUnit.Pixel), new SolidBrush(DigitalValueColor), new PointF(center.X, (float)(center.Y + gaugeEdge / 4)),format);

            // Draw Gauge Display //
            int resolution =int.MaxValue;
            for (double i = Min; i <= Max; i+=SlitInt)
            {

                double angle = ((250.0 / Math.Abs(Max - Min)) * Math.Abs(i - Min)) - 125.0;
                Point DisplayLine1 = RotatePoint(new Point(GaugeSize.Width / 2, 1), center, angle);
                Point DisplayLine2 = RotatePoint(new Point(GaugeSize.Width / 2, gaugeEdge / 20), center, angle);
                Point DisplayLine3 = RotatePoint(new Point(GaugeSize.Width / 2, gaugeEdge / 10), center, angle);
                gr.DrawLine(new Pen(ScreenSlitColor), DisplayLine1, DisplayLine2);
                
                if (resolution >= ValInte)
                {
                    format.LineAlignment = StringAlignment.Center;
                    gr.DrawString(i.ToString(), new Font(SF.FontFamily, SFratio * gaugeEdge, FontStyle.Bold, GraphicsUnit.Pixel), new SolidBrush(ScreenValueColor), DisplayLine3, format);
                    resolution = 0;
                }
                resolution++;

            }

            // Drawing Gauge Pointer //
            double GaugePointerangle = ((250.0 / Math.Abs(Max - Min)) * Math.Abs(Value - Min)) - 125.0;
            Point GaugePointerTip = RotatePoint(new Point((gaugeEdge / 2), gaugeEdge / 10),center,GaugePointerangle);
            Point GaugePointerBack1 = RotatePoint(new Point((gaugeEdge / 2) - (gaugeEdge / 20), gaugeEdge / 2), center, GaugePointerangle);
            Point GaugePointerBack2 = RotatePoint(new Point((gaugeEdge / 2) + (gaugeEdge / 20), gaugeEdge / 2), center, GaugePointerangle);
            Point[] GaugePointerArray = { GaugePointerTip, GaugePointerBack1, GaugePointerBack2 };
            gr.FillPolygon(new SolidBrush(PointerNeedleColor),GaugePointerArray);
            gr.DrawLine(new Pen(this.ForeColor), GaugePointerTip, GaugePointerBack1);
            gr.DrawLine(new Pen(this.ForeColor), GaugePointerTip, GaugePointerBack2);
            gr.FillEllipse(new SolidBrush(PointerCenterColor), new Rectangle(center.X - (gaugeEdge / 14), center.Y - (gaugeEdge / 14), (gaugeEdge / 7) - 1, (gaugeEdge / 7) - 1));
            gr.DrawEllipse(new Pen(this.ForeColor), new Rectangle(center.X - (gaugeEdge / 14), center.Y - (gaugeEdge / 14), (gaugeEdge / 7) - 1, (gaugeEdge / 7) - 1));
            
            e.Graphics.DrawImageUnscaled(bm, GaugeOffset);
        }

        private void Gauge_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void Refresher_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
