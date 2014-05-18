using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MediaCaptureTest
{
    public static class MainHelper
    {

        /// <summary>
        /// 將錯誤或警告訊息，用 Toast notifications 來顯示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public static void ShowToast(string title, string msg)
        {
            //var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            var elements = toastXml.GetElementsByTagName("text");
            elements[0].AppendChild(toastXml.CreateTextNode(title));
            elements[1].AppendChild(toastXml.CreateTextNode(msg));

            // Create a ToastNotification from our XML, and send it to the Toast Notification Manager
            var toast = new ToastNotification(toastXml);
            toast.Failed += (o, args) =>
            {
                var message = args.ErrorCode;
            };
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }


    }
}
