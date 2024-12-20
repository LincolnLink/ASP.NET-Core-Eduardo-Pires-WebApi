# ASP.NET-Core-Eduardo-Pires-WebApi

 - Projeto do Curso do AspNet Core com Eduardo Pires

# Comandos atualizado(15-05-2022) 

 - Comando para atualizar e gerar tabelas no banco de dados:

 - update-database -Context MeuDbContext -Verbose

 - update-database -Context ApplicationDbContext 

# REST

 ### O Protocolo HTTP

 - HTTP REQUEST, deve informar: 
    - o verbo, 
    - URI(endereço),
    - versão, 
    - request HEADER (informação do sistema operacional ou cookie),
    - message (dados de algum produto).

 - HTTP RESPONSE, ele contem:
    - resposta do servidor(200,403,500),
    - versao do http,
    - response HEADER(cookie,token,JWT),
    - response Message(dados, lista de produtos, pagina HTML)

 ### REST vs SOAP

 - REST: Representational State Transfer

    - Esta em formato de carta, é leve, são apenas texto. 

 - SOAP: Simple Object Access Protocol

    - Tem padrão de formatação, é baseado em XML, mais pesado.

    - WCF tb implementa REST.

 ### Arquitetura REST

 - A logica de negocios está distribuida em API's

 - MicroServicos: São pequenas APIs que faz apenas uma tarefa, o desafio é gerencia todas essas API's.

# Criando uma aplicação

 ### Novo projeto com linha de comando

 - Executa o comando para criar um projeto novo
 
 <blockquete>

    dotnet new webapi -n "nomeDaApi"

 </blockquete>

 ### Executando o projeto com Visual Studio Code

 - executa o comando para abrir o projeto no VS code

 <blockquete>

   code .

 </blockquete>

 - bin: aonde fica todas as bibliotecas.

 - Executando a API

 <blockquete>

   dotnet run

 </blockquete>

 - para executar dando um re-start
 
 <blockquete>

   dotnet watch run

 </blockquete>

 ### Novo projeto com Visual Studio 2019

 - Cria uma nova API

 ### Visão geral de uma aplicação WebAPI

 - as controller herda da classe "controllerBase"

 - decorada com [ApiController]

 - Os método segue uma convenção da webapi diferente do MVC, o nome dos metodos tem os nomes dos verbos.

 # Controller Diferenciada

 - Controllerbase e o decoraitor [controller] da suporte aos controllers;

# Rotas

 - A rota é um atributo, e passa um strime que é um template.

 - Podemos definir a propriedade que vai na rota, no memsmo local que definimos o verbo get por exemplo
 
 <blockquete>

   // GET api/values/5
   [HttpGet("{id}")]
   public ActionResult<string> Get(int id)
   {
      return "value";
   }

 </blockquete>

 - Por segunraça é sempre bom está tipando

 <blockquete>

   [ HttpGet("{id:int}") ]

 </blockquete>

# Action Results e Formatadores de Respostas

- ActionResults -> E o resultado de uma action(metodo da controller), ele diz que vai ter um resultado para aquela funcao.

<blockquete>

   // GET api/values
   [HttpGet]
   public ActionResult<IEnumerable<string>> Get()
   {
      return new string[] { "value1", "value2", "value3" };
   }

   // GET api/values/obter-valores
   [HttpGet("obter-valores")]
   public IEnumerable<string> ObterValores()
   {
      return new string[] { "value1", "value2", "value3", "value4" };
   }

</blockquete>

- Criando um outro get, com outro nome, e rota diferente, o tipo de resultado e diferente, sem o ActionResult.

- Porem em uma API deve se por o tipo ActionResult, porque o retorno pode ser "Ok()" ou "BadRequest()".

- Sempre que for retornar um ActionResult e bom definir o tipo dele, mas pode também não tipar e retornar mé todos que retorna um ActionResult tipo "ok()"

- O modificador dos metodos "[FromBody] " tem o objetivo de informar que o valor do "value" objeto, vem dentro do corpo(body/messagem) do request Http.


<blockquete>
 
   // POST api/values/
   [HttpPost]
   public void Post([FromBody] string value)
   {            
   }

</blockquete>

- O modificador dos metodos "[FromRoute] " indica que o id vem da rota, só que não é mais necessario por hoje em dia.

<blockquete>

   // PUT api/values/5
   [HttpPut("{id}")]
   public void Put([FromRoute] int id, [FromBody] string value)
   {
   }

</blockquete>

- O modificador dos metodos "[FromFrom] " informa que os dados vem de uma formData(formDeira).

- Pode está tipando o objeto que vem por parametro.

- A vantagem de tipar o parametro é que não tem a necessidade de por o "[FromBody]",
porq já tem o [ApiController] configurado na controller.

- O modificador dos metodos "[FromHeader] " informa que tem informações está na Header.

- O modificador dos metodos "[FromQuery] " informa que vem pela QueryString, mas ele não está definido na rota, porem é por query.

- O modificador dos metodos "[FromServices] " não é sobre receber dados, e sim injetar uma interface, classe, que ele faz uma resolução da classe via INjeção de dependencia.

### POST retornando um ActionResult

- Modifica o comportamento da Action, informando o tip ode dados que vai produzir.

- Status 201 Creat: é um OK de criação.

- Ajuda na documentação, troca o retorno de 200 paa 201!

<blockquete>

   // POST api/values/
   [ HttpPost]
   [ ProducesResponseType(typeof(product), StatusCodes.Status201Created)]
   [ ProducesResponseType(StatusCodes.Status400BadRequest)]
   public ActionResult Post2(product product)
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

</blockquete>


# Formatador de Response Personalizado

- Criando um ActionResult personalizado.

 <blockquete>

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

 </blockquete>

- Para cria um Result customizado, deve criar um classe herdando a classe "ObjectResult".


# Analisadores e Convenções

- Ajuda a documentar, Instala o pacote.

<blockquete>

   Microsoft.AspNetCore.Mvc.Api.Analyzers

</blockquete>

- Apenas lembra de tipar acima do método, oque foi implementado.

### Convenção de API

 - Dessa forma fica mais simplificado

 - Ele gera os dois tipos de produce.

 <blockquete>

   // POST api/values/
   [HttpPost]
   [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
   public ActionResult Post3(Product product)
   {
      if (product.Id == 0)
      {
            return BadRequest();
      }

      // add no banco

      // retorna um ok, mas seria um 200
      //return Ok(product);

      // retorna um 201
      return CreatedAtAction(actionName: "Post2", product);

      // retorna um 201
      // return CreatedAtAction(actionName: nameof(Post2), product);
   }

 </blockquete>

# Processo rápido - CRUD

- Instala os pacotes

 <blockquete>

   Microsoft.EntityFrameworkCore Microsoft.EntityFrameworkCore.SqlServer

 </blockquete>

 <blockquete>

   Microsoft.EntityFrameworkCore.SqlServer.Design

 </blockquete>

 <blockquete>

   Microsoft.EntityFrameworkCore.Tools

 </blockquete>

- Cria o Model

<blockquete>

  public class FornecedorViewModel
  {
      [Key]
      public Guid Id { get; set; }


      [Required(ErrorMessage = "O campo {0} é obrigatório")]
      [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
      public string Nome { get; set; }

      [Required(ErrorMessage = "O campo {0} é obrigatório")]
      [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
      public string Documento { get; set; }

      public int TipoFornecedor { get; set; }

      public bool Ativo { get; set; }
  }

</blockquete>

- Cria uma classe chamada  "ApiDbContext" e herda o DbContext

 <blockquete>

  public class ApiDbContext : DbContext
  {
      public ApiDbContext(DbContextOptions options) : base(options)
      {}
  }

   public DbSet<Fornecedor> Fornecedores { get; set; }

 </blockquete>

- Na startUp configura o entityframework

<blockquete>

  services.AddDbContext<ApiDbContext>(optionsAction:options =>
   options.UseSqlServer(Configuration.GetConnectionString(name: "DefaultConnection")));

</blockquete>

- Executa o comando da migration
 
 <blockquete>

   dotnet ef migrations add "nomeDpMigration"

 </blockquete>
 
- Depois o comando que atualiza.

 <blockquete>

   dotnet ef database update

 </blockquete>
 
- Cria a controller usando entityframework

- Escolha a modelo(fornecedor), e o contexto existente.

- dotnet por padrão retorna tudo async

# Setup - API Completa

- Cria o projeto de Web API, e copia e cola os projetos de Dados, e Business quefoi feita no curso de MVC.

# Visão do fluxo da arquitetura

 - Explicando o projeto.

# Implementando DTOs (ViewModels)

 - Cria uma pasta chamada ViewModel ou DTO.

 - Essas model já tem ps meta dados e validações.

 - As viewModel são: FornecedorViewModel, ProdutoImagemViewModel, EnderecoViewModel.

 - É importante criar um viewModel porq nela não tem as propriedades de relacionamentos entre as entidades, isso causa bug no Jsom.

 - ScaffoldColumn: não é ultilizado como dado de entrada, serve mais para o Razon.
  
 <blockquete>

      public class FornecedorViewModel
      {
         [Key]
         public Guid Id { get; set; }

         [Required(ErrorMessage = "O campo {0} é obrigatório")]
         [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
         public string Nome { get; set; }

         [Required(ErrorMessage = "O campo {0} é obrigatório")]
         [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
         public string Documento { get; set; }

         public int TipoFornecedor { get; set; }

         public EnderecoViewModel Endereco { get; set; }

         public bool Ativo { get; set; }

         public IEnumerable<ProdutoViewModel> Produtos { get; set; }
      }

 </blockquete>
 
- ViewModel de Produto.

 <blockquete>

      public class ProdutoViewModel
      {
         [Key]
         public Guid Id { get; set; }

         [Required(ErrorMessage = "O campo {0} é obrigatório")]

         public Guid FornecedorId { get; set; }

         [Required(ErrorMessage = "O campo {0} é obrigatório")]
         [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
         public string Nome { get; set; }

         [Required(ErrorMessage = "O campo {0} é obrigatório")]
         [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
         public string Descricao { get; set; }

         public string ImagemUpload { get; set; }

         public string Imagem { get; set; }

         [Required(ErrorMessage = "O campo {0} é obrigatório")]
         public decimal Valor { get; set; }

         [ScaffoldColumn(false)]
         public DateTime DataCadastro { get; set; }

         public bool Ativo { get; set; }

         [ScaffoldColumn(false)]
         public string NomeFornecedor { get; set; }
      }
 
 </blockquete>

# Setup - Controllers e Startup

 - Renomeia a controler que ja vem para "mainController"

 - Criando uma Classe Base das controllers, Class abstrata so pode ser herdada.

 - Validação de notificação de erro, ModelState e Operações de Negocios.

 - Cria o "FornecedoresController", o primeiro método vai ser o getAll

 - Injeta no construtor o repositorio de fornecedores.

 - Não esquecendo de por o await, para esperar o resultado do EF.

### AutoMapper

 - Usa o automaper para poder converter a entidade para viewModel.
   
<blockquete>

        Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection

</blockquete>

 - Configura na startUp

<blockquete>

    services.AddAutoMapper(typeof(Startup));

</blockquete>

 - Cria uma pasta chamado "Configuration" e um arquivo chamado "AutomapperConfig".

 - Nesse arquivo é configurado o vinculo das viewModel com as Models.
  
<blockquete>

        public class AutomapperConfig : Profile
        {
            public AutomapperConfig()
            {
                CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
                CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
                CreateMap<ProdutoViewModel, Produto>();

                CreateMap<ProdutoImagemViewModel, Produto>().ReverseMap();

                CreateMap<Produto, ProdutoViewModel>()
                    .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
            }
        }

</blockquete>

 - Injeta o autoMapper no metodo construtor do colerller de fornecedor.

 - Converte a model para viewModel

 - Na pasta Configuration, cria o arquivo "DependencyInjectionConfig".
 
 - Nesse arquivo fica configurado as Injeções de Dependencia.

<blockquete>

            public static IServiceCollection ResolveDependencies(this IServiceCollection services)
            {
                services.AddScoped<MeuDbContext>();
                services.AddScoped<IProdutoRepository, ProdutoRepository>();
            }

</blockquete>

 - Chama a configuração na StratUp.
 
<blockquete>

            services.ResolveDependencies();

</blockquete>

 - Configura o banco na startUp.
 
<blockquete>

            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

</blockquete>

 - Configuração do arquivo appSettings.json
 
<blockquete>

          "ConnectionStrings": {
            "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MinhaAPICoreCompleta;Trusted_Connection=True;MultipleActiveResultSets=true"
          }

</blockquete>

 - executa o comando
  
<blockquete>

            Add-Migration Initial -Verbose -Context MeuDbContext

            update-database -verbose
 
</blockquete>

# Modelando a controller de Fornecedores

### cria as outras actions, por exemplo o método obterPorId().
- Cria um método isolado que busca por id, para que seja reaproveitado.

 <blockquete>

            [HttpGet("{id:guid}")]
            public async Task < ActionResult< FornecedorViewModel>> ObterPorId(Guid id)
            {
                // Converte o o model fornecedor para fornecedorViewModel.
                var fornecedor = await ObterFornecedorProdutosEndereco(id);

                if (fornecedor == null) return NotFound(); //404 não encontrado.

                return Ok(fornecedor); //200 Ok
            }

            public async Task< FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
            {
                return _ mapper.Map< FornecedorViewModel>(await _ fornecedorRepository.ObterFornecedorProdutosEndereco(id));
            }

 </blockquete>

### verbo post

- No "FornecedorService" bota a Task como Task<bool>, para retornar algo e não void, 
com isso fica melhor te tratar o retorno, deve por da implementação da interface, retornando algum valor bool,
em todas as saidas.

- Bota o verbo "HttpPost" 

<blockquete>

            [HttpPost]
            public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
            {
                if (ModelState.IsValid) return BadRequest();

                var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
                var result = await _fornecedorService.Adicionar(fornecedor);

                if (!result) return BadRequest();

                return Ok();
            }

</blockquete>

### verbo put

- Bota o nome do metodo para Atualizar e bota o verbo put, recebendo um guid.
- O método recebe um guid e um fornecedorViewModel, com isso você compara se o guid é igual ao id do objeto,
faz isso antes mesmo da validar a modelstate.
- O task retorna bool, todas as saidas deve retornar um resultado bool.

<blockquete>

            [HttpPut("{id:guid}")]
            public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
            {
                if (id != fornecedorViewModel.Id) return BadRequest();

                if (ModelState.IsValid) return BadRequest();

                var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
                var result = await _fornecedorService.Atualizar(fornecedor);

                if (!result) return BadRequest();

                return Ok(fornecedor);
            }

</blockquete>

- remover

<blockquete>

            [HttpDelete("{id:guid}")]
            public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
            {
                var fornecedor = await ObterFornecedorEndereco(id);

                if (fornecedor == null) return NotFound();

                await _fornecedorService.Remover(id);

            }

</blockquete>

# Testando o resultado com Postman
 - Foi testado e está funcionando, só precisa tratar as notificações.

# Padronizando erros de validação e de negócios (notificações).
 - Desativando a validação do modelState, para poder usar uma validação personalizada.
  
<blockquete>

             services.Configure<ApiBehaviorOptions>(options =>
             {
                options.SuppressModelStateInvalidFilter = true;
             });
             
</blockquete>
 
 - Cria uma sequencia de métodos da classe "MainController", esses métodos vai tratar a "modelState".
  
 - Método OperacaoValida    
    - Retorna se tem notificação de erro, ou não.

 - Método NotificarErroModelInvalida: 
    - Ele notifica o erro da modelState invalida.
    - Pega apenas os erros usando o linQ com o método "SelectMany".
    - Dentro de um for, garante que foi pego um método do "Exception" também.
    - chama o método "NotificarErro()" passando as mensagens como parametro.
   
 - Método NotificarErro:
    - Lança o objeto notificação para uma fila de erros.
    - Esse método recebe uma instancia de "INotificador", que foi injetada no construtor.
    - a interface INotificador já foi implementada em outro arquivo.
   
 - Método CustomResponse
    - Trata o erro da ModelState, erros antes da camada de negocios.
    - Esse método retorna um "ActionResult", e recebe um "ModelStateDictionary".
    - Ele verifica se a modelState está valida.
    - Se não for valida, então chama o método "NotificarErroModelInvalida".
    - Caso seja valida chama a sobreCarga do método "CustomResponse".

 - SobreCarga do método CustomResponse
    - Tratamento de erros da camada de negocios.
    - A diferença principal é que ele recebe um object com valor de null como parametro.
    - Se não tiver notificação retorna 200, se não retorna um bad Request.

### Aplicando os métodos de resposta customisada na controller.

### Adicionar
 - É executado o "CustomResponse" quando inicia a requisição para valiar a ModelState.
 - E é executada o "CustomResponse" no final da requisição.
 - "CustomResponse" já trata os erros e acertos.

<blockquete>

            [HttpPost]
            public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);
                       
                await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

                return CustomResponse(fornecedorViewModel);
            }

</blockquete>

### Atualizar
 - O tratamento que verifica se os id são iguais, não precisa ser trocado por "CustomResponse", 
 a não ser que queira passar uma mensagem.
  
<blockquete>

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

</blockquete>


### Excluir
 - Em alguns casos não precisa por o "CustomResponse";
  
<blockquete>

            [HttpDelete("{id:guid}")]
            public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
            {
                var fornecedorViewModel = await ObterFornecedorEndereco(id);

                if (fornecedorViewModel == null) return NotFound();

                await _fornecedorService.Remover(id);

                //return Ok(fornecedorViewModel);   
                return CustomResponse(fornecedorViewModel);
            }

</blockquete>

### Inportante: herdando classe com Injeção de Dependencia!
 - A interface "INotificador" deve ser injetada também na classe "FornecedoresController", porque a classe
 "MainController" que é herdada tem ela injetada também, e depois passar para classe base.

### finalizando a FornecedoresController

- Cria uma action chamada "ObterEnderecoPorId".
 
<blockquete>

            [HttpGet("endereco/{id:guid}")]
            public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id)
            {
                return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
            }

</blockquete>

- Cria uma action chamada "AtualizarEndereco".
 
<blockquete>

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

</blockquete>

# Cadastro de Produtos e upload de imagem

 - O CRUD do controller é o mesmo do fornecedor, só muda o tipo de objeto e interface de repositorio e serviço.
 - O desafio fica na parte de fazer o UPLOAD da imagem.

### UPLOAD da imagem

 - Cria um método com um algoritimo que é uma receita de bolo na internet.

 - Converte o string para base64, usando o método "Convert.FromBase64String()"

 - Verifica com o método "IsNullOrEmpty()", se o arquivo está null ou vazio.
 - Se tiver null ou vazio pode notificar usando o método "NotificarErro" ou passando erro na modelState.

 - Pega o diretorio mais o nome da imagem que foi passada, usando o método "Path.Combine()", e
 bota na variavel "filePath".

 - Verifica se a imgaem já existe, usando o método "System.IO.File.Exists(filePath)".
 - Caso já exista a imagem, use o método "NotificarErro()" ou adiciona na modelState 
 uando o "ModelState.AddModelError()".

 - Se não salva a imagem "System.IO.File.WriteAllBytes(filePath, imageDataByteArray);", retorne verdadeiro.
  
<blockquete>

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

</blockquete>

### Aplicando o método na action que adiciona.

 - Cria um nome personalizado para a imagem.
  
<blockquete>

            // Criando nome personalizado, e chamando o método que faz upload de imagem.
            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
            if(!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                // Caso de erro notifica o erro para o client.
                return CustomResponse(produtoViewModel);
            }

</blockquete>

# Consumindo a API via Angular 7

 - Bota o projeto em Angular para rodar, não esqueça de desligar o IIS e reiniciar o pc,
 também deve limpar o cache, cookies, etc, fazer isso tudo para não dar erro.

 - instala o nodeModule "npm i".

 - Troca a URL no serviço para fazer as requisições corretamente.

 - Sean ajudou a configurar o cors.
 https://stackoverflow.com/questions/40043097/cors-in-net-core

 - Cria uma configuração na startUp, para não receber erro de cors, o 
 primiero é dentro do método "ConfigureServices" e o segundo é dentro do "Configure"


<blockquete>

            services.AddCors(options => options.AddPolicy("AllowAll", p => 
                p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

</blockquete>

<blockquete>

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                   configurePolicy:builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            
</blockquete>

<blockquete>

            app.UseCors("AllowAll");

</blockquete>

 - Na startUp adiciona o "AddNewtonsoftJson()", para evitar erros.

 
<blockquete>

            "services.AddControllers().AddNewtonsoftJson();".

</blockquete>


 - Cria uma configuração melhorada no arquivo que tem o "AutomapperConfig()", 
 para que todas as propriedades da viewModel seja alimentadas.

 - Com isso pega o nome do fornecedor.

 
<blockquete>

            CreateMap<ProdutoViewModel, Produto>();
 
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));

</blockquete>

 - Botão de excluir, vizualizar deve ser inplementado depois.

# Upload de arquivos grandes com IFormFile.

 - Cria um outra viewModel chamada "ProdutoImagemViewModel".
 - Nela vc troca o tipo do "ImagemUpload" para "IFormFile".
 - Esse tipo carrega a imagem em fatias.
 - Duplica o método adicionar, e renomeia para adicionarAlternativo.
 - Bota o nome da rota para "[HttpPost("Adicionar")]", recebendo um "ProdutoImagemViewModel".
 - Não precisa concatenar com guid, apenas cria um guid, para usar como nome.
 - Chama o método "UploadArquivoAlternativo", faz o tratamento para ver se existe ou se tem um tamanho.
 - salva o nome na propriedade "Imagem".
 
 <blockquete>

            //[ClaimsAuthorize("Produto", "Adicionar")]
            [HttpPost("Adicionar")]
            public async Task<ActionResult<ProdutoViewModel>> AdicionarAlternativo(ProdutoImagemViewModel produtoViewModel)
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return CustomResponse(ModelState);
                }

                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
                await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

                return CustomResponse(produtoViewModel);
            }

 </blockquete>

 - Método que sobe imagem Alternativo.

 <blockquete>

            //[DisableRequestSizeLimit]
            [RequestSizeLimit(40000000)]
            [HttpPost("imagem")]
            public ActionResult AdicionarImagem(IFormFile file)
            {
                return Ok(file);
            }
       
            private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
            {
                if (arquivo == null || arquivo.Length == 0)
                {
                    NotificarErro("Forneça uma imagem para este produto!");
                    return false;
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Angular/src/assets", imgPrefixo + arquivo.FileName);

                if (System.IO.File.Exists(path))
                {
                    NotificarErro("Já existe um arquivo com este nome!");
                    return false;
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }
                return true;
            }


 </blockquete>

 - Pesquisar uma forma do Angular aceitar o tipo "IFormFile".

 - Usar o dataanotation no endpoint, "[RequestSizeLimit(40000000)]", para limitar o tamanho da imagem/arquivos enviada.

 - Se usa o "[DisableRequestSizeLimit]" para desativar o limite.

 - solução alternativa que aceita JSON e IFormFile(não é da microsoft):

 - TODO: se aprofundar mais depois.(minuto 22 do video)

 - Cria um arquivo na pasta de extenção, com a configuração.

 <blockquete>     

            namespace DevIO.Api.Extensions
            {
                // Binder personalizado para envio de IFormFile e ViewModel dentro de um FormData compatível com .NET Core 3.1 ou superior (system.text.json)
                public class ProdutoModelBinder : IModelBinder
                {
                    public Task BindModelAsync(ModelBindingContext bindingContext)
                    {
                        if (bindingContext == null)
                        {
                            throw new ArgumentNullException(nameof(bindingContext));
                        }

                        var serializeOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                            PropertyNameCaseInsensitive = true
                        };

                        var produtoImagemViewModel = JsonSerializer.Deserialize<ProdutoImagemViewModel>(bindingContext.ValueProvider.GetValue("produto").FirstOrDefault(), serializeOptions);
                        produtoImagemViewModel.ImagemUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

                        bindingContext.Result = ModelBindingResult.Success(produtoImagemViewModel);
                        return Task.CompletedTask;
                    }
                }
            }

 </blockquete>

 - Depois disso tipa a model com o novo tipo, que da suporte ao JSON com IFormFile!

 <blockquete>
     
                    namespace DevIO.Api.ViewModels
                    {
                    // Binder personalizado para envio de IFormFile e ViewModel dentro de um FormData compatível com .NET Core 3.1 ou superior (system.text.json)
                    [ModelBinder(BinderType = typeof(ProdutoModelBinder))]
                    public class ProdutoImagemViewModel
                    {
                        [Key]
                        public Guid Id { get; set; }

                        [Required(ErrorMessage = "O campo {0} é obrigatório")]

                        public Guid FornecedorId { get; set; }

                        [Required(ErrorMessage = "O campo {0} é obrigatório")]
                        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
                        public string Nome { get; set; }

                        [Required(ErrorMessage = "O campo {0} é obrigatório")]
                        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
                        public string Descricao { get; set; }

                        // Evita o erro de conversão de string vazia para IFormFile
                        [JsonIgnore]        
                        public IFormFile ImagemUpload { get; set; }

                        public string Imagem { get; set; }

                        [Required(ErrorMessage = "O campo {0} é obrigatório")]
                        public decimal Valor { get; set; }

                        [ScaffoldColumn(false)]
                        public DateTime DataCadastro { get; set; }

                        public bool Ativo { get; set; }

                        [ScaffoldColumn(false)]
                        public string NomeFornecedor { get; set; }
                    }
                }

 </blockquete>

 - na configuração do "AutomapperConfig" adicionaessa linha:

 <blockquete> 
            CreateMap< ProdutoImagemViewModel, Produto>().ReverseMap(); 
 </blockquete>

 - Testar depois no Angular.



# Concluindo a modelagem da API

 - Cria o método atualizar. 

 - Cria um classe chamada "ApiConfig" nela isola algumas configrações da classe StratUp.


# Autenticação

 - Tratando da segurança da API.
 - Bota o atributo "Authorize" nas controllers, informa o uso do filtro, obriga a apresentar um login.
 - Bota o atributo "[AllowAnonymous]" nas actions que vc esta liberando para todas as pessoas.

# Implementando o ASP.NET Identity

 - Precisa da implementação do Identity, para o atributo Authorize funcionar.

 - Precisa de toso os pacotes instalados!

<blockquete>

    Install-Package Microsoft.EntityFrameworkCore
    Install-Package Microsoft.EntityFrameworkCore.SqlServer
    Install-Package Microsoft.EntityFrameworkCore.Tools
    Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    Install-Package Microsoft.AspNetCore.Identity.UI
    
</blockquete>

### 1° passo 

 - Cria um classe "static" chamada "IdentityConfig.cs". para isolar a confgiração do Identity
 - implementa um método de extensão, que implementa o IServiceCollection.
 - O parametro 'configuration' é do microsft extention e não do automaper.

 - No arquivo "Startup", chama a classe isolada que nos criamos "IdentityConfig".
 - Paasa a "configuration" por parametro, que é a configuração da conectionString.

 <blockquete>
            services.AddIdentityConfig(Configuration);
 </blockquete>

 - Cria um outro dbcontext chamado "ApplicationDbContext", dentro de uma pasta chamada 'data' que deve ser criada.

 - Configurao o ApplicationDbContext, herdando a classe 'IdentityDbContext' e no construtor passa os paremetros.

<blockquete>

        public class ApplicationDbContext : IdentityDbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        }

</blockquete>

 - cria uma classe de fabrica dbcontext para evitar erros.

<blockquete>

    using Microsoft.EntityFrameworkCore;
        using Microsoft.EntityFrameworkCore.Design;
        using Microsoft.Extensions.Configuration;
        using System.IO;

        namespace Pet.Api.Data
        {
            public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
            {
                public ApplicationDbContext CreateDbContext(string[] args)
                {
                    // Build configuration
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                    // Create DbContextOptionsBuilder
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    var connectionString = configuration.GetConnectionString("DefaultConnection");

                    optionsBuilder.UseSqlServer(connectionString);

                    return new ApplicationDbContext(optionsBuilder.Options);
                }
            }
        }

</blockquete>


 - Copia e cola a configuração do "AddDbContext" que esta na startUp para a o método "AddIdentityConfig",
 que esta na classe "IdentityConfig", usando o contexto "ApplicationDbContext".

 - executa o comando para gerar a migration.
 
 <blockquete>

            add-migration Identity -Context ApplicationDbContext 

 </blockquete>

 - Executa o comando que gera as tabelas.

 <blockquete>

            update-database -Context ApplicationDbContext 

 </blockquete>

### 2° passo 

 - Na classe 'IdentityConfig' configura o idadentity, inciando com o '.AddDefaultIdentity<IdentityUser>()'.

 - Configurando  a model do Identity.
 - "AddRoles<IdentityRole>()" determina uma classe com regras, foi passada a padrão.
 - "AddEntityFrameworkStores<ApplicationDbContext>()" informa que esta trabalhando com entityframework.
 - "AddDefaultTokenProviders" gerador de token para resetar senha e confirmar email.
 - 
<blockquete>

    services.AddDefaultIdentity< IdentityUser>()
    .AddRoles< IdentityRole>()
    .AddEntityFrameworkStores< ApplicationDbContext>()
    .AddDefaultTokenProviders();
    //.AddErrorDescriber< IdentityMensagensPortugues>()

</blockquete>
 
 - No método "UseApiConfig" que fica na classe "ApiConfig" bota a coonfiguração "app.UseAuthentication();".

 - Caso a classe não foi isolada, vai no metodo 'Configure' da classe 'startUp' e coleque o 'app.UseAuthentication();'.

 - Para a autenticação funcionar, a chamada do 'app.UseAuthentication();' sempre vai vim antes da chamada do 'useMvc...'

# Controller de Autenticação
 
 - Cria um viewModel chamada "UserViewModel", nesse arquivo se cria varias viewModel.
    - RegisterUserViewModel.
    - LoginUserViewModel.
    - UserTokenViewModel.
    - LoginResponseViewModel.
    - ClaimViewModel.

 - Cria o controller chamado "AuthController", herda a classe "MainController", ele é responsavel por
 fazer login e resgistro de usuarios.

### Registrar

 - Cria o método "Registrar", que recebe um "RegisterUserViewModel" como parametro.

 - Injeta as dependencias "UserManager", responsavel por cria usuario e fazer outras manipulações.
 - Injeta as dependencias "SignInManager", responsavel por autenticar o usuario.
 - Injeta a dependencia de "INotificador" que ja foi usada para notificar erros.

 - Verifica se a ModelState é valida.
 - Cria um objto IdentityUser.

 - Usando o _userManager, chama o método ".CreateAsync(user, registerUser.Password);",
 passando o usuario e senha, passando o resultado para uma variavel.
 - Passa o 'Password' como segundo parametro porq ele salva o rash, e não salva a senha.
 - Retorna um IdentityResult.

 - Usando a propriedade "Succeeded", verifica se foi criado corretamente.
 - Se teve sucesso, então usa o "signInManager" para chamar o método '.SignInAsync(user, false)'.

 - o false é para não memorizar o login.

 - retorna erro caso tenha.
 - retorna http 200 caso de tudo certo.

 

<blockquete>

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GerarJwt(user.Email));
            }
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }
            return CustomResponse(registerUser);
        }

</blockquete>

### Login

 - Criando a Task< ActionResult> de logar, recebe como parametro um objeto "LoginUserViewModel".

 - Verifica se a modelState é valida.
 - Usa o "_signInManager" para chamada o método "PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);".
 - Ele recebe o usuario, senha e dois bool, o primeiro é para informar se é persistence, e o segundo vai travar o usuario para tentativas invalidas depois de x minutos.

 - Caso consiga logar com sucesso, retorna o 'CustomResponse()',  cria documenta no logger. 
 - Se não conseguiu logar trata o erro com "CustomResponse".
  
<blockquete>

        [HttpPost("entrar")]
        public async Task < ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _ signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(loginUser);
                //_ logger.LogInformation("Usuario " + loginUser.Email + " logado com sucesso");
                //return CustomResponse(await GerarJwt(loginUser.Email));
            }
            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }
            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

 </blockquete>
 
# Customizando os erros do Identity

 - Botando as mensagem do identity em português.

 - Na pasta "Extensions" cria uma classe chamado "IdentityMensagensPortugues", 
 herdando "IdentityErrorDescriber".

 - Nessa classe cria sobreescrita de métodos que retorna as mensagens de erro.
 - Para funcionar deve configurar essa nova classe, na classe de configuração do identyti a "IdentityConfig".

<blockquete>

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<IdentityMensagensPortugues>();

</blockquete>

# JWT - JSON Web Token

 - O usuario faz a autenticação, pode ser de qualquer lugar, angular, google, mobile, etc
 - vai ser validado e devolve um token, pode guardar no borwser ou cache.
 - Toda vez que fazer algo , manda o token, e verifica as regras.
 - A api responde com o que foi solicitado.

 - Site que explica o que é um JWT: https://jwt.io/

 - Token
 - A parte vermelha é o header do token, informa incriptação e o tipo.
 - A parte roxa é o dado do token.
 - A parte azul é a assinatura do token.
  
 - O lado do client sempre guarda o Token.

 ### cores que representa um JWT

 - Vermelho: forma de criptação, e a forma que ele é,
     diz o tipo de criptografia.

 - Roxa: é o dado, o Json.

 - Azul: É a assinatura, informa a chave de criptografia, a chave fica na aplicação,
  para descriptografar e criptografar.

# Implementando o JWT
  
### Arquivo AppSettings.cs

- Na pasta de extenção cria uma classe chamada "AppSettings.cs", É uma classe para gerenciar propriedades do token 
    - Secret: Chave de criptografia do token.
    - ExpiracaoHoras: expirações em horas que o token vai perder a validade.
    - Emissor: Quem emite (a aplicação).
    - ValidoEm: Em quais URL esse token é valido.

### Arquivo appsettings.json

- Deve ser implementado essa classe no "appsettings.json" mesmo arquivo que fica a connectionString.

    <blockquete>

      "AppSettings": {
        "Secret": "MEUSEGREDOSUPERSECRETO",
        "ExpiracaoHoras": 2,
        "Emissor": "MeuSistema",
        "ValidoEm": "https://localhost"
      }

    </blockquete>

    - secret e emissor foi criado manualmente.
    - o 2 em "ExpiracaoHoras" representa 2h de duração
    - Emissor e ValidoEm, são as propriedades que vai ser usada para validar o token.
    - Deve informar a url que você vai usar, para a propriedade ValidoEm.

### Arquivo IdentityConfig.cs

- Na configuração "IdentityConfig", deve configurar o JWT, apartir daqui toda configuração fica nesse classe.

    - Configuração:

    1° A variavel "appSettingsSection", recebe o valor, que vem do parametro, "configuration" que chama o método "GetSection()" é passado o valor de "AppSettings" para ele entender de qual sessão deve buscar o valor, no json.

    2° Para os valores ja vim populados, deve configurar no aspNet Core, usando o parametro "services", chama o "configure", define o tipo dele
    como "AppSettings" e passa como parametro a variavel "appSettingsSection".

    <blockquete>

        var appSettingsSection = configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

    </blockquete>

- Pegando valores configurado e criando uma chave.

    1° Cria um variavel chamada "appSettings" que recebe o valor de "appSettingsSection.Get<AppSettings>()", variavel que acabou de configurar.
     
    2° Cria uma variavel que recebe, valor do "encoding", com base no "segredo".

    <blockquete>

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

    </blockquete>

- Configurando o token do JWT (criando e configurando uma authenticação)

    1° "services.AddAuthentication()": Adiciona a autenticação e configura.

    2° DefaultAuthenticateScheme: define um padrão de validação, 
     que é gerar um token.

    3° DefaultChallengeScheme: Toda vez que for validar, verifica o token.

    <blockquete>

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

    </blockquete>

    OBS: Para não da erro deve instalar a versão 3.0.0 do pacote.

    solução: https://qastack.com.br/programming/58593240/how-to-replace-addjwtbearer-extension-in-net-core-3-0
  
    <blockquete>

        Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 3.0.0

    </blockquete>

- Adicionando configurações a mais com o método "AddJwtBearer()".

    1° RequireHttpsMetadata: se for trabalhar apenas com Https, pode deixar true, define que só vai trabalhar com https, para evitar ataques.

    2° SaveToken: Pergunta se o token deve ser guardado no "AuthenticationProperties", depois de uma autenticação de sucesso, É bom que quarde porq fica mais facil da aplicação validar, app apresentação do token.

    3° Outras configurações (criando o token), "TokenValidationParameters" cria uma serie de outros parametros!

        - ValidateIssuerSigningKey: Valida se quem esta emitindo é o mesmo que está no token.(baseada nome do Issuer e na chave).
        - IssuerSigningKey: configura a chave, transforma de asp2 para uma chave criptografada.
        - ValidateIssuer: valida apenas o Ussuer conforme o nome.
        - ValidateAudience: aonde o token é valido em qual Audience.
        - ValidAudience: Informa qual é o "Audience".
        - ValidIssuer: Informa qual é o "Essuer".

    Configuração completa:

    <blockquete>

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings.ValidoEm,
                        ValidIssuer = appSettings.Emissor
                    };
                });

    </blockquete>

### Arquivo AuthController

- Chamando o GerarJwt(), método privado

    - O cadastro tendo sucesso, deve gerar o token e devolver para o client, isso na action de registro.
    - É chamado o métod tanto para o "Criar" como para "Registrar".
    - A chamada do "GerarJwt": 

<blockquete>

        if (result.Succeeded)
        {      
            await _ signInManager.SignInAsync(user, false);
            // Gerando o token e devolve ele !
            return CustomResponse(await GerarJwt(user.Email)); 
        }

</blockquete>

- Criando o método GerarJwt(), ele devolve o token que foi gerado!

- 1° Caracteristicas do Método GerarJwt().

    - Ele é privado e async, devolve um Task<string>, recebe um email do usuario.
    - Gera um token para aquele email.

    2° Cria uma instancia do método "JwtSecurityTokenHandler".

    - instancia a classe "JwtSecurityTokenHandler" e guarda na variavel "TokenHandler".

<blockquete>

    var tokenHandler = new JwtSecurityTokenHandler();

</blockquete>

- 3° Injetando o "_ appSettings" no construtor.
    (tem um espaço entre o " _ " para não da erro na documentação)

    - Essa injeção tem uma forma diferente, ela usa a interface "IOptions"

    - Cria a propriedade "private readonly AppSettings _ appSettings;"
    - Cria um novo parametro para o construtor chamado: "IOptions<AppSettings> appSettings,"
    - Dentro do construtor alimenta a propriedade "_ appSettings = appSettings.Value;"

- 4° Cria a chave de criptografia.

    - Cria uma variavel chamada "Key", para ela receber a chave de criptografia.
    - o metodo "Encoding.ASCII.GetBytes", recebe uma string.

<blockquete>

        var key = Encoding.ASCII.GetBytes( _ appSettings.Secret);

</blockquete>

- 5° Gerando o Token.

    - A: configuração: chama o método "tokenHandler.CreateToken" 
    e instancia a classe "new SecurityTokenDescriptor"

    - B: configuração: dentro dessa instancia passa toda as coordenadas dentro do token. 

        - Issuer: indica o Emissor que está no "_ appSettings.Emissor";

        - Audience: informa o ValidoEm, que está no "_ appSettings.ValidoEm";

        - Expires: Converta o "_ appSettings.ExpiracaoHoras"  usando o
         "DateTime.UtcNow.AddHours" para usar a hora da local.

        - SigningCredentials: cria uma instancia de "new SigningCredentials()" e ,
        Passando como parametro uma instancia de "new SymmetricSecurityKey" nessa instancia é 
        passado o "key" como parametro, o segundo parameto é "SecurityAlgorithms.HmacSha256Signature".

        - SecurityAlgorithms.HmacSha256Signature: é um algoritimo de criptografia que vai ser usado.

<blockquete>

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _ appSettings.Emissor,
            Audience = _ appSettings.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_ appSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

</blockquete>

- 6° Escrever o token.

 - Cria uma variavel chamada "encodedToken", que recebe o método "tokenHandler.WriteToken()",
     que recebe a variavel "token" que foi configurado como parametro.      

 - Esse método deixando o token com padrão da web.

 - Retorna a variavel "encodedToken"!

<blockquete>  

        var encodedToken = tokenHandler.WriteToken(token);

</blockquete>

# Autorização baseada em Claims via JWT (botando as claims no token[JWT])

 - Primeiro cria uma Claim do tipo "Fornecedor" e valor "Atualizar,Remover", e o id do usuario.

    - Diretamente na tabela "dbo.AspNetUserClaims", apenas para testar.

 ### Criando um atributo de extenção do Identity, para validar Claims!

 - Cria uma classe/ arquivo de extenção chamada "CustomAuthorization".

    - Nessa classe tem um método chamado "ValidarClaimsUsuario", que recebe como parametro
 o contexto, o nome da claim e o valor da claim, classe com o método que valida os valores.

    - Usando o (HttpContext)contexto é possivel verificar se o usuario está autenticado, usando a
 propriedade "IsAuthenticated".

    - Podemos também verificar se ele tem o tipo/nome da claim e o valor da claim que foi informado,
 usando o ".Claims.Any(...)" do linQ, o (HttpContext)contexto seria informações vinda da base de dados.

    <blockquete>

            public class CustomAuthorization
            {
                public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
                {
                    return context.User.Identity.IsAuthenticated &&
                           context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
                }
            }

    </blockquete>

 - Dentro do mesmo arquivo é criada uma 2° classe chamada "ClaimsAuthorizeAttribute" que recebe como herança a
 classe "TypeFilterAttribute", dessa forma define essa 2° classe como um atributo, para ser usado nos controller.

    - O método construtor dessa classe recebe o nome e valor da claim como parametro.

    - Dentro do construtor é chamado uma propriedade da classe "TypeFilterAttribute" chamado "Arguments", nele é
 atribuido uma instancia de array de objetos, aonde tem uma instancia de "Claim" que recebe o nome e o valor da claim
 como parametro.

    - Por ultimo deve passar uma 3° classe na base do construtor da 2° classe.

    <blockquete>

            public class ClaimsAuthorizeAttribute : TypeFilterAttribute
            {
                public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
                {
                    Arguments = new object[] { new Claim(claimName, claimValue) };
                }
            }

    </blockquete>

 - Para finalizar a configuração personalizada de tratamento das claim,
 deve se criar uma 3° classe chamada "RequisitoClaimFilter".

     - Passando nessa 3° classe uma interface chamada "IAuthorizationFilter".
     - Criando um propriedade chamada "_ claim" do tipo "Claim".
     - O construtor recebe uma injeção de dependencia de "Claim", passando para a propriedade.

    - Cria um método chamado "OnAuthorization" que recebe como parametro 
 o "context" do tipo "AuthorizationFilterContext".

    - Com um if ele verifica se o usuario está autenticado usando a propriedade: "context.HttpContext.User.Identity.IsAuthenticated"
    - E com outro if verifica se o usuario tem a permição de claim, usando a primeira classe para validar.

 <blockquete>

        public class RequisitoClaimFilter : IAuthorizationFilter
        {
            private readonly Claim _ claim;

            public RequisitoClaimFilter(Claim claim)
            {
                _ claim = claim;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new StatusCodeResult(401);
                    return;
                }

                if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _ claim.Type, _ claim.Value))
                {
                    context.Result = new StatusCodeResult(403);
                }
            }
        }

 </blockquete>

 ### Aplicando o atributo personalizado no Controller de fornecedor.  

 - Aplica o "[Authorize]" no controler.
    - Passe o atributo personalizado(ClaimsAuthorize) que foi criado no action de adicionar, passando o nome e valor da claim.
    - Aplica o atributo "ClaimsAuthorize" nas action de atualizar, atualizarEndereco e remover.

 <blockquete>

        [ ClaimsAuthorize("Fornecedor","Adicionar") ]
        [ HttpPost ]
        public async Task< ActionResult< FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
        if (!ModelState.IsValid) return CustomResponse(ModelState);
               
        await _ fornecedorService.Adicionar(_ mapper.Map< Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
        }

 </blockquete>

 ### Passando as claims para o token.(arquivo AuthController)

 - Voltando a implementação do método("GerarJwt()") que cria os tokens, que fica na "AuthController".
    - É passado por parametro deo método GerarJwt(), um email do tipo string.

    - Com esse email é possivel obter o usuario usando o método "_ userManager.FindByEmailAsync(email)".
    - Com o usuario é possivel obter uma LISTA de Claims e as Roles.

    - O método GerarJwt() ele se torna async, troca o retorno dele para "async Task<string>".

 <blockquete>

    var user = await _ userManager.FindByEmailAsync(email);
    var claims = await _ userManager.GetClaimsAsync(user);
    var userRoles = await _ userManager.GetRolesAsync(user);
     
 </blockquete>

 ### Passando Claims adicionais para o token.(Essas clams são do token)

 - É passado para para a variavel "claims" algumas outras claims, além das que existe no banco de dados.
    - Exemplo: Sub, Email, Jti, Nbf, Iat, são as claims do token.

    - É criado um método privado chamado "ToUnixEpochDate", para converter a hora para um tipo especifico. aonde é passada a hora exata da região.

    - A listagem de Roles é passada para a listagem de claim.

    - É preciso fazer uma conversão da list de claim para IdentityClams, isso é possivel usando o método "identityClaims.AddClaims", deve ser criado antes uma instancia de "ClaimsIdentity" para ultilizar o método.

    <blockquete>

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
 
            foreach (var userRole in userRoles)
            {
               claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

    </blockquete>

    - Com essa lista de claims completa e convertida, devemos passar para o token, ultilizando o atributo  "Subject". 

    <blockquete>

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
               
                Subject = identityClaims,
                ....
            });

    </blockquete>

 - Para finalizar devemos passar o email como parametro aonde o método GerarJwt() é chamado.
    - [OBS]Não esqueça de por "await" agora que o método GerarJwt() é async !, se não ele retorna o result que é uma maquina de stado.
    - Testando o put no postman. provavel que vá da 200.

    - Testando o adicionar no postman, provavel que de 403 porque não tem a autorização(não tem a claim de adicionar).

# Finalizando a autorização com JWT (retornando mais dados além do token para o usuario)

 - Vamos criar mais 3 viewModel para devolver mais informações para o usuario.(No arquivo "UserViewModel")

    - 1° viewModel: UserTokenViewModel, aonde tem o id, email e uma lista de claimsVielModel.
    - 2° viewModel: ClaimViewModel, aonde tem o tipo e valor das claims
    - 3° viewModel: LoginResponseViewModel, aonde tem o token, o tempo de expiração, e o UserTOken.

    - O método "GerarJwt" deve retornar agora uma "Task< LoginResponseViewModel>"
    - No final do método cria uma instancia de "LoginResponseViewModel", passando os valores de:

    - AccessToken: que é o "encodedToken", o token completo e finalizado.

    - ExpiresIn: que é o tempo de inspiração, deve ser convertido usando o "TimeSpan.FromHours(_ appSettings.ExpiracaoHoras).TotalSeconds," convertendo para segundos.

    - UserToken: cria uma instancia de "UserTokenViewModel", passa o id e o email, a propriedade Claims, recebe o list de claims, aonde com o método ".select()" do linQ, cria instancias de "ClaimViewModel" para cada claim, passando tipo e valor

    <blockquete>

            var encodedToken = tokenHandler.WriteToken(token);
           
            // Uma forma de retorno aonde é possivel passar mais informações alem do token.
            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_ appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };

            return response;

    </blockquete>

 - É retornado um objeto complexo.

 <blockquete>

            {
                "success": true,
                "data": {
                    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGb3JuZWNlZG9yIjoiQXR1YWxpemFyLEV4Y2x1aXIiLCJzdWIiOiI0MmI5YzJjMC03NDAzLTQ2ZWQtODQxNi02NjliOGM5OWNiZTciLCJlbWFpbCI6ImxpbmsyQGVtYWlsLmNvbSIsImp0aSI6IjI0NjA5ODIxLTA2YWItNDUyOS04NzBiLWVmM2UzZTc1ODdiOSIsIm5iZiI6MTYyODI3NTA5OCwiaWF0IjoxNjI4Mjc1MDk4LCJleHAiOjE2MjgyODIyOTgsImlzcyI6Ik1ldVNpc3RlbWEiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdCJ9.1W80QVE0ms-tyZ1SkxrrYX-KUW9V6KhdqLeFMOg0he8",
                    "expiresIn": 7200.0,
                    "userToken": {
                        "id": "42b9c2c0-7403-46ed-8416-669b8c99cbe7",
                        "email": "link2@email.com",
                        "claims": [
                            {
                                "value": "Atualizar,Excluir",
                                "type": "Fornecedor"
                            },
                            {
                                "value": "42b9c2c0-7403-46ed-8416-669b8c99cbe7",
                                "type": "sub"
                            },
                            {
                                "value": "link2@email.com",
                                "type": "email"
                            },
                            {
                                "value": "24609821-06ab-4529-870b-ef3e3e7587b9",
                                "type": "jti"
                            },
                            {
                                "value": "1628275098",
                                "type": "nbf"
                            },
                            {
                                "value": "1628275098",
                                "type": "iat"
                            }
                        ]
                    }
                }
            }

 </blockquete>

# Consumindo e testando a segurança da API via Angular

 - Aplica as "claims" e "Authorize" no controller de produto.

 - Na aplicação em Angular configura o component de Login.

 ### LoginComponent 

 - configurando:

    - Cria um formulario com o campo email e password.
    - Cria um método login, Com o if verifica se os dados são validos e se não estão sujos.
    - Transforma o objeto vazio, em um objeto do tipo user com o valores passado, usando "Object.assign".
    - Chama o serviço "UserService" que tem o método de login, que faz a requisição para a API.

    - Retornando sucesso, método "onSaveComplete" que salva o token e outras informação no navegador.
    - É passado os dados para o navegador, usando o método "persistirUserApp" do serviço "UserService".
    - Volta para a pagina que lista os produtos usando "this.router.navigateByUrl('/lista-produtos');"

    <blockquete>

                userForm: FormGroup;
                user: User;
                errors: any[] = [];

                constructor(private fb: FormBuilder,
                private router: Router,
                private userService: UserService) { }

                ngOnInit() {
                    this.userForm = this.fb.group({
                        email: '',
                        password: ''
                    });
                }

                login() {
                    if (this.userForm.valid && this.userForm.dirty) {

                        // Transforma o objeto vazio, em um objeto do tipo user com o valores passado.
                        let _ user = Object.assign({}, this.user, this.userForm.value);

                        this.userService.login(_ user)
                        .subscribe(
                            result => { this.onSaveComplete(result) },
                            fail => { this.onError(fail) }
                        );
                    }
                }
           
                onSaveComplete(response: any) {
                this.userService.persistirUserApp(response);
                this.router.navigateByUrl('/lista-produtos');
                }

    </blockquete>
    
 ### UserService 

 - O serviço tem o método "login()" que retorna um Observable<User> e "persistirUserApp".

    - login: a url que serve para a fazer a requisição, está dentro da propriedade "UrlServiceV1".
    - Essa propriedade fica no "BaseService", uma classe que é herdada.
    - com isso faz a concatenação com "entrar" que é a action que faz login, "user" que é o objeto que tem email e senha,
    e o método "super.ObterHeaderJson()".

    -Tem um tratamento dentro do .map, aonde vai ser retornado o objeto ou um objeto vazio.

    - persistirUserApp(): cria um localStorage, com o nome de "app.token" para quardar o "response.accessToken",
    que é o token gerado na API.
    - Também cria um localStorage chamado "app.user" que é um objeto com os dados do usuario.
    - O localStorage "app.user" é convertido para string, usando o metodo "JSON.stringify", para poder ser salvo.

    <blockquete>

                @Injectable()
                export class UserService extends BaseService {

                    constructor(private http: HttpClient) { super() }

                    login(user: User): Observable< User> {

                        return this.http
                            .post(this.UrlServiceV1 + 'entrar', user, super.ObterHeaderJson())
                            .pipe(
                                map(super.extractData),
                                catchError(super.serviceError)
                            );
                    }

                    persistirUserApp(response: any){
                        localStorage.setItem('app.token', response.accessToken);
                        // transforma em string para poder armazenar.
                        localStorage.setItem('app.user', JSON.stringify(response.userToken));
                    }
                }

    </blockquete>

 - O método "super.ObterHeaderJson()" fica na classe "BaseService", ele retorna um objeto que tem a propriedade "headers".
  
    <blockquete>

            protected ObterHeaderJson() {
                return {
                    headers: new HttpHeaders({
                        'Content-Type': 'application/json'
                    })
                };
            }

    </blockquete>

 ### MenuUserComponent.html

    - Usa o component Angular "ngSwitch" para informar se o usuario está logado ou não.

    <blockquete>

        < ul [ngSwitch]="userLogado()" class="nav navbar-nav navbar-right">

            < li * ngSwitchCase="false">< a  class="nav-link text-dark" [ routerLink]="['/entrar']">Entrar</ a>< /li>
            < li * ngSwitchCase="true">< a  class="nav-link text-dark">{{ saudacao }}</ a></ li>

        </ ul>

    </blockquete>

 ### MenuUserComponent.ts

 - Chama o servico que tem o método "obterUsuario" para verificar se tem usuario logado ou não. 

    <blockquete>

                saudacao: string;

                constructor(private userService: UserService) {  }

                userLogado(): boolean {

                    var user = this.userService.obterUsuario();
                    if (user) {
                        this.saudacao = "Olá " + user.email;
                        return true;
                    }

                    return false;
                }

    </blockquete>

 ### BaseService

 - No serviço "BaseService" tem um método "obterUsuario" que pega dados do "localStorage" usnado o método "getItem".
 - É passado a chave do "localStorage" que tem o nome "'app.token'", esse resultado é convertido usando o método "JSON.parse()"

    <blockquete>

            public obterUsuario() {
                return JSON.parse(localStorage.getItem('app.user'));
            }

    </blockquete>

 ### Passando um HEADER em uma requisição do Angular.

 - É passado um método chamado "ObterAuthHeaderJson()", como 2° parametro na requisição.

    <blockquete>

            obterTodos(): Observable<Produto[]> {
                return this.http
                    .get<Produto[]>(this.UrlServiceV1 + "produtos", super.ObterAuthHeaderJson())
                    .pipe(
                        catchError(this.serviceError));
            }

    </blockquete>

    - Ele retorna um objeto que tem um propriedade chamada "headers", que é uma instancia de "HttpHeaders"
    - Essa informação é importante, por que dessa forma é passada o token do usuario logado, usando o atributo "Authorization".
    - O valor passado é o tipo do token e uma concatenação do método que retorna no token usando o método "obterTokenUsuario".
    - O método "obterTokenUsuario()" obtem o token do localStorage.
    - Assim ele autoriza a fazer as requisições.
  
    <blockquete>

            protected ObterAuthHeaderJson(){
                return {
                    headers: new HttpHeaders({
                        'Content-Type': 'application/json',
                        'X-Requested-With': 'XMLHttpRequest',
                        'MyClientCert': '',        // This is empty
                        'MyToken': ''   ,        // This is empty ,
                        'Authorization': `Bearer ${this.obterTokenUsuario()}`,
                        'Access-Control-Allow-Origin': ' * ',
                        'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
                        'Access-Control-Allow-Headers':'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers',

                    })
                };
            }

    </blockquete>

# Interagindo com o usuário logado de qualquer camada

 - No controler é possivel ter acesso ao usuario logado usando uma classe/propriedade chamada "User",
 aonde é possivel obter alguns valores do usuario logado.
 - Mas só é possivel ter acesso na camada de Controller, porem esse tutorial vai explicar 
 ter como obter acesso em qualquer camada.

 - Na camada de negocio, cria uma interface chamada "IUser".

    <blockquete>

        public interface IUser
        {
            /// < summary>Nome do usuario </ summary>
            string Name { get;}

            /// < summary>Obtenha o Id </ summary>
            Guid GetUserId();

            /// < summary>Obtenha o email </ summary>
            string GetUserEmail();

            /// < summary>Verifica se está autenticado </ summary>
            bool IsAuthenticated();

            /// < summary>Verifica se tem a Role informada </ summary>
            bool IsInRole(string role);

            /// < summary>Retorna uma lista de Claim </ summary>
            IEnumerable< Claim> GetClaimsIdentity();
        }

    </blockquete>

 - Na pasta de extenções na camada de Api, cria uma classe chamada "AspNetUser.cs" para implementar a interface.

 <blockquete>

        public class AspNetUser : IUser
        {
            private readonly IHttpContextAccessor _ accessor;

            public AspNetUser(IHttpContextAccessor accessor)
            {
                _ accessor = accessor;
            }

            public string Name => _ accessor.HttpContext.User.Identity.Name;

            public Guid GetUserId()
            {
                return IsAuthenticated() ? Guid.Parse(_ accessor.HttpContext.User.GetUserId()) : Guid.Empty;
            }

            public string GetUserEmail()
            {
                return IsAuthenticated() ? _ accessor.HttpContext.User.GetUserEmail() : "";
            }

            public bool IsAuthenticated()
            {
                return _ accessor.HttpContext.User.Identity.IsAuthenticated;
            }

            public bool IsInRole(string role)
            {
                return _ accessor.HttpContext.User.IsInRole(role);
            }

            public IEnumerable<Claim> GetClaimsIdentity()
            {
                return _ accessor.HttpContext.User.Claims;
            }
        }

        public static class ClaimsPrincipalExtensions
        {
            public static string GetUserId(this ClaimsPrincipal principal)
            {
                if (principal == null)
                {
                    throw new ArgumentException(nameof(principal));
                }

                var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
                return claim?.Value;
            }

            public static string GetUserEmail(this ClaimsPrincipal principal)
            {
                if (principal == null)
                {
                    throw new ArgumentException(nameof(principal));
                }

                var claim = principal.FindFirst(ClaimTypes.Email);
                return claim?.Value;
            }
        }

 </blockquete>

 - O método "ClaimsPrincipalExtensions" cria uma extenção dos métodos da classe "ClaimsPrincipal".
 - Essa extenção funciona passando um atriuto do tipo "ClaimsPrincipal", com o "this" antes. como parametro 
   para um método static, de uma classe static, esse método será uma extenção de da classe "ClaimsPrincipal".

 - Faz um tratamento verificando se a classe é null.
 - Com o método "FindFirst()" é possivel obter uma valor informado pelo parametro.
 - Retornando o valor do que foi passando no parametro. (ler a documentação para entender melhor.)
 - https://docs.microsoft.com/pt-br/dotnet/api/system.security.claims.claimsprincipal.findfirst?view=net-5.0

 - Cria uma injeção de dependencia da interface "IHttpContextAccessor".
 - Ela permite acessar o httpContext de qualquer lugar.

    - GetUserId(): Caso o usuario esteja autenticado, retorne o id do usuario, se não retorna um guid vazio.

    - GetUserEmail():  Caso o usuario esteja autenticado, retorne o email do usuario, se não retorna uma strig vazia.

    - IsAuthenticated(): Retorna se o suaurio está autenticado.

    - IsInRole(): verifica se o usuario tem aquela Role.
    https://docs.microsoft.com/pt-br/dotnet/api/system.web.security.roleprincipal.isinrole?view=netframework-4.8

    - GetClaimsIdentity(): retorna uma lista de Claim.

 ### configurando a injelçao de dependencia.
 - Configurando a dependencia na classe "DependencyInjectionConfig".
 - Usa o "AddSingleton" na interface "IHttpContextAccessor", para não confundir de usuario.

    <blockquete>

            services.AddSingleton< IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped< IUser, AspNetUser>();  

    </blockquete>

 ### configurando na mainController.

 - Na classe "MainController" o controller que todos herdam, é criado uma propriedade chamada "AppUser",
 do tipo "IUser", aonde recebe uma injeção de dependencia de "IUser".

 - Cria uma propriedade chamada "UsuarioAutenticado" do tipo bool que recebe true ou false se o usuario estiver logado ou não. 
 - Cria uma propriedade chamada "UsuarioId" que recebe o id do usuario caso ele esteja logado.
 - Essas propriedade ajuda na logica de programar, caso seja preciso.
  
 <blockquete>

                    protected Guid UsuarioId { get; set; }
                    protected bool UsuarioAutenticado { get; set; }

                    protected MainController(INotificador notificador, IUser appUser)
                    {
                        _ notificador = notificador;
                        // Propriedade usada nas controller para ter facil acesso.
                        AppUser = appUser;
            
                        if (appUser.IsAuthenticated())
                        {
                            UsuarioId = appUser.GetUserId();
                            UsuarioAutenticado = true;
                        }
                    }

 </blockquete>

 - Toda controller que herda a MainController, deve injetar o "IUser" e por no ": base()" que fica no método construtor.

 ### Manipulando o usuario no serviço do produto. (camada de negocio).

 - Faz uma injeção de dependencia de "IUser", no construtor do serviço.
 - Usa a propriedade e chama o método "GetUserId()", para obter o id do usuario, e atribui em uma variavel.

 - [Ferramenta do VS] quando está debugando pode usar a  ferramentachamada "QuickWatch", para explorar outros valores dentro de uma propriedade que é um objeto por exemplo.

# Trabalhando com HTTPS

 - Configurar no "UseApiConfig" que fica no Startup.
 - Devemos configurar na StartUp com o uso do "Hsts" para informar que ele só aceita o https.
 - Mas devemos usar o "app.UseHttpsRedirection();" para redirecionar o usuario ao https.
 - Testar quando a aplicação estiver no ar (em produção).

# CORS - Cross-Origin Resource Sharing

 - O CORS ele relaxa a segurança do sistema, para o front end fazer requisições.

 - O CORS é aplicado pelo browser, ele bloqueia automatico.

 - options.AddDefaultPolicy(): define uma politica padrão.
 - AllowCredentials(): não tem muita efetividade.
 
 - [DisableCors] : desativa o cors na controller, assim deixa mais seguro, nada sobreescreve ele, no asp.net core 2.2.
 - preciso fazer alguns testes.

 <blockquete>


 </blockquete>

# Versionamento e Documentação da API.

 - Ajuda a ter as duas verções da API, evitando bug ou erros de quem a usa.

 - Instala o pacote: 

 <blockquete> Microsoft.AspNetCore.Mvc.Versioning </blockquete>

 - Instala um segundo pacote: 

 <blockquete> Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer </blockquete>

 - No arquivo ApiConfig, implementa o codigo de versionamento.

 <blockquete>

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

 </blockquete>

 - AssumeDefaultVersionWhenUnspecified : assuma a versão defaut.
 - DefaultApiVersion: defina a maior versão e a menor.
 - ReportApiVersions: Quando consome a API informa se está absoleta ou não.
 
 <blockquete>

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

 </blockquete>

 - GroupNameFormat: agrupamento da versão.
 - SubstituteApiVersionInUrl: Seta ou pega um valor, que pode ser subistituido pelas rotas,
 para aplicação não ficar com problema de roteamento.

 - Configuração nos Controllers.

 <blockquete>
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/produtos")]

 </blockquete>

 <blockquete>

        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/fornecedores")]

 </blockquete>

 - version: o aspnet core ja entende que o tipo "apiVersion" é trocado pela verção.
 
 - A rota da requisição muda.

 <blockquete>

        https://localhost:44320/api/v1/fornecedores

 </blockquete>

 - Declarar que é antigo.

 <blockquete>

        [ApiVersion("2.0")]
        [ApiVersion("1.0", Deprecated = true)] // usa esse termo para indicar que a verção é antiga.
        [Route("api/v{version:apiVersion}/fornecedores")]

 </blockquete>
 
 - adiciona uma pasta  chamada V1 e outra V2 para quardar as controlers

# Swagger - modo simples

 - Documentando a API.

 - Abra o Package Manager Console, e digita o comando que instala o Swagger.

 <blockquete>

        Install-Package Swashbuckle.AspNetCore

 </blockquete> 

 - Configura o StartUp, no metodo ConfigureServices", botando a v1 como padrão.
   
 <blockquete>

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            }); 

 </blockquete> 

 - Depois bota a configuração no metodo "Configure"

 <blockquete>

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name:"My API V1");
            });

 </blockquete> 

# Swagger - modo Complexo.

 - Cria um arquivo chamada "SwaggerConfig" na pasta configurações, dentro dela vai ser criada 4 classes!

 - A primeira classe se chama "ConfigureSwaggerOptions"

 - IConfigureOptions< SwaggerGenOptions>: estende a classe, estende a configuração. 

 - IApiVersionDescriptionProvider: interface do pacote de versionamento.

 - ConfigureSwaggerOptions: recebe um provider.

 - Configure: faz um foreach para pegar todas as verções da API, e adiciona um doc para cada uma delas.

 - CreateInfoForApiVersion: cria uma documentação minima da API.

 - O if serve para verificar se a versão está absoleta, para adicionar uma descrição extra.

 - 

 <blockquete>

        public class ConfigureSwaggerOptions : IConfigureOptions< SwaggerGenOptions>
        {
                readonly IApiVersionDescriptionProvider provider;

                public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

                public void Configure(SwaggerGenOptions options)
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }
                }

                static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
                {
                    var info = new OpenApiInfo()
                    {
                        Title = "API - desenvolvedor.io",
                        Version = description.ApiVersion.ToString(),
                        Description = "Esta API faz parte do curso REST com ASP.NET Core WebAPI.",
                        Contact = new OpenApiContact() { Name = "Eduardo Pires", Email = "contato@desenvolvedor.io" },
                        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                    };

                    if (description.IsDeprecated)
                    {
                        info.Description += " Esta versão está obsoleta!";
                    }

                    return info;
                }
         }

 </blockquete> 
 
 - Configurando a Injeção de dependencia, no arquivo "DependencyInjectionConfig", 

 <blockquete>
        services.AddTransient< IConfigureOptions< SwaggerGenOptions>, ConfigureSwaggerOptions>();
 </blockquete> 

 - Remove o "AddSwaggerGen" do arquivo Startup!

 - Cria uma classe chamada "SwaggerConfig" no arquivo "SwaggerConfig", com o metodo "AddSwaggerConfig", que é chamado na Startup.

 <blockquete>

            public static class SwaggerConfig
            {
                public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
                {
                    services.AddSwaggerGen(c =>
                    {
                        c.OperationFilter< SwaggerDefaultValues>();

                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                            Name = "Authorization",
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        });

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                            }
                        });
                    });

                    return services;
                }

                public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
                {
                    //app.UseMiddleware< SwaggerAuthorizedMiddleware>();
                    app.UseSwagger();
                    app.UseSwaggerUI(
                        options =>
                        {
                            foreach (var description in provider.ApiVersionDescriptions)
                            {
                                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                            }
                        });
                    return app;
                }
            }


 </blockquete>

 - No método "AddSwaggerGen" que é uma estenção da service, cria uma configuração "c.OperationFilter< SwaggerDefaultValues>();".

 - Cria a classe "SwaggerDefaultValues" 

 - Ela implementa configuração que o Eduardo pires criou.

 - OperationFilter: serve para aceitar classes personalizadas. 

 <blockquete>

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = context.ApiDescription
                    .ParameterDescriptions
                    .First(p => p.Name == parameter.Name);

                var routeInfo = description.RouteInfo;

                operation.Deprecated = OpenApiOperation.DeprecatedDefault;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (routeInfo == null)
                {
                    continue;
                }

                if (parameter.In != ParameterLocation.Path && parameter.Schema.Default == null)
                {
                    parameter.Schema.Default = new OpenApiString(routeInfo.DefaultValue.ToString());
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }

 </blockquete>

 - IApplicationBuilder : Criando uma abstração do "ApplicationBuilder" 

 - Aplica o " services.AddSwaggerConfig();" na metodo "ConfigureServices" da "Startup".

 - Aplica o "app.UseSwaggerConfig(provider);" no método "Configure" da "Startup".

 - Codigo alternativo para deixar os endpoint antigo de cor cinza

 <blockquete>

            var apiVersionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType< ApiVersionAttribute>().FirstOrDefault(); 
            operation.Deprecated = apiVersionMetadata?.Deprecated ?? false;

 </blockquete>

 - Esconde o codigo: 

 <blockquete> 

            //operation.Deprecated = OpenApiOperation.DeprecatedDefault; 

 </blockquete>

# Autorização via JWT no Swagger

 - Criando uma configuração de autenticação pelo swagger no método "AddSwaggerConfig" da classe "SwaggerConfig".

 - 

 <blockquete>

    public static class SwaggerConfig
        {
            public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
            {
                services.AddSwaggerGen(c =>
                {
                    c.OperationFilter< SwaggerDefaultValues>();

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                        Name = "Authorization",
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                });

                return services;
            }

            public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
            {
                //app.UseMiddleware< SwaggerAuthorizedMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });
                return app;
            }
        }

 - 

 </blockquete>

# Autorização via JWT no Swagger

 - Cria uma configuração para a autenticação no método "AddSwaggerGen" na classe "AddSwaggerConfig"

 <blockquete>

                services.AddSwaggerGen(c =>
                        {
                            c.OperationFilter<SwaggerDefaultValues>();

                            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                            {
                                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                                Name = "Authorization",
                                Scheme = "Bearer",
                                BearerFormat = "JWT",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey
                            });

                            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    new string[] {}
                                }
                            });
                        });

                return services;

 </blockquete>

 - Com isso ja pode fazer o login no Swagger.

# Restringindo o acesso ao Swagger(Essa validação de autenticação não está funcionando muito bem, refazer depois.)

 - Cria um Middleware chamado:  SwaggerAuthorizedMiddleware

 <blockquete>

                public class SwaggerAuthorizedMiddleware
                {
                    private readonly RequestDelegate _ next;

                    public SwaggerAuthorizedMiddleware(RequestDelegate next)
                    {
                        _ next = next;
                    }

                    public async Task Invoke(HttpContext context)
                    {
                        if (context.Request.Path.StartsWithSegments("/swagger")
                            && !context.User.Identity.IsAuthenticated)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }

                        await _ next.Invoke(context);
                    }
                }

 </blockquete>

 - RequestDelegate: necessario para o "Middleware" funcionar, ele é um componente que passa em um tuneo de componentes.

 - Ele sempre passa um request para o proximo Middleware. 

 - Invoke: Verifica se o usuario está autenticado quando acessa a URL.(TRATAMENTO QUE VER SE ESTÁ AUTENTICADO).

 - Bota a configuração no metodo "IApplicationBuilder", na classe "SwaggerConfig".

 <blockquete>

                app.UseMiddleware< SwaggerAuthorizedMiddleware>();

 </blockquete>

# Implementando o Logging

 - Monitora erros da sua aplicação.

 - O primeiro exemplo vai ser na classe de autenticação.

 - Injeta o ILogger na controller de autenticação.

 <blockquete>

             private readonly ILogger _ logger;

 </blockquete>

 - No construtor da classe, deve por a classe como tipo.

 <blockquete>

            ILogger< AuthController> logger,

 </blockquete>
 
 - LogInformation é a classe que é usada para criar um logger, nesse exemplo é usado um que informa que o loggin foi feito.

 - O logger não precisa resolver a injeção de dependencia, porque o ele já está dentro do Asp.net

 <blockquete>

            _ logger.LogInformation("Usuario " + loginUser.Email + " logado com sucesso");

 </blockquete>

 - No TesteControler que está na pasta V2, foi criado varios exemplos.

 <blockquete>

                _ logger.LogTrace("Log de Trace");
                _ logger.LogDebug("Log de Debug");
                _ logger.LogInformation("Log de Informação");
                _ logger.LogWarning("Log de Aviso");
                _ logger.LogError("Log de Erro");
                _ logger.LogCritical("Log de Problema Critico");

 </blockquete>

 - Trace: log minimo menos impactante, desenvolvimento.
 - Debug: deve ser usando durante o desenvolvimento.

 - information: Informação.
 - Warning: aviso.
 - error: erro.
 - critical: critico.

 - É bom salvar o log no banco usando uma classe de provider.(???)

 - Lembrar depois do KissLogger

# elmah.io

 - Ferramenta paga, para monitoramento.

 - Tutorial de como instalar.

 <blockquete>

            https://docs.elmah.io


            https://docs.elmah.io/logging-to-elmah-io-from-aspnet-core/

 </blockquete>

 - Cria um arquivo chamado LoggerConfig, classe que configura o logger.

 - Aonde que bota as chaves, que o elmah identifica o usuario.

 - Instala uma extenção do elmah, caso queira configurar o provider.

 <blockquete>

        install-package Elmah.io.extensions.logging

        serivices.AddElmahIo(....)

 </blockquete>

 - AddElmahIo só registra os erros, o outro registr tudo.

# Monitorando a saúde da API com HealthChecks

 - Verifica a saude da aplicação. 

 - Na startUp no método "ConfigureServices" adiciona o chamada " services.AddHealthChecks(); "
 - Ainda na StartUp, no método "Configure", adiciona a chamada " app.UseHealthChecks("/hc"); "


 - Instala um pacote chamado: Xabaril.
 - Ele adiciona HealthChecks para outros pacotes instalados na API, sql, Azure, mongoD, etc...  

 <blockquete>

            ht tps://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

 </blockquete>

 - Instala O pacote "HealthChecks", escolha a versão 5.1 caso seja o dotNET CORE 5. 

 <blockquete>

            Install-Package AspNetCore.HealthChecks.SqlServer

 </blockquete>

 - Configurando

 <blockquete>

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString(name:"DefaultConnection"), name: "BnacoSQL");

 </blockquete>

 - No metodo "AddHealthChecks()" é colocado a configuração, para identificar o HealthChecks que esta sendo monitorado.

 - Instala mais um pacote que cria uma interface visual, escolha a versão 5.1 caso seja o dotNET CORE 5. 

 <blockquete>

        Install-Package AspNetCore.HealthChecks.ui

 </blockquete>

 - Configuração do método "UseApiConfig" da classe "AddApiConfig", configuração do "MapHealthChecksUI".

 <blockquete>

                endpoints.MapHealthChecks("/api/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI(options =>
                {
                    options.UIPath = "/api/hc-ui";
                    options.ResourcesPath = "/api/hc-ui-resources";

                    options.UseRelativeApiPath = false;
                    options.UseRelativeResourcesPath = false;
                    options.UseRelativeWebhookPath = false;
                });

 </blockquete>

 - Cria um arquivo com o nome de "SqlServerHealthCheck", configura uma validação que verifica se tem produtos no banco.

 <blockquete>

            public class SqlServerHealthCheck : IHealthCheck
            {
                readonly string _ connection;

                public SqlServerHealthCheck(string connection)
                {
                    _ connection = connection;
                }

                public async Task< HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
                {
                    try
                    {
                        using (var connection = new SqlConnection(_ connection))
                        {
                            await connection.OpenAsync(cancellationToken);

                            var command = connection.CreateCommand();
                            command.CommandText = "select count(id) from produtos";

                            return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) > 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                        }
                    }
                    catch (Exception)
                    {
                        return HealthCheckResult.Unhealthy();
                    }
                }
            }


 </blockquete>

 - No tutorial é ensinado a configural o HealthCheck junto com o "elmah", como o elmah é pago, não vou está testando essa opção.

 - seguir depois outros tutoriais.

 <blockquete>

        https://renatogroffe.medium.com/net-5-health-checks-exemplos-de-implementação-em-projetos-asp-net-core-3488cc807608

        https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0
        
 </blockquete>

# Realizando o deploy no IIS Local

 - Instala o Hosting Bundle for Windowns.

 - AspNet core 5: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-5.0.17-windows-hosting-bundle-installer


 - AspNet core 7(mais atualizada): https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/hosting-bundle?view=aspnetcore-7.0

 - Se o pacote de hospedagem for instalado antes do IIS, a instalação do pacote deverá ser reparada. Execute o instalador do Hosting Bundle novamente após instalar o IIS.

 - Depois que instala, deve para o serviço do IIS e iniciar novamente
 
 <blockquete>

    net stop was /y
    net start w3svc
    
 </blockquete>

 - Instalando o IIS: https://pt.stackoverflow.com/questions/185603/como-ativar-o-iis-no-windows-10

 - Instalando e Configurando o IIS: https://www.youtube.com/watch?v=BihwoBW1NCM

 - Instalando:
    
  - 1° Buscar por "Ativar ou desativar recursos do Windows".

  - 2° Marca todas as opções de "Serviço de informações da internet".

  - 3° Marca todas as opções de "Recursos de desenvolvimento de aplicativos". 

  - 4° De ok e espere instalar.

 - Configuração do IIS no Windows 10:

  - 1° De um duplo click em "Mapeamento de Manipulador", "editar permissão de recurso", marca a opção de executar.

  - 2° No menu principal clica em "Restringir ou habilitar extensões ISAPI e CGI especificas no servidor Web", "editar permissão de recurso", e marcar as 2 checkBox. pode alterar a porta padrão(80), clica em sites, default web site, editar associações, add, porta 82( exemplo).

 <blockquete>

    net stop was /y
    net start w3svc

 </blockquete>

 - Configurando o site no IIS.

 - Cria a pasta "curso.webapi".

 - O caminho fisico é "C:\inetpub\wwwroot\curso.webapi".

 - Entra no IIS, adiciona um site, na opção Sites, na aba esquerda,

    - Nome: curso.webapi.

    - Cria uma nova porta.

 - Na coluna a direita clica em Binding(Associações), adiciona mais um binding na 

    - porta : 443.

    - SSL certificado: pode usar o do visual studio para testar.

    - (PESQUISAR DEPOIS COMO GERAR ESSES CERTIFICADOS).

 - No menu a esquerda, clica no "Pools de Aplicação", verifica se o site foi criado.

# Publish (Inicia de modo administrador)

 - Clica com o botão direito no projeto API, escolhe a opção Publish.

 - IIS, FTP, etc

 - Escolhe a opção Web Deploy(inplantação da WEB), ele conecta direto no IIS e publica.

 - server: localhost ou outra porta

 - SiteName: o nome que foi criado no IIS: curso.webapi.

 - Destino URL: http://localhost

 - Valida a conecção, para da o icnone verdinho.

 - next: 

 - configuração: Relesse.

 - Target framework: netcoreaoo2.2

 - deployment mode: 

    - framework-dependent : depende da maquina com esse framwork.(a melhor opção)

    - self-content : leva o framework junto.

 - Target runtime: Portable (funciona em qualquer sistema operacional)

 - File publish options: remove os arquivo, antes de por os arquivos novos.(deixa marcado)

 - Database: bota a conectionsString, SQLEXPRESS de uma maquina local.

 - Entity framework migrations: não marca nada.

 - Dicas: recomendado fazer o deploy com o IIS desligado.

 - não esqueça de por o s no https na url.

# Rodando a API em SelfHosting

 - Fazendo uma publicação rapida.

 - Vai no caminho do projeto

 - digita o comando: 

 <blockquete>

        dotnet run --project devio.api.csproj

 </blockquete>

 - Demora um pouco mas starta a aplicação, fica rodando no processo do windows, não é completo igual ao IIS.

 - 2° Maneira:

 - Publica usando o visual estudio mas escolhe a forma para publicar em uma pasta.

 - Ele gera os arquivos.

 - Dentro da pasta executa o comando.

 <blockquete>

        dotnet devio.api.dll

 </blockquete>

 - 

# Videos extras para ver depois

 - configurando dominio e SSL no IIS. 

 <blockquete>

        https://www.youtube.com/watch?v=b19wo8tHJc8&t=77s


 </blockquete>

 - Video falando sobre windows server.

 <blockquete>

            https://www.youtube.com/watch?v=auWeL4fxqkE&t=1248s

 </blockquete>

# Realizando o deploy no Azure

 - Vai no Publish, e cria um novo perfil, 
 - Escolhe "App serve", "Azure App Service", advanced são as mesma configuração que a anterior.
 - adiciona uma conta.


 - Escolha um nome
 - Subscription: Microsoft Azure Sponsors
 - Resource Group: Demos.
 - Hosting plan:
    - Hosting plan: demosnplan
    - Location: Brazil South:
    - size: Free. 
 - Application Insights: None.

 - Cria um novo banco.
  - nome: demos_db.

 - Cria um SQLDataBase, um servidor de banco de dados.
  - demosdeviolincoln-dbserver.
  - Location: Brazil.
  - curso@Api123.
  - adm username: deviodemos.

 
 - app, plano de hospedagem, servidor de banco, e um banco.

 - configura a conexao de banco, e rodar os migration do entityframework.

# Experiencia em 2023

  - Destino: Azure.
  - Destino especifico: azure aplicação windows.
  - Serviço de aplicativo: cria uma nova instancia.
   - Nome: DevIOApi.
   - Nome da assinatura: Azure subscription 1.
   - Grupo de recursos: Cria um novo chamado "Demos".
   - Plano de hospedagem: Cria um novo que seja de graça(free).
    - Plano de Hospegagem: DemosPlan.
    - Local: Brazil South.
    - Tamanho: Gratuito(free).
  - Gerenciamento de API: 
   - Nome da API: DevIOApi.
   - Nome da assinatura: Azure sbscription 1.
   - Grupo de recursos: Demos.
   - Serviço de Gerenciamento de API: 
    - nome:
    - local:
    - e-mail:
   -Sufixo de URL da API:   
  - Tipo de Implantação:  

 - CONFIGURA O BANCO DE DADOS DEPOIS


- Na classe startup no construtor remove o "IConfiguration" e coloca o "IHostingEnvironment".

 <blockquete>

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(hostingEnvironment.ContentRootPath)
                 .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile(path: $"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();

            if(hostingEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

 </blockquete>
 
 - cria um appsettings para o azure. com a conection string do azure e sua endereço.

 - seta para produção

 <blockquete>

 $env:ASPNETCORE_ENVIRONMENT='Production'

 </blockquete>

 - Executa o comando para atualizar o banco.

 - no azure bota o ip no firewall

 <blockquete>

    update-database -Verbose -Context Meudbcontext

 </blockquete>

 - aposta para api

 <blockquete>

    update-database -Verbose -Context ApplicationDbContext

 </blockquete>

 - no painel de configuração do azure, vai em configuração> e adiciona um novo
 application settings.

  - Name: ASPNETCORE_ENVIRONMENT
  - value: Production


 -  A opção: Editor do Serviço de Aplicativo (Visualização), exibe os arquivos do projeto.

 - você pode publicar só o arquivo que não subiu, apenas clicando nele.

 - 

 <blockquete>
 </blockquete>
 -

 <blockquete>
 </blockquete>
 -

 <blockquete>
 </blockquete>
 -

 <blockquete>
 </blockquete>
 -

 <blockquete>
 </blockquete>
 -

 <blockquete>
 </blockquete>