using AutoMapper;
using TareasMVC.Entidades;
using TareasMVC.Models;

namespace TareasMVC.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tareas, TareaDTO>()
                .ForMember(dto => dto.PasosTotal, pasos => pasos.MapFrom(p => p.Pasos.Count()))
                .ForMember(dto => dto.PasosRealizados, pasos => pasos.MapFrom(p => p.Pasos.Where(p => p.Realizado).Count()));

        }
    }
}
