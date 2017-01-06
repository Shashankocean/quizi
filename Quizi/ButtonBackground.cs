using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace Quizi
{
    class ButtonBackground
    {
        public LinearGradientBrush levelBackground()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(77, 158, 224), 0.0));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(52, 111, 159), 0.495));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(77, 158, 224), 0.840));
            changegradient.RelativeTransform = transgp;
            changegradient.Opacity = 40.0;
            return changegradient;
        }
        public LinearGradientBrush brushtrueA_C_Default()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Colors.Black, 0.060));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(52, 111, 159), 0.900));
            changegradient.RelativeTransform = transgp;

            return changegradient;


        }
        public LinearGradientBrush brushtrueB_D_Default()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(24, 50, 72), 0.0));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(52, 111, 159), 0.495));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(77, 158, 224), 0.840));
            changegradient.RelativeTransform = transgp;

            return changegradient;
        }
        public LinearGradientBrush brushtrueA_C_true()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Colors.Black, 0.060));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(7, 223, 7), 0.494));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(52, 111, 159), 0.900));
            changegradient.RelativeTransform = transgp;

            return changegradient;


        }
        public LinearGradientBrush brushtrueA_C_false()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Colors.Black, 0.060));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(248, 55, 16), 0.494));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(52, 111, 159), 0.900));
            changegradient.RelativeTransform = transgp;

            return changegradient;
        }
        public LinearGradientBrush brushtrueB_D_true()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(24, 50, 72), 0.0));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(7, 223, 7), 0.495));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(77, 158, 224), 0.840));
            changegradient.RelativeTransform = transgp;

            return changegradient;
        }
        public LinearGradientBrush brushtrueB_D_false()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(24, 50, 72), 0.0));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(248, 55, 16), 0.495));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(77, 158, 224), 0.840));
            changegradient.RelativeTransform = transgp;

            return changegradient;
        }
        public LinearGradientBrush brushtrueA_C_Locked()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Colors.Black, 0.060));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(240, 129, 38), 0.494));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(52, 111, 159), 0.900));
            changegradient.RelativeTransform = transgp;

            return changegradient;


        }
        public LinearGradientBrush brushtrueB_D_Locked()
        {
            RotateTransform ro = new RotateTransform();
            ro.Angle = -2.3;
            TransformGroup transgp = new TransformGroup();
            transgp.Children.Add(ro);
            LinearGradientBrush changegradient = new LinearGradientBrush();
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(24, 50, 72), 0.0));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(240, 129, 38), 0.495));
            changegradient.GradientStops.Add(new GradientStop(Color.FromRgb(77, 158, 224), 0.840));
            changegradient.RelativeTransform = transgp;

            return changegradient;
        }
        
    }
}
