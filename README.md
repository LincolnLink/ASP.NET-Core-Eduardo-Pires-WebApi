# ASP.NET-Core-Eduardo-Pires-WebApi

 - Projeto do Curso do AspNet Core com Eduardo Pires


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
            public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
            {
                // Converte o o model fornecedor para fornecedorViewModel.
                var fornecedor = await ObterFornecedorProdutosEndereco(id);

                if (fornecedor == null) return NotFound(); //404 não encontrado.

                return Ok(fornecedor); //200 Ok
            }

            public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
            {
                return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
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

 - TODO (por a explicação do modo de carregar imagens pesadas)!!!

# Concluindo a modelagem da API

 - Cria o método atualizar. 

 - Cria um classe chamada "ApiConfig" nela isola algumas configrações da classe StratUp.

# Autenticação

 - Tratando da segurança da API.
 - Bota o atributo "Authorize" nas controllers.
 - Bota o atributo "[AllowAnonymous]" nas actions que vc esta liberando para todas as pessoas.

# Implementando o ASP.NET Identity

### 1° passo 

 - Cria um classe chamada "IdentityConfig.cs". para isolar a confgiração do Identity
 - Chama a classe isolada no startUp.
 - Paasa a "configuration" por parametro, que é a configuração da conectionString.

<blockquete>
            services.AddIdentityConfig(Configuration);
</blockquete>

 - Cria um outro dbcontext chamado "ApplicationDbContext", 
  dentro de uma pasta chamada data que deve ser criada.
 - Configurao o ApplicationDbContext.

<blockquete>

            public class ApplicationDbContext : IdentityDbContext
            {
                public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
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

 - Configurando  a model do Identity.
 - "AddRoles<IdentityRole>()" determina uma classe com regras, foi passada a padrão.
 - "AddEntityFrameworkStores<ApplicationDbContext>()" informa que esta trabalhando com entityframework.
 - "AddDefaultTokenProviders" gerador de token para resetar senha e confirmar email.

<blockquete>

           services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
                //.AddErrorDescriber<IdentityMensagensPortugues>()

</blockquete>
 
 - No método "UseApiConfig" que fica na classe "ApiConfig" bota a coonfiguração "app.UseAuthentication();".

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
 - Usando a propriedade "Succeeded", verifica se foi criado corretamente.
 - Se teve sucesso, então usa o "_signInManager" para chamar o método ".SignInAsync(user, false);",
 o false é para informar se deve ser salvo as informações no proximo login.

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

 - Criando a action de logar, recebe como parametro um objeto "LoginUserViewModel".

 - Verifica se a modelState é valida.
 - Usa o "_signInManager" para chamada o método 
 "PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);", ele recebe o usuario, senha e
 dois bool, o primeiro é para informar se é persistence, e o segundo vai travar o usuario para tentativas
 invalidas depois de x minutos.

 - Caso consiga logar com sucesso cria documenta no logger. 
 - Se não conseguiu logar trata o erro com "CustomResponse".
  
<blockquete>

            [HttpPost("entrar")]
            public async Task<ActionResult> Login(LoginUserViewModel loginUser)
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

                if (result.Succeeded)
                {
                    return CustomResponse(loginUser);
                    //_logger.LogInformation("Usuario " + loginUser.Email + " logado com sucesso");
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
  
 - O lado do client sempre guarda o Token.

### cores que representa um JWT

  - Vermelho: forma de criptação, e a forma que ele é,
     diz o tipo de criptografia.

  - Roxa: é o dado, o Json.

  - Azul: É a assinatura, informa a chave de criptografia, a chave fica na aplicação,
  para descriptografar e criptografar.

# Implementando o JWT

  - Na pasta de extenção cria uma classe chamada "AppSettings.cs" 
  - É uma classe para gerenciar propriedades do token 
     - Secret: Chave de criptografia do token.
     - ExpiracaoHoras: expirações em horas que o token vai perder a validade.
     - Emissor: Quem emite (a aplicação).
     - ValidoEm: Em quais URL esse token é valido.

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

 - Na configuração "IdentityConfig", deve configurar o JWT, apartir daqui toda configuração fica nesse classe.
 - Congifura a classe com o json, já está populando a classe.
    - .GetSection(): pega a sessão do json appsettings, e guarda em uma variavel.
    - .Configure(): cria um vinculo do json e a classe no asp.net core.
 - Quando injetar a classe "AppSettings" ela já vai vim com os dados populados.

<blockquete>

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

</blockquete>

- Pegando os dados da classe "AppSettings" que acabou de ser configurada, 
usando o método "appSettingsSection.Get<AppSettings>()".
- Definindo a chave "key" baseado no segredo, fazendo Encoding.
- Usa a chave que é uma coleçã ode bites, para criptografar os dados do token.

<blockquete>

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

</blockquete>

### Configurando o token do JWT (criando e configurando uma authenticação)

 - "services.AddAuthentication()": Adiciona a autenticação e configura.

 - DefaultAuthenticateScheme: define um padrão de validação, 
 que é gerar um token.

 - DefaultChallengeScheme: Toda vez que for validar, verifica o token.

 - Para não da erro deve instalar a versão 3.0.0 do pacote.

 - solução: https://qastack.com.br/programming/58593240/how-to-replace-addjwtbearer-extension-in-net-core-3-0
  
<blockquete>

            Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 3.0.0

</blockquete>

 - O método "AddJwtBearer()" adiciona mais configurações.

  - RequireHttpsMetadata: se for trabalhar apenas com Https, 
  pode deixar true, define que só vai trabalhar com https, para evitar ataques.

  - SaveToken: Pergunta se o token deve ser guardado no 
  "AuthenticationProperties", depois de uma autenticação de sucesso.

  - É bom que quarde porq fica mais facil da aplicação validar,
  app apresentação do token.

### outras configurações (criando o token)
 - TokenValidationParameters: Cria uma serie de outros parametros:

  - ValidateIssuerSigningKey: Valida se quem esta emitindo é o mesmo que está no token.(baseada nome do Issuer e na chave).
  - IssuerSigningKey: configura a chave, transforma de asp2 para uma chave criptografada.
  - ValidateIssuer: valida apenas o Ussuer conforme o nome.
  - ValidateAudience: aonde o token é valido em qual Audience.
  - ValidAudience: Informa qual é o "Audience"
  - ValidIssuer: Informa qual é o "Essuer"


 - Configuração completa

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

### Configurando o controller auth (Devolvendo o token que foi gerado)

 - O cadastro tendo sucesso, deve gerar o token e devolver para o client,
 isso na action de registro.
  
<blockquete>

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);               

                // Gerando o token e devolve ele !
                return CustomResponse(await GerarJwt(user.Email));
            }

</blockquete>

### Injetando classe com "IOptions".

 - Injeta a classe "AppSettings" usando o "IOptions".
   - Cria a propriedade "private readonly AppSettings _appSettings;"
   - No paramtro passa o "IOptions<AppSettings> appSettings,"
   - Dentro do construtor alimenta a propriedade " _appSettings = appSettings.Value;"

### Criando o método que gera o token e devolve pro client.

 - Cria um método com nome de "GerarJwt", dentro no método aplica os comandos.
 
 - instancia a classe "JwtSecurityTokenHandler" e guarda na variavel TokenHandler.
 
<blockquete>

            var tokenHandler = new JwtSecurityTokenHandler();

</blockquete>

 - Antes de criar a chave de criptografia, devemos receber o appSettings,
 injetado no controlador.

 - Essa injeção de dependencia é feita de uma forma diferente, é usando o "IOptions",
 esse "IOptions" é usado para pegar dados usado de parametros.

 - Por isso que no construtor é possivel passar o valor do objeto.

<blockquete>

        _appSettings = appSettings.Value;

</blockquete

 - Cria a chave de criptografia: 
  
<blockquete>

          var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

</blockquete>

 - Gerando um token, Nessa etapa você está alimentando a configuração 
 do token, e criando ele.

    - Issuer: indica o emissor que está no "_appSettings";

    - Audience: informa o ValidoEm, que está no "_appSettings";

    - Expires: a expiração, quanto tempo vai ser valido, é 
    usado o " DateTime.UtcNow.AddHours" para usar a hora da local.

    - SigningCredentials: cria uma instancia de "SigningCredentials()" e ,
    Passando como parametro uma instancia de "SymmetricSecurityKey" nessa instancia é 
    passado o "key" como parametro. o segundo parameto é "SecurityAlgorithms.HmacSha256Signature"

    - SecurityAlgorithms.HmacSha256Signature: é um algoritimo de criptografia que vai ser usado.

<blockquete>

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
         

</blockquete>

 - Escreva o token, cria uma variavel chamada "encodedToken", chama o método "tokenHandler.WriteToken()",
 que recebe o "token" que foi configurado.
 
 Esse método deixando o token com padrão da web.

<blockquete>
         
            var encodedToken = tokenHandler.WriteToken(token);

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


 -


<blockquete>

         

</blockquete>

-
-
-
-


<blockquete>


</blockquete>

-
-
-
-


<blockquete>


</blockquete>

-
-
-
-


<blockquete>


</blockquete>

-
-
-
-


<blockquete>


</blockquete>

-
-
-
-


<blockquete>


</blockquete>

-
-
-
-


<blockquete>


</blockquete>

-
-
-
-


<blockquete>


</blockquete>







