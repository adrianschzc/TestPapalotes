using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Requerimiento admin #1: asignar roles a los usuarios registrados por grupo.
    public class UsuariosController : Controller
    {
        private readonly ControlEscolarContext _db;
        public UsuariosController(ControlEscolarContext db) => _db = db;

        private void CargarCombos(Usuario? u = null)
        {
            ViewBag.Roles = new SelectList(Enum.GetValues<Rol>(), u?.Rol);
            ViewBag.Grupos = new SelectList(_db.Grupos.OrderBy(g => g.Clave), "Id", "Clave", u?.GrupoId);
        }

        public async Task<IActionResult> Index() =>
            View(await _db.Usuarios.Include(u => u.Grupo).ToListAsync());

        public IActionResult Create()
        {
            CargarCombos();
            return View(new Usuario());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid) { CargarCombos(usuario); return View(usuario); }
            _db.Add(usuario);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            CargarCombos(usuario);
            return View(usuario);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();
            if (!ModelState.IsValid) { CargarCombos(usuario); return View(usuario); }
            _db.Update(usuario);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _db.Usuarios.Include(u => u.Grupo).FirstOrDefaultAsync(u => u.Id == id);
            return usuario == null ? NotFound() : View(usuario);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario != null) _db.Usuarios.Remove(usuario);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
