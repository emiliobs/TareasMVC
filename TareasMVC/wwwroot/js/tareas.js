function agregarNuevaTareaAlListado() {

    tareasListadoViewModel.tareas.push(new tareasElementoListadoViewModel({ id: 0, titulo: '' }));

    //jquery
    $("[name=titulo-tarea]").last().focus();

}

async function manejadorFocusoutTituloTarea(tarea)
{

    const titulo = tarea.titulo();

    if (!titulo) {

        tareasListadoViewModel.tareas.pop();

        return;

    }

    /*tarea.id(1);*/

    const data = JSON.stringify(titulo);

    const respuesta = await fetch('/api/tareas',
        {

        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (respuesta.ok) {
        const json = await respuesta.json();
        tarea.id(json.id);
    } else {
        //Mostrar mensaje de error.
    }

}

async function obtenerTareas()
{

    tareasListadoViewModel.cargando(true);

    const respuesta = await fetch('/api/tareas', {

        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }

    });

    if (!respuesta.ok) {

        return;

    }

    const json = await respuesta.json();
    tareasListadoViewModel.tareas([]);

    json.forEach(valor => {
        tareasListadoViewModel.tareas.push(new tareasElementoListadoViewModel(valor));
    });

    tareasListadoViewModel.cargando(false);
}