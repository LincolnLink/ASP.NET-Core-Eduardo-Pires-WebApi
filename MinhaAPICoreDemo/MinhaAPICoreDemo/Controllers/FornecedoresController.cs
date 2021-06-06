using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaAPICoreDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaAPICoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FornecedoresController(ApiDbContext context )
        {
            _context = context;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
        {
            return await _context.Fornecedores.ToListAsync();
        }

        // GET: api/Fornecedores/5
        /// <summary>
        /// Se não existir retorna NotFound, se não retorna o fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(Guid id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);

            if(fornecedor == null)
            {
                return NotFound();
            }

            return fornecedor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fornecedor"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(Guid id, Fornecedor fornecedor)
        {
            if(id != fornecedor.Id)
            {
                return BadRequest();
            }

            //Seta o satus no ef para modificado.
            _context.Entry(fornecedor).State = EntityState.Modified;

            try
            {
                // tentativa de savar
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                // Verifica se realmente existe!
                if(!FornecedorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // 204 modificado?
            return NoContent();
        }

        // POST: api/Fornecedores
        /// <summary>
        /// Retorna uma rota com o objeto novo que foi salvo no banco
        /// </summary>
        /// <param name="fornecedor"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Fornecedor>> PostFornecedor(Fornecedor fornecedor)
        {
            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            // pesquisar melhor sobre esse metodo!
            return CreatedAtAction("GetFornecedor", new { id = fornecedor.Id }, fornecedor);
        }

        /// <summary>
        /// Busca o id no banco, e depois deleta o objeto!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fornecedor>> DeleteFornecedor(Guid id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if(fornecedor == null)
            {
                return NotFound();
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return fornecedor;
        }

        /// <summary>
        /// Verifica se existe o id passado no banco!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool FornecedorExists(Guid id)
        {
            return _context.Fornecedores.Any(e => e.Id == id);
        }




    }
}
