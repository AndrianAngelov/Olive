namespace Olive.Desktop.WPF.Behavior
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Controls;

    public static class CommandsBehavior
    {

        private static void ExecuteClickCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var control = d as UIElement;
            var control = d as ListViewItem;
            if (control == null)
            {
                return;
            }
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                control.Selected += (snd, args) =>
                {
                    var command = (snd as ListViewItem).GetValue(CommandsBehavior.ClickProperty) as ICommand;
                    command.Execute((snd as ListViewItem));
                };
            }

            //OldVersio
            //var control = d as UIElement;
            //if (control == null)
            //{
            //    return;
            //}
            //if ((e.NewValue != null) && (e.OldValue == null))
            //{
            //    control.MouseLeftButtonDown += (snd, args) =>
            //    {
            //        var command = (snd as UIElement).GetValue(CommandsBehavior.ClickProperty) as ICommand;
            //        command.Execute(args);
            //    };
            //}
        }

        private static void ExecuteKeyUpCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBox;
            if (control == null)
            {
                return;
            }
            if ((e.NewValue != null) && (e.OldValue == null))
            {
                control.KeyUp += (snd, args) =>
                {
                    var command = (snd as TextBox).GetValue(CommandsBehavior.KeyUpProperty) as ICommand;
                    command.Execute((snd as TextBox).Text);
                    //command.Execute(args);
                };
            }
        }

        public static ICommand GetClick(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClickProperty);
        }

        public static void SetClick(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClickProperty, value);
        }

        // Using a DependencyProperty as the backing store for Click.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickProperty =
            DependencyProperty.RegisterAttached("Click", 
                                                typeof(ICommand), 
                                                typeof(CommandsBehavior), 
                                                new PropertyMetadata(ExecuteClickCommand));



        public static ICommand GetKeyUp(TextBox textBox)
        {
            return (ICommand)textBox.GetValue(KeyUpProperty);
        }

        public static void SetKeyUp(TextBox textBox, ICommand value)
        {
            textBox.SetValue(KeyUpProperty, value);
        }

        // Using a DependencyProperty as the backing store for KeyUpProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyUpProperty =
            DependencyProperty.RegisterAttached("KeyUp", 
                                                typeof(ICommand),
                                                typeof(CommandsBehavior),
                                                new UIPropertyMetadata(ExecuteKeyUpCommand));

        
    }
}
