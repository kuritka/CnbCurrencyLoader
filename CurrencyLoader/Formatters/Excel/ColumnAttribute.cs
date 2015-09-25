using System;

namespace CurrencyLoader.Formatters.Excel
{
    /// <summary>
    /// Attribute specifies size and text in the column.    
    /// </summary>
    public class ColumnAttribute : Attribute
    {        
        public ColumnAttribute(string description, double columnWidth)
        {
            Description = description;
            ColumnWidth = columnWidth;            
        }

        public string Description { get; set; }

        public double ColumnWidth { get; set; }        
    }
}
