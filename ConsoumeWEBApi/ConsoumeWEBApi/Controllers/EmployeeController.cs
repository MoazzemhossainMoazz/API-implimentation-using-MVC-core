using ConsoumeWEBApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConsoumeWEBApi.Controllers
{
    public class EmployeeController : Controller
    {
        private HttpClient _httpClient;
        public EmployeeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7236/api/"); // Replace with your API base address
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeVM employeeVM)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<EmployeeVM>("Employee", employeeVM);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", response.ReasonPhrase ?? "unknown error occoured");
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Index()
        {

            var response = await _httpClient.GetAsync("Employees");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeVM>>();
                return View(content); // Or deserialize to a specific object
            }
            else
            {
                // Handle error status codes
                ModelState.AddModelError("", $"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return View();
            }

        }
        public IActionResult Create()
        {

            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<EmployeeVM>();
                return View(content); // Or deserialize to a specific object
            }
            else
            {
                // Handle error status codes
                ModelState.AddModelError("", $"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return View();
            }
        }
         public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Employees/{id}");
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index"); // Or deserialize to a specific object
            }
            else
            {
                // Handle error status codes
                ModelState.AddModelError("", $"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeVM employeeVM)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<EmployeeVM>($"Employees/{employeeVM.Id}", employeeVM);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", response.ReasonPhrase ?? " unknown error occured");
            }

            return View(employeeVM);
        }

    }
}
