﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharingSkills_HSE_backend.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Почта
        /// </summary>
        [RegularExpression(@"[_A-Za-z0-9]+@edu.hse.ru", ErrorMessage = "Некорректный почтовый адрес")]
        [Key]
        public string Mail { get; set; }

        /// <summary>
        /// Код подтверждения почты, хранящийся на сервере
        /// </summary>
        public int ConfirmationCodeServer { get; set; }

        /// <summary>
        /// Код подтверждения почты, запрашиваемый у пользователя
        /// </summary>
        [Compare("ConfirmationCodeServer", ErrorMessage = "Неверный код подтверждения")]
        public int ConfirmationCodeUser { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [DataType(DataType.Password)]
        [StringLength(41, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 40 символов")]
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [StringLength(41, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 40 символов")]
        [RegularExpression(@"[А-Яа-яЁёA-Za-z]+$", ErrorMessage = "В имени могут присутствовать только буквы")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [StringLength(41, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от 2 до 40 символов")]
        [RegularExpression(@"[А-Яа-яЁёA-Za-z]+$", ErrorMessage = "В фамилии могут присутствовать только буквы")]
        public string Surname { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [DateRange(13, 100, ErrorMessage = "Некорректная дата рождения")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        [Range(0, 2, ErrorMessage = "Некорректный пол")]
        public int Gender { get; set; }

        /// <summary>
        /// Курс
        /// <summary>
        [Range(0, 7, ErrorMessage = "Некорректный ID курса")]
        public int StudyingYearId { get; set; }

        /// <summary>
        /// Образовательная программа
        /// </summary>
        [Range(0, 53, ErrorMessage = "Некорректный ID образовательной программы")] 
        public int MajorId { get; set; }

        /// <summary>
        /// Расположение корпуса
        /// </summary>
        [Range(0, 5, ErrorMessage = "Некорректный ID расположения корпуса")]
        public int CampusLocationId { get; set; }

        /// <summary>
        /// Общежитие
        /// </summary>
        [Range(0, 12, ErrorMessage = "Некорректный ID общежития")]
        public int DormitoryId { get; set; }

        /// <summary>
        /// О себе
        /// </summary>
        [StringLength(201, ErrorMessage = "Слишком много символов")]
        public string About { get; set; } = "";

        /// <summary>
        /// Ссылка на мессенджер (telegram или vk)
        /// </summary>
        [RegularExpression(@"(vk.com/|t.me/)+[-A-Za-z_.0-9]+$", ErrorMessage = "Некорректный формат ссылки")]
        public string Contact { get; set; }

        /// <summary>
        /// Фотография
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// Обмены
        /// </summary>
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// Навыки
        /// </summary>
        public List<Skill> Skills { get; set; } = new List<Skill>();

        /// <summary>
        /// Отзывы
        /// </summary>
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        /// <summary>
        /// Количество оценок
        /// </summary>
        public int GradesCount { get; set; } = 0;

        /// <summary>
        /// Сумма оценок
        /// </summary>
        public int GradesSum { get; set; } = 0;

        /// <summary>
        /// Средняя оценка
        /// </summary>
        public double AverageGrade { get; set; } = 0;

        /// <summary>
        /// Является ли пользователь модератором
        /// </summary>
        public bool IsModer { get; set; } = false;
    }
}
