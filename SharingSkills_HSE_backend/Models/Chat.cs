using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharingSkills_HSE_backend.Models
{
    /// <summary>
    /// Переписка
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// ID переписки
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Последнее сообщение
        /// </summary>
        [StringLength(301, ErrorMessage = "Слишком много символов")]
        public string LastMessage { get; set; }

        /// <summary>
        /// Время отправки последнего сообщения
        /// </summary>
        public string SendTime { get; set; }

        /// <summary>
        /// Почта первого собеседника
        /// </summary>
        [RegularExpression(@"[_A-Za-z0-9]+@edu.hse.ru", ErrorMessage = "Некорректный почтовый адрес собеседника")]
        public string Mail1 { get; set; }

        /// <summary>
        /// Имя первого собеседника
        /// </summary>
        [StringLength(41, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 40 символов")]
        [RegularExpression(@"[А-Яа-яЁёA-Za-z]+$", ErrorMessage = "В имени могут присутствовать только буквы")]
        public string Name1 { get; set; }

        /// <summary>
        /// Фамилия первого собеседника
        /// </summary>
        [StringLength(41, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 40 символов")]
        [RegularExpression(@"[А-Яа-яЁёA-Za-z]+$", ErrorMessage = "В фамилии могут присутствовать только буквы")]
        public string Surname1 { get; set; }

        /// <summary>
        /// Фотография первого собеседника
        /// </summary>
        public byte[] Photo1 { get; set; }

        /// <summary>
        /// Почта второго собеседника
        /// </summary>
        [RegularExpression(@"[_A-Za-z0-9]+@edu.hse.ru", ErrorMessage = "Некорректный почтовый адрес собеседника")]
        public string Mail2 { get; set; }

        /// <summary>
        /// Имя второго собеседника
        /// </summary>
        [StringLength(41, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 40 символов")]
        [RegularExpression(@"[А-Яа-яЁёA-Za-z]+$", ErrorMessage = "В имени могут присутствовать только буквы")]
        public string Name2 { get; set; }

        /// <summary>
        /// Фамилия второго собеседника
        /// </summary>
        [StringLength(41, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 40 символов")]
        [RegularExpression(@"[А-Яа-яЁёA-Za-z]+$", ErrorMessage = "В фамилии могут присутствовать только буквы")]
        public string Surname2 { get; set; }

        /// <summary>
        /// Фотография второго собеседника
        /// </summary>
        public byte[] Photo2 { get; set; }

        /// <summary>
        /// Собеседники
        /// </summary>
        public List<User> Users { get; set; } = new List<User>();
    }
}
