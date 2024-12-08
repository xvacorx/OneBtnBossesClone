using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using Firebase;

public class FirebaseAuthManager : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text feedbackText;

    private FirebaseAuth auth;
    private bool isRegistering = false;
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        playButton.interactable = auth.CurrentUser != null;

        feedbackText.text = auth.CurrentUser != null
            ? $"Bienvenido, {auth.CurrentUser.Email}!"
            : "Por favor, inicia sesión para jugar.";
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
            var authResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            feedbackText.text = $"Bienvenido, {user.Email}!";
            playButton.interactable = true;
            HideLoginPanel();
        }
        catch (System.Exception e)
        {
            feedbackText.text = "No se pudo iniciar sesión. Verifica tus credenciales.";
            Debug.LogError(e.Message);
        }
    }

    public async void Register()
    {
        if (isRegistering)
        {
            feedbackText.text = "Registro en proceso...";
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Por favor, completa todos los campos.";
            return;
        }

        isRegistering = true;

        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            feedbackText.text = $"Usuario registrado: {user.Email}. Ahora puedes iniciar sesión.";
        }
        catch (FirebaseException e)
        {
            if (e.ErrorCode == (int)AuthError.EmailAlreadyInUse)
            {
                feedbackText.text = "El correo electrónico ya está en uso. Intenta con otro.";
            }
            else
            {
                feedbackText.text = "No se pudo registrar el usuario. Intenta de nuevo.";
            }

            Debug.LogError(e.Message);
        }
        catch (System.Exception e)
        {
            feedbackText.text = "Ocurrió un error inesperado durante el registro.";
            Debug.LogError(e.Message);
        }
        finally
        {
            isRegistering = false;
        }
    }

    public void Logout()
    {
        auth.SignOut();
        playButton.interactable = false;
        feedbackText.text = "Sesión cerrada. Por favor, inicia sesión para jugar.";
        ShowLoginPanel();
    }
}