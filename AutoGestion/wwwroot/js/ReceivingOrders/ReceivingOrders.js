document.addEventListener("DOMContentLoaded", function () {
    const vehicleSelect = document.getElementById("vehicleSelect");
    const clientNameText = document.getElementById("clientNameText");
    const clientPhoneText = document.getElementById("clientPhoneText");

    if (!vehicleSelect) return;

    vehicleSelect.addEventListener("change", function () {
        const vehicleId = this.value;

        if (vehicleId) {
            // Petición AJAX al handler del Razor Page
            fetch(`?handler=VehicleClientInfo&vehicleId=${vehicleId}`)
                .then(response => {
                    if (!response.ok) throw new Error("Error en la respuesta del servidor");
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        clientNameText.textContent = `Propietario: ${data.clientName}`;
                        clientPhoneText.textContent = `Teléfono: ${data.phone}`;
                    } else {
                        clientNameText.textContent = "Cliente no encontrado";
                        clientPhoneText.textContent = "---";
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    clientNameText.textContent = "Error al cargar datos del cliente";
                    clientPhoneText.textContent = "---";
                });
        } else {
            clientNameText.textContent = "Seleccione un vehículo para ver el propietario";
            clientPhoneText.textContent = "---";
        }
    });

    // Ejecutar al cargar por si la página inicia con un valor preseleccionado (al Editar o fallar validación)
    if (vehicleSelect.value) {
        vehicleSelect.dispatchEvent(new Event("change"));
    }
});