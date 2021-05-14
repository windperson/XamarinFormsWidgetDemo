using Android.Appwidget;
using Android.Content;
using Xamarin.Forms;
using XamarinFormsWidgetDemo.Services;

[assembly: Dependency(typeof(XamarinFormsWidgetDemo.Droid.UpdateWidgetService))]
namespace XamarinFormsWidgetDemo.Droid
{
    public class UpdateWidgetService : IUpdateWidget
    {
        public void PushUpdate(string updated)
        {
            var context = Android.App.Application.Context;
            var intent = new Intent(context, typeof(DemoClickWidgetProvider));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra("push_data", updated);
            intent.SetAction(DemoClickWidgetProvider.ACTION_WIDGET_RECEIVE_UPDATE);
            //var pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, 0);

            UpdateAppWidget(context, intent);
        }

        static void UpdateAppWidget(Context context, Intent intent)
        {
            var widgetManager = AppWidgetManager.GetInstance(context);
            var appWidgetIds = widgetManager.GetAppWidgetIds(new ComponentName(context, Java.Lang.Class.FromType(typeof(DemoClickWidgetProvider))));
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            context.SendBroadcast(intent);
        }
    }
}