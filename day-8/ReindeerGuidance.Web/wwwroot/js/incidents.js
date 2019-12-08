$.get(apiConfig.endpointUrl + '/subscribe', function (response) {
    const options = {
        accessTokenFactory: () => response.accessToken
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(response.url, options)
        .build();

    connection.on('broadcast', async (data) => {
        renderServiceStatus();
        var incidents = await getIncidentsAsync();
        renderIncidents(incidents);
    });

    connection.start();
});

function getIncidentsAsync() {
    return new Promise((resolve, reject) => {
        $.get(apiConfig.endpointUrl + '/incident', function (response) {
            resolve(response);
        }).fail(function () {
            reject();
        });
    });
}

function renderServiceStatus() {
    $.get(apiConfig.endpointUrl + '/status', function (response) {
        if (response !== "ok") {
            $('#service-status').removeClass('badge-success').addClass('badge-danger').text('Services are experiencing issues');
        } else {
            $('#service-status').removeClass('badge-danger').addClass('badge-success').text('Services up and running');
        }
    });
}

function renderIncidents(incidents) {
    var $tbody = $('#table-incidents').find('tbody');
    $tbody.html("");

    for (var i = 0; i < incidents.length; ++i) {
        var $row = $('<tr>');
        $row.append($('<td>').text(incidents[i].status));
        $row.append($('<td>').text(incidents[i].title));
        $row.append($('<td>').text(incidents[i].notes));
        $row.append($('<td>').text(incidents[i].modifiedAt));
        $tbody.append($row);
    }
}

$(document).ready(async function () {
    renderServiceStatus();
    renderIncidents(await getIncidentsAsync());
});
