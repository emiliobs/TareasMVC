async function manejoErrorApi(respuesta) {

    let mensajeError = '';

    if (respuesta.status === 400) {

        mensajeError = await respuesta.text();

    } else if (respuesta.status === 404) {

        mensajeError = recursoNoEncontrado;

    } else{

        mensajeError = errorInesperado;
    }

    mostrarMEnsajeError(mensajeError);
}

function mostrarMEnsajeError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Oops.....',
        text: mensaje,
        
    })
}


function confirmarAccion({ callBackAceptar, callbackCancelar, titulo }) {
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

            callBackAceptar();
        } else if (callbackCancelar) {

            //El usuario ha presionado el botón de cancelar.
            callbackCancelar();
        }
    });
}