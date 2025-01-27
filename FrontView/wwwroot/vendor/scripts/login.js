$(document).ready(function () {
    $("#loginForm").on("submit", function (e) {
        e.preventDefault();

        // Obtener los datos del formulario
        const username = $("#username").val();
        const password = $("#password").val();

        // Enviar solicitud a la API
        $.ajax({
            url: "https://localhost:7219/api/token", // Cambia esta URL a la de API
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                usuario: username,
                password: password
            }),
            success: function (response)
            {

                const username = document.getElementById("username").value;
              
                //const token = localStorage.setItem("authToken", response.token);
                //localStorage.setItem("username", username);

                sessionStorage.setItem("username", username);
                sessionStorage.setItem("authToken", response.token);

                if (!response.token)
                {
                    alert('No tienes acceso. Por favor, inicia sesion');
                    window.location.href = "/Index"; 

                }

                window.location.href = "/General"; 
            },
            error: function () {
              
                $("#errorMessage").fadeIn().delay(3000).fadeOut();
            }
        });
    });
});