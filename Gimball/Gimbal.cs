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
    public partial class Gimbal : UserControl
    {
        private double pitch;
        private double yaw;
        private double roll;
        [Description("Roll"), Category("Gimbal")]
        public double Gimbal_Roll
        {
            get
            {
                return roll;
            }
            set
            {
                roll = value;
                this.Refresh();
            }
        }
        [Description("Pitch"), Category("Gimbal")]
        public double Gimbal_Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
                this.Refresh();
            }
        }
        [Description("Yaw"), Category("Gimbal")]
        public double Gimbal_Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = value;
                this.Refresh();
            }
        }
        public Gimbal()
        {
            InitializeComponent();
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
        private void Gimbal_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }

        private void Gimbal_Paint(object sender, PaintEventArgs e)
        {
            StringFormat centerstr = new StringFormat();
            centerstr.Alignment = StringAlignment.Center;
            centerstr.LineAlignment = StringAlignment.Center;
            Bitmap buff = new Bitmap(Width, Height);
            Graphics gr = Graphics.FromImage(buff);
            int hWidth = Width / 2; int hHeight = Height / 2;
            Point center = new Point(hWidth, hHeight);

            // draw glound green
            double slit_0 = ((Height / 90) * -(0 - pitch)) + hHeight;

            Point PitchZero1 = RotatePoint(new Point(-100000, (int)slit_0), center, roll);
            Point PitchZero2 = RotatePoint(new Point(+100000, (int)slit_0), center, roll);
            Point PitchZero3 = RotatePoint(new Point(-100000,  100000), center, roll);
            Point PitchZero4 = RotatePoint(new Point(+100000,  100000), center, roll);
            Point PitchZero5 = RotatePoint(new Point(-100000, -100000), center, roll);
            Point PitchZero6 = RotatePoint(new Point(+100000, -100000), center, roll);
            Point[] groundpl = { PitchZero1, PitchZero2, PitchZero3, PitchZero4 };
            Point[] skypolyg = { PitchZero1, PitchZero2, PitchZero5, PitchZero6 };
            gr.FillPolygon(new SolidBrush(Color.SaddleBrown), groundpl);
            gr.FillPolygon(new SolidBrush(Color.Aqua), skypolyg);
            // Draw Gui
            //gr.DrawLine(new Pen(this.ForeColor), new Point(0, hHeight), new Point(Width, hHeight));
            // gr.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(hWidth - Width / 4, hHeight - 3, hWidth,6));
            gr.DrawLine(new Pen(this.ForeColor), new Point(hWidth-20, hHeight), new Point(hWidth+20, hHeight));
            gr.DrawLine(new Pen(this.ForeColor), new Point(hWidth , hHeight - 10), new Point(hWidth , hHeight + 10));
            gr.FillEllipse(new SolidBrush(this.BackColor), new Rectangle(hWidth - 5,hHeight-5,10,10));
            gr.DrawEllipse(new Pen(this.ForeColor), new Rectangle(hWidth - 5, hHeight - 5, 10, 10));

            // Draw Angleman
            Point pitchMark1 = RotatePoint(new Point(hWidth - Width / 4, 100000), center, roll);
            Point pitchMark2 = RotatePoint(new Point(hWidth - Width / 4, -100000), center, roll);
            Point pitchMark3 = RotatePoint(new Point(hWidth + Width / 4, 100000), center, roll);
            Point pitchMark4 = RotatePoint(new Point(hWidth + Width / 4, -100000), center, roll);
            gr.DrawLine(new Pen(this.ForeColor), pitchMark1, pitchMark2);
            gr.DrawLine(new Pen(this.ForeColor), pitchMark3, pitchMark4);

            int counter = 0;
            int vertcnt = 0;
            for (double i = -180; i < 180; i++)
            {
                double slit_Y = ((Height / 90) * -(i - pitch)) + hHeight;
                
                Point PitchSlit  = RotatePoint(new Point((hWidth - Width / 4) - 30, (int)slit_Y), center, roll);
                Point PitchSlit1 = RotatePoint(new Point((hWidth - Width / 4) - 20, (int)slit_Y),center,roll);
                Point PitchSlit2 = RotatePoint(new Point((hWidth - Width / 4) - 10, (int)slit_Y), center, roll);
                Point PitchSlit3 = RotatePoint(new Point((hWidth - Width / 4), (int)slit_Y), center, roll);

                Point PitchSlit4 = RotatePoint(new Point((hWidth + Width / 4) + 30, (int)slit_Y), center, roll);
                Point PitchSlit5 = RotatePoint(new Point((hWidth + Width / 4) + 20, (int)slit_Y), center, roll);
                Point PitchSlit6 = RotatePoint(new Point((hWidth + Width / 4) + 10, (int)slit_Y), center, roll);
                Point PitchSlit7 = RotatePoint(new Point((hWidth + Width / 4), (int)slit_Y), center, roll);

                gr.DrawLine(new Pen(this.ForeColor), PitchZero1, PitchZero2);
                if (counter >= 5)
                {
                    if (vertcnt >= 5)
                    {
                        gr.DrawLine(new Pen(Color.Gray), PitchSlit, PitchSlit4);
                        vertcnt = 0;
                    }
                    gr.DrawString(i.ToString(), this.Font, new SolidBrush(this.ForeColor), PitchSlit.X,PitchSlit.Y, centerstr);
                    gr.DrawString(i.ToString(), this.Font, new SolidBrush(this.ForeColor), PitchSlit4.X, PitchSlit4.Y, centerstr);
                    gr.DrawLine(new Pen(this.ForeColor), PitchSlit1, PitchSlit3);
                    gr.DrawLine(new Pen(this.ForeColor), PitchSlit5, PitchSlit7);
                    counter = 0;
                    vertcnt++;
                }
                else
                {
                    gr.DrawLine(new Pen(this.ForeColor), PitchSlit2, PitchSlit3);
                    gr.DrawLine(new Pen(this.ForeColor), PitchSlit6, PitchSlit7);
                }
                counter++;
            }
            gr.DrawLine(new Pen(this.ForeColor), RotatePoint(new Point(-100000, hHeight - (Height / 4)),center,roll), RotatePoint(new Point(100000, hHeight - (Height / 4)), center, roll));
            gr.DrawLine(new Pen(this.ForeColor), RotatePoint(new Point(-100000, hHeight + (Height / 4)), center, roll), RotatePoint(new Point(100000, hHeight + (Height / 4)), center, roll));
            counter = 0;
            vertcnt = 0;
            for (double i = -180; i < 180; i++)
            {
                double slit_X = ((Width / 90) * -(i - yaw)) + hWidth;
                Point YawSlit = RotatePoint(new Point((int)slit_X,  hHeight - (Height / 4)), center, roll);
                Point YawSlit1 = RotatePoint(new Point((int)slit_X, hHeight - (Height / 4) + 10), center, roll);
                Point YawSlit2 = RotatePoint(new Point((int)slit_X, hHeight - (Height / 4) + 20), center, roll);
                Point YawSlit3 = RotatePoint(new Point((int)slit_X, hHeight - (Height / 4) + 30), center, roll);

                Point YawSlit4 = RotatePoint(new Point((int)slit_X, hHeight + (Height / 4)), center, roll);
                Point YawSlit5 = RotatePoint(new Point((int)slit_X, hHeight + (Height / 4) - 10), center, roll);
                Point YawSlit6 = RotatePoint(new Point((int)slit_X, hHeight + (Height / 4) - 20), center, roll);
                Point YawSlit7 = RotatePoint(new Point((int)slit_X, hHeight + (Height / 4) - 30), center, roll);

                if (counter >= 5)
                {
                    if (vertcnt >= 5)
                    {
                        gr.DrawLine(new Pen(Color.Gray), YawSlit, YawSlit4);
                        vertcnt = 0;
                    }
                    gr.DrawString(i.ToString(), this.Font, new SolidBrush(this.ForeColor), YawSlit3.X, YawSlit3.Y,centerstr);
                    gr.DrawString(i.ToString(), this.Font, new SolidBrush(this.ForeColor), YawSlit7.X, YawSlit7.Y, centerstr);
                    gr.DrawLine(new Pen(this.ForeColor), YawSlit, YawSlit2);
                    gr.DrawLine(new Pen(this.ForeColor), YawSlit4, YawSlit6);
                    counter = 0;
                    vertcnt++;
                }
                else
                {
                    gr.DrawLine(new Pen(this.ForeColor), YawSlit , YawSlit1);
                    gr.DrawLine(new Pen(this.ForeColor), YawSlit4, YawSlit5);
                }
                counter++;
            }
            gr.DrawLine(new Pen(this.ForeColor), new Point(hWidth - 20, hHeight), new Point(hWidth + 20, hHeight));
            gr.DrawLine(new Pen(this.ForeColor), new Point(hWidth, hHeight - 10), new Point(hWidth, hHeight + 10));
            gr.FillEllipse(new SolidBrush(this.BackColor), new Rectangle(hWidth - 5, hHeight - 5, 10, 10));
            gr.DrawEllipse(new Pen(this.ForeColor), new Rectangle(hWidth - 5, hHeight - 5, 10, 10));
            e.Graphics.DrawImage(buff, new PointF(0, 0));
        }

        private void Gimbal_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
