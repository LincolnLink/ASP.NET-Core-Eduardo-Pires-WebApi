using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DevIO.Api.ViewModels;
using DevIO.Business.Models;
using DevIO.Business.Interfaces;

namespace DevIO.Api.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;        

        public FornecedoresController(
            IMapper mapper,
            INotificador notificador,
            IFornecedorService fornecedorService,
            IEnderecoRepository enderecoRepository,
            IFornecedorRepository fornecedorRepository) : base(notificador)
        {
            _mapper = mapper;           
            _fornecedorService = fornecedorService;
            _enderecoRepository = enderecoRepository;
            _fornecedorRepository = fornecedorRepository;
        } 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
            // Converte o o model fornecedor para fornecedorViewModel.
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return Ok(fornecedores); //200 Ok
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            // Converte o o model fornecedor para fornecedorViewModel.
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound(); //404 não encontrado.

            return Ok(fornecedor); //200 Ok
        }


        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
                       
            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                NotificarErro(mensagem: "O id informado não é o mesmo que foi passado  na query");
                return CustomResponse(fornecedorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);
                        
            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));
                       
            return CustomResponse(fornecedorViewModel);
        }



        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            //return Ok(fornecedorViewModel);
            return CustomResponse(fornecedorViewModel);
        }

        [HttpGet("endereco/{id:guid}")]
        public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
        }

        //[ClaimsAuthorize("Fornecedor", "Atualizar")]
        [HttpPut("endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }


        public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
        /*
        [HttpPost()]
        public async Task<ActionResult<FornecedorViewModel>> Post(FornecedorViewModel fornecedorViewModel)
        {
            // Se os dados não estiver valido, retorna um BadRequest!
            if (!ModelState.IsValid) return BadRequest();

            // Ultiliza o serviço para poder entrar na regra de negocio!
            
            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            var result = await _fornecedorService.Adicionar(fornecedor);

            if (!result) return BadRequest();// erro nas regras

            return Ok(fornecedor); //200 Ok
        }*/

        /*
        [HttpPut("{id:guid}")] // não precisa definir o fronBody
        public async Task<ActionResult<FornecedorViewModel>> Update(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return BadRequest(); //Se o id for diferente o id do objeto, retorna 500

            // Se os dados não estiver valido, retorna um BadRequest!
            if (!ModelState.IsValid) return BadRequest();

            // Ultiliza o serviço para poder entrar na regra de negocio!
            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            var result = await _fornecedorService.Atualizar(fornecedor);

            if (!result) return BadRequest();// erro nas regras

            return Ok(fornecedor); //200 Ok
        }*/

        /*
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Delete(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterPorId(id));

            if (fornecedor == null) return BadRequest();

            var result = await _fornecedorService.Remover(id);

            if (!result) return BadRequest();

            return Ok(fornecedor);
        }*/

    }
}
