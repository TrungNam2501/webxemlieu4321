using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KendaWeb.Model
{
    public class TempData
    {
        public static DataTable tableMES { get; set; }
        public static DataTable tableDetail { get; set; }
        public static DataTable tableDoNguoc { get; set; }
        public static DataTable tableHC { get; set; }




        public static DataTable tableBonHC { get; set; }
        public static DataTable prdebe { get; set; }
        public static DataTable tableDoNguocRL { get; set; }
        public static DataTable tablePrdebc { get; set; }

        public static DataTable tablePrdebe { get; set; }

        public static DataTable tablePlc { get; set; }
        public static DataTable tablePlc1 { get; set; }
        public static DataTable tableLrPlan { get; set; }
        public static DataTable tableLrRecipe { get; set; }
        public static DataTable tableLrLot { get; set; }
        public static DataTable tableLrWeight { get; set; }
        public static DataTable tableLrBarcodelog { get; set; }
        public static DataTable tableMes2RawMaterial { get; set; }


        public static DataTable tableLrPlanCantay { get; set; }
        public static DataTable tableIfscan2Mes { get; set; }
        public static DataTable tableLrRecipeCantay { get; set; }
        public static DataTable tableMes2RawMaterialCantay { get; set; }
        public static DataTable tableLrWeighCantay { get; set; }

    }
   
}