using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Infra.Messages.Models;

namespace UsersAPI.Infra.Messages.Helpers
{
    public class MailHelper
    {
        #region Atributos

        private readonly string _host = "localhost";
        private readonly int _port = 1025;
        private readonly string _from = "noreply@example.com"; 

        #endregion

        public void SendMail(RegisteredUser user)
        {
            //escrevendo o assunto do email
            var subject = "Conta de usuário criada com sucesso - COTI Informática";

            //escrevendo o corpo do email
            var body = @$"
                <div style='font-family: Verdana, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                    <div style='max-width: 600px; background-color: #ffffff; padding: 20px; border-radius: 8px; margin: auto; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);'>
                        <img src='https://www.cotiinformatica.com.br/imagens/logo-coti-informatica.svg' 
                                alt='COTI Informática' 
                                style='max-width: 200px; margin-bottom: 20px;' />

                        <h2 style='color: #333;'>Olá {user.Name},</h2>
                        <p style='font-size: 16px; color: #666;'>Sua conta foi criada com sucesso! 🎉</p>

                        <div style='background-color: #f0f0f0; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p style='font-size: 16px; color: #333;'><strong>Seja bem-vindo ao sistema COTI Informática!</strong></p>
                            <p style='font-size: 14px; color: #555;'>O seu perfil de acesso é <strong>{user.Role}</strong>.</p>
                        </div>

                        <p style='font-size: 14px; color: #777;'>Se precisar de ajuda, entre em contato com nosso suporte.</p>

                        <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;' />

                        <p style='font-size: 12px; color: #999;'>Atenciosamente,</p>
                        <p style='font-size: 14px; font-weight: bold; color: #333;'>Equipe COTI Informática</p>
                    </div>
                </div>";


            //Criando o objeto que fará o envio dos emails
            var smtpClient = new SmtpClient(_host, _port)
            {
                EnableSsl = false
            };

            //Configurando o remetente, destinatário, assunto e corpo da mensagem
            var mailMessage = new MailMessage(_from, user.Email, subject, body);

            //Configurando o corpo da mensagem para HTML
            mailMessage.IsBodyHtml = true;

            //enviando a mensagem
            smtpClient.Send(mailMessage);
        }
    }
}
