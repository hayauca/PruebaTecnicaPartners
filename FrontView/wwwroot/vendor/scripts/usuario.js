
var usuariosTable;

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

async function loadDataPersonasUsuarios() {


    try {
        const url = "https://localhost:7219/api/persona/getpersonas";

        const data = await makeHttpRequest('Consulta Personas', url, 'GET', null);
        if (data) {          

         
            const selectElement = document.getElementById("personaNombre");


            data.forEach(opcion => {
                const optionElement = document.createElement("option");

                optionElement.value = opcion.identificador;
                optionElement.textContent = opcion.nombres;

                selectElement.appendChild(optionElement);
            });


        } else {
            console.error("Error al cargar los datos Pesonas");
        }
    } catch (error) {
        console.error("Error en la solicitud datos Personas: ", error);
    }

}

function LimpiarTablaUsuario() {

    if ($.fn.DataTable.isDataTable('#usuariosTable')) {
        $('#usuariosTable').DataTable().destroy();

    }

    $('#usuariosTable tbody').empty();

}
async function loadDataUsuarios() {


    try {
        const url = "https://localhost:7219/api/usuario/getusuarios";

        const data = await makeHttpRequest('Consulta Usuarios', url, 'GET', null);
        if (data) {

            CargarTablaUsuarios(data)

        } else {
            console.error("Error al cargar los datos Usuarios");
        }
    } catch (error) {
        console.error("Error en la solicitud datos Usuarios: ", error);
    }

}

function CargarTablaUsuarios(listData) {

    const columnas =
        [
            { data: 'identificador', targets: 0 },
            { data: 'usuarioNombre', targets: 1 },
            { data: 'pass', targets: 2 },
        
            {
                'data': 'fechaCreacion', targets: 3,
                render: function (data, type, row) {
                    if (data) { return data.replace('T', ' '); } return data;
                }
            },

            {
                data: null, targets: 4,
                render: function (data) {
                    return `
                <button class="btn btn-warning btn-sm btn-edit" data-id="${data.identificador}">Editar</button>
                <button class="btn btn-danger btn-sm btn-delete" data-id="${data.identificador}">Eliminar</button>
              `;
                }
            }

        ];

    ConfigurarTablaUsuarios('usuariosTable', listData, columnas);
}

function ConfigurarTablaUsuarios(idTabla, listData, columnas) {

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

    usuariosTable = tabla;


    $('#' + idTabla + ' thead th').css('font-size', '14px');
    $('#' + idTabla + ' thead tr').clone(true).addClass('fila-filtros').appendTo('#' + idTabla + ' thead');

    $('#' + idTabla + ' thead tr:eq(1) th').each(function (index) {
        if (index === 4) {
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

async function ActualizarUsuario(rowData) {

    const url = "https://localhost:7219/api/usuario/editarusuario";

    const data = await makeHttpRequest('Actualizar Usuario', url, 'POST', rowData);
    if (data) {

        console.log('Datos actualizados exitosamente:', data);


        showAlert("Actualizar Usuario", "Datos actualizados correctamente.", "success")
            .then((value) => {
                console.log("El usuario cerró la alerta:", value);
            });

    }
}

async function CrearUsuario(objectParams) {


    const url = "https://localhost:7219/api/usuario/crearusuario";

    const data = await makeHttpRequest('Crear Usuario', url, 'POST', objectParams);
    if (data) {

        console.log('Datos grabados exitosamente:', data);

        showAlert("Crear Usuario", "Datos guardados correctamente.", "success")
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

    loadDataPersonasUsuarios();

   
    $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        let currentTab = $(e.target).text(); 

        switch (currentTab)
        {
            case 'Usuarios':

                $('#usuariosTable').DataTable().columns.adjust().draw(); 

                break;

            case 'Personas':

                $('#personasTable').DataTable().columns.adjust().draw(); 

                break;

            default: 
        };
    });

    loadDataUsuarios();

   
    $("#btnCreateUsuario").click(function ()
    {

        const selectContainer = document.getElementById("selectContainer");
        selectContainer.classList.remove("d-none"); // Mostrar el select

        $("#usuarioForm")[0].reset();
        $("#usuarioModalLabel").text("Registrar Usuario");
        $("#usuarioModal").modal("show");
    });

    $("#usuariosTable").on("click", ".btn-edit", function () {
       
        const rowData = usuariosTable.row($(this).parents('tr')).data();
        const selectContainer = document.getElementById("selectContainer");
        selectContainer.classList.add("d-none"); // Ocultar el select

        $("#personaNombre").val(rowData.identificador);
        $("#usuarioNombre").val(rowData.usuarioNombre);
        $("#password").val(rowData.pass);
      
        $("#usuarioModalLabel").text("Actualizar Usuario");
        $("#usuarioModal").modal("show");

    });


    $("#usuarioForm").on("submit", function (e) {
        e.preventDefault();

        const modalTitle = document.getElementById("usuarioModalLabel");


        const selectElement = document.getElementById("personaNombre"); 
        const selectedValue = selectElement.value; 

        const usuarioData = {
            identificador: selectedValue,
            usuarioNombre: $("#usuarioNombre").val(),
            pass: $("#password").val()
            
        };

        if (modalTitle.textContent === "Registrar Usuario")
        {

            CrearUsuario(usuarioData);

           
        }
        else if (modalTitle.textContent === "Actualizar Usuario")
        {
         
            ActualizarUsuario(usuarioData);

        }

        LimpiarTablaUsuario();
        usuariosTable = [];

        loadDataUsuarios();

        $("#usuarioModal").modal("hide");

    });

    $("#usuariosTable").on("click", ".btn-delete", function ()
    {
        const id = $(this).data("id");

      
        if (confirm("¿Está seguro de eliminar esta persona?")) {
           

            $.ajax({
                url: `https://localhost:7219/api/usuario/eliminarusuario/${id}`,
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${token}` // Incluye el token como encabezado
                },
                success: function () {
                    LimpiarTablaUsuario();
                    usuariosTable = [];
                    loadDataUsuarios();
                },
                error: function (xhr, status, error) {
                    console.error("Error al eliminar el usuario:", error);
                    alert("Hubo un problema al intentar eliminar el usuario.");
                }
            });
        }
    });
});