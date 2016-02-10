using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Moses.Web.Controls
{
	/// <summary>
	/// MosesTextBox para simplificar a validação de dados.
	/// </summary>
	[ToolboxData("<moses:MosesTextBox runat=\"server\"></moses:MosesTextBox>")]

	public class MosesTextBox : System.Web.UI.WebControls.TextBox
	{
		// Definição da Propriedade de Máscara:
		public enum tpMascara
		{
            None,
			CPF,
			CNPJ,
            CPFCNPJ,
			TelefoneNacional,
			TelefoneInternacional,
			DataDigitada,
			HoraMinuto,
			NumeroDecimal,
			NumeroInteiro,
            NumeroInteiroPositivo,
            Moeda,
			CEP
		}

        /// <summary>
        /// Get. Faz o Parse para Int32. Caso tenha sucesso retornará o valor convertido senão retornará 0 (zero).
        /// </summary>
        public int? IntValue
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    return Convert.ToInt32(base.Text);
                }

                return null;
            }
        }

        public DateTime? DateTimeValue
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    return Convert.ToDateTime(base.Text);
                }

                return null;
            }
        }

        public decimal? DecimalValue
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    return Convert.ToDecimal(base.Text);
                }

                return null;
            }
        }

        public bool? BooleanValue
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    return Convert.ToBoolean(base.Text);
                }

                return null;
            }
        }

		tpMascara vMascara;
		public tpMascara Mascara
		{
			get
			{
				return vMascara;
			}
			set
			{
				vMascara = value;
			}
		}
		// Fim da Definição da Propriedade de Máscara

		// Definição da Propriedade Validar
		// Valida dados em JavaScript.
		public bool bValidar = false;
		public bool Validar
		{
			get
			{
				return bValidar;
			}
			set
			{
				bValidar = value;
			}
		}

		// Definição da Propriedade ValidarMensagem
		// Caso o campo seja validado, Exibir esta Mensagem:
		public string sValidarMensagem = "Este Campo é Obrigatório!";
		public string ValidarMensagem
		{
			get
			{
				return sValidarMensagem;
			}
			set
			{
				sValidarMensagem = value;
			}
		}

		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
		{
			switch (Mascara)
			{
				// None
				case tpMascara.None:
					// Não faz nada!
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; }");
					}
					break;
				// CPF
				case tpMascara.CPF:
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraCPF(this); } else { return false; }");
					base.MaxLength = 14;
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; } else if (!ValidaCPF(this.value)) { alert('CPF Inválido!'); return false; }");
					}
					break;
				// CNPJ
				case tpMascara.CNPJ:
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraCNPJ(this, event); } else { return false; }");
					base.MaxLength = 18;
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; } else if (!ValidaCNPJ(this.value)) { alert('CNPJ Inválido!'); return false; }");
					}
					break;
                // CPFCNPJ
                case tpMascara.CPFCNPJ:
                    writer.AddAttribute("OnKeyUp", "javascript: if (SomenteNumero(event, '', '')) { MascaraCPFCNPJ(this, event); } else { return false; }");
                    base.MaxLength = 18;
                    if (Validar)
                    {
                        writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + ValidarMensagem + "'); return false; }"
                        + "else if (!ValidaCPF(this.value) && this.value.length == 14) { alert('CNPJ Inválido!'); return false;"
                        + "else if (!ValidaCNPJ(this.value) && this.value.length == 18) { alert('CNPJ Inválido!'); return false; }");
                    }
                    break;
				// Telefone Nacional
				case tpMascara.TelefoneNacional:
					base.MaxLength = 13;
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, ' ', '')) { MascaraTelefoneNacional(this); } else { return false; }");
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; }");
					}
					break;
				// TelefoneInternacional
				case tpMascara.TelefoneInternacional:
					base.MaxLength = 30;
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, ' ', '-')) { MascaraTelefoneInternacional(this); } else { return false; }");
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; }");
					}
					break;
				// DataDigitar
				case tpMascara.DataDigitada:
					base.MaxLength = 10;
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraData(this); } else { return false; }");
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { return false; } else if (!ValidaData(this.value)) { return false; }");
						//Com a mensgem de validação, q ainda não funciona:
						//writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; } else if (!ValidaData(this.value)) { return false; }");
					}
					break;
				// HoraMinuto
				case tpMascara.HoraMinuto:
					base.MaxLength = 5;
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraHoraMinuto(this); } else { return false; }");
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; } else if (!ValidaHoraMinuto(this.value)) { return false; }");
					}
					break;
				// NumeroDecimal
				case tpMascara.NumeroDecimal:
					writer.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', ',')) { return false; }");
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; }");
					}
					break;
				// NumeroInteiro
				case tpMascara.NumeroInteiro:
					writer.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', '-')) { return false; }");
					if (Validar)
					{
						writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; }");
					}
					break;
                // NumeroInteiroPositivo
                case tpMascara.NumeroInteiroPositivo:
                    writer.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', '')) { return false; }");
                    if (Validar)
                    {
                        writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + ValidarMensagem + "'); return false; }");
                    }
                    break;
				// Moeda
				case tpMascara.Moeda:
                    writer.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event,'','')) { return false; }");

                    //Esse if foi colocado aqui para os casos em que se queira utilizar esse evento na página. Conforme a necessidade, pode-se adicionar para os outros eventos.
                    if (this.Attributes["onKeyUp"] == null)
                    {
                        writer.AddAttribute("onKeyUp", "MascaraValor(event, this)");
                    }
                    else
                    {
                        writer.AddAttribute("onKeyUp", "MascaraValor(event, this);" + this.Attributes["onKeyUp"]);
                    }

                    if (Validar)
                    {
                        writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + ValidarMensagem + "'); return false; }");
                    }
                    break;
				// CEP
				case tpMascara.CEP:
					base.MaxLength = 9;
					writer.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event,'','')) { MascaraCEP(this); } else { return false; }");
                    if (Validar)
                    {
                        writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + ValidarMensagem + "'); return false; }");
                    }
					break;
			}

			// Renderizar Scripts Elementos
			base.AddAttributesToRender(writer);
		}
	}
}
