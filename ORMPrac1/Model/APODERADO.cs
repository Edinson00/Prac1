//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ORMPrac1.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class APODERADO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string telefono { get; set; }
        public Nullable<int> Id_alumno { get; set; }
    
        public virtual ALUMNO ALUMNO { get; set; }
    }
}
