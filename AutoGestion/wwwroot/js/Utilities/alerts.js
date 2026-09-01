// wwwroot/js/alerts.js
document.addEventListener("DOMContentLoaded", function () {
    if (typeof window.swalConfigData !== 'undefined' && window.swalConfigData !== null) {
        const config = window.swalConfigData;

        let swalOptions = {
            icon: config.icon || 'success',
            title: config.title || '',
            text: config.text || '',
            toast: config.toast || false,
            position: config.position || 'center',
            showConfirmButton: config.showConfirmButton !== undefined ? config.showConfirmButton : true,
            showCancelButton: config.showCancelButton || false,
            confirmButtonText: config.confirmButtonText || 'Aceptar',
            cancelButtonText: config.cancelButtonText || 'Cancelar',
            confirmButtonColor: config.confirmButtonColor || '#0d6efd',
            cancelButtonColor: config.cancelButtonColor || '#6c757d',
            timer: config.timer,
            timerProgressBar: config.timerProgressBar || false
        };

        Swal.fire(swalOptions).then((result) => {
            if (result.isConfirmed) {
                // Si el usuario aceptó y hay una URL configurada
                if (config.redirectUrl) {
                    window.location.href = config.redirectUrl;
                }
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                // Si el usuario canceló y hay una URL alternativa para cancelar
                if (config.cancelRedirectUrl) {
                    window.location.href = config.cancelRedirectUrl;
                }
            } else {
                // Comportamiento por defecto (si se cerró por timer)
                if (config.redirectUrl && config.toast) {
                    window.location.href = config.redirectUrl;
                }
            }
        });
    }
});