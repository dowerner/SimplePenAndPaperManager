using SimplePenAndPaperManager.UserInterface.View.States;
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
        public delegate void TransformationChangedHandler(object sender, TransformationEvent transformationEvent);
        public event TransformationChangedHandler TransformationChanged;

        public static readonly DependencyProperty XProperty =
                DependencyProperty.Register("X", typeof(double), typeof(TransformationGizmo), new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty YProperty =
                DependencyProperty.Register("Y", typeof(double), typeof(TransformationGizmo), new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register("Orientation", typeof(double), typeof(TransformationGizmo), new PropertyMetadata(0.0, OrientationPropertyChanged));

        private static void XDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransformationGizmo gizmo = (TransformationGizmo)d;
            if(!(bool)e.NewValue) gizmo.IsHitTestVisible = true;
        }

        private static void YDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransformationGizmo gizmo = (TransformationGizmo)d;
            if (!(bool)e.NewValue) gizmo.IsHitTestVisible = true;
        }

        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TransformationGizmo gizmo = (TransformationGizmo)d;
            gizmo.RotationIndicatorTransform.Angle = gizmo.Orientation;
            gizmo.RotationBallTransform.Angle = gizmo.Orientation;
            gizmo.RotationText.Text = string.Format("{0}°", Math.Round(gizmo.Orientation, 2));
        }

        public static readonly DependencyProperty DraggingXProperty =
                DependencyProperty.Register("DraggingX", typeof(bool), typeof(TransformationGizmo), new PropertyMetadata(false, XDraggingPropertyChanged));

        public static readonly DependencyProperty DraggingYProperty =
                DependencyProperty.Register("DraggingY", typeof(bool), typeof(TransformationGizmo), new PropertyMetadata(false, YDraggingPropertyChanged));

        public static readonly DependencyProperty IsRotatingProperty =
               DependencyProperty.Register("IsRotating", typeof(bool), typeof(TransformationGizmo), new PropertyMetadata(false, null));

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

        public bool DraggingX
        {
            get { return (bool)GetValue(DraggingXProperty); }
            set
            {
                SetValue(DraggingXProperty, value);
                if (!value) IsHitTestVisible = true;
            }
        }

        public bool DraggingY
        {
            get { return (bool)GetValue(DraggingYProperty); }
            set
            {
                SetValue(DraggingYProperty, value);
                if(!value) IsHitTestVisible = true;
            }
        }

        public bool IsRotating
        {
            get { return (bool)GetValue(IsRotatingProperty); }
            set
            {
                SetValue(IsRotatingProperty, value);
                if (!value) IsHitTestVisible = true;
            }
        }

        public TransformationGizmo()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsRotating)
            {
                Point position = e.GetPosition(Center);
                double angle = Math.Atan2(position.Y, position.X)*180/Math.PI + 180;
                Orientation = angle;
            }
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Center);
            X = point.X;
            Y = point.Y;

            if (sender is Path)
            {
                Path arrow = (Path)sender;
                if (arrow.Name == "XManipulator") DraggingX = true;
                else DraggingY = true;
                IsHitTestVisible = false;
                TransformationChanged?.Invoke(this, TransformationEvent.TranslationStarted);
            }
            else if(sender is Rectangle)
            {
                DraggingX = true;
                DraggingY = true;
                IsHitTestVisible = false;
                TransformationChanged?.Invoke(this, TransformationEvent.TranslationStarted);
            }
            else
            {
                e.MouseDevice.Capture((UIElement)sender);
                IsRotating = true;
                TransformationChanged?.Invoke(this, TransformationEvent.RotationStarted);
            }           
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingX || DraggingY) TransformationChanged?.Invoke(this, TransformationEvent.TranslationEnded);
            else TransformationChanged?.Invoke(this, TransformationEvent.RotationEnded);
            IsRotating = false;
            DraggingX = false;
            DraggingY = false;
            e.MouseDevice.Capture(null);
        }
    }
}
