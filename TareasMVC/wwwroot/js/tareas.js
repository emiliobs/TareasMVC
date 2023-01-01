function agregarNuevaTareaAlListado() {

    tareasListadoViewModel.tareas.push(new tareasElementoListadoViewModel({ id: 0, titulo: '' }));

    //jquery
    $("[name=titulo-tarea]").last().focus();

}

function manejadorFocusoutTituloTarea(tarea) {

    const titulo = tarea.titulo();

    if (!titulo) {

        tareasListadoViewModel.tareas.pop();

        return;

    }

    tarea.id(1);
}