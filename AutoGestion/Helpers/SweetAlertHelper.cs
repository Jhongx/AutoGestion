using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace AutoGestion.Helpers
{
    public static class SweetAlertHelper
    {
        public static void ShowAlert(ITempDataDictionary tempData, string icon, string title, string text, string? confirmButtonText = null, string? redirectUrl = null, bool isToast = false)
        {
            var options = new
            {
                icon = icon, // 'success', 'error', 'warning', 'info', 'question'
                title = title,
                text = text,
                toast = isToast,
                position = isToast ? "top-end" : "center",
                showConfirmButton = !isToast || confirmButtonText != null,
                confirmButtonText = confirmButtonText ?? "Aceptar",
                timer = isToast ? 3500 : (string.IsNullOrEmpty(redirectUrl) ? (icon == "success" ? 3500 : (int?)null) : null),
                timerProgressBar = isToast || icon == "success",
                redirectUrl = redirectUrl // URL opcional si quieres redirigir al hacer clic o confirmar
            };

            tempData["SwalConfig"] = JsonSerializer.Serialize(options);
        }

        // Método específico para confirmaciones con botón de "Sí/Cancelar" y redirección condicional
        public static void ShowConfirm(ITempDataDictionary tempData, string title, string text, string confirmButtonText, string cancelButtonText, string confirmRedirectUrl, string cancelRedirectUrl)
        {
            var options = new
            {
                icon = "question",
                title = title,
                text = text,
                showCancelButton = true,
                confirmButtonText = confirmButtonText,
                cancelButtonText = cancelButtonText,
                confirmButtonColor = "#198754", // Verde éxito para la acción positiva
                cancelButtonColor = "#6c757d",  // Gris secundario para cancelar
                redirectUrl = confirmRedirectUrl,
                cancelRedirectUrl = cancelRedirectUrl
            };

            tempData["SwalConfig"] = JsonSerializer.Serialize(options);
        }
    }
}
