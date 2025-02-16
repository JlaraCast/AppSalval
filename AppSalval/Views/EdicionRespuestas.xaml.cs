using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AppSalval.DTOS_API;

namespace AppSalval.Views;

public partial class EdicionRespuestas : ContentPage
{
    public ObservableCollection<RespuestasDTO> Respuestas { get; set; } // Definir la propiedad Respuestas

    public event Action OnRespuestaUpdated; // Evento para actualizar la lista

    public EdicionRespuestas(RespuestasDTO respuesta)
    {
        InitializeComponent();

        // Asignar la respuesta a la propiedad y llenar los controles con sus datos
        Respuestas = new ObservableCollection<RespuestasDTO> { respuesta };
        BindingContext = this;

        EntryIdRespuesta.Text = respuesta.IdRespuesta.ToString();
        EntryIdFormulario.Text = respuesta.IdFormulario.ToString();
        EntryIdentificacionEncuestado.Text = respuesta.IdentificacionEncuestado;
        DatePickerFechaRespuesta.Date = respuesta.FechaRespuesta;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();  // Regresar a la vista anterior
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        await UpdateRespuestaInApiAsync();
    }

    private async Task UpdateRespuestaInApiAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(EntryIdRespuesta.Text) || string.IsNullOrEmpty(EntryIdFormulario.Text) || DatePickerFechaRespuesta.Date == null)
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                return;
            }

            var updatedRespuesta = new RespuestasDTO
            {
                IdRespuesta = int.Parse(EntryIdRespuesta.Text.Trim()),
                IdFormulario = int.Parse(EntryIdFormulario.Text.Trim()),
                IdentificacionEncuestado = EntryIdentificacionEncuestado.Text?.Trim(),
                FechaRespuesta = DatePickerFechaRespuesta.Date
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://savalapi.somee.com/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedRespuesta), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"Respuesta/{updatedRespuesta.IdRespuesta}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("�xito", "Respuesta actualizada correctamente", "OK");
                OnRespuestaUpdated?.Invoke();  // Notificar actualizaci�n
                await Navigation.PopAsync();
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo actualizar la respuesta: {errorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al conectar con la API: {ex.Message}", "OK");
        }
    }
}
