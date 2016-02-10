using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Moses.Extensions
{
   
    public class CpfCnpjAttribute : ValidationAttribute
    {
        public CpfCnpjAttribute ()
            : base("CPF/CNPJ inválido.")
	    {

	    }

        public bool CpfOnly { get; set; }
        public bool CnpjOnly { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string s = value as string;
             if ( s != null){
                if ( s == null) return new ValidationResult("O valor de entrada não é uma string válida");
                if ( ( s.IsValidCpf() || !CnpjOnly) || (s.IsValidCnpj() || !CpfOnly) )
 	                return ValidationResult.Success;
                return new ValidationResult(null);
             }
             else
                 return new ValidationResult("O valor de entrada não é uma string válida");
        }

    }

    
    public class CepAttribute : ValidationAttribute
    {
        public CepAttribute()
            : base("CPF/CNPJ inválido.")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string s = value as string;
            if (!s.IsNullOrEmpty())
            {
                if (s.IsFormatedCep())
                    return ValidationResult.Success;
                return new ValidationResult(null);
            }
            else
            {
                if (s == null) return new ValidationResult("O Cep não está em um formato válido");
                return new ValidationResult("O valor de entrada não é uma string válida");
            }
        }

    }

    
    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute() : base("Email Inválido") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string s = value as string;
            if ( s != null){
                if ( s.IsValidEmailAddress() )
 	                return ValidationResult.Success;
                return new ValidationResult(null);
            }
            else
                return new ValidationResult("O valor de entrada não é uma string válida");
        }
    }
    
}
