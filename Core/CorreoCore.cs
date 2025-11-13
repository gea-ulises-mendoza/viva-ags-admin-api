using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace VivaAguascalientesAPI.Core
{
    public class CorreoCore
    {
        private SmtpClient servidorCorreo;
        private MailMessage correo;

        private string servidor;
        private string cuenta;
        private string clave;

        public CorreoCore(string servidor, string cuenta, string clave)
        {
            this.servidor = "https://autodiscover.aguascalientes.gob.mx";//servidor;
            this.cuenta = "GOBAGS\\ofma.apps";//cuenta;
            this.clave = "0fma4P9s";//clave;

            servidorCorreo = new SmtpClient();
            servidorCorreo.Host = this.servidor;
            servidorCorreo.Credentials = new System.Net.NetworkCredential(this.cuenta, this.clave);
            servidorCorreo.EnableSsl = false;
        }

        public bool enviar(string asunto, string cuerpo, string de, string[] para, string[] copia, string[] copia_oculta, Dictionary<string, byte[]> ldictionary)
        {
            correo = new MailMessage();
            correo.From = new System.Net.Mail.MailAddress(de);

            if (!(ldictionary == null))
            {
                foreach (KeyValuePair<string, byte[]> d in ldictionary)
                {
                    if (d.Value != null)
                    {
                        correo.Attachments.Add(new Attachment(new MemoryStream(d.Value), d.Key));
                    }
                }
            }

            if (!(para == null))
            {
                for (short indice = 0; indice < para.Length; indice++)
                {
                    if (!string.IsNullOrEmpty(para[indice]))
                    {
                        correo.To.Add(para[indice]);
                    }
                }
            }

            if (!(copia == null))
            {
                for (short indice = 0; indice < copia.Length; indice++)
                {
                    if (!string.IsNullOrEmpty(copia[indice]))
                    {
                        correo.CC.Add(copia[indice]);
                    }
                }
            }

            if (!(copia_oculta == null))
            {
                for (short indice = 0; indice < copia_oculta.Length; indice++)
                {
                    if (!string.IsNullOrEmpty(copia_oculta[indice]))
                    {
                        correo.Bcc.Add(copia_oculta[indice]);
                    }
                }
            }

            correo.Subject = asunto;
            correo.Body = cuerpo;
            correo.IsBodyHtml = true;
            correo.Priority = System.Net.Mail.MailPriority.Normal;
            try
            {
                servidorCorreo.Send(correo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
