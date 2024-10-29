<script src="https://cdnjs.cloudflare.com/ajax/libs/bcrypt/5.0.1/bcrypt.min.js"></script>

// Registrierungs-Formular Event Listener
document.getElementById('register-form').addEventListener('submit', async function (e) {
    e.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirm-password').value;
    const errorMessage = document.getElementById('error-message');

    // Password Checks
    // Length
    if (password.length < 16) {
        errorMessage.textContent = 'Password must be at least 16 characters long!';
        return;
    }
    // Confirmation
    if (password !== confirmPassword) {
        errorMessage.textContent = 'Passwords do not match!';
        return;
    }

    // Frontend Hashing
    const saltRounds = 10;
    const hashedPassword = await bcrypt.hash(password, saltRounds);

    // Communication to Database API
    fetch('http://localhost:5000/api/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            email: email,
            password: hashedPassword,
        }),
    })
    .then(response => response.json())
    .then(data => {
        if (data.message) {
            console.log('Registrierung erfolgreich:', data);

            window.location.href = 'login_page.html';
        } else {
            console.error('Registrierung fehlgeschlagen:', data);
        }
    })
    .catch((error) => {
        console.error('Error:', error);
    });
});

// Login-Formular Event Listener
document.getElementById('login-form').addEventListener('submit', async function (e) {
    e.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const saltRounds = 10;
    const hashedPassword = await bcrypt.hash(password, saltRounds);

    fetch('http://localhost:5000/api/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            email: email,
            password: hashedPassword,
        }),
    })
    .then(response => response.json())
    .then(data => {
        if (data.token) {
            console.log('Login erfolgreich:', data);
            // Speichern des Tokens (optional, falls Token verwendet werden)
            localStorage.setItem('authToken', data.token);
            
            // Weiterleitung zur Task-Seite nach erfolgreichem Login
            window.location.href = './mainpage/taskpage.html';
        } else {
            errorMessage.textContent = 'Login failed: ' + data.error;
            console.error('Login fehlgeschlagen:', data);
        }
    })
    .catch((error) => {
        console.error('Error:', error);
    });
});
