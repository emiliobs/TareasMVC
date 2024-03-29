﻿const urlTareas = "/api/tareas";

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

        manejarErrorApi(respuesta);

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

        manejoErrorApi(respuesta);

    }

    const json = await respuesta.json();
    tareasListadoViewModel.tareas([]);

    json.forEach(valor => {
        tareasListadoViewModel.tareas.push(new tareasElementoListadoViewModel(valor));
    });

    tareasListadoViewModel.cargando(false);
}

async function actualizarOrdenTareas() {

    const ids = obtenerIdsTareas();

    await enviarIDsTareasAlBackend(ids);

    const arregloOrdenado = tareasElementoListadoViewModel.tareas.sorted(function () {

        return ids.indexOf(a.id().toStrig()) - ids.indexOf(b.id().toStrig());

    });

    tareasListadoViewModel.tareas([]);
    tareasListadoViewModel.tareas(arregloOrdenado);

}

function obtenerIdsTareas() {

    const ids = $("[name=titulo-tarea]").map(function () {

        return $(this).attr("data-id");

    }).get();

    return ids;

}

async function enviarIDsTareasAlBackend(ids) {

    var data = JSON.stringify(ids);

    await fetch("api/tareas/ordener", {

        method: 'POST',
        body: data,
        headers: {

            'Content-Type': 'application/json'
        }

    });

}



async function manajearClickTarea(tarea) {

    if (tarea.esNuevo()) {
        return;
    }

    const respuesta = await fetch(`/api/tareas/${tarea.id()}`, {

        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }

    });

    if (!respuesta.ok) {
        manejoErrorApi(respuesta);
        return;
    }

    const json = await respuesta.json();

    /*console.log(json);*/

    tareaEditarVM.id = json.id;
    tareaEditarVM.titulo(json.titulo);
    tareaEditarVM.descripcion(json.descripcion);

    //Limpiar el arreglo de pasos:
    tareaEditarVM.pasos([]);

    json.pasos.forEach(paso => {
        tareaEditarVM.pasos.push({ ...paso, modoEdicion: false })
    });

    modalEditarTareaBootstrap.show();


}

async function manejarCambioEditarTarea() {
    const obj = {
        id: tareaEditarVM.id,
        titulo: tareaEditarVM.titulo(),
        descripcion: tareaEditarVM.descripcion()
    }

    if (!obj.titulo) {
        return;
    }

    await editarTareaCompleta(obj);

    const indice = tareasListadoViewModel.tareas().findIndex(t => t.id() === obj.id);
    const tarea = tareasListadoViewModel.tareas()[indice];
    tarea.titulo(obj.titulo);
}

async function editarTareaCompleta(tarea) {
    const data = JSON.stringify(tarea);

    const respuesta = await fetch(`/api/tareas/${tarea.id}`, {
        method: 'PUT',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!respuesta.ok) {

        manejoErrorApi(respuesta);
        throw "Error";

    }
}


function intentarBorrarTarea(tarea) {
    modalEditarTareaBootstrap.hide();

    confirmarAccion({
        callbackAceptar: () => {
            borrarTarea(tarea);
        },
        callbackCancelar: () => {
            modalEditarTareaBootstrap.show();
        },

        titulo: `Desea borrar la tarea  ${tarea.titulo()}?`
    });
}

async function borrarTarea(tarea) {
   /* const idTarea = tarea.id;*/

    const respuesta = await fetch("/api/tareas/" + tarea.id, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (respuesta.ok) {

        const indice = obtenerIndiceTareaEnEdicion();
        tareasListadoViewModel.tareas.splice(indice, 1);
    }
}


function obtenerIndiceTareaEnEdicion() {
    return tareasListadoViewModel.tareas().findIndex(t => t.id() == tareaEditarVM.id);

}


$(function () {

    $("#reordenable").sortable({

        axis: 'y',
        stop: async function () {

            await actualizarOrdenTareas();

        }

    })

})