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
    public class AssetController : ControllerBase
    {
        private readonly DCDemoContext _context;

        public AssetController(DCDemoContext context)
        {
            _context = context;
        }

        // GET: api/Asset
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAsset()
        {
            return await _context.Asset.ToListAsync();
        }

        // GET: api/Asset/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var asset = await _context.Asset.FindAsync(id);

            if (asset == null)
            {
                return NotFound();
            }

            return asset;
        }

        // PUT: api/Asset/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsset(int id, Asset asset)
        {
            if (id != asset.AssetId)
            {
                return BadRequest();
            }

            _context.Entry(asset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetExists(id))
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

        // POST: api/Asset
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Asset>> PostAsset(Asset asset)
        {
            _context.Asset.Add(asset);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AssetExists(asset.AssetId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAsset", new { id = asset.AssetId }, asset);
        }

        // DELETE: api/Asset/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Asset>> DeleteAsset(int id)
        {
            var asset = await _context.Asset.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            _context.Asset.Remove(asset);
            await _context.SaveChangesAsync();

            return asset;
        }

        private bool AssetExists(int id)
        {
            return _context.Asset.Any(e => e.AssetId == id);
        }
    }
}
