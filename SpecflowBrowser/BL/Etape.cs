//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Etape
    {
        public System.Guid Id { get; set; }
        public string Keyword { get; set; }
        public string NativeKeyword { get; set; }
        public string DocString { get; set; }
        public System.Guid ScenarioId { get; set; }
        public System.Guid ScenarioOutlineId { get; set; }
        public string Nom { get; set; }
        public Nullable<System.Guid> Table_Id { get; set; }
        public int EtapeIndex { get; set; }
    
        public virtual Table Table { get; set; }
        public virtual Scenario Scenario { get; set; }
    }
}