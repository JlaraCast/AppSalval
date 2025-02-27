using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AppSalval.DTOS_API;

namespace AppSalval.Views;

public partial class EdicionRespuestas : ContentPage
{
    // Definir la propiedad Respuestas
    public ObservableCollection<RespuestasDTO> Respuestas { get; set; }

    // Evento para actualizar la lista
    public event Action OnRespuestaUpdated;

    public EdicionRespuestas(RespuestasDTO respuesta)
    {
        InitializeComponent();

        // Asignar la respuesta a la propiedad y llenar los controles con sus datos
        Respuestas = new ObservableCollection<RespuestasDTO> { respuesta };
        BindingContext = this;

        // Asignar valores a los controles de la vista
        EntryIdRespuesta.Text = respuesta.IdRespuesta.ToString();
        EntryIdFormulario.Text = respuesta.IdFormulario.ToString();
        EntryIdentificacionEncuestado.Text = respuesta.IdentificacionEncuestado;
        DatePickerFechaRespuesta.Date = respuesta.FechaRespuesta;
    }

    // Método para manejar el evento de clic en el botón de cancelar
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();  // Regresar a la vista anterior
    }

    // Método para manejar el evento de clic en el botón de guardar cambios
    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        await UpdateRespuestaInApiAsync();
    }

    // Método para actualizar la respuesta en la API
    private async Task UpdateRespuestaInApiAsync()
    {
        try
        {
            // Validar que todos los campos estén completos
            if (string.IsNullOrEmpty(EntryIdRespuesta.Text) || string.IsNullOrEmpty(EntryIdFormulario.Text) || DatePickerFechaRespuesta.Date == null)
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                return;
            }

            // Crear un objeto RespuestasDTO con los datos actualizados
            var updatedRespuesta = new RespuestasDTO
            {
                IdRespuesta = int.Parse(EntryIdRespuesta.Text.Trim()),
                IdFormulario = int.Parse(EntryIdFormulario.Text.Trim()),
                IdentificacionEncuestado = EntryIdentificacionEncuestado.Text?.Trim(),
                FechaRespuesta = DatePickerFechaRespuesta.Date
            };

            // Configurar el cliente HTTP
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://savalapi.somee.com/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serializar el objeto actualizado a JSON y enviarlo a la API
            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedRespuesta), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"Respuesta/{updatedRespuesta.IdRespuesta}", jsonContent);

            // Manejar la respuesta de la API
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Respuesta actualizada correctamente", "OK");
                OnRespuestaUpdated?.Invoke();  // Notificar actualización
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
