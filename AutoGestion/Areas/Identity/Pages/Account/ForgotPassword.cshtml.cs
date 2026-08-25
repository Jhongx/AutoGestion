// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using AutoGestion.Data;

namespace AutoGestion.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme)!;

            // Plantilla HTML moderna y profesional
            var resetLink = HtmlEncoder.Default.Encode(callbackUrl);
            var htmlMessage = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f6f9; margin: 0; padding: 0; }}
                        .email-container {{ max-width: 600px; margin: 30px auto; background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05); }}
                        .email-header {{ background: #212529; color: #ffffff; padding: 20px; text-align: center; }}
                        .email-body {{ padding: 30px; color: #333333; line-height: 1.6; }}
                        .btn-reset {{ display: inline-block; background-color: #0d6efd; color: #ffffff !important; text-decoration: none; padding: 12px 25px; border-radius: 5px; font-weight: bold; margin: 20px 0; }}
                        .email-footer {{ background: #f8f9fa; padding: 15px; text-align: center; font-size: 12px; color: #6c757d; }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <div class=""email-header"">
                            <h2>AutoGestión</h2>
                        </div>
                        <div class=""email-body"">
                            <p>Hola,</p>
                            <p>Has solicitado restablecer tu contraseña para tu cuenta en <strong>AutoGestión</strong>.</p>
                            <p>Haz clic en el siguiente botón para continuar con el proceso:</p>
                            <div style=""text-align: center;"">
                                <a href=""{resetLink}"" class=""btn-reset"">Restablecer Contraseña</a>
                            </div>
                            <p><small>Si no solicitaste este cambio, puedes ignorar este mensaje de manera segura.</small></p>
                        </div>
                        <div class=""email-footer"">
                            &copy; {DateTime.Now.Year} AutoGestión. Todos los derechos reservados.
                        </div>
                    </div>
                </body>
                </html>";

            try
            {
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Restablece tu contraseña - AutoGestión",
                    htmlMessage);
            }
            catch (Exception ex)
            {
                // Punto de interrupción en caso de error en el envío
                Console.WriteLine(ex.Message);
            }

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        return Page();
    }
}
