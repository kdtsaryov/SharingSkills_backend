using System;
using System.ComponentModel.DataAnnotations;

namespace SharingSkills_HSE_backend.Models
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class Message
    {
        /// <summary>
        /// ID сообщения
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Время отправки
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        [StringLength(301, ErrorMessage = "Слишком много символов")]
        public string Text { get; set; }

        /// <summary>
        /// Почта отправителя
        /// </summary>
        [RegularExpression(@"[_A-Za-z0-9]+@edu.hse.ru", ErrorMessage = "Некорректный почтовый адрес отправителя")]
        public string SenderMail { get; set; }

        /// <summary>
        /// Почта получателя
        /// </summary>
        [RegularExpression(@"[_A-Za-z0-9]+@edu.hse.ru", ErrorMessage = "Некорректный почтовый адрес получателя")]
        public string ReceiverMail { get; set; }

        /// <summary>
        /// Прочитано ли сообщение получателем
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Фотография
        /// </summary>
        public byte[] Photo { get; set; }
    }
}
