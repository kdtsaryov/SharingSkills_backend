using System;
using System.ComponentModel.DataAnnotations;

namespace SharingSkills_HSE_backend.Models
{
    /// <summary>
    /// Отзыв
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// ID отзыва
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Оценка
        /// </summary>
        [Range(0, 5, ErrorMessage = "Некорректная оценка")]
        public int Grade { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [StringLength(301, ErrorMessage = "Слишком много символов")]
        public string Comment { get; set; } = "";

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
    }
}
