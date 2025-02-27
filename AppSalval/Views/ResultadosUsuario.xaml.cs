using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using AppSalval.Services;
using AppSalval.DTOS_API;
using System.Diagnostics;

namespace AppSalval.Views
{
    public partial class ResultadosUsuario : ContentPage
    {
        public ResultadosUsuario(List<string> recomendaciones, List<string> factoresRiesgo)
        {
            InitializeComponent();

            Debug.WriteLine($"? Constructor de ResultadosUsuario llamado");
            Debug.WriteLine($"?? Total de recomendaciones recibidas: {recomendaciones.Count}");
            Debug.WriteLine($"?? Total de factores de riesgo recibidos: {factoresRiesgo.Count}");

            CargarRecomendaciones(recomendaciones);
            CargarFactoresRiesgo(factoresRiesgo);
        }

        private void CargarRecomendaciones(List<string> recomendaciones)
        {
            if (recomendaciones.Count == 0)
            {
                RecomendacionesContainer.Children.Add(new Label
                {
                    Text = "No hay recomendaciones disponibles.",
                    FontSize = 16,
                    TextColor = Colors.White,
                    Margin = new Thickness(5)
                });
            }
            else
            {
                foreach (var rec in recomendaciones)
                {
                    RecomendacionesContainer.Children.Add(new Label
                    {
                        Text = $" {rec}",
                        FontSize = 16,
                        TextColor = Colors.White,
                        Margin = new Thickness(5)
                    });
                }
            }
        }

        private void CargarFactoresRiesgo(List<string> factoresRiesgo)
        {
            if (factoresRiesgo.Count == 0)
            {
                RiesgosContainer.Children.Add(new Label
                {
                    Text = "No hay factores de riesgo registrados.",
                    FontSize = 16,
                    TextColor = Colors.White,
                    Margin = new Thickness(5)
                });
            }
            else
            {
                foreach (var riesgo in factoresRiesgo)
                {
                    RiesgosContainer.Children.Add(new Label
                    {
                        Text = $" {riesgo}",
                        FontSize = 16,
                        TextColor = Colors.White,
                        Margin = new Thickness(5)
                    });
                }
            }
        }


        private async void OnSalirClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Formularios());
        }
    }
}
