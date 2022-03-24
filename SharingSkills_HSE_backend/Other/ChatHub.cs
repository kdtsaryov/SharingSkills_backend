using Microsoft.AspNetCore.SignalR;
using SharingSkills_HSE_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingSkills_HSE_backend.Other
{
    /// <summary>
    /// Класс с информацией о подключениях к чату
    /// </summary>
    public class UserChatInfo
    {
        /// <summary>
        /// ID подключения
        /// </summary>
        public string ConnectionID { get; set; }

        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mail">Почта</param>
        /// <param name="connectionId">ID подключения</param>
        public UserChatInfo(string mail, string connectionId)
        {
            this.Mail = mail;
            this.ConnectionID = connectionId;
        }
    }

    /// <summary>
    /// Хаб всех чатов
    /// </summary>
    public class ChatHub : Hub
    {
        public static List<UserChatInfo> users = new List<UserChatInfo>();

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="mail">Почта получателя</param>
        /// <param name="message">Сообщение</param>
        public async Task Send(string mail, Message message)
        {
            // Находим пользователя по почте в подключенных к хабу
            var user = users.Where(u => u.Mail == mail).FirstOrDefault();
            if (user != null)
            {
                await Clients.Client(user.ConnectionID).SendAsync("Receive", message);
                await Clients.Caller.SendAsync("Receive", message);
            }
            else
                await Clients.Caller.SendAsync("Receive", message);
        }

        /// <summary>
        /// Сопоставление почты с ID подключения
        /// </summary>
        /// <param name="mail">Почта</param>
        public void SetMail(string mail)
        {
            // Находим пользователя с текущим ID подключения в подключенных к хабу
            var user = users.SingleOrDefault(u => u.ConnectionID == Context.ConnectionId);
            // Если такой есть, то устанавливаем почту
            if (user != null)
                user.Mail = mail;
        }

        /// <summary>
        /// Подключение клиента к хабу
        /// </summary>
        public override Task OnConnectedAsync()
        {
            // Находим пользователя с текущим ID подключения в подключенных к хабу
            var user = users.Where(u => u.ConnectionID == Context.ConnectionId).SingleOrDefault();
            // Если такого нет, то добавляем с пустым значением почты
            if (user == null)
            {
                user = new UserChatInfo("", Context.ConnectionId);
                users.Add(user);
            }
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Отключение клиента от хаба
        /// </summary>
        public override Task OnDisconnectedAsync(Exception e)
        {
            // Находим пользователя с текущим ID подключения в подключенных к хабу
            var user = users.Where(p => p.ConnectionID == Context.ConnectionId).FirstOrDefault();
            // Если такой есть, то удаляем его из списка подключенных
            if (user != null)
                users.Remove(user);
            return base.OnDisconnectedAsync(e);
        }
    }
}