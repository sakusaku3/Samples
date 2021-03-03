using System.Windows;

namespace Calculator
{
    public class MessengerBehavior
    {
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.RegisterAttached(
                "Actions",
                typeof(ActionCollection),
                typeof(MessengerBehavior),
                new UIPropertyMetadata(null, OnActionsPropertyChanged));

        public static ActionCollection GetActions(Window target)
        {
            return (ActionCollection)target.GetValue(ActionsProperty);
        }

        public static void SetActions(Window target, ActionCollection value)
        {
            target.SetValue(ActionsProperty, value);
        }

        private static void OnActionsPropertyChanged(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Window window)) return;
            if ((e.NewValue as ActionCollection) == null) return;

            window.Loaded -= new RoutedEventHandler(window_Loaded);
            window.Loaded += new RoutedEventHandler(window_Loaded);
        }

        static void window_Loaded(object sender, RoutedEventArgs e)
        {
            ActionCollection ac = GetActions(sender as Window);
            ac.RegisterAll(sender as Window);
        }
    }
}
