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