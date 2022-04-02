using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharingSkills_HSE_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingSkills_HSE_backend.Controllers
{
    /// <summary>
    /// Контроллер сообщений
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly SharingSkillsContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public MessagesController(SharingSkillsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает конкретное сообщение
        /// </summary>
        /// <param name="id">ID сообщения</param>
        // GET: api/Messages/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(long id)
        {
            var m = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (m == null)
                return NotFound();
            return m;
        }

        /// <summary>
        /// Возвращает все сообщения
        /// </summary>
        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        /// <summary>
        /// Возвращает 100 последних сообщений между двумя конкретными пользователями
        /// </summary>
        /// <param name="mail1">Почта 1</param>
        /// <param name="mail2">Почта 2</param>
        /// <param name="n">Какая по счету подгрузка</param>
        // GET: api/Messages/kdtsaryov@edu.hse.ru/eoshtanko@edu.hse.ru/0
        [HttpGet("{mail1}/{mail2}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(string mail1, string mail2, int n)
        {
            // Находим пользователей
            var u1 = await _context.Users.FindAsync(mail1);
            var u2 = await _context.Users.FindAsync(mail2);
            if (u1 == null || u2 == null || u1 == u2)
                return BadRequest();
            // Находим все сообщения между этими двумя пользователями
            var messages = await _context.Messages.ToListAsync();
            messages = messages.FindAll(m => (m.SenderMail == mail1 && m.ReceiverMail == mail2) || (m.SenderMail == mail2 && m.ReceiverMail == mail1));
            // Сортируем по времени отправки
            messages = messages.OrderByDescending(m => m.SendTime).ToList();
            // Пропускаем n*100 сообщений
            messages = messages.Skip(n * 100).ToList();
            // Берем 100 сообщений
            messages = messages.Take(100).ToList();
            return messages;
        }

        /// <summary>
        /// Изменение конкретного сообщения
        /// </summary>
        /// <param name="id">ID сообщения</param>
        /// <param name="message">Сообщение</param>
        // PUT: api/Messages/kdtsaryov@edu.hse.ru
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(long id, Message message)
        {
            if (id != message.Id)
                return BadRequest();
            _context.Entry(message).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Добавление нового сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            // Если такое сообщение уже есть
            var m = await _context.Messages.FindAsync(message.Id);
            if (m != null)
                return BadRequest();
            // Находим отправителя и получателя
            var sender = await _context.Users.FindAsync(message.SenderMail);
            var receiver = await _context.Users.FindAsync(message.ReceiverMail);
            if (sender == null || receiver == null || sender == receiver)
                return BadRequest();
            // Добавляем сообщение
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="id">ID сообщения</param>
        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return NotFound();
            // Удаляем сообщение
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Проверка наличия сообщения
        /// </summary>
        /// <param name="id">ID сообщения</param>
        private bool MessageExists(long id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
