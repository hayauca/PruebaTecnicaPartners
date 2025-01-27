
var personasTable;

var username = '';
var token = '';

function showAlert(title, text, icon) {
    let options = {
        title: title,
        text: text,
        icon: icon,
    };
    return swal(options).then((value) => {
       
        return value;
    });
}


async function makeHttpRequest(title, url, method = 'GET', params = null, headers = { 'Content-Type': 'application/json', "Authorization": `Bearer ${token}` }) {

    httpstatus = 0;


    try {
        const options = {
            method,
            headers,
        };

        if (params) {
            options.body = JSON.stringify(params);
        }

        const response = await fetch(url, options);

        
        const responseBody = await response.text(); 
        if (!response.ok) {

            httpstatus = response.status;

            console.error("HTTP Error: " + response.status + " " + response.statusText);
            console.error("Detalles del error: " + responseBody);
            throw new Error("HTTP Error: " + '{"result":{"status":false,"message":"No hubo datos de consulta"}}' + " " + response.statusText + ". Detalles: " + responseBody);
        }

      
        try {
            return JSON.parse(responseBody);
        } catch (error) {
           
            return responseBody;
        }

    } catch (error) {

        console.error("Error en la solicitud HTTP a " + url + ": " + (error.message || error));

    }
}

function LimpiarTablaPersona() {

    if ($.fn.DataTable.isDataTable('#personasTable')) {
        $('#personasTable').DataTable().destroy();

    }

    $('#personasTable tbody').empty();

}
async function loadDataPersonas() {


    try {
        const url = "https://localhost:7219/api/persona/getpersonas";

        const data = await makeHttpRequest('Consulta Personas', url, 'GET', null);
        if (data)
        {

            CargarTablaPersonas(data)

        } else {
            console.error("Error al cargar los datos Pesonas");
        }
    } catch (error) {
        console.error("Error en la solicitud datos Personas: ", error);
    }

}

function CargarTablaPersonas(listData) {

    const columnas =
        [
            { data: 'identificador', targets: 0 },
            { data: 'nombres', targets: 1},
            { data: 'apellidos', targets: 2 },
            { data: 'numeroIdentificacion', targets: 3 },
            { data: 'email', targets: 4},
            { data: 'tipoIdentificacion', targets: 5 },
            {
                'data': 'fechaCreacion', targets: 6,
                render: function (data, type, row) {
                    if (data) { return data.replace('T', ' '); } return data;
                }
            },

            {
                data: null, targets: 7,
                render: function (data) {
                    return `
                <button class="btn btn-warning btn-sm btn-edit" data-id="${data.identificador}">Editar</button>
                <button class="btn btn-danger btn-sm btn-delete" data-id="${data.identificador}">Eliminar</button>
              `;
                }
            }

        ];

    ConfigurarTablaPersonas('personasTable', listData, columnas);
}

function ConfigurarTablaPersonas(idTabla, listData, columnas) {

    let listCargaLocal = [];

    listCargaLocal = listData;

    if ($.fn.DataTable.isDataTable('#' + idTabla)) {
        $('#' + idTabla).DataTable().destroy();
    }
 
    $('#' + idTabla + ' tbody').empty();
    $('#' + idTabla + ' thead .fila-filtros').remove();
  

    const opcionesBase = {
        data: listCargaLocal,
        responsive: true,
        dom: 'Blfrtip',
        ordering: false,
        scrollX: true,
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Spanish.json"
        },
        lengthChange: true,
        columns: columnas,
    };

    const configFinal = Object.assign({}, opcionesBase);

    const tabla = $('#' + idTabla).DataTable(configFinal);

    personasTable = tabla;

     
    $('#' + idTabla + ' thead th').css('font-size', '14px');
    $('#' + idTabla + ' thead tr').clone(true).addClass('fila-filtros').appendTo('#' + idTabla + ' thead');

    $('#' + idTabla + ' thead tr:eq(1) th').each(function (index)
    {
        if (index === 7) {
            $(this).html('');
            return;
        }


        $(this).html('<input type="text" placeholder="Filtrar...." style="width: 100%; box-sizing: border-box;" />');

        $('input', this).on('keyup change', function () {
            if (tabla.column(index).search() !== this.value) {
                tabla.column(index).search(this.value).draw();
            }
        });
    });

}

async function ActualizarPersona(rowData)
{

    const url = "https://localhost:7219/api/persona/editarpersona";

    const data = await makeHttpRequest('Actualizar Persona', url, 'POST', rowData);
    if (data) {

        console.log('Datos actualizados exitosamente:', data);

        showAlert("Actualizar Persona", "Datos actualizados correctamente.", "success")
            .then((value) => {
                console.log("El usuario cerró la alerta:", value);
            });

    }
}


async function EliminarPersona(id) {

    const url = "https://localhost:7219/api/persona/eliminarpersona";

    const data = await makeHttpRequest('Eliminar Persona', url, 'PUT', rowData);
    if (data) {

        console.log('Datos eliminados exitosamente:', data);

        showAlert("Eliminar Persona", "Datos eliminados correctamente.", "success")
            .then((value) => {
                console.log("El usuario cerró la alerta:", value);
            });

    }
}
async function CrearPersona(objectParams) {


    const url = "https://localhost:7219/api/persona/crearpersona";

    const data = await makeHttpRequest('Crear Persona', url, 'POST', objectParams);
    if (data) {

        console.log('Datos grabados exitosamente:', data);

        showAlert("Crear Persona", "Datos guardados correctamente.", "success")
            .then((value) => {
                console.log("El usuario cerró la alerta:", value);
               
            });

    }
}


$(document).ready(function () {

    username = '';
    token = '';

    username = sessionStorage.getItem("username");
    token = sessionStorage.getItem("authToken");

    $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        let currentTab = $(e.target).text();

        switch (currentTab) {
            case 'Usuarios':

                $('#usuariosTable').DataTable().columns.adjust().draw();

                break;

            case 'Personas':

                $('#personasTable').DataTable().columns.adjust().draw();

                break;

            default:
        };
    });


    loadDataPersonas();

    $("#btnCreate").click(function () {
        $("#personaForm")[0].reset();
        $("#personaId").val("");
        $("#personaModalLabel").text("Registrar Persona");
        $("#personaModal").modal("show");
    });


    $("#personasTable").on("click", ".btn-edit", function () {
    
        const rowData = personasTable.row($(this).parents('tr')).data();
     
        $("#personaId").val(rowData.identificador);
        $("#nombres").val(rowData.nombres);
        $("#apellidos").val(rowData.apellidos);
        $("#identificacion").val(rowData.numeroIdentificacion);
        $("#tipoIdentificacion").val(rowData.tipoIdentificacion);
        $("#email").val(rowData.email);
        $("#personaModalLabel").text("Actualizar Persona");
        $("#personaModal").modal("show");

    });


    $("#personaForm").on("submit", function (e) {
        e.preventDefault();

        const personaData = {
            identificador: $("#personaId").val(),
            nombres: $("#nombres").val(),
            apellidos: $("#apellidos").val(),
            numeroIdentificacion: $("#identificacion").val(),
            tipoIdentificacion: $("#tipoIdentificacion").val(),
            email: $("#email").val()
        };

        if (personaData.identificador === '') {
            personaData.identificador = 0;

            CrearPersona(personaData);

        }
        else
        {
            ActualizarPersona(personaData);

        }

    
        LimpiarTablaPersona();
        personasTable = [];

        loadDataPersonas();
      
        $("#personaModal").modal("hide");

    });

    $("#personasTable").on("click", ".btn-delete", function () {


        const id = $(this).data("id");

        if (confirm("¿Está seguro de eliminar esta persona?"))
        {
           

            $.ajax({
                url: `https://localhost:7219/api/persona/eliminarpersona/${id}`,
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${token}` // Incluye el token como encabezado
                },
                success: function () {
                    LimpiarTablaPersona();
                    loadDataPersonas();
                },
                error: function (xhr, status, error) {
                    console.error("Error al eliminar la persona:", error);
                    alert("Hubo un problema al intentar eliminar la persona.");
                }
            });
        }



    });
});