using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Data.Parser.Flat
{
    public interface IFileReader
    {
        int Id {get;set;}
        int IdContrato {get;set;}
        string Name {get;set;}
        FileReaderType FileReaderType { get; set; }
        bool IsDeleted { get; set; }
        DateTime DateCreation { get; set; }
        Guid CreatedBy { get; set; }
        string Reference { get; set; }

        List<IFileReaderSection> GetSectionList();
        IFileReaderSection GetSection(string sectionReference);
    }

    public interface IFileReaderSection
    {
        int Id { get; set; }
        int IdContrato { get; set; }
        string Name { get; set; }
        bool IsDeleted { get; set; }
        List<IFileReaderField> Fields { get; set; }
    }

    /// <summary>
    /// Guarda a estrutura dos campos
    /// </summary>
    public interface IFileReaderField
    {
        int Id { get; set; }
        int IdContrato { get; set; }
        int IdFileReader { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Position { get; set; }
        int Length { get; set; }
        byte FieldType { get; set; }
        object DefaultValue { get; set; }
        string Reference { get; set; }
    }

    public enum FileReaderType
    {
        CSV,
        CNAB240
    }
}
