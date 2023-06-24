using AutoMapper;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Api.Controllers;

namespace DevIO.Api.V1.Controllers
{

    [Authorize]    
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/produtos")]//[Route("api/produtos")]
    public class ProdutosController : MainController
    {        
        private readonly IMapper _mapper;
        private readonly IProdutoService _produtoService;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(
            IMapper mapper, IUser user,
            INotificador notificador,
            IProdutoService produtoServico,
            IProdutoRepository produtoRepository) : base(notificador, user)
        {
            _mapper = mapper;
            _produtoService = produtoServico;
            _produtoRepository = produtoRepository;
        }

        
        [HttpGet("ObterTodos")]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
        {
            var produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
            return Ok(produtos);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            // Converte o model produto para produtoViewModel.           
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound(); //404 não encontrado.

            return Ok(produtoViewModel);
        }


        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
        }


        [ClaimsAuthorize("Produto", "Adicionar")]
        [HttpPost("Adicionar")]
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

            produtoViewModel.Imagem = imagemNome;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }
                

        [ClaimsAuthorize("Produto", "Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse();
            }

            var produtoAtualizacao = await ObterProduto(id);

            if (string.IsNullOrEmpty(produtoViewModel.Imagem))
                produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
                {
                    return CustomResponse(ModelState);
                }

                produtoAtualizacao.Imagem = imagemNome;
            }

            produtoAtualizacao.FornecedorId = produtoViewModel.FornecedorId;
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(produtoViewModel);
        }


        [ClaimsAuthorize("Produto", "Excluir")]
        [HttpDelete("{id:guid}")] //converte para guid
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            //var produtoViewModel = await ObterProdutosFornecedor(id);

            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remover(id);

            // Return Ok(produtoViewModel);
            return CustomResponse(produtoViewModel);
        }
        
        
        private bool UploadArquivo(string arquivo, string imgNome)
        {          
            // Se o arquivo estiver null ou vazio.
            if(string.IsNullOrEmpty(arquivo))
            {
                //Pode notificar ou adicionar na modelstate, caso não tenha o método de notificação.
                // Adiciona o erro na modelState ou notifica o erro.
                //ModelState.AddModelError(key: string.Empty, errorMessage: "Forneça uma imagem para este produto!");

                NotificarErro(mensagem: "Forneça uma imagem para este produto!");
                return false;
            }

            // Converte o string para base64.
            var imageDataByteArray = Convert.FromBase64String(arquivo);

            // Pega o diretorio mais o nome da imagem que foi passada.
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imgNome);

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


        #region UploadAlternativo

        [ClaimsAuthorize("Produto", "Adicionar")]
        [HttpPost("AdicionarAlternativo")]
        public async Task<ActionResult<ProdutoViewModel>> AdicionarAlternativo(ProdutoImagemViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            // Salva o arquivo no diretorio
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return CustomResponse(ModelState);
            }

            // Salva o objeto que tem o nome do arquivo.
            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        //[DisableRequestSizeLimit]        
        //[DisableRequestSizeLimit]
        [RequestSizeLimit(40000000)]
        [HttpPost("imagem")]
        public async Task<ActionResult> AdicionarImagem(IFormFile file)
        {
            return Ok(file);
        }

        /// <summary> Cria imagens de arquivos pesados.</summary>        
        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
        {
            // Verifica se exite
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }            
            //false||false

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imgPrefixo + arquivo.FileName);

            // Verifica se já tem.
            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }
            // Copiar para a maquina(servidor), cria no path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

        #endregion
    }
}
