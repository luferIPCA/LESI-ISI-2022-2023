/** 
 * XML Web Services
 * Cliente de um Serviço
 * lufer & Oscar
 * 2022-2023
 */
using System;
using System.Net.Mail;
using System.Web.Services;

namespace EmailValidator
{
    /// <summary>
    /// Summary description for EmailValid
    /// </summary>
    [WebService(Namespace = "http://www.ipca.pt/", Description ="Validação sintática de email")]
    public class EmailValid : System.Web.Services.WebService
    {
        [WebMethod(Description ="Valida sintaticamente um endreeço de email")]
        public bool EmailValidator(string email)
        {

            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        
    }
}
