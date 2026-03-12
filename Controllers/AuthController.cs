using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VivaAguascalientesAPI.Core;
using VivaAguascalientesAPI.DAO;
using VivaAguascalientesAPI.DAO.Model;
using VivaAguascalientesAPI.DTO.RequestDTO;
using VivaAguascalientesAPI.DTO.ResponseDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VivaAguascalientesAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController, Produces("application/json")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration )
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Autentificación al sistema
        /// </summary>
        /// <param name="request">Parametros para el login</param>
        /// <response code="200">Usuario con acceso</response>
        /// <response code="401">Credenciales invalidas</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Post([FromBody] AuthRequest request)
        {
            var dataParams = new {
                Contrasenia=request.Password,
                Usuario=request.Usuario
            };
            var dataAccess = new DataAccessModelService();
            var result = dataAccess.GetListModel<AuthLoginModel, object>("[dbo].[sp_AT_Login]", dataParams);
            if (result.Count == 1)
            {
                var userclaim = new[] {
                    new Claim(ClaimTypes.Name, request.Usuario),
                    new Claim(ClaimTypes.Sid, ""+result.First().Id)
                };
                var token = getToken(userclaim);

                var refresh = RefreshToken(result.First().Id, token.RefreshToken);

                if (!refresh)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                    {
                        Title = "No se pudo registrar el refresh token",
                        Status = StatusCodes.Status500InternalServerError
                    });
                }
                return Ok(token);
            }
            return Unauthorized(new ProblemDetails
            {
                Title = "Credenciales invalidas",
                Detail = "El usuario o la contrasena no coinciden.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        /// <summary>
        /// Restablece el token vencido para el usuario
        /// </summary>
        /// <param name="refreshToken">Token para validar si se puede o no restablecer el JWT</param>
        /// <response code="200">Nuevos accesos JWT y Refresh token</response>
        /// <response code="404">Accesos no concedidos, problemas con refresh token no valido</response>
        [HttpGet("{refreshToken}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAsync(string refreshToken)
        {
            string accessToken = Request.Headers["Authorization"];
            if (accessToken == null || accessToken.Equals(""))
            {
                return NotFound();
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(accessToken.Replace("Bearer ", "")) as JwtSecurityToken;
            var user = int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
            Claim[] claims = jwtToken.Claims.ToArray();

            var dataParams = new
            {
                IdUsuario = user,
                RefreshToken = refreshToken
            };

            var dataAccess = new DataAccessModelService();
            var result = dataAccess.ExecuteNonQueryModel<object>("[dbo].[sp_AT_ValidaToken]", dataParams);

            if (result != 200)
            {
                return NotFound();
            }
            var token = getToken(claims);

            var refresh = RefreshToken(user, token.RefreshToken);
            if (!refresh)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        /// <summary>
        /// Se genera token y se envia el enlace para generar la nueva contraseña
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("forget")]
        [AllowAnonymous]
        public ActionResult ForgetPassword([FromBody] ForgetAuthRequest request)
        {
            // Buscar si el correo existe
            var dataParams = new
            {
                Usuario = request.Usuario
            };
            var dataAccess = new DataAccessModelService();

            IList<AuthLoginModel> result = dataAccess.GetListModel<AuthLoginModel, object>("[dbo].[sp_AT_UsuarioPorCorreo]", dataParams);
            if (result.Count == 0)
            {
                return NotFound();
            }
            var user = result.First();

            // TODO: Si existe generar token de reset

            dataAccess = new DataAccessModelService();
            var token = Guid.NewGuid().ToString();

            int dataResult = dataAccess.ExecuteNonQueryModel<object>("[sp_AT_ResetToken]", new
            {
                IdUsuario = user.Id,
                Token = token
            });
            // TODO: Generar correo adecuado para el token con la URL
            CorreoCore correo = new CorreoCore(_configuration["Mail:servidor"], _configuration["Mail:cuenta"], _configuration["Mail:clave"]);
            var para = new string[]
            {
                request.Usuario
            };

            var linkRecovery = $"Da click en el enlace para recuperar tu contraseña {_configuration["Site"]}recovery/{token}";
            correo.enviar("Recuperar contraseña", linkRecovery, "juan.flores@aguascalientes.gob.mx", para, null, null, null);
            return NoContent();
        }

        [HttpGet("validate/{resetToken}")]
        [AllowAnonymous]
        public ActionResult ValidateResetToken(string resetToken)
        {
            var dataParams = new {
                Token = resetToken,
            };

            var dataAccess = new DataAccessModelService();

            var resets = ResetTokenModel.GetTokenBy(resetToken);
            if (resets.Count == 0)
            {
                return NotFound();
            }
            var reset = resets.First();

            return Ok(reset);
        }

        /// <summary>
        /// Restablece una nueva contraseña de un usuario.
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <param name="resetCode">Token de reseteo</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset")]
        public ActionResult ResetPassword([FromBody] ResetAuthRequest request)
        {
            if (String.IsNullOrEmpty(request.ResetCode) || String.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }

            var dataParams = new
            {
                IdUsuario = request.Id,
                Contrasenia = request.Password,
                Token = request.ResetCode,
            };

            var dataAccess = new DataAccessModelService();
            var result = dataAccess.ExecuteNonQueryModel<object>("[sp_AT_ModificarContrasenia]", dataParams);

            if (result != 200)
            {
                return BadRequest();
            }

            return Ok();
        }

        private string ResetToken(int id)
        {
            var reset = new
            {
                IdUsuario = id,
                Token = Guid.NewGuid().ToString(),
                LifeTime = new DateTime().AddHours(2),
            };
            var dataAccess = new DataAccessModelService();
            // TODO: Cambiar el SP por el que registre el reset token
            var dataResult = dataAccess.ExecuteNonQueryModel<object>("[sp_AT_RefreshToken]", reset);
            return reset.Token;
        }

        private void SendEmail(string toEmail, string subject, string content)
        {
            SmtpClient client = new SmtpClient();
        }

        /// <summary>
        /// Agrega el refresh token a la base de datos relacionandolo con el usuario
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <param name="refreshToken">Objeto token </param>
        /// <returns></returns>
        private bool RefreshToken(int id, string refreshToken)
        {
            var dataAccess = new DataAccessModelService();
            var dataResult = dataAccess.ExecuteNonQueryModel<object>("[sp_AT_RefreshToken]", new
            {
                IdUsuario = id,
                RefreshToken = refreshToken
            });
            return true;
        }

        /// <summary>
        /// Genera un objeto con el token
        /// </summary>
        /// <param name="claims">Claims para el token</param>
        /// <returns></returns>
        private TokenResponse getToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:secretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:issuer"],
                    audience: _configuration["JWT:audience"],
                    claims: claims,
                    //expires: DateTime.Now.AddMinutes(1),
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var refreshToken = Guid.NewGuid().ToString();
            return new TokenResponse
            {
                RefreshToken = refreshToken,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
