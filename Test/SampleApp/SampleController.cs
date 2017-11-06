using Microsoft.AspNetCore.Mvc;
using Moses.Configuration;
using Moses.Extensions;
using Moses.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Moses.Test.SampleApp
{
    public class SampleController : Controller
    {
        private readonly IEmailService _email;
        private readonly MosesAppConfiguration _config;


        public SampleController(IEmailService email, MosesServiceOptions service)
        {
            _email = email;
            _config = service.AppConfiguration;
        }

        public ActionResult Configuration()
        {
            return Json(_config);
        }

        public ActionResult SendTestEmail(string nome, string email, string fone, string mensagem, string noneField)
        {
            try
            {
                // Validação de dados
                if (nome == "" || !email.IsValidEmailAddress() || mensagem == "" || !string.IsNullOrEmpty(noneField))
                {
                    return Json(new { success = false, msg = "Mensagem Inválida" });
                }

                // Mensagem do E-mail simples
                var txtEmailSimples = string.Format(@"Nome: {0}\n
	                            E-mail: {1}\n
	                            Fone: {2}\n
						        Mensagem: {3}\n", nome, email, fone.GetPhoneNumber(), mensagem);

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress( "dev@exodus.eti.br", "Site Finanças (TEST)");
                msg.To.Add(new MailAddress( "dev@exodus.eti.br", "Exodus Finanças(TEST)"));

                // Assunto e Corpo do email
                msg.Subject = "Mensagem do Site (TEST)";
                msg.IsBodyHtml = true;
                msg.Body = txtEmailSimples;

                _email.SendAsync(msg);


                //------------------------------- MONTAR MENSAGEM QUE SERÁ ENVIADA AO CLIENTE --------------------------//
                var rspMensagem = string.Format(@"Olá <strong>{0}</strong>,<br /><br />
		            Acabamos de receber seu email e entraremos em contato o mais breve possivel.<br /><br />
		            Sinta-se a vontade para entrar em contato pelos nossos telefones, ou via <a href='http://site.exodusfinancas.com.br/chat/livezilla.php?intid=bGVvbmFyZG8_'>atendimento online</a>.
		            <br /><br />
		            Atenciosamente,
		            <br /><br />
		            Olavo Neto<br />
		            Diretor Comercial
		            <br /><br />
		            <a href='http://www.exodusfinancas.com.br/'>www.exodusfinancas.com.br</a><br />
		            <a href='mailto:contato@exodusfinancas.com.br'>contato@exodusfinancas.com.br</a><br />
		            (91) 4040-4243<br />", nome);



                // Configuração de SMTP
                MailMessage rspMsg = new MailMessage();

                rspMsg.From = new MailAddress("site@exodusfinancas.com.br","Site Exodus Finanças");
                rspMsg.To.Add(new MailAddress(email,nome));

                // Assunto e Corpo do email
                rspMsg.Subject = "Exodus Finanças | Confirmação de recebimento";
                rspMsg.IsBodyHtml = true;
                rspMsg.Body = rspMensagem;

                //rspMsg.SendMail();
                LogService.Log.LogInformation("Teste");
                return Json(new { success = true, msg = "Mensagem enviada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "Erro no processamento do e-mail. O administrador do site foi notificado." });
            }
        }
    }
}
