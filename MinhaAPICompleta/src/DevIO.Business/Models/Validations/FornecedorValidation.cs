using DevIO.Business.Models.Validations.Documentos;
using FluentValidation;
    

namespace DevIO.Business.Models.Validations
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {
        public FornecedorValidation()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(min: 2, max: 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            // Quando ele for uma pessoa fisica.
            When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, action:() =>
            {
                // Caso o tamanho do documento não seja igual ao valor da propriedade, retorna a mensagem.
                RuleFor(f => f.Documento.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                
                // Caso o resultado do método não seja verdadeiro, retorna a mensagem.
                RuleFor(f => CpfValidacao.Validar(f.Documento)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });

            // Quando ele for uma pessoa Juridica.
            When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, action:() =>
            {
                // Caso o tamanho do documento não seja igual ao valor da propriedade, retorna a mensagem.
                RuleFor(f => f.Documento.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                // Caso o resultado do método não seja verdadeiro, retorna a mensagem.
                RuleFor(f => CnpjValidacao.Validar(f.Documento)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }

    }
}
