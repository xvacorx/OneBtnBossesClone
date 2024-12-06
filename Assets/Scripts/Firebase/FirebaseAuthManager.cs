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

        loginPanel.SetActive(false);

        if (auth.CurrentUser != null)
        {
            playButton.interactable = true;
            feedbackText.text = $"Bienvenido, {auth.CurrentUser.Email}!";
        }
        else
        {
            playButton.interactable = false;
            feedbackText.text = "Por favor, inicia sesión para jugar.";
        }

        loginButton.onClick.AddListener(ShowLoginPanel);
        registerButton.onClick.AddListener(ShowLoginPanel);
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
            feedbackText.text = $"Error: {e.Message}";
            playButton.interactable = false;
        }
    }

    public async void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = authResult.User;

            feedbackText.text = $"Usuario registrado: {user.Email}";
            playButton.interactable = true;
            HideLoginPanel();
        }
        catch (System.Exception e)
        {
            feedbackText.text = $"Error: {e.Message}";
        }
    }

    public void Logout()
    {
        auth.SignOut();
        playButton.interactable = false;
        feedbackText.text = "Sesión cerrada. Por favor, inicia sesión para jugar.";
    }
}
