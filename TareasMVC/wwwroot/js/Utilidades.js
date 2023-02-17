async function manejarErrorApi(respuesta) {

    let mensajeError = '';

    if (respuesta.status === 400) {

        mensajeError = await respuesta.text();

    } else if (respuesta.status === 404) {

        mensajeError = recursoNoEncontrado;

    } else{

        mensajeError = errorInesperado;
    }

    mostrarMensajeError(mensajeError);
}

function mostrarMensajeError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Oops.....',
        text: mensaje,
        
    })
}


function confirmarAccion({ callbackAceptar, callbackCancelar, titulo }) {
    Swal.fire({
        title: titulo || 'Realmente deseas hacer esto?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si',
        focusConfirm: true
    }).then((resultado) => {
        if (resultado.isConfirmed) {

            callbackAceptar();
        } else if (callbackCancelar) {

            //El usuario ha presionado el bot�n de cancelar.
            callbackCancelar();
        }
    });
}