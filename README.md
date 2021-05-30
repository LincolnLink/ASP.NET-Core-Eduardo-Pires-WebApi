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

 - 

 - 

 
 <blockquete>

 </blockquete>

 
 <blockquete>

 </blockquete>

 
 <blockquete>

 </blockquete>


 
 <blockquete>

 </blockquete>

 
 <blockquete>

 </blockquete>

 
 <blockquete>

 </blockquete>

 
 <blockquete>

 </blockquete>







