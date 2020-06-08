using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dc_demo_api.Models;

namespace dc_demo_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FolderController : ControllerBase
  {
    private readonly DCDemoContext _context;

    public FolderController(DCDemoContext context)
    {
      _context = context;
    }

    // GET: api/Folder
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Folder>>> GetFolder()
    {
      // must load every folder because EF Core does not handle recursive relations
      var folders = await _context.Folder
       .Include(folder => folder.SubFolders)
       .ToListAsync();
      return folders
        .Where(f => f.ParentId == null)
        .ToList();
    }

    // GET: api/Folder/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Folder>> GetFolder(int id)
    {
      // must load every folder because EF Core does not handle recursive relations
      var folders = await _context.Folder
       .Include(folder => folder.SubFolders)
       .ToListAsync();
      folders
        .Where(sf => sf.ParentId == null)
        .ToList();
      var folder = folders.FirstOrDefault(f => f.FolderId == id);

      if (folder == null)
      {
        return NotFound();
      }

      return folder;
    }

    // PUT: api/Folder/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFolder(int id, Folder folder)
    {
      if (id != folder.FolderId)
      {
        return BadRequest();
      }

      _context.Entry(folder).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FolderExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Folder
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Folder>> PostFolder(Folder folder)
    {
      _context.Folder.Add(folder);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (FolderExists(folder.FolderId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetFolder", new { id = folder.FolderId }, folder);
    }

    // DELETE: api/Folder/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Folder>> DeleteFolder(int id)
    {
      var folder = await _context.Folder.FindAsync(id);
      if (folder == null)
      {
        return NotFound();
      }

      _context.Folder.Remove(folder);
      await _context.SaveChangesAsync();

      return folder;
    }

    private bool FolderExists(int id)
    {
      return _context.Folder.Any(e => e.FolderId == id);
    }
  }
}
