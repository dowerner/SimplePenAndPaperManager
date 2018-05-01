using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SimplePenAndPaperManager.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for TransformationGizmo.xaml
    /// </summary>
    public partial class TransformationGizmo : UserControl
    {
        public static readonly DependencyProperty XProperty =
                DependencyProperty.Register("X", typeof(double), typeof(TransformationGizmo), new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty YProperty =
                DependencyProperty.Register("Y", typeof(double), typeof(TransformationGizmo), new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register("Orientation", typeof(double), typeof(TransformationGizmo), new PropertyMetadata(0.0, OrientationPropertyChanged));

        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransformationGizmo gizmo = (TransformationGizmo)d;
            gizmo.RotationIndicatorTransform.Angle = gizmo.Orientation;
            gizmo.RotationBallTransform.Angle = gizmo.Orientation;
            gizmo.RotationText.Text = string.Format("{0}°", Math.Round(gizmo.Orientation, 2));
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public double Orientation
        {
            get { return (double)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private bool _dragging = false;

        public TransformationGizmo()
        {
            InitializeComponent();

            RotationBall.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(Ellipse_MouseLeftButtonDown));
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point position = e.GetPosition(Center);
                double angle = Math.Atan2(position.Y, position.X)*180/Math.PI + 180;
                Orientation = angle;
            }
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragging = true;
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
        }
    }
}
