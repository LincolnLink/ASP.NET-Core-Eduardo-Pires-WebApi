using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinhaAPICoreDemo.Controllers
{
    [Route("api/[controller]")]    
    public class ValuesController : MainController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> ObterTodos()
        {
            var valores = new string[] { "value1", "ActionResult tipado" };

            if (valores.Length < 5000)
                return BadRequest();

            return valores;
        }

        // GET api/values
        [HttpGet]
        public ActionResult ObterResultado()
        {
            var valores = new string[] { "value1", "ActionResult sem tipar" };

            if (valores.Length < 5000)
                return BadRequest();

            return Ok(valores);
        }

        // GET api/values/obter-valores
        [HttpGet("obter-valores")]
        public IEnumerable<string> ObterValores()
        {
            return new string[] { "value1", "value2", "value3", "value4" };
        }

        // GET api/values/5
        [HttpGet("{id:int}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values/
        [HttpPost]
        public void Post([FromBody] string value)
        {            
        }

        // POST api/values/
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult Post2(Product product)
        {
            if(product.Id == 0 )
            {
                return BadRequest();
            }

            // add no banco

            // retorna um ok, mas seria um 200
            //return Ok(product);

            // retorna um 201
            return CreatedAtAction(actionName:"Post2", product);


            // retorna um 201
            // return CreatedAtAction(actionName: nameof(Post2), product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


    // Classe model para teste
    public class Product 
    {
        public int Id { get; set; }
        public string nome { get; set; }

        public int idade { get; set; }
    }


    // Criando um Result personalizado
    [ApiController]
    public abstract class MainController: ControllerBase
    {
        protected ActionResult CustomResponse(object result = null)
        {
            if(OperacaoValida())
            {
                return Ok(value: new
                {
                   sucess = true,
                   data = result
                });
            }

            return BadRequest(error: new
            {
                sucess = false,
                errors = ObterErros()
            });
        }

        public bool OperacaoValida()
        {
            // as minhas validacoes
            return true;
        }

        // Retorna uma coleção de erros
        protected string ObterErros()
        {
            return "";
        }
    }


}
