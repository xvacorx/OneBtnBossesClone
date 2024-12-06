using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;

public class FirebaseAuthManager : MonoBehaviour
{
    public GameObject loginPanel;
    public Button playButton;
    public Button loginButton;
    public Button registerButton;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text feedbackText;

    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Configuración inicial de los botones
        playButton.interactable = auth.CurrentUser != null;

        feedbackText.text = auth.CurrentUser != null
            ? $"Bienvenido, {auth.CurrentUser.Email}!"
            : "Por favor, inicia sesión para jugar.";

        // Asignar listeners
        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
    }

    public void HideLoginPanel()
    {
        loginPanel.SetActive(false);
    }

    public async void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Por favor, completa todos los campos.";
            return;
        }

        try
        {
            // Intentar iniciar sesión
            var authResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            feedbackText.text = $"Bienvenido, {user.Email}!";
            playButton.interactable = true; // Habilitar el botón Play
            HideLoginPanel(); // Ocultar el panel de login
        }
        catch (System.Exception e)
        {
            feedbackText.text = "No se pudo iniciar sesión. Verifica tus credenciales.";
            Debug.LogError(e.Message);
        }
    }

    public async void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Por favor, completa todos los campos.";
            return;
        }

        try
        {
            // Intentar registrar un nuevo usuario
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            feedbackText.text = $"Usuario registrado: {user.Email}. Ahora puedes iniciar sesión.";
        }
        catch (System.Exception e)
        {
            feedbackText.text = "No se pudo registrar el usuario. Intenta de nuevo.";
            Debug.LogError(e.Message);
        }
    }

    public void Logout()
    {
        auth.SignOut();
        playButton.interactable = false; // Deshabilitar el botón Play
        feedbackText.text = "Sesión cerrada. Por favor, inicia sesión para jugar.";
    }
}
