using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;

namespace XamarinFormsWidgetDemo.Droid
{
    [BroadcastReceiver(Label = "Widget Button Click")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [IntentFilter(new string[] { "com.demoe.xamarinformswidgetdemo.ACTION_WIDGET_BUTTON_01" })]
    [IntentFilter(new string[] { "com.demoe.xamarinformswidgetdemo.ACTION_WIDGET_BUTTON_02" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/demo_click_widget_provider")]
    public class DemoClickWidgetProvider : AppWidgetProvider
    {
        public static string ACTION_WIDGET_BUTTON_01 = "Button 1 click";
        public static string ACTION_WIDGET_BUTTON_02 = "Button 2 click";

        public static string ACTION_WIDGET_RECEIVE_UPDATE = "widget_text updated";

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            UpdateAppWidget(context, appWidgetManager, appWidgetIds);
        }

        #region private method for handling UI composing & event hook up

        private void UpdateAppWidget(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds, string updatedText = null)
        {
            //Update Widget layout
            //Run when create widget or meet update time
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(DemoClickWidgetProvider)));
            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds, updatedText));
        }

        private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds, string updatedText = null)
        {
            //Build widget layout
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.demo_click_widget);

            if (!string.IsNullOrEmpty(updatedText))
            {
                //Change text of element on Widget
                SetTextViewText(widgetView, updatedText);
            }

            //Handle click event of button on Widget
            RegisterClicks(context, appWidgetIds, widgetView);

            return widgetView;
        }

        private static void SetTextViewText(RemoteViews widgetView, string text)
        {
            widgetView.SetTextViewText(Resource.Id.textView1, text);
        }

        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            //Button 1
            widgetView.SetOnClickPendingIntent(Resource.Id.button01, GetPendingSelfIntent(context, appWidgetIds, ACTION_WIDGET_BUTTON_01));

            //Button 2
            widgetView.SetOnClickPendingIntent(Resource.Id.button02, GetPendingSelfIntent(context, appWidgetIds, ACTION_WIDGET_BUTTON_02));
        }

        private PendingIntent GetPendingSelfIntent(Context context, int[] appWidgetIds, string action)
        {
            var intent = new Intent(context, typeof(DemoClickWidgetProvider));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            intent.SetAction(action);
            return PendingIntent.GetBroadcast(context, 0, intent, 0);
        }

        #endregion

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (ACTION_WIDGET_BUTTON_01.Equals(intent.Action))
            {
                Log.Info("DemoWidget", "Bring back Xamarin Forms App via start Activity");
                
                var packageManager = context.PackageManager;
                var packageName = "tw.idv.demo.xamarinformswidgetdemo";
                var launchIntent = packageManager.GetLaunchIntentForPackage(packageName);
                context.StartActivity(launchIntent);

                return;
            }


            if (ACTION_WIDGET_BUTTON_02.Equals(intent.Action))
            {
                Log.Info("DemoWidget", "Bring back Xamarin Forms App via Shell routing");

                var packageManager = context.PackageManager;
                var packageName = "tw.idv.demo.xamarinformswidgetdemo";
                var launchIntent = packageManager.GetLaunchIntentForPackage(packageName);
                context.StartActivity(launchIntent);

                Shell.Current.GoToAsync("//Demo/PushDataDemoPage").ConfigureAwait(false);

                return;
            }


            if (ACTION_WIDGET_RECEIVE_UPDATE.Equals(intent.Action))
            {
                var bundle = intent.Extras;
                if (bundle == null)
                {
                    return;
                }

                var updated = bundle.GetString("push_data");
                Log.Info("DemoWidget", $"receive updated data: {updated}");

                var appWidgetIds = bundle.GetIntArray(AppWidgetManager.ExtraAppwidgetIds);
                var appWidgetManager = AppWidgetManager.GetInstance(context);

                UpdateAppWidget(context, appWidgetManager, appWidgetIds, updated);
            }
        }
    }
}