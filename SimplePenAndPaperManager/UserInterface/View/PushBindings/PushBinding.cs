using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SimplePenAndPaperManager.UserInterface.View.PushBindings
{
    public class PushBinding : FreezableBinding
    {
        public static DependencyProperty TargetPropertyMirrorProperty =
            DependencyProperty.Register("TargetPropertyMirror",
                                        typeof(object),
                                        typeof(PushBinding));
        public static DependencyProperty TargetPropertyListenerProperty =
            DependencyProperty.Register("TargetPropertyListener",
                                        typeof(object),
                                        typeof(PushBinding),
                                        new UIPropertyMetadata(null, OnTargetPropertyListenerChanged));

        private static void OnTargetPropertyListenerChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PushBinding pushBinding = (PushBinding)sender;
            pushBinding.TargetPropertyValueChanged();
        }

        public PushBinding()
        {
            Mode = BindingMode.OneWayToSource;
        }

        public object TargetPropertyMirror
        {
            get { return GetValue(TargetPropertyMirrorProperty); }
            set { SetValue(TargetPropertyMirrorProperty, value); }
        }

        public object TargetPropertyListener
        {
            get { return GetValue(TargetPropertyListenerProperty); }
            set { SetValue(TargetPropertyListenerProperty, value); }
        }

        [DefaultValue(null)]
        public string TargetProperty { get; set; }

        [DefaultValue(null)]
        public DependencyProperty TargetDependencyProperty { get; set; }

        public void SetupTargetBinding(DependencyObject targetObject)
        {
            if (targetObject == null) return;

            if (DesignerProperties.GetIsInDesignMode(this)) return;

            Binding listenerBinding = new Binding
            {
                Source = targetObject,
                Mode = BindingMode.TwoWay
            };

            if (TargetDependencyProperty != null) listenerBinding.Path = new PropertyPath(TargetDependencyProperty);
            else listenerBinding.Path = new PropertyPath(TargetProperty);

            BindingOperations.SetBinding(this, TargetPropertyListenerProperty, listenerBinding);
            BindingOperations.SetBinding(this, TargetPropertyMirrorProperty, Binding);

            TargetPropertyValueChanged();
            if(targetObject is FrameworkElement)
            {
                ((FrameworkElement)targetObject).Loaded += delegate { TargetPropertyValueChanged(); };
            }
            else if(targetObject is FrameworkContentElement)
            {
                ((FrameworkContentElement)targetObject).Loaded += delegate { TargetPropertyValueChanged(); };
            }
        }

        private void TargetPropertyValueChanged()
        {
            object targetPropertyValue = GetValue(TargetPropertyListenerProperty);
            this.SetValue(TargetPropertyMirrorProperty, targetPropertyValue);
        }

        protected override void CloneCore(Freezable sourceFreezable)
        {
            PushBinding pushBinding = (PushBinding)sourceFreezable;
            TargetProperty = pushBinding.TargetProperty;
            TargetDependencyProperty = pushBinding.TargetDependencyProperty;
            base.CloneCore(sourceFreezable);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new PushBinding();
        }
    }
}
