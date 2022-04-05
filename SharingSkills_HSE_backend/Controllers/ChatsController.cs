using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharingSkills_HSE_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingSkills_HSE_backend.Controllers
{
    /// <summary>
    /// Контроллер переписок
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly SharingSkillsContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public ChatsController(SharingSkillsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает конкретную переписку
        /// </summary>
        /// <param name="id">ID переписки</param>
        // GET: api/Chats/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChat(long id)
        {
            var c = await _context.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (c == null)
                return NotFound();
            return c;
        }

        /// <summary>
        /// Возвращает все переписки
        /// </summary>
        // GET: api/Chats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            return await _context.Chats.ToListAsync();
        }

        /// <summary>
        /// Возвращает все переписки конкретного пользователя
        /// </summary>
        /// <param name="mail">Почта</param>
        // GET: api/Chats/kdtsaryov@edu.hse.ru/user
        [HttpGet("{mail}/user")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats(string mail)
        {
            // Находим пользователя
            var user = await _context.Users.Include(u => u.Chats).FirstOrDefaultAsync(u => u.Mail == mail);
            if (user == null)
                return BadRequest();
            return user.Chats;
        }

        /// <summary>
        /// Изменение конкретной переписки
        /// </summary>
        /// <param name="id">ID переписки</param>
        /// <param name="chat">Переписка</param>
        // PUT: api/Chats/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChat(long id, Chat chat)
        {
            if (id != chat.Id)
                return BadRequest();
            _context.Entry(chat).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Добавление новой переписки
        /// </summary>
        /// <param name="chat">Переписка</param>
        // POST: api/Chats
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            // Если такая переписка уже есть
            var c = await _context.Chats.FindAsync(chat.Id);
            if (c != null)
                return BadRequest();
            // Находим собеседников
            var user1 = await _context.Users.FindAsync(chat.Mail1);
            var user2 = await _context.Users.FindAsync(chat.Mail2);
            if (user1 == null || user2 == null || user1 == user2)
                return BadRequest();
            // Добавляем переписку
            user1.Chats.Add(chat);
            user2.Chats.Add(chat);
            _context.Chats.Add(chat);
            // Сохраняем изменения
            _context.Entry(user1).State = EntityState.Modified;
            _context.Entry(user2).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetChat", new { id = chat.Id }, chat);
        }

        /// <summary>
        /// Удаление переписки
        /// </summary>
        /// <param name="id">ID переписки</param>
        // DELETE: api/Chats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(long id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
                return NotFound();
            // Удаляем переписку
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Проверка наличия переписки
        /// </summary>
        /// <param name="id">ID переписки</param>
        private bool ChatExists(long id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
