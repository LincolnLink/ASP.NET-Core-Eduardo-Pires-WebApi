using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IProdutoService _produtoService;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(
            IMapper mapper,
            INotificador notificador,
            IProdutoService produtoServico,
            IProdutoRepository produtoRepository) : base(notificador)
        {
            _mapper = mapper;
            _produtoService = produtoServico;
            _produtoRepository = produtoRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
        {
            var produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            // Converte o o model fornecedor para fornecedorViewModel.
            var fornecedor = await ObterProdutosFornecedor(id);

            if (fornecedor == null) return NotFound(); //404 não encontrado.

            return Ok(fornecedor); //200 Ok
        }


        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            // Criando nome personalizado, e chamando o método que faz upload de imagem.
            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
            if(!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                // Caso de erro notifica o erro para o client.
                return CustomResponse(produtoViewModel);
            }

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                NotificarErro(mensagem: "O id informado não é o mesmo que foi passado  na query");
                return CustomResponse(produtoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produtoViewModel = await ObterProdutosFornecedor(id);

            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remover(id);

            // Return Ok(produtoViewModel);
            return CustomResponse(produtoViewModel);
        }

        public async Task<FornecedorViewModel> ObterProdutosFornecedor(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
        }

        /// <summary>
        /// Método que faz upload de imagem.
        /// </summary>
        /// <param name="arquivo">imagem</param>
        /// <param name="imgNome">nome da imagem</param>
        /// <returns></returns>
        private bool UploadArquivo(string arquivo, string imgNome)
        {
            // Converte o string para base64.
            var imageDataByteArray = Convert.FromBase64String(arquivo);

            // Se o arquivo estiver null ou vazio.
            if(string.IsNullOrEmpty(arquivo))
            {
                // Adiciona o erro na modelState ou notifica o erro.
                //ModelState.AddModelError(key: string.Empty, errorMessage: "Forneça uma imagem para este produto!");

                //Pode notificar ou adicionar na modelstate, caso não tenha o método de notificação.
                NotificarErro(mensagem: "Forneça uma imagem para este produto!");
                return false;
            }

            // Pega o diretorio mais o nome da imagem que foi passada.
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            // Verifica se a imgaem já existe
            if (System.IO.File.Exists(filePath))
            {
                // Se existe, retorna um erro informando que ela já existe no sistema.
                //ModelState.AddModelError(key: string.Empty, errorMessage: "Já existe um arquivo com este nome!");

                // Tem a alternativa de notificar o erro usando o método NotificarErro()
                NotificarErro(mensagem:"Já existe um arquivo com este nome!");
                return false;
            }

            // Salva o arquivo na pasta
            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;

        }
    }
}
