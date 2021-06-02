using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaAPICoreDemo.Model;
using System;
using System.Collections.Generic;
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






    }
}
