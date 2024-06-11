const apiUrl = process.env.AUTH_URL;
function register() {
    const password = document.getElementById("password").value;
    const passwordRepeat = document.getElementById("repeat-password").value;
    if (password != passwordRepeat)
        return;

    const userName = document.getElementById("user-name").value;
    const firstName = document.getElementById("first-name").value;
    const secondName = document.getElementById("second-name").value;
    const email = document.getElementById("email").value;

    const userData = {
        userName: userName,
        firstName: firstName,
        secondName: secondName,
        email: email,
        password: password
    };

    fetch(`${apiUrl}/api/auth/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.succeeded === true) {
                window.location.href = 'LoginPageStudent.html';
            }
        })
        .catch(error => {
            console.error('Ошибка:', error);
        });
}

function login() {
    const userName = document.getElementById("user-name").value;
    const password = document.getElementById("password").value;

    const userData = {
        userName: userName,
        password: password
    };
    
    fetch(`${apiUrl}/api/auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    })
    .then(function(response) {
        if (!response.ok) {
            throw new Error("HTTP status " + response.status);
        }
        return response.json();
    })
        .then(data => {
            console.log(data);
            localStorage.setItem('accessToken', data.accessToken);
            window.location.href = '../HelloPage.html';
        })
        .catch(error => {
            console.error(error);
        });
}

function logout() {
    const accessToken = localStorage.getItem("accessToken");
    fetch(`${apiUrl}/api/auth/logout`, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer' + ' ' + accessToken
        }
    })
    .then(function(response) {
        if (!response.ok) {
            throw new Error("HTTP status " + response.status);
        }
        localStorage.clear();
        window.location.href = '/index.html';//todo: переделать!!!!!
    })
        .catch(error => {
            console.error(error);
        });
}

window.register = register;
window.login = login;
window.logout = logout;