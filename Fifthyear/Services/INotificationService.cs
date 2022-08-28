using Fifthyear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Services
{
    public interface INotificationService
    {
        Task<NotificationResponse> SendNotification(NotificationModel notificationModel);
    }
}
