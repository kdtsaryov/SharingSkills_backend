using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharingSkills_HSE_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharingSkills_HSE_backend.Other;
using System.Linq;

namespace SharingSkills_HSE_backend.Controllers
{
    /// <summary>
    /// Контроллер отзывов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly SharingSkillsContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public FeedbacksController(SharingSkillsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает все отзывы
        /// </summary>
        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        /// <summary>
        /// Возвращает конкретный отзыв
        /// </summary>
        /// <param name="id">ID отзыва</param>
        // GET api/Feedbacks/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var f = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);
            if (f == null)
                return NotFound();
            return f;
        }

        /// <summary>
        /// Добавление нового отзыва
        /// </summary>
        /// <param name="feedback">Отзыв</param>
        // POST api/Feedbacks
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
            // Если такой отзыв уже есть
            var f = await _context.Feedbacks.FindAsync(feedback.Id);
            if (f != null)
                return BadRequest();
            // Находим отправителя и получателя
            var sender = await _context.Users.FindAsync(feedback.SenderMail);
            var receiver = await _context.Users.FindAsync(feedback.ReceiverMail);
            if (sender == null || receiver == null || sender == receiver)
                return BadRequest();
            // Цензурим комментарий
            feedback.Comment = Censorship.DoCensorship(feedback.Comment);
            // Добавляем отзыв
            receiver.GradesCount++;
            receiver.GradesSum += feedback.Grade;
            receiver.AverageGrade = (double)receiver.GradesSum / receiver.GradesCount;
            receiver.Feedbacks.Add(feedback);
            _context.Feedbacks.Add(feedback);
            // Даем пользователю права модератора
            if (receiver.GradesCount >= 5 && receiver.AverageGrade >= 3.5)
                receiver.IsModer = true;
            // Оповещаем получателя нового отзыва
            //await Mail.SendEmailAsync(receiver.Mail, "Новый отзыв",
            //    $"{sender.Name} {sender.Surname} оставил(а) Вам новый отзыв.\n" +
            //    $"Зайдите в приложение \"Обмен навыками\", чтобы узнать детали.");
            // Сохраняем изменения
            _context.Entry(receiver).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }

        /// <summary>
        /// Изменение отзыва
        /// </summary>
        /// <param name="id">ID отзыва</param>
        /// <param name="feedback">Отзыв</param>
        // PUT api/Feedbacks/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(long id, Feedback feedback)
        {
            if (id != feedback.Id)
                return BadRequest();;
            // Цензурим комментарий
            feedback.Comment = Censorship.DoCensorship(feedback.Comment);
            _context.Entry(feedback).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Удаление отзыва пользователем
        /// </summary>
        /// <param name="mail">Почта</param>
        /// <param name="id">ID отзыва</param>
        // DELETE api/Feedbacks/user/kdtsaryov@edu.hse.ru/1
        [HttpDelete("user/{mail}/{id}")]
        public async Task<IActionResult> DeleteFeedbackUser(string mail, long id)
        {
            var f = await _context.Feedbacks.FindAsync(id);
            if (f == null)
                return NotFound();
            // Чужой отзыв удалять нельзя
            if (f.SenderMail != mail)
                return BadRequest();
            // Находим получателя
            var receiver = await _context.Users.FindAsync(f.ReceiverMail);
            // Удаляем отзыв
            receiver.GradesCount--;
            receiver.GradesSum -= f.Grade;
            receiver.AverageGrade = receiver.GradesCount > 0 ? (double)receiver.GradesSum / receiver.GradesCount : 0;
            receiver.Feedbacks.Remove(f);
            _context.Feedbacks.Remove(f);
            // Сохраняем изменения
            _context.Entry(receiver).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Удаление отзыва модератором
        /// </summary>
        /// <param name="mail">Почта</param>
        /// <param name="id">ID отзыва</param>
        // DELETE api/Feedbacks/moder/kdtsaryov@edu.hse.ru/1
        [HttpDelete("moder/{mail}/{id}")]
        public async Task<IActionResult> DeleteFeedbackModer(string mail, long id)
        {
            var f = await _context.Feedbacks.FindAsync(id);
            if (f == null)
                return NotFound();
            // Отзыв себе удалять нельзя
            if (f.ReceiverMail == mail)
                return BadRequest();
            // Находим получателя
            var receiver = await _context.Users.FindAsync(f.ReceiverMail);
            // Удаляем отзыв
            receiver.GradesCount--;
            receiver.GradesSum -= f.Grade;
            receiver.AverageGrade = receiver.GradesCount > 0 ? (double)receiver.GradesSum / receiver.GradesCount : 0;
            receiver.Feedbacks.Remove(f);
            _context.Feedbacks.Remove(f);
            // Сохраняем изменения
            _context.Entry(receiver).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Проверка наличия отзыва
        /// </summary>
        /// <param name="id">ID отзыва</param>
        private bool FeedbackExists(long id)
        {
            return _context.Feedbacks.Any(f => f.Id == id);
        }
    }
}
