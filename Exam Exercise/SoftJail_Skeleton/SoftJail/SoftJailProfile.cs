﻿namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System.Linq;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<ImportCellDto, Cell>();
            this.CreateMap<ImportMailDto, Mail>();
            this.CreateMap<Mail, ExportPrisonerMailDto>()
                .ForMember(d=>d.Description,mo=>mo.MapFrom(s=>string.Join("",s.Description.Reverse())));
            this.CreateMap<Prisoner, ExportPrisonerDto>()
                .ForMember(d => d.IncarcerationDate, mo => mo.MapFrom(s => s.IncarcerationDate.ToString("yyyy-MM-dd")));
         
        }
    }
}
