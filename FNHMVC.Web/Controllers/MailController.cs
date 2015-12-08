using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net.Mvc;
using FNHMVC.Domain.Commands;
using FNHMVC.Model;
using FNHMVC.Web.Models;

namespace FNHMVC.Web.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult VerificationEmail(TokenVerificationMail model)
        {
            try
            {
                To.Add(model.Email);
                From = "info@lexstore.com";
                Subject = "Bienvenido a goMarket!";
                return Email("VerificationEmail", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("VerificationEmail", ex);
                return Email("");
            }
        }

        public EmailResult SendUserPassword(User model)
        {
            try
            {
                To.Add(model.Email);
                From = "info@lexstore.com";
                Subject = "Bienvenido a goMarket!";
                return Email("SendUserPassword", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendUserPassword", ex);
                return Email("");
            }
        }

        public EmailResult SendUserAccountDelete(User model)
        {
            try
            {
                To.Add(model.Email);
                From = "info@lexstore.com";
                Subject = "Tu cuenta ha sido eliminada!";
                return Email("SendUserAccountDelete", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendUserAccountDelete", ex);
                return Email("");
            }
        }

        public EmailResult SendEmailDenyChange(Sale model)
        {
            try
            {
                To.Add(model.User.Email);
                From = "info@lexstore.com";
                Subject = "Publicación denegada!";
                return Email("SendEmailDenyChange", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendEmailDenyChange", ex);
                return Email("");
            }
        }



        public EmailResult SendUserAccountLock(User model)
        {
            try
            {
                To.Add(model.Email);
                From = "info@lexstore.com";
                Subject = "Tu cuenta ha sido bloqueada!";
                return Email("SendUserAccountLock", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendUserAccountLock", ex);
                return Email("");
            }
        }

        public EmailResult SendUserPurchaseCompleted(User model)
        {
            try
            {
                To.Add(model.Email);
                From = "info@lexstore.com";
                Subject = "Tú compra fue completada con éxito!";
                return Email("SendUserPurchaseCompleted", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendUserPurchaseCompleted", ex);
                return Email("");
            }
        }

        public EmailResult ResetPasswordEmail(TokenResetPasswordMail model)
        {
            try
            {
                To.Add(model.Email);
                From = "info@lexstore.com";
                Subject = "Debe restablecer la contraseña para mayor seguridad!";
                return Email("ResetPasswordEmail", model);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("ResetPasswordEmail", ex);
                return Email("");
            }
        }

        public EmailResult SendEmailToClients(User model, CreateOrUpdateSaleCommand command)
        {
            try
            {
                foreach (var follower in model.Followers)
                {
                    To.Add(follower.User.Email);
                }
                From = "info@lexstore.com";
                Subject = "Nueva publicación de " + model.FirstName + " " + model.LastName;
                return Email("SendEmailToClients", command);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendEmailToClients", ex);
                return Email("");
            }
        }


        public EmailResult SendEmailToSeller(CreateOrUpdateUserInboxCommand command)
        {
            try
            {
                To.Add(command.Seller.Email);

                From = "info@lexstore.com";
                Subject = command.Subject;
                return Email("SendEmailToSeller", command);
            }
            catch (Exception ex)
            {
                Helpers.LoggerManager.WriteError("SendEmailToSeller", ex);
                return Email("");
            }
        }
    }

    public enum MailOperationType
    {
        RegisterAccount = 1,
        ResetPassword = 4
    }
}
