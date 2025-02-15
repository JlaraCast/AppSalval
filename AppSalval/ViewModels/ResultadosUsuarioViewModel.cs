using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSalval.ViewModels
{
    internal class ResultadosUsuarioViewModel : BaseViewModel
    {
        private string _recomendacion1;
        public string Recomendacion1
        {
            get => _recomendacion1;
            set => SetProperty(ref _recomendacion1, value);
        }

        private string _recomendacion2;
        public string Recomendacion2
        {
            get => _recomendacion2;
            set => SetProperty(ref _recomendacion2, value);
        }

        private string _recomendacion3;
        public string Recomendacion3
        {
            get => _recomendacion3;
            set => SetProperty(ref _recomendacion3, value);
        }

        private string _recomendacion4;
        public string Recomendacion4
        {
            get => _recomendacion4;
            set => SetProperty(ref _recomendacion4, value);
        }

        private string _recomendacion5;
        private string _riesgo1;

        private string _riesgo2;

        private string _riesgo3;
        public string Recomendacion5
        {
            get => _recomendacion5;
            set => SetProperty(ref _recomendacion5, value);
        }
        public string Riesgo1
        {
            get => _riesgo1;
            set => SetProperty(ref _riesgo1, value);
        }
        public string Riesgo2
        {
            get => _riesgo2;
            set => SetProperty(ref _riesgo2, value);
        }
        public string Riesgo3
        {
            get => _riesgo3;
            set => SetProperty(ref _riesgo3, value);
        }

        public ResultadosUsuarioViewModel()
        {
            //toca cargar las recomendaciones de la API 
            Recomendacion1 = "Recomendacion 1";
            Recomendacion2 = "Recomendacion 2";
            Recomendacion3 = "Recomendacion 3";
            Recomendacion4 = "Recomendacion 4";
            Recomendacion5 = "Recomendacion 5";

            Riesgo1 = "Riesgo 1";
            Riesgo2 = "Riesgo 2";
            Riesgo3 = "Riesgo 3";
        }
    }
}
