document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    if (!calendarEl) return;

    // Normalizar eventos para evitar que abarquen múltiples días si son de un solo día
    var rawEvents = JSON.parse(calendarEl.dataset.events || '[]').map(function (evt) {
        // Copiar objeto para no mutar el original
        var cleanEvt = Object.assign({}, evt);

        // Si no es un rango real de días, omitir 'end' para evitar solapamiento de 2 días en FullCalendar
        if (cleanEvt.start && cleanEvt.end) {
            var startDate = cleanEvt.start.split('T')[0];
            var endDate = cleanEvt.end.split('T')[0];
            if (startDate === endDate) {
                delete cleanEvt.end; // Al quitar 'end', la vista List lo renderiza SOLO en su día
            }
        }
        cleanEvt.allDay = true; // Forzar a que sea de día completo si no usa horas estrictas
        return cleanEvt;
    });

    var selectedDayEl = null;

    function renderSidePanel(dateStr) {
        var listContainer = document.getElementById('sidePanelOrdersList');
        if (!listContainer) return;

        listContainer.innerHTML = '';
        var dateParts = dateStr.split('-');
        var formattedDate = `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;

        var badgeEl = document.getElementById('selectedDateBadge');
        if (badgeEl) badgeEl.innerText = formattedDate;

        // Filtrar coincidencia exacta de fecha
        var dayEvents = rawEvents.filter(e => e.start && e.start.startsWith(dateStr));

        if (dayEvents.length === 0) {
            listContainer.innerHTML = `
                <div class="text-center py-4 text-muted">
                    <i class="bi bi-calendar-x fs-1 d-block mb-2 text-secondary"></i>
                    <p class="mb-0 fw-semibold">Sin recepciones programadas</p>
                    <small class="text-muted">Selecciona un día en el calendario.</small>
                </div>`;
            return;
        }

        dayEvents.forEach(function (evt) {
            var props = evt.extendedProps || {};
            var cardHtml = `
                <div class="card border border-light-subtle shadow-sm rounded-3 overflow-hidden" style="border-left: 4px solid ${evt.color || '#0d6efd'} !important;">
                    <div class="card-body p-3">
                        <div class="d-flex justify-content-between align-items-start mb-2">
                            <span class="badge bg-dark bg-opacity-10 text-dark fw-bold">
                                <i class="bi bi-clock me-1"></i>${props.time || 'Todo el día'}
                            </span>
                            <span class="badge" style="background-color: ${evt.color || '#0d6efd'};">
                                ${props.serviceType || 'Servicio'}
                            </span>
                        </div>
                        <h6 class="fw-bold text-dark mb-1">${props.fullTitle || evt.title}</h6>
                        <div class="small text-muted mb-3">
                            <div class="mb-1"><i class="bi bi-person me-1 text-primary"></i><strong>Cliente:</strong> ${props.client || 'N/A'}</div>
                            <div><i class="bi bi-car-front me-1 text-primary"></i><strong>Vehículo:</strong> ${props.vehicle || 'N/A'}</div>
                        </div>
                        <a href="${evt.url || '#'}" class="btn btn-sm btn-primary w-100 fw-semibold">
                            Ver Detalle <i class="bi bi-arrow-right ms-1"></i>
                        </a>
                    </div>
                </div>`;
            listContainer.innerHTML += cardHtml;
        });
    }

    function highlightSelectedDay(dayCellElement) {
        if (selectedDayEl) {
            selectedDayEl.classList.remove('fc-day-selected');
        }
        if (dayCellElement) {
            dayCellElement.classList.add('fc-day-selected');
            selectedDayEl = dayCellElement;
        }
    }

    var isMobile = window.innerWidth < 768;

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: isMobile ? 'listMonth' : 'dayGridMonth',
        locale: 'es',
        timeZone: 'local',
        displayEventTime: false,
        dayMaxEvents: 2,
        headerToolbar: isMobile ? {
            left: 'prev,next',
            center: 'title',
            right: 'today'
        } : {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,listMonth'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            list: 'Agenda'
        },
        events: rawEvents,

        eventContent: function (arg) {
            // En vista lista (móvil) usamos la representación nativa limpia
            if (arg.view.type === 'listMonth') {
                return null;
            }
            // En vista de cuadrícula (desktop)
            let titleEl = document.createElement('div');
            titleEl.classList.add('fc-custom-event');
            titleEl.innerHTML = `<span class="fc-event-dot-custom" style="background-color: ${arg.event.backgroundColor}"></span> <span class="fc-event-title-custom">${arg.event.title}</span>`;
            return { domNodes: [titleEl] };
        },

        dateClick: function (info) {
            highlightSelectedDay(info.dayEl);
            renderSidePanel(info.dateStr);
        },

        eventClick: function (info) {
            info.jsEvent.preventDefault();

            // Extraer fecha del evento
            var eventDate = info.event.startStr.split('T')[0];

            if (info.view.type === 'dayGridMonth') {
                var dayCell = info.el.closest('.fc-daygrid-day');
                if (dayCell) highlightSelectedDay(dayCell);
            }

            // Cargar el panel lateral con la orden del evento tocado
            renderSidePanel(eventDate);
        },

        height: 'auto',
        aspectRatio: isMobile ? 0.9 : 1.25
    });

    calendar.render();

    // Seleccionar automáticamente el primer día que tenga eventos o el día de hoy
    var todayStr = new Date().toISOString().split('T')[0];
    var initialDate = todayStr;

    // Si hoy no tiene registros, seleccionar el primer evento disponible para que la sección no arranque vacía
    var firstEvent = rawEvents.find(e => e.start >= todayStr) || rawEvents[0];
    if (firstEvent && firstEvent.start) {
        initialDate = firstEvent.start.split('T')[0];
    }

    renderSidePanel(initialDate);
});