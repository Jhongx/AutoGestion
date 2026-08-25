document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    if (!calendarEl) return;

    var rawEvents = JSON.parse(calendarEl.dataset.events || '[]').map(function (evt) {
        var cleanEvt = Object.assign({}, evt);
        if (cleanEvt.start && cleanEvt.end) {
            if (cleanEvt.start.split('T')[0] === cleanEvt.end.split('T')[0]) {
                delete cleanEvt.end;
            }
        }
        cleanEvt.allDay = true;
        return cleanEvt;
    });

    function renderSidePanel(dateStr) {
        var listContainer = document.getElementById('sidePanelOrdersList');
        if (!listContainer) return;

        listContainer.innerHTML = '';
        var dateParts = dateStr.split('-');
        var formattedDate = `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;

        var badgeEl = document.getElementById('selectedDateBadge');
        if (badgeEl) badgeEl.innerText = formattedDate;

        var dayEvents = rawEvents.filter(e => e.start && e.start.startsWith(dateStr));

        if (dayEvents.length === 0) {
            listContainer.innerHTML = `
                <div class="text-center py-4 text-muted">
                    <i class="bi bi-calendar-x fs-1 d-block mb-2 text-secondary"></i>
                    <p class="mb-0 fw-semibold">No hay citas programadas</p>
                    <small class="text-muted">Selecciona otro día en la agenda.</small>
                </div>`;
            return;
        }

        dayEvents.forEach(function (evt) {
            var props = evt.extendedProps || {};
            var detailsUrl = evt.url || `/InspectionAppointments/Details?id=${evt.id}`;

            // Construcción condicional del botón de recepción según la propiedad canReceive del backend
            var receiveButtonHtml = '';
            if (props.canReceive) {
                receiveButtonHtml = `
                    <a href="/ReceivingOrders/Create?appointmentId=${evt.id}" class="btn btn-sm btn-primary w-50 fw-semibold">
                        Recepcionar <i class="bi bi-arrow-right ms-1"></i>
                    </a>`;
            }

            // Si ya está convertida, el botón de detalle toma todo el ancho disponible
            var detailsButtonClass = props.canReceive ? 'w-50' : 'w-100';

            var cardHtml = `
        <div class="card border border-light-subtle shadow-sm rounded-3 overflow-hidden" style="border-left: 4px solid ${evt.color || '#0d6efd'} !important;">
            <div class="card-body p-3">
                <div class="d-flex justify-content-between align-items-start mb-2">
                    <span class="badge bg-dark bg-opacity-10 text-dark fw-bold">
                        <i class="bi bi-clock me-1"></i>${props.time || 'Pendiente'}
                    </span>
                    <span class="badge" style="background-color: ${evt.color || '#0d6efd'};">
                        ${props.status || 'Programada'}
                    </span>
                </div>
                <h6 class="fw-bold text-dark mb-1">${props.fullTitle || evt.title}</h6>
                <div class="small text-muted mb-3">
                    <div class="mb-1"><i class="bi bi-person me-1 text-primary"></i><strong>Cliente:</strong> ${props.client}</div>
                    <div class="mb-1"><i class="bi bi-car-front me-1 text-primary"></i><strong>Vehículo:</strong> ${props.vehicle}</div>
                    <div><i class="bi bi-chat-left-text me-1 text-primary"></i><strong>Sintomatología:</strong> ${props.reason}</div>
                </div>
                <div class="d-flex gap-2">
                    <a href="${detailsUrl}" class="btn btn-sm btn-outline-secondary ${detailsButtonClass} fw-semibold">
                        Ver Detalle
                    </a>
                    ${receiveButtonHtml}
                </div>
            </div>
        </div>`;
            listContainer.innerHTML += cardHtml;
        });
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
        buttonText: { today: 'Hoy', month: 'Mes', week: 'Semana', list: 'Agenda' },
        events: rawEvents,

        dateClick: function (info) { renderSidePanel(info.dateStr); },
        eventClick: function (info) {
            info.jsEvent.preventDefault();
            renderSidePanel(info.event.startStr.split('T')[0]);
        },
        height: 'auto',
        aspectRatio: isMobile ? 0.9 : 1.25
    });

    calendar.render();

    var todayStr = new Date().toISOString().split('T')[0];
    var firstEvent = rawEvents.find(e => e.start >= todayStr) || rawEvents[0];
    renderSidePanel(firstEvent && firstEvent.start ? firstEvent.start.split('T')[0] : todayStr);
});